using System.Runtime.InteropServices;
using Serilog;

namespace MagpyServerLinux
{
    class InstanceManager
    {
        private const string LOCK_FILE_PATH = "/tmp/Magpy-e33aa2f2-3e0c-4f17-b25d-245e47fa92f3.lock";
        private const string PID_FILE_PATH = "/tmp/Magpy-e33aa2f2-3e0c-4f17-b25d-245e47fa92f3.pid";
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

                using FileStream fsPid = new FileStream(PID_FILE_PATH, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                using StreamWriter writer = new StreamWriter(fsPid);
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

                if (File.Exists(LOCK_FILE_PATH))
                {
                    File.Delete(LOCK_FILE_PATH);
                }

                if (File.Exists(PID_FILE_PATH))
                {
                    File.Delete(PID_FILE_PATH);
                }
            }
        }

        public static bool IsInstanceRunning()
        {
            try
            {
                if (!File.Exists(LOCK_FILE_PATH))
                {
                    return false;
                }

                fs = new FileStream(LOCK_FILE_PATH, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read);
                fs.Close();
                return false;
            }
            catch (IOException)
            {
                return true;
            }
        }

        public static int GetInstancePID()
        {
            if (!IsInstanceRunning())
            {
                return -1;
            }

            string pidFileContent = File.ReadAllText(PID_FILE_PATH);

            if (int.TryParse(pidFileContent, out int existingPid))
            {
                return existingPid;
            }
            else
            {
                return -1;
            }
        }

    }
}
