using System.Diagnostics;
using Serilog;

namespace MagpyServerLinux
{
    public class ServerManager
    {
        public static void OpenWebInterface()
        {
            Process process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "xdg-open",
                    Arguments = Constants.serverUrl,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            process.OutputDataReceived += OutputDataReceived;
            process.ErrorDataReceived += OutputDataReceived;
        }

        private static void OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                Log.Debug("xdg-open: " + e.Data);
            }
        }

    }
}
