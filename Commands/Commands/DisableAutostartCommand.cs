using System;

namespace MagpyServerLinux.Commands;

public class DisableAutostartCommand : ICommandExecutor
{
  public Task Run()
  {
    if (!UpdateManager.IsAppInstalled())
    {
      Console.WriteLine("App is not installed");
    }
    {
      AutostartupManager.DisableDesktopAutoStart();
      Console.WriteLine("Autostart disabled.");
    }
    return Task.CompletedTask;
  }
}
