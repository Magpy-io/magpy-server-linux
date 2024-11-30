using System;

namespace MagpyServerLinux.Commands;

public class ClearAllDataCommand : ICommandExecutor
{
  public Task Run()
  {
    if (InstanceManager.IsInstanceRunning())
    {
      Console.WriteLine("App is running. Stop app before clearing data.");
      return Task.CompletedTask;
    }

    bool accepted = Utils.GetUserConfirmation("Are you sure you want to proceed?\nAll your server config and photos data will be lost. (y/n)\n(Actual photo files will not be deleted)");
    if (!accepted)
    {
      Console.WriteLine("Operation cancelled by user.");
    }

    Console.WriteLine("Clearing all magpy data.");
    string deleted = PathManager.ClearAppDataFolder();
    Console.WriteLine($"Deleted folder {deleted}");
    Console.WriteLine("Removing autostart desktop file.");
    AutostartupManager.DisableDesktopAutoStart();
    Console.WriteLine("All magpy data cleared.");
    Console.WriteLine("Autostart disabled.");

    return Task.CompletedTask;
  }
}
