using System.Diagnostics;
using System.Runtime.InteropServices;
using Serilog;

namespace MagpyServerLinux
{

  public class SignalManager
  {
    private const int SIGINT = 2;
    private const int SIGTERM = 15;


    [DllImport("libc")]
    private static extern int sigaction(int signum, ref SigAction act, IntPtr oldact);
    private delegate void SignalHandler(int signal);

    [DllImport("libc")]
    private static extern int kill(int pid, int sig);


    private static SignalHandler? _signalHandler;

    public static void SendKillSignal(int pid)
    {
      kill(pid, SIGTERM);
    }

    public static bool IsProcessRunning(int pid)
    {
      try
      {
        Process a = Process.GetProcessById(pid);
        return true;
      }
      catch (ArgumentException)
      {
        return false;
      }
    }

    public static void SetupSignalWatchers()
    {
      _signalHandler = OnSignalReceived;

      SigAction sigAction = new SigAction
      {
        sa_handler = Marshal.GetFunctionPointerForDelegate(_signalHandler),
        sa_flags = 0,
        sa_mask = 0
      };


      sigaction(SIGINT, ref sigAction, IntPtr.Zero);
      sigaction(SIGTERM, ref sigAction, IntPtr.Zero);
    }

    private static void OnSignalReceived(int signal)
    {
      switch (signal)
      {
        case SIGINT:
          Log.Debug($"Signal SIGINT received. Shutting down app.");
          NodeManager.KillNodeServer();
          Environment.Exit(0);
          break;
        case SIGTERM:
          Log.Debug($"Signal SIGTERM received. Shutting down app.");
          NodeManager.KillNodeServer();
          Environment.Exit(0);
          break;
      }
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct SigAction
    {
      public IntPtr sa_handler;
      public ulong sa_mask;
      public int sa_flags;
      public IntPtr sa_restorer;
    }
  }
}

