using System;

namespace MagpyServerLinux.Commands;

public class UndefinedCommand : ICommandExecutor
{
  public Task Run()
  {
    Console.WriteLine("Wrong parameter.");
    return Task.CompletedTask;
  }
}
