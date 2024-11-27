using System.Diagnostics;

namespace MagpyServerLinux
{
    public class ServerManager
    {
        public static void OpenWebInterface()
        {
            Process.Start("xdg-open", Constants.serverUrl);
        }
    }
}
