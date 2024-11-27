using System.Runtime.InteropServices;
using Serilog;

namespace MagpyServerLinux
{
    class InstanceManager
    {
        private const string LOCK_FILE_PATH = "/tmp/Magpy-e33aa2f2-3e0c-4f17-b25d-245e47fa92f3.lock";
        private static FileStream? fs;
        public static bool HoldInstance()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                throw new Exception("Platform not supported. Needs to be as Linux system.");
            }

            try
            {
                fs = new FileStream(LOCK_FILE_PATH, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                fs.Lock(0, 0);
                StreamWriter writer = new StreamWriter(fs);
                writer.WriteLine(System.Diagnostics.Process.GetCurrentProcess().Id);
                writer.Flush();

                return true;
            }
            catch (IOException e)
            {
                Log.Debug("Failed to hold app instance. Error: " + e.Message);
                return false;
            }

        }

        public static void ReleaseInstance()
        {
            if (fs != null)
            {
                fs.Close();
            }
        }

    }
}
