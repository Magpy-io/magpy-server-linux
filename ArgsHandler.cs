namespace MagpyServerLinux
{
  public class ArgsHandler
  {
    private string[] args;

    public ArgsHandler(string[] args)
    {
      this.args = args;
    }

    public Action GetAction()
    {
      if (args.Length == 0)
      {
        return Action.START;
      }

      switch (args[0])
      {
        case "--silent":
          return Action.START;
        case "stop":
          return Action.STOP;
        case "--open-webui":
          return Action.LAUNCH_WEBUI;
        case "--status":
          return Action.STATUS;
        case "--version":
          return Action.DISPLAY_VERSION;
        case "update":
          return Action.UPDATE;
        case "--clear-server-data":
          return Action.CLEAR_SERVER_DATA;
        case "--enable-autostart":
          return Action.ENABLE_DESKTOP_AUTOSTART;
        case "--disable-autostart":
          return Action.DISABLE_DESKTOP_AUTOSTART;
      }
      return Action.NONE;
    }

    public bool IsLaunchSilent()
    {
      if (args.Length == 0)
      {
        return false;
      }

      switch (args[0])
      {
        case "--silent":
          return true;
      }

      return false;
    }
  }

  public enum Action
  {
    NONE,
    START,
    STOP,
    LAUNCH_WEBUI,
    STATUS,
    DISPLAY_VERSION,
    UPDATE,
    CLEAR_SERVER_DATA,
    ENABLE_DESKTOP_AUTOSTART,
    DISABLE_DESKTOP_AUTOSTART
  }
}

