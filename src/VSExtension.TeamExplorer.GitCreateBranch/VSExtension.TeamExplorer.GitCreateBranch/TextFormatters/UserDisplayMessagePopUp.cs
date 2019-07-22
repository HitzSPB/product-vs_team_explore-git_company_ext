using System.Windows.Forms;

namespace VSExtension.TeamExplorer.GitCreateBranch.TextFormatters
{
  public static class UserDisplayMessagePopUp
  {
    public static void DisplayMessage(string message)
    {
      MessageBox.Show(message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    public static void DisplayWarning(string message)
    {
      MessageBox.Show(message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    public static void DisplayError(string message)
    {
      MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
  }
}
