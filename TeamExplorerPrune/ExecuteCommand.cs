using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace TeamExplorerPrune
{
    public class ExecuteCommand
    {
        public string Execute(string gitRepoPath)
        {

            try
            {
                //Used to execute our Git Commands
                if (string.IsNullOrWhiteSpace(gitRepoPath) == false)
                {
                    Process CommandProcess = new Process();
                    CommandProcess.StartInfo.FileName = "cmd.exe";
                    CommandProcess.StartInfo.Arguments = "/c cd " + gitRepoPath + "&git remote prune origin --dry-run";
                    CommandProcess.StartInfo.RedirectStandardOutput = true;
                    CommandProcess.StartInfo.UseShellExecute = false;
                    CommandProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    CommandProcess.StartInfo.CreateNoWindow = false;
                    CommandProcess.Start();
                    CommandProcess.WaitForExit();
                    if(!string.IsNullOrEmpty(CommandProcess.StandardOutput.ReadToEnd()))
                    {
                        return "Script have been executed:: " + CommandProcess.StandardOutput.ReadToEnd();
                    }
                    else
                    {
                        return "Nothing to prune";
                    }
                    
                }
                return "You are not executing a valid command";

            }
            catch (Exception e)
            {
                MessageBox.Show("An exception have been thrown :: " + e);
                throw;
            }
        }
    }
}
