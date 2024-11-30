using MagpyServerLinux.Commands;
using MagpyServerLinux.Commands.Commands;
using Serilog;

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
                UpdateManager.Init();
                Log.Debug("Updating setup finished.");

                ICommandExecutor command = ParseArgs(args);

                await command.Run();
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
            }
            finally
            {
                InstanceManager.ReleaseInstance();
            }
        }

        private static ICommandExecutor ParseArgs(string[] args)
        {
            ArgsHandler argsHandler = new ArgsHandler(args);

            Action action = argsHandler.GetAction();
            bool isLaunchSilent = argsHandler.IsLaunchSilent();

#if DEBUG
            isLaunchSilent = true;
#endif

            ICommandExecutor command;

            switch (action)
            {
                case Action.START:
                    command = new StartCommand(isLaunchSilent);
                    break;
                case Action.LAUNCH_WEBUI:
                    command = new LaunchWebUICommand();
                    break;
                case Action.STATUS:
                    command = new GetStatusCommand();
                    break;
                case Action.STOP:
                    command = new StopCommand();
                    break;
                case Action.DISPLAY_VERSION:
                    command = new VersionCommand();
                    break;
                case Action.UPDATE:
                    command = new UpdateCommand();
                    break;
                case Action.CLEAR_SERVER_DATA:
                    command = new ClearServerDataCommand();
                    break;
                case Action.CLEAR_ALL_DATA:
                    command = new ClearAllDataCommand();
                    break;
                case Action.ENABLE_DESKTOP_AUTOSTART:
                    command = new EnableAutostartCommand();
                    break;
                case Action.DISABLE_DESKTOP_AUTOSTART:
                    command = new DisableAutostartCommand();
                    break;
                default:
                    command = new UndefinedCommand();
                    break;
            }

            return command;
        }
    }
}
