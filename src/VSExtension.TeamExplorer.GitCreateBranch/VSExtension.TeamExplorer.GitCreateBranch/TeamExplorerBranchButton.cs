using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.TeamFoundation.Controls;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;

namespace VSExtension.TeamExplorer.GitCreateBranch
{
  [TeamExplorerNavigationItem(BranchExtensionPackage.PackageGuidString, 100)]
  internal class TeamExplorerBranchButton : ITeamExplorerNavigationItem2
  {

    private bool _isVisible = true;
    string ITeamExplorerNavigationItem.Text => "Git Create Branch";
    Image ITeamExplorerNavigationItem.Image => Properties.Resource.branch.ToBitmap();
    bool ITeamExplorerNavigationItem2.IsEnabled => true;
    int ITeamExplorerNavigationItem2.ArgbColor => Color.Aqua.ToArgb();
    object ITeamExplorerNavigationItem2.Icon => Properties.Resource.branch.ToBitmap();

    [ImportingConstructor]
    public TeamExplorerBranchButton([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
    {
      ServiceProvider = serviceProvider;
      //Creates all our Dependency Injections
      Facade.SetupDependencies();
      ((ITeamExplorerNavigationItem)this).Invalidate();
    }

    private IServiceProvider ServiceProvider { get; set; }
    public T GetService<T>()
    {
      if (ServiceProvider != null)
      {
        return (T)ServiceProvider.GetService(typeof(T));
      }
      return default(T);
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
      ITeamExplorer service = GetService<ITeamExplorer>();
      if (service == null)
      {
        return;
      }
      service.NavigateToPage(new Guid(BranchExtensionPackage.CreateBranchGuidString), null);
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

    public static IGitRepositoryInfo GetActiveRepo()
    {
      Facade.Logger.Verbose("Method hit");
      return GetGit()?.ActiveRepositories?.FirstOrDefault();
    }

    // Get GitExt which holds information about active git repos.
    public static IGitExt GetGit()
    {
      Facade.Logger.Verbose("Method hit");
      return (IGitExt)Microsoft.VisualStudio.Shell.ServiceProvider.GlobalProvider.GetService(typeof(IGitExt));
    }
  }
}
