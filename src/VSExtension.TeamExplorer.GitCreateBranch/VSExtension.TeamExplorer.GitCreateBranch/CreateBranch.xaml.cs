using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.VisualStudio.PlatformUI;
using VSExtension.TeamExplorer.GitCreateBranch.TFS;

namespace VSExtension.TeamExplorer.GitCreateBranch
{
  /// <summary>
  /// Interaction logic for CreateBranch.xaml
  /// </summary> 
  public partial class CreateBranch : UserControl
  {
    string selectedTeam = "Default - Teams";
    private readonly GitBranchHandler BranchHandler;
    private readonly TfsInformationHandler TFSDataHandler;
    public CreateBranch()
    {
      InitializeComponent();
      TFSDataHandler = new TfsInformationHandler();
      TeamComboBox.ItemsSource = TFSDataHandler.GetTeamList(selectedTeam);
      HeaderName.Foreground = GetForeGroundColorVSTheme();
      HeaderName.Foreground = GetBackgroundColorVSTheme();
      BranchHandler = new GitBranchHandler();
    }

    private SolidColorBrush GetForeGroundColorVSTheme()
    {
      System.Drawing.Color defaultForeground = VSColorTheme.GetThemedColor(EnvironmentColors.ToolWindowTextColorKey);
      return new SolidColorBrush(Color.FromArgb(defaultForeground.A, defaultForeground.R, defaultForeground.G, defaultForeground.B));
    }

    private SolidColorBrush GetBackgroundColorVSTheme()
    {
      System.Drawing.Color defaultBackground = VSColorTheme.GetThemedColor(EnvironmentColors.ToolWindowBackgroundColorKey);
      return new SolidColorBrush(Color.FromArgb(defaultBackground.A, defaultBackground.R, defaultBackground.G, defaultBackground.B));
    }

    private void CreateBranchButton(object sender, RoutedEventArgs e)
    {
      BranchHandler.CreateBranch(TeamComboBox.Text, TFSWorkIdTextBox.Text, BranchNameTextBox.Text, CreationSelectionComboBox.Text, DeveloperVersionNumberComboBox.Text);
    }

    private void ClearAllTextBoxButton(object sender, RoutedEventArgs e)
    {
      TeamComboBox.SelectedIndex = -1;
      DeveloperVersionNumberComboBox.SelectedIndex = -1;
      TFSWorkIdTextBox.Clear();
      BranchNameTextBox.Clear();
    }

    private void BranchNameTextBox_TextInput(object sender, TextCompositionEventArgs e)
    {
      BranchNameTextBox.Text = "";
    }
  }
}

