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
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), APPDATA_MAGPY_FOLDER_NAME, relativePath);
        }

        public static void ClearAppDataFolder()
        {
            Directory.Delete(RelativeAppDataToAbsolute("."), true);
        }
    }
}
