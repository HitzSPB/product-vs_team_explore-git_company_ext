using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace VSExtension.TeamExplorer.GitCreateBranch.TextFormatters
{
  public static class GitCommandCreator
  {
    public static (string path, string command) CreateCommand(string team, WorkItem workItemName, string branchName, string versionNumber)
    {
      return (TeamExplorerBranchButton.GetActiveRepo().RepositoryPath, $"git checkout -b tfs/{team}/{versionNumber}/{workItemName.Type.Name.ToLower()}-{workItemName.Id}{branchName}");
    }
  }
}
