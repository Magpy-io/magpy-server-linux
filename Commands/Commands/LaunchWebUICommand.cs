using System;

namespace MagpyServerLinux.Commands;

public class LaunchWebUICommand : ICommandExecutor
{
  public Task Run()
  {
    if (!InstanceManager.IsInstanceRunning())
    {
      Console.WriteLine("App is stopped.");
    }
    else
    {
      Console.WriteLine("Launching web browser.");
      ServerManager.OpenWebInterface();
    }
    return Task.CompletedTask;
  }
}
