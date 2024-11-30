using System;

namespace MagpyServerLinux.Commands;

public class UpdateCommand : ICommandExecutor
{
  public async Task Run()
  {
    if (!UpdateManager.IsAppInstalled())
    {
      Console.WriteLine("App is not installed");
      return;
    }
    if (InstanceManager.IsInstanceRunning())
    {
      Console.WriteLine("App is running. Stop app before updating server.");
      return;
    }
    Console.WriteLine("Checking for updates");
    await UpdateManager.UpdateMyAppAndExit();
  }
}
