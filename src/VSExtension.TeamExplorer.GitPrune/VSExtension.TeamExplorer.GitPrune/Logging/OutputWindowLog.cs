using System;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace VSExtension.TeamExplorer.GitPrune
{
  internal class OutputWindowLog : ILog
  {
    private static Guid _customGuid = new Guid("0F44E2D1-F5FA-4d2d-AB30-22BE8ECD9789");
    private const string OutputWindowTitle = "TFS - Prune Extension";
    private readonly LogLevel _currentLogLevel = LogLevel.Information;
    private readonly LogLevel _activateLogPaneLogLevel = LogLevel.Error;


    private static readonly Lazy<IVsOutputWindow> _outWindow = new Lazy<IVsOutputWindow>(() => Package.GetGlobalService(typeof(SVsOutputWindow)) as IVsOutputWindow);
    private static readonly Lazy<IVsOutputWindowPane> _customPane = new Lazy<IVsOutputWindowPane>(() =>
    {

        // Creating new Output Window in Visual Studio
        _outWindow.Value.CreatePane(ref _customGuid, OutputWindowTitle, 1, 1);
      _outWindow.Value.GetPane(ref _customGuid, out IVsOutputWindowPane customPane);

      return customPane;

    });

    private IVsOutputWindowPane CustomPane => _customPane.Value;


    void ILog.Verbose(object message, string method)
    {
      WriteLine("Verbose", message, method, LogLevel.Verbose);
    }

    void ILog.Information(object message, string method)
    {
      WriteLine("Information", message, method, LogLevel.Information);
    }
    void ILog.Warn(object outputMessage, string method)
    {
      WriteLine("Warning", outputMessage, method, LogLevel.Warning);
    }

    void ILog.Error(object outputMessage, string method)
    {
      WriteLine("Error", outputMessage, method, LogLevel.Error);

    }

    void ILog.Error(object outputMessage, Exception exception, string method)
    {
      WriteLineWithException("Error", outputMessage, method, exception, LogLevel.Error);
    }
    private void WriteLineWithException(string level, object message, string method, Exception e, LogLevel logLevel)
    {
      WriteLine(level, $"{message} - Exception:: {e}", method, logLevel);
    }

    private void WriteLine(string level, object message, string method, LogLevel logLevel)
    {
      if (_currentLogLevel < logLevel)
      {
        return;
      }

      CustomPane.OutputString($"{DateTime.Now} - {level}{method}:: {message}{Environment.NewLine}");

      if (_activateLogPaneLogLevel <= logLevel)
      {
        CustomPane.Activate();
      }

    }

  }

}