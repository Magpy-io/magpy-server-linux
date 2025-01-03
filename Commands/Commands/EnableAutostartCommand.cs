using System;

namespace MagpyServerLinux.Commands;

public class EnableAutostartCommand : ICommandExecutor
{
  public Task Run()
  {
    if (!UpdateManager.IsAppInstalled())
    {
      Console.WriteLine("App is not installed");
      return Task.CompletedTask;
    }

    string autostartFilePath = AutostartupManager.EnableDesktopAutoStart();
    Console.WriteLine($"Autostart enabled. Created file {autostartFilePath}");

    return Task.CompletedTask;
  }
}
