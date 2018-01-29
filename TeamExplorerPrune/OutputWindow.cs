using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamExplorerPrune
{
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;

    public class OutputWindow
    {
        readonly IVsOutputWindow _outWindow = Package.GetGlobalService(typeof(SVsOutputWindow)) as IVsOutputWindow;
        private readonly IVsOutputWindowPane _customPane;
        private string OutputWindowTitle { get; } = "UNIK - Extension Output";

        //GUID ID til window
        readonly Guid _customGuid = new Guid("0F44E2D1-F5FA-4d2d-AB30-22BE8ECD9789");
        public OutputWindow()
        {
            // Creating new Output Window
            _outWindow.CreatePane(ref _customGuid, OutputWindowTitle, 1, 1);
            _outWindow.GetPane(ref _customGuid, out _customPane);

        }

        //Each call adds a new text line to Output Window
        public void WriteText(string outputDescription)
        {
            _customPane.OutputString(outputDescription + Environment.NewLine);
            _customPane.Activate();
        }
    }
}
