using Serilog;

namespace MagpyServerLinux
{
  public class App
  {
    public static async Task Start()
    {
      UpdateManager.Init();
      Log.Debug("Updating setup finished.");

      UpdateManager.SetupPeriodicUpdate();

      AppDomain.CurrentDomain.ProcessExit += Program_Exited;
      SignalWatcher.SetupSignalWatchers();

      Log.Debug("Looking for node executable.");

      bool nodefound = NodeManager.VerifyNodeExe();

      if (!nodefound)
      {
        throw new Exception("Node executable not found.");
      }

      Log.Debug("Node executable found.");
      Log.Debug("Staring node server.");

      NodeManager.StartNodeServer();

      Log.Debug("Node server started.");

      await Task.Delay(500);

      ServerManager.OpenWebInterface();

      while (true)
      {
        await Task.Delay(100000);
      }
    }

    private static void Program_Exited(object? sender, EventArgs e)
    {
      Log.Debug("Program closing, Killing node server.");
      NodeManager.KillNodeServer();
      InstanceManager.ReleaseInstance();
    }
  }

}
