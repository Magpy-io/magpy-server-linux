using static MagpyServerLinux.Constants;

namespace MagpyServerLinux.Commands;

public class VersionCommand : ICommandExecutor
{
  public Task Run()
  {
    Console.WriteLine(AppName + " v" + version);
    return Task.CompletedTask;
  }
}
