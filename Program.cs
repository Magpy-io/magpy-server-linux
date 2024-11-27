using Serilog;

namespace MagpyServerLinux
{
    public class Program
    {
        static async Task MainInner(string[] args)
        {
            UpdateManager.Init();

            Log.Debug("Updating setup finished.");

            UpdateManager.SetupPeriodicUpdate();

            Log.Debug("Looking for node executable.");

            bool nodefound = NodeManager.VerifyNodeExe();

            if (!nodefound)
            {
                throw new Exception("Node executable not found.");
            }

            Log.Debug("Node executable found.");
            Log.Debug("Staring node server.");

            NodeManager.StartNodeServer();

            Log.Debug("Node server started.");

            while (true)
            {
                await Task.Delay(100000);
            }
        }

        static async Task Main(string[] args)
        {
#if DEBUG
            LoggingManager.InitEarly();
#endif
            AppDomain.CurrentDomain.ProcessExit += Program_Exited;
            SignalWatcher.SetupSignalWatchers();

            try
            {
                bool instanceCreated = InstanceManager.HoldInstance();
                if (instanceCreated)
                {
                    LoggingManager.Init();
                    Log.Debug("Logging initialized.");

                    await MainInner(args);
                }
                else
                {
                    Console.WriteLine("Failed to run Magpy. An instance of the app is already running.");
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "Error launching app, checking for updates...");

                try
                {
                    await UpdateManager.UpdateMyApp();
                }
                catch (Exception ex)
                {
                    Log.Debug(ex, "Error while trying to update server.");
                }
#if DEBUG
                Console.ReadKey();
#endif
            }
            finally
            {
                InstanceManager.ReleaseInstance();
            }
        }

        private static void Program_Exited(object? sender, EventArgs e)
        {
            Log.Debug("Program closing, Killing node server.");
            NodeManager.KillNodeServer();
            InstanceManager.ReleaseInstance();
        }
    }
}
