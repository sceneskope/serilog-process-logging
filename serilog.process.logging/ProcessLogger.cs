using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace Serilog.Process.Logging
{
    public class ProcessLogger
    {
        public static ProcessLogger StartNew(ILogger log, CancellationToken ct)
        {
            var logger = new ProcessLogger(log);
            logger.RunAsync(ct);
            return logger;
        }

        public ILogger Log { get; }
        public TimeSpan Delay { get; set; } = TimeSpan.FromMinutes(1);

        private readonly System.Diagnostics.Process _process;
        private Task _runner;

        public ProcessLogger(ILogger logger)
        {
            _process = System.Diagnostics.Process.GetCurrentProcess();
            Log = logger.ForContext("Process", _process.Id);
        }

        public Task RunAsync(CancellationToken ct)
        {
            _runner = PeriodicDumperAsync(ct);
            return _runner;
        }

        private async Task PeriodicDumperAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    Dump();
                }
                catch (Exception ex) when (!(ex is OperationCanceledException))
                {
                    Log.Warning(ex, "Failed to dump: {exception}", ex.Message);
                }
                await Task.Delay(Delay, ct).ConfigureAwait(false);
            }
        }

        private ProcessInformation _latestInformation;

        private void Dump()
        {
            var information = new ProcessInformation(_process);
            var logger = Log.ForContext("Information", information, true);

            TimeSpan deltaProcessorTime;

            if (_latestInformation != null)
            {
                var delta = new ProcessInformation(_latestInformation, information);
                logger = logger.ForContext("Delta", delta, true);
                deltaProcessorTime = delta.TotalProcessorTime;
            }
            else
            {
                deltaProcessorTime = information.TotalProcessorTime;
            }

            _latestInformation = information;
            logger.Information("Process @ {ram} ({working}) used {processor} seconds", information.TotalMemory, information.WorkingSet, deltaProcessorTime.TotalSeconds);
        }
    }
}
