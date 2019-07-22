using System.Text;

namespace VSExtension.TeamExplorer.GitCreateBranch.TextFormatters
{
  public static class InputValidator
  {
    // Input Validator
    public static (string Output, bool IsValid) InputIsValid(string selectedTeam, string selectedWorkID, string branchName)
    {
      bool inputValid = true;
      var errorOutput = new StringBuilder();

      if (string.IsNullOrWhiteSpace(selectedTeam))
      {
        errorOutput.AppendLine("* You've not selected a team");
        inputValid = false;
      }

      if (branchName.Length > 150)
      {
        errorOutput.AppendLine("* Selected Branch name is too long");
        inputValid = false;
      }
      if (selectedWorkID.Length > 10)
      {
        inputValid = false;
        errorOutput.AppendLine("The selected workID is larger than INT_MAX");
      }

      if (inputValid)
      {
        return ("All your input have been validated", true);
      }
      return (errorOutput.ToString(), false);
    }
  }
}
