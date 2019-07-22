using System;
using System.ComponentModel;
using Microsoft.TeamFoundation.Controls;


namespace VSExtension.TeamExplorer.GitCreateBranch
{
  [TeamExplorerPage(BranchExtensionPackage.CreateBranchGuidString)]
  internal class TeamExplorerCreateBranchPage : ITeamExplorerPage
  {
    private IServiceProvider serviceProvider;

    private bool isBusy;

    public void Cancel()
    {
    }

    public object GetExtensibilityService(Type serviceType)
    {
      return null;
    }

    public void Initialize(object sender, PageInitializeEventArgs e)
    {
      serviceProvider = e.ServiceProvider;
    }

    public bool IsBusy
    {
      get => isBusy;
      private set
      {
        isBusy = value;
        FirePropertyChanged("IsBusy");
      }
    }

    public void Loaded(object sender, PageLoadedEventArgs e)
    {
    }

    public object PageContent => new CreateBranch();

    public void Refresh()
    {
    }

    public void SaveContext(object sender, PageSaveContextEventArgs e)
    {
    }

    public string Title => " Create Branch";

    public void Dispose()
    {
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void FirePropertyChanged(string propertyName)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}