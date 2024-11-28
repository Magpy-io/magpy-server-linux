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
    }
}
