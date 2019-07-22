using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace VSExtension.TeamExplorer.GitCreateBranch.TFS
{
  internal class TfsInformationHandler
  {
    private readonly Uri _tfsUri = new Uri("http://tfs:8080/tfs/DefaultCollection");
    private static TfsTeamProjectCollection _Tpc { get; set; }
    private readonly WorkItemStore Store;
    public TfsInformationHandler()
    {
      Store = GetWorkItemStore(_tfsUri);
    }

    private static WorkItemStore GetWorkItemStore(Uri tpcAddress)
    {
      _Tpc = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(tpcAddress);
      _Tpc.Authenticate(); // windows authentication

      var result = new WorkItemStore(_Tpc);
      return result;
    }


    public (WorkItem workItemHolder, string message, bool state) FindWorkItemById(int wid)
    {
      WorkItem LastWorkitem;
      try { LastWorkitem = Store.GetWorkItem(wid); }
      catch { LastWorkitem = null; }

      if (LastWorkitem != null)
      {
        return (LastWorkitem, "The selected work ID has been found", true);
      }
      else
      {
        return (LastWorkitem, "The selected work ID has not been found", false);
      }

    }

    public List<string> GetTeamList(string team)
    {
      XmlDocument globallistxml = Store.ExportGlobalLists();
      var Teams = new List<string>();
      foreach (XmlElement element in globallistxml.GetElementsByTagName("GLOBALLIST"))
      {
        if (element.Attributes["name"].Value.ToLower().Equals(team.ToLower()))
        {
          foreach (XmlNode childNode in element.ChildNodes)
          {
            if (childNode.Attributes != null)
              Teams.Add(StringHandler.StringInputChecker(childNode.Attributes["value"].Value));

          }
        }
      }
      return Teams;
    }


  }
}

