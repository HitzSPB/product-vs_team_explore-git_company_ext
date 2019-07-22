namespace VSExtension.TeamExplorer.GitPrune
{
  public static class Facade
  {
    public static ILog Logger { get; private set; }

    //Used to run DI
    public static void SetupDependencies(ILog log = null)
    {
      Logger = log ?? new OutputWindowLog();
    }
  }
}
