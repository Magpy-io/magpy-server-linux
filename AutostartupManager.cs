namespace MagpyServerLinux
{
  public class AutostartupManager
  {
    private const string DESKTOP_FILE_PATH_RELATIVE = "assets/magpy.desktop";
    private const string DESKTOP_FILE_NAME = "magpy.desktop";
    private const string ICON_FILE_PATH_RELATIVE = "assets/icon.png";

    private static string DESKTOP_FILE_PATH
    {
      get
      {
        return PathManager.RelativeExeToAbsolute(DESKTOP_FILE_PATH_RELATIVE);
      }
    }

    private static string CopyIconToAppDataIfMissing()
    {
      string iconPathPackage = PathManager.RelativeExeToAbsolute(ICON_FILE_PATH_RELATIVE);
      string iconPathAppData = PathManager.RelativeAppDataToAbsolute(ICON_FILE_PATH_RELATIVE);

      string? iconPathAppDataDirectory = Path.GetDirectoryName(iconPathAppData);

      if (iconPathAppDataDirectory != null && !Directory.Exists(iconPathAppDataDirectory))
      {
        Directory.CreateDirectory(iconPathAppDataDirectory);
      }

      if (!File.Exists(iconPathAppData))
      {
        File.Copy(iconPathPackage, iconPathAppData);
      }
      return iconPathAppData;
    }

    private static string FetchDesktopFileContent()
    {
      string iconPathAppData = CopyIconToAppDataIfMissing();

      string desktopFileContent = File.ReadAllText(DESKTOP_FILE_PATH);
      string desktopFileFormatted = string.Format(desktopFileContent, iconPathAppData, PathManager.GetAppImagePath());

      return desktopFileFormatted;
    }

    public static string EnableDesktopAutoStart()
    {
      string autostartDir = PathManager.GetDesktopFileFolderPath();
      if (!Directory.Exists(autostartDir))
      {
        Directory.CreateDirectory(autostartDir);
      }

      string desktopFileContent = FetchDesktopFileContent();

      string autostartFilePath = Path.Combine(autostartDir, DESKTOP_FILE_NAME);

      File.WriteAllText(autostartFilePath, desktopFileContent);
      return autostartFilePath;
    }

    public static void DisableDesktopAutoStart()
    {
      string autostartDir = PathManager.GetDesktopFileFolderPath();
      string autostartFilePath = Path.Combine(autostartDir, DESKTOP_FILE_NAME);

      if (File.Exists(autostartFilePath))
      {
        File.Delete(autostartFilePath);
      }
    }
  }
}

