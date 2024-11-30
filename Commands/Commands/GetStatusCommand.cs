using System;

namespace MagpyServerLinux.Commands;

public class GetStatusCommand : ICommandExecutor
{
  public Task Run()
  {
    if (InstanceManager.IsInstanceRunning())
    {
      Console.WriteLine("App is running.");
    }
    else
    {
      Console.WriteLine("App is stopped.");
    }

    return Task.CompletedTask;
  }
}
