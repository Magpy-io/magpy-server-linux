using System;

namespace MagpyServerLinux.Commands;

public class StopCommand : ICommandExecutor
{
  public Task Run()
  {
    Console.WriteLine("stopping running instance.");
    App.StopRunningInstance();
    return Task.CompletedTask;
  }
}
