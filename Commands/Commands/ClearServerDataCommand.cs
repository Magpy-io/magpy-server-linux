using System;

namespace MagpyServerLinux.Commands;

public class ClearServerDataCommand : ICommandExecutor
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

    Console.WriteLine("Clearing magpy server data.");
    string[] deleted = PathManager.ClearServerDataFolder();
    Array.ForEach(deleted, item => Console.WriteLine($"Deleted folder {item}"));
    Console.WriteLine("Data cleared.");

    return Task.CompletedTask;
  }
}
