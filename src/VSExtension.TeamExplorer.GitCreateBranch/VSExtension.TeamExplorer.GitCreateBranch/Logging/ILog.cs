using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace VSExtension.TeamExplorer.GitCreateBranch
{
  public interface ILog
  {
    void Verbose(object message, [CallerMemberName] string method = null);
    void Information(object message, [CallerMemberName] string method = null);
    void Warn(object outputMessage, [CallerMemberName] string method = null);
    void Error(object outputMessage, [CallerMemberName] string method = null);
    void Error(object outputMessage, Exception exception, [CallerMemberName] string method = null);
  }
}
