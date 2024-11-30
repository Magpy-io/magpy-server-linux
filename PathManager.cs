using System.Reflection;

namespace MagpyServerLinux
{
    class PathManager
    {
        public const string APPDATA_MAGPY_FOLDER_NAME = "Magpy";
        public const string SERVER_DATA_FOLDER_NAME = "serverData";
        public const string DB_FOLDER_NAME = "db";
        public const string LOGS_FOLDER_NAME = "LogFiles";

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

        public static string ClearAppDataFolder()
        {
            if (Directory.Exists(AppDataPath()))
            {
                Directory.Delete(AppDataPath(), recursive: true);
            }
            return AppDataPath();
        }

        public static string[] ClearServerDataFolder()
        {
            string dbFolderPath = RelativeAppDataToAbsolute(DB_FOLDER_NAME);
            string serverDataFolderPath = RelativeAppDataToAbsolute(SERVER_DATA_FOLDER_NAME);
            string logsFolderPath = RelativeAppDataToAbsolute(LOGS_FOLDER_NAME);

            if (Directory.Exists(dbFolderPath))
            {
                Directory.Delete(dbFolderPath, recursive: true);
            }

            if (Directory.Exists(serverDataFolderPath))
            {
                Directory.Delete(serverDataFolderPath, recursive: true);
            }

            if (Directory.Exists(logsFolderPath))
            {
                Directory.Delete(logsFolderPath, recursive: true);
            }

            return [dbFolderPath, serverDataFolderPath, logsFolderPath];
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
