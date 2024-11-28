using System.Reflection;

namespace MagpyServerLinux
{
    class PathManager
    {
        public const string APPDATA_MAGPY_FOLDER_NAME = "Magpy";
        public static string RelativeExeToAbsolute(string relativePath)
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string? assemblyFolder = Path.GetDirectoryName(assemblyLocation);

            if (assemblyFolder == null)
            {
                throw new Exception("Could not get assembly location folder.");
            }

            return Path.Combine(assemblyFolder, relativePath);
        }

        public static string RelativeAppDataToAbsolute(string relativePath)
        {
            return Path.Combine(AppDataPath(), relativePath);
        }

        public static string AppDataPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), APPDATA_MAGPY_FOLDER_NAME);
        }

        public static void ClearAppDataFolder()
        {
            if (Directory.Exists(AppDataPath()))
            {
                Directory.Delete(AppDataPath(), recursive: true);
            }
        }

        public static string? GetAppImagePath()
        {
            return Environment.GetEnvironmentVariable("APPIMAGE");
        }

        public static string GetDesktopFileFolderPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config", "autostart");
        }
    }
}
