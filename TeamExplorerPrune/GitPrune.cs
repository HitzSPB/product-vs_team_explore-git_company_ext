using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;
using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Resources;
using System.Windows.Forms;
using Microsoft.TeamFoundation.Controls;
using Microsoft.VisualStudio.Shell;

namespace TeamExplorerPrune
{
    [TeamExplorerNavigationItem(TeamExtensionGit.PackageGuidString, 315)]

    public class GitPrune : ITeamExplorerNavigationItem2
    {

        readonly ExecuteCommand _command = new ExecuteCommand();
        OutputWindow output = new OutputWindow();
        private string _executeStringCommand;
        private string _path;

        private bool _isVisible = true;
        public string Text => "GitPrune";

        //fix
        public Image Image => Properties.Resource.prunegit.ToBitmap();
        public bool IsEnabled => true;
        public int ArgbColor => Color.Aqua.ToArgb();
        public object Icon => Properties.Resource.prunegit.ToBitmap();

        [ImportingConstructor]
        public GitPrune()
        {
            Invalidate();
        }



        public bool IsVisible
        {
            get => this._isVisible;
            set => RegisterPropertyChange(ref _isVisible, value);
        }

        //Executes out button
        public void Execute()
        {
            _path = GetActiveRepo()?.RepositoryPath;
            _executeStringCommand = "/c cd " + _path + "&git remote prune origin --dry-run";
            if (!string.IsNullOrWhiteSpace(_path))
            {
                MessageBox.Show(_command.Execute(_path));
            }
            else
            {
                MessageBox.Show("Did not execute Command:: " + _executeStringCommand);
            }
            LogCalledMethod();
        }

        //Ensures that out Team Explorer button is only visible when we have an active Git Repo.
        public void Invalidate()
        {
            IsVisible = !String.IsNullOrWhiteSpace(GetActiveRepo()?.RepositoryPath);
            output.WriteText("Current Button Status is ::" + IsVisible);
            LogCalledMethod();
        }

        public void Dispose() { }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RegisterPropertyChange<T>(ref T backingField, T newValue, [CallerMemberName]string propertyName = "") where T : IEquatable<T>
        {
            if (EqualityComparer<T>.Default.Equals(backingField, newValue))
            {
                return;
            }
            backingField = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            LogCalledMethod();
        }

        //Get active GitRepo
        private IGitRepositoryInfo GetActiveRepo()
        {
            LogCalledMethod();
            return GetGit()?.ActiveRepositories?.FirstOrDefault();
        }

        //Get IGitExt Information - Contains all Information needed
        private IGitExt GetGit()
        {
            LogCalledMethod();
            return (IGitExt)ServiceProvider.GlobalProvider.GetService(typeof(IGitExt));
        }

        //Writes to Output Window foreach action
        private void LogCalledMethod([CallerMemberName] string method = null)
        {
            output.WriteText($"{method} called");
        }

    }
}