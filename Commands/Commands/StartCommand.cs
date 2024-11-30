using Serilog;

namespace MagpyServerLinux.Commands.Commands;

public class StartCommand : ICommandExecutor
{
  private bool isLaunchSilent;
  public StartCommand(bool isLaunchSilent)
  {
    this.isLaunchSilent = isLaunchSilent;
  }
  public async Task Run()
  {
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
}
