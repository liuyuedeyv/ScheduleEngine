using FD.Simple.Utils.Logging;
using Microsoft.Extensions.Logging;
using System;


namespace FD.Simple.DB
{
    public class SqlExcuteLogHelper : IDisposable
    {
        IFDLogger _logger = null;
        System.Diagnostics.Stopwatch sw = null;
        public SqlExcuteLogHelper(IFDLogger logger)
        {
            _logger = logger;
            sw = new System.Diagnostics.Stopwatch();
            sw.Start();
        }

        public void Write(string msg, System.Exception exception = null)
        {
            if(this._logger == null)
            {
                return;
            }
            sw.Stop();
            var secodes = sw.ElapsedMilliseconds;

            _logger.LogInformation(msg);
            if (exception != null)
            {
                _logger.LogError(msg, exception);
            }
            else if (secodes > 100)
            {
                _logger.LogWarning(msg);
            }

        }
        public void Dispose()
        {
            sw = null;
        }
    }
}
