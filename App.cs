using Serilog;

namespace MagpyServerLinux
{
  public class App
  {
    public static async Task Start(bool displayUI)
    {
      AppDomain.CurrentDomain.ProcessExit += Program_Exited;
      SignalManager.SetupSignalWatchers();

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
      Console.WriteLine("Server started.");

      await Task.Delay(500);

      if (displayUI)
      {
        try
        {
          ServerManager.OpenWebInterface();
        }
        catch (Exception e)
        {
          Log.Debug(e, "Failed to open web interface.");
          Console.WriteLine("Failed to open web interface");
        }
      }

      while (true)
      {
        await Task.Delay(100000);
      }
    }

    private static void Program_Exited(object? sender, EventArgs e)
    {
      Log.Debug("Program closing, Killing node server.");
      Console.WriteLine("Program closing.");
      NodeManager.KillNodeServer();
      InstanceManager.ReleaseInstance();
    }

    public static void StopRunningInstance()
    {
      if (!InstanceManager.IsInstanceRunning())
      {
        return;
      }

      int instancePid = InstanceManager.GetInstancePID();

      if (SignalManager.IsProcessRunning(instancePid))
      {
        SignalManager.SendKillSignal(instancePid);
      }
    }
  }

}

