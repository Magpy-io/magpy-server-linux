using Serilog;
using static MagpyServerLinux.Constants;

namespace MagpyServerLinux
{
    public class Program
    {
        static async Task Main(string[] args)
        {
#if DEBUG
            LoggingManager.InitEarly();
#endif
            UpdateManager.Init();
            Log.Debug("Updating setup finished.");

            try
            {
                ArgsHandler argsHandler = new ArgsHandler(args);

                Action action = argsHandler.GetAction();
                bool isLaunchSilent = argsHandler.IsLaunchSilent();

#if DEBUG
                isLaunchSilent = true;
#endif

                switch (action)
                {
                    case Action.NONE:
                        Console.WriteLine("Wrong parameter.");
                        return;
                    case Action.LAUNCH_WEBUI:
                        if (!InstanceManager.IsInstanceRunning())
                        {
                            Console.WriteLine("App is stopped.");
                        }
                        else
                        {
                            Console.WriteLine("Launching web browser.");
                            ServerManager.OpenWebInterface();
                        }
                        return;
                    case Action.STATUS:
                        if (InstanceManager.IsInstanceRunning())
                        {
                            Console.WriteLine("App is running.");
                        }
                        else
                        {
                            Console.WriteLine("App is stopped.");
                        }
                        return;
                    case Action.STOP:
                        Console.WriteLine("stopping running instance.");
                        App.StopRunningInstance();
                        return;
                    case Action.DISPLAY_VERSION:
                        Console.WriteLine(AppName + " v" + version);
                        return;
                    case Action.UPDATE:
                        if (!UpdateManager.IsAppInstalled())
                        {
                            Console.WriteLine("App is not installed");
                            return;
                        }
                        if (InstanceManager.IsInstanceRunning())
                        {
                            Console.WriteLine("App is running. Stopping app...");
                            App.StopRunningInstance();
                        }
                        Console.WriteLine("Checking for updates");
                        await UpdateManager.UpdateMyAppAndExit();
                        return;
                    case Action.CLEAR_DATA:
                        if (InstanceManager.IsInstanceRunning())
                        {
                            Console.WriteLine("App is running. Stop app before clearing data.");
                        }
                        else
                        {
                            Console.WriteLine("Clearing magpy data.");
                            PathManager.ClearAppDataFolder();
                            Console.WriteLine("Data cleared.");
                        }
                        return;
                    case Action.ENABLE_DESKTOP_AUTOSTART:
                        if (!UpdateManager.IsAppInstalled())
                        {
                            Console.WriteLine("App is not installed");
                            return;
                        }
                        string autostartFilePath = AutostartupManager.EnableDesktopAutoStart();
                        Console.WriteLine($"Autostart enabled. Created file {autostartFilePath}");
                        return;
                    case Action.DISABLE_DESKTOP_AUTOSTART:
                        if (!UpdateManager.IsAppInstalled())
                        {
                            Console.WriteLine("App is not installed");
                            return;
                        }
                        AutostartupManager.DisableDesktopAutoStart();
                        Console.WriteLine("Autostart disabled.");
                        return;

                }

                bool instanceCreated = InstanceManager.HoldInstance();
                if (instanceCreated)
                {
                    LoggingManager.Init();
                    Log.Debug("Logging initialized.");

                    await App.Start(!isLaunchSilent);
                }
                else
                {
                    Console.WriteLine("Failed to run Magpy. An instance of the app is already running.");
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "Error launching app, checking for updates...");

                Console.WriteLine("Error running app.");
                Console.WriteLine(e.Message);

                try
                {
                    await UpdateManager.UpdateMyAppAndExit();
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
    }
}
