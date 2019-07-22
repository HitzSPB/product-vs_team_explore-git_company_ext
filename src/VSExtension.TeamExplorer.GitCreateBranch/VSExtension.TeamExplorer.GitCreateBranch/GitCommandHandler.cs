using VSExtension.TeamExplorer.GitCreateBranch.TextFormatters;

namespace VSExtension.TeamExplorer.GitCreateBranch
{
  public class GitCommandHandler
  {
    private readonly ShellHandler shellHandler;
    public GitCommandHandler()
    {
      shellHandler = new ShellHandler();
    }
    public void GitCreateBranch(string path, string command, string selectedMainBranch)
    {
      if (!shellHandler.ExecuteShellCommand(path, "git fetch origin --prune"))
      {
        UserDisplayMessagePopUp.DisplayError("Command failed - Check log for errors");
        Facade.Logger.Error("Something went wrong under a Git Fetch Command. contact team core");
        return;
      }

      bool useActiveBranch = selectedMainBranch == "Active Branch";
      if (!useActiveBranch)
      {
        if (!shellHandler.ExecuteShellCommand(path, $"git checkout {selectedMainBranch.ToLower()}"))
        {
          UserDisplayMessagePopUp.DisplayError("Command failed - Check log for errors");
          Facade.Logger.Error("Something went wrong under a git checkout Command. You might have uncommited changes.");
          return;
        }
      }

      string rebasePostfix = string.Empty;

      if (!useActiveBranch)
      {
        rebasePostfix = $" origin {selectedMainBranch.ToLower()}";
      }

      string commandToExecute = $"git pull --rebase{rebasePostfix}";
      if (!shellHandler.ExecuteShellCommand(path, commandToExecute))
      {
        UserDisplayMessagePopUp.DisplayError("Command failed - Check log for errors");
        Facade.Logger.Error($"Check for merge conflicts or uncommited changes - Failed at command: {commandToExecute}");
        return;
      }

      if (shellHandler.ExecuteShellCommand(path, command))
      {
        UserDisplayMessagePopUp.DisplayMessage("Your Branch has been created");
        Facade.Logger.Verbose("Create Branch was successful");
      }
      else
      {
        UserDisplayMessagePopUp.DisplayError("Command failed - Check log for errors");
        Facade.Logger.Error("Git checkout new branch went wrong");
      }

    }
  }
}
