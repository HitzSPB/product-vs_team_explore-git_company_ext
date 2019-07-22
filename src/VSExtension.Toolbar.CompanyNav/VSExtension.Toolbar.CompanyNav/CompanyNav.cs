using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace VSExtension.Toolbar.CompanyNav
{
  /// <summary>
  /// Command handler
  /// </summary>
  internal sealed class CompanyNav
  {
    /// <summary>
    /// Command ID.
    /// </summary>
    public const int link1 = 0x0100;
    public const int link2 = 0x0101;
    public const int link3 = 0x0102;
    public const int link4 = 0x0103;
    public const int link5 = 0x0104;
    public const int link6 = 0x0105;
    public const int link7 = 0x0106;


    /// <summary>
    /// Command menu group (command set GUID).
    /// </summary>
    public static readonly Guid CommandSet = new Guid("53ca7a15-26cd-4f1a-853d-3cfa0d5654c5");

    /// <summary>
    /// VS Package that provides this command, not null.
    /// </summary>
    private readonly AsyncPackage package;

    /// <summary>
    /// Initializes a new instance of the <see cref="CompanyNav"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    /// <param name="commandService">Command service to add command to, not null.</param>
    private CompanyNav(AsyncPackage package, OleMenuCommandService commandService)
    {
      this.package = package ?? throw new ArgumentNullException(nameof(package));
      commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

      commandService.AddCommand(AddCommandService(CommandSet, KodeStandard));
      commandService.AddCommand(AddCommandService(CommandSet, GitHaanbogen));
      commandService.AddCommand(AddCommandService(CommandSet, BranchingStruktur));
      commandService.AddCommand(AddCommandService(CommandSet, CommandId3));
      commandService.AddCommand(AddCommandService(CommandSet, CommandId4));
      commandService.AddCommand(AddCommandService(CommandSet, CommandId5));
      commandService.AddCommand(AddCommandService(CommandSet, SonarQube));
    }

    private MenuCommand AddCommandService(Guid commandSet, int commandID)
    {
      var menuCommandID = new CommandID(commandSet, commandID);
      return new MenuCommand(Execute, menuCommandID);
    }

    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static CompanyNav Instance
    {
      get;
      private set;
    }

    /// <summary>
    /// Gets the service provider from the owner package.
    /// </summary>
    private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider => package;

    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(AsyncPackage package)
    {
      // Switch to the main thread - the call to AddCommand in CompanyNav's constructor requires
      // the UI thread.
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

      var commandService = await package.GetServiceAsync((typeof(IMenuCommandService))) as OleMenuCommandService;
      Instance = new CompanyNav(package, commandService);
    }

    /// <summary>
    /// This function is the callback used to execute the command when the menu item is clicked.
    /// See the constructor to see how the menu item is associated with this function using
    /// OleMenuCommandService service and MenuCommand class.
    /// </summary>
    /// <param name="sender">Event sender.</param>
    /// <param name="e">Event args.</param>
    private void Execute(object sender, EventArgs e)
    {
      int commandID = ((MenuCommand)sender).CommandID.ID;

      switch (commandID)
      {
        //Kodestandard
        case 256:
          System.Diagnostics.Process.Start("http://link");
          break;
        //Git Håndbogen
        case 257:
          System.Diagnostics.Process.Start("http://link");
          break;
        //Git Branching
        case 258:
          System.Diagnostics.Process.Start("http://link");
          break;
        // UI UX håndbogen
        case 259:
          System.Diagnostics.Process.Start("http://link");
          break;
        // Intranet
        case 260:
          System.Diagnostics.Process.Start("http://link");
          break;
        // IT Support
        case 261:
          System.Diagnostics.Process.Start("http://link");
          break;
        // SonarQube
        case 262:
          System.Diagnostics.Process.Start("http://link");
          break;
        // Git Håndbogen, if error happens
        default:
          System.Diagnostics.Process.Start("http://link");
          break;
      }
    }
  }
}
