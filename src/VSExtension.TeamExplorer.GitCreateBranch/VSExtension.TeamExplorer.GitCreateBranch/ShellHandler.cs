using System.Diagnostics;

namespace VSExtension.TeamExplorer.GitCreateBranch
{
  public class ShellHandler
  {
    public ShellHandler()
    {
    }
    public bool ExecuteShellCommand(string path, string command)
    {
      var CommandProcess = new Process
      {
        StartInfo = new ProcessStartInfo
        {
          FileName = "cmd.exe",
          RedirectStandardOutput = false,
          RedirectStandardInput = true,
          UseShellExecute = false,
          WindowStyle = ProcessWindowStyle.Hidden,
          CreateNoWindow = true,
          WorkingDirectory = path
        }
      };

      CommandProcess.Start();
      CommandProcess.StandardInput.WriteLine(command);
      CommandProcess.StandardInput.WriteLine("exit");
      CommandProcess.WaitForExit();

      if (CommandProcess.ExitCode == 128 || CommandProcess.ExitCode == 1)
      {
        Facade.Logger.Error("Excute Command procces with parameter: " + command + "Exitted with exit code " + CommandProcess.ExitCode);
        return false;
      }
      Facade.Logger.Verbose("Excute Command procces with parameter: " + command + "Exitted with exit code " + CommandProcess.ExitCode);
      return true;
    }
  }
}
