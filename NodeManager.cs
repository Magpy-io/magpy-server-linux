using Serilog;
using System.Diagnostics;
using static MagpyServerLinux.PathManager;

namespace MagpyServerLinux
{
    class NodeManager
    {
        static public Process? child;

        public static string NodePath
        {
            get
            {
                return RelativeExeToAbsolute("./redis/node");
            }
        }

        public static string JsEntryFilePath
        {
            get
            {
                string pathWithPotentialSpaces = RelativeExeToAbsolute("./bundle/js/bundle.js");

                // Escaping spaces if in path
                return $"\"{pathWithPotentialSpaces}\"";
            }
        }

        public static bool VerifyNodeExe()
        {
            return File.Exists(RelativeExeToAbsolute(NodePath));
        }

        public static void StartNodeServer()
        {
            child = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = NodePath,
                    Arguments = JsEntryFilePath,
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true,
                }
            };
            child.Start();
            child.EnableRaisingEvents = true;
            child.Exited += Child_Exited;

            child.BeginOutputReadLine();
            child.OutputDataReceived += Child_OutputDataReceived;

            child.BeginErrorReadLine();
            child.ErrorDataReceived += Child_OutputErrorReceived;
        }

        public static void KillNodeServer()
        {
            if (child != null && !child.HasExited)
            {
                child.Kill();
            }
        }

        public static void SendData(string data)
        {
            if (child != null)
            {
                child.StandardInput.WriteLine(data);
            }
        }

        private static void Child_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                LoggingManager.LoggerNode?.Debug(e.Data);
            }
        }

        private static void Child_OutputErrorReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                LoggingManager.LoggerNode?.Error(e.Data);
            }
        }

        private static void Child_Exited(object? sender, EventArgs e)
        {
            Log.Debug("Server node exited. Closing app.");
            Console.WriteLine("Node server stopped. Closing app.");
            Environment.Exit(0);
        }
    }
}
