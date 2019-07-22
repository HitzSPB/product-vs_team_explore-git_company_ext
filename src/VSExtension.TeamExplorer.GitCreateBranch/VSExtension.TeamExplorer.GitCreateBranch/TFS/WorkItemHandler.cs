using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace VSExtension.TeamExplorer.GitCreateBranch.TFS
{
  public class WorkItemHandler
  {
    private readonly TfsInformationHandler tfsHandler;
    public WorkItemHandler()
    {
      tfsHandler = new TfsInformationHandler();
    }

    public (WorkItem, string, bool) VerifyAndReceiveWorkItem(string tfsWorkItemInput)
    {
      if (int.TryParse(tfsWorkItemInput, out int tfsWorkItemId))
      {
        Facade.Logger.Verbose("Numeric value was inserted into WorkItemID textbox");
        (WorkItem tfsWorkItemHolder, string tfsWorkItemMessage, bool tfsWorkItemstate) = tfsHandler.FindWorkItemById(tfsWorkItemId);
        if (tfsWorkItemstate)
        {
          if (tfsWorkItemHolder.State == "Active")
            return (tfsWorkItemHolder, tfsWorkItemMessage, true);
          else
            return (null, "Work item is not active", false);

        }
        else
        {
          return (null, tfsWorkItemMessage, false);
        }
      }
      else
      {
        return (null, "A non numeric value was entered", false);
      }
    }
  }
}
