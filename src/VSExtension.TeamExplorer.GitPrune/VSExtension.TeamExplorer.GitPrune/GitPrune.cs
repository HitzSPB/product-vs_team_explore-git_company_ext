using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.TeamFoundation.Controls;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;


namespace VSExtension.TeamExplorer.GitPrune
{
  [TeamExplorerNavigationItem(GitPrunePackage.PackageGuidString, 315)]
  public class GitPrune : ITeamExplorerNavigationItem2
  {
    private bool _isVisible = true;
    string ITeamExplorerNavigationItem.Text => "Delete merged branches";
    Image ITeamExplorerNavigationItem.Image => Properties.Resource.prunegit.ToBitmap();
    bool ITeamExplorerNavigationItem2.IsEnabled => true;
    int ITeamExplorerNavigationItem2.ArgbColor => Color.Aqua.ToArgb();
    object ITeamExplorerNavigationItem2.Icon => Properties.Resource.prunegit.ToBitmap();

    [ImportingConstructor]
    public GitPrune()
    {
      //Creates all our Dependency Injections
      Facade.SetupDependencies();
      ((ITeamExplorerNavigationItem)this).Invalidate();
    }

    public bool IsVisible
    {
      get => _isVisible;
      set => RegisterPropertyChange(ref _isVisible, value);
    }

    /// <summary>
    ///Execute script for our TeamExplorer button 
    /// </summary>
    void ITeamExplorerNavigationItem.Execute()
    {
      string path = GetActiveRepo()?.RepositoryPath;
      Facade.Logger.Verbose($"method hit - path for Repository = {path}");
      try
      {
        if (!string.IsNullOrWhiteSpace(path))
        {
          (string message, bool success) = ExecuteShellCommand(path, removeMergedBranchesCommand);
          if (success)
          {
            DisplayMessage("Git prune succeeded");
            Facade.Logger.Information(message);
          }
          else
          {
            DisplayWarning("Nothing to prune");
            Facade.Logger.Information(message);
          }
        }
        else
        {
          DisplayWarning("No active git repo could be found. Disabling the feature until a git repo is loaded");
          Facade.Logger.Error("No path was found on method call");
          ((ITeamExplorerNavigationItem)this).Invalidate();
        }
      }
      catch (Exception e)
      {
        Facade.Logger.Error("An error have thrown the following exception:", e);
        DisplayError("An error has occured, check the log window for further info");

      }
    }

    /// <summary>
    ///Ensures that our Team Explorer button is only visible when we have an active Git Repo.
    /// </summary>
    void ITeamExplorerNavigationItem.Invalidate()
    {
      IsVisible = !string.IsNullOrWhiteSpace(GetActiveRepo()?.RepositoryPath);
      Facade.Logger.Verbose($"method hit - IsVisible: {IsVisible}");
    }

    void IDisposable.Dispose()
    {
      // Nothing to dispose…
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void RegisterPropertyChange<T>(ref T backingField, T newValue, [CallerMemberName]string propertyName = "") where T : IEquatable<T>
    {
      if (EqualityComparer<T>.Default.Equals(backingField, newValue))
      {
        return;
      }
      T currentBackingField = backingField;
      backingField = newValue;
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
      Facade.Logger.Verbose($"Change event was fired for {propertyName}. Value = {currentBackingField} was changed to {newValue}");
    }

    private IGitRepositoryInfo GetActiveRepo()
    {
      Facade.Logger.Verbose("Method hit");
      return GetGit()?.ActiveRepositories?.FirstOrDefault();
    }

    // Get GitExt which holds information about active git repos.
    private IGitExt GetGit()
    {
      Facade.Logger.Verbose("Method hit");
      return (IGitExt)ServiceProvider.GlobalProvider.GetService(typeof(IGitExt));
    }


    private void DisplayMessage(string message)
    {
      MessageBox.Show(message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void DisplayWarning(string message)
    {
      MessageBox.Show(message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    private void DisplayError(string message)
    {
      MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    /// <summary>
    /// Used to execute git commands in a hidden shell
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public (string message, bool success) ExecuteShellCommand(string path, string command)
    {
      Facade.Logger.Information($"Following path have been found {path} and follow command is being executed {command}");
      var CommandProcess = new Process
      {
        StartInfo = new ProcessStartInfo
        {
          FileName = "powershell.exe",
          RedirectStandardOutput = true,
          RedirectStandardInput = true,
          UseShellExecute = false,
          WindowStyle = ProcessWindowStyle.Normal,
          CreateNoWindow = false,
          WorkingDirectory = path
        }
      };
      CommandProcess.Start();
      CommandProcess.StandardInput.WriteLine(command);
      CommandProcess.StandardInput.WriteLine("exit");
      CommandProcess.CloseMainWindow();
      CommandProcess.WaitForExit();

      if (CommandProcess.ExitCode == 0)
        return ("You've executed the prune command. Exit Code: " + CommandProcess.ExitCode, true);
      else
        return ("Something went wrong, contact an administrator. Exit Code: " + CommandProcess.ExitCode, false);
    }
    private const string removeMergedBranchesCommand = @"
git fetch --prune
$BranchesToDelete = git branch --merged 

foreach($item in $BranchesToDelete)
{
  if($item -notmatch ""^\s*(master|release\\.+|\*.+)\s*$"")
  {
    git branch -d $item.Trim()    
  }
}
";
  }
}
