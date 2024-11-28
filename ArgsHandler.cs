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
        case "--clear-data":
          return Action.CLEAR_DATA;
      }
      return Action.START;
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
    START,
    STOP,
    LAUNCH_WEBUI,
    STATUS,
    DISPLAY_VERSION,
    UPDATE,
    CLEAR_DATA
  }
}

