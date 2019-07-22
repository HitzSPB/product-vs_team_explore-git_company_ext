using Microsoft.TeamFoundation.WorkItemTracking.Client;
using VSExtension.TeamExplorer.GitCreateBranch.TextFormatters;
using VSExtension.TeamExplorer.GitCreateBranch.TFS;

namespace VSExtension.TeamExplorer.GitCreateBranch
{
  public class GitBranchHandler
  {
    private readonly WorkItemHandler ItemHandler;
    private readonly GitCommandHandler commandHandler;
    public GitBranchHandler()
    {
      ItemHandler = new WorkItemHandler();
      commandHandler = new GitCommandHandler();
    }

    public void CreateBranch(string teamName, string tfsWorkItemID, string branchName, string selectedMainBranch, string versionNumber)
    {
      if (string.IsNullOrWhiteSpace(versionNumber))
      {
        SendInformationToLogger("Version number is null", false);
        return;
      }

      string selectedTeam = StringHandler.StringInputChecker(teamName);
      string selectedTaskID = StringHandler.StringInputChecker(tfsWorkItemID);
      string selectedBranchName = StringHandler.StringInputChecker(branchName);

      (string InputLog, bool validatorOutcome) = InputValidator.InputIsValid(selectedTeam, selectedTaskID, selectedBranchName);
      SendInformationToLogger(InputLog, validatorOutcome);

      if (!validatorOutcome)
      {
        return;
      }

      (WorkItem tfsWorkItem, string logMessage, bool tfsWorkItemState) = ItemHandler.VerifyAndReceiveWorkItem(selectedTaskID);
      SendInformationToLogger(logMessage, tfsWorkItemState);

      if (!tfsWorkItemState)
      {
        return;
      }

      if (!string.IsNullOrWhiteSpace(selectedBranchName))
      {
        selectedBranchName = $"-{selectedBranchName}";
      }
      (string path, string command) = GitCommandCreator.CreateCommand(selectedTeam, tfsWorkItem, selectedBranchName, versionNumber);
      commandHandler.GitCreateBranch(path, command, selectedMainBranch);

    }


    public void SendInformationToLogger(string message, bool isLogTypeInformation)
    {
      if (isLogTypeInformation)
      {
        Facade.Logger.Information(message);
      }
      else
      {
        Facade.Logger.Error(message);
        UserDisplayMessagePopUp.DisplayError(message);
      }
    }
  }
}
