using Serilog;

namespace MagpyServerLinux
{
    public class Program
    {
        static async Task Main(string[] args)
        {
#if DEBUG
            LoggingManager.InitEarly();
#endif
            try
            {
                bool instanceCreated = InstanceManager.HoldInstance();
                if (instanceCreated)
                {
                    LoggingManager.Init();
                    Log.Debug("Logging initialized.");

                    await App.Start();
                }
                else
                {
                    Console.WriteLine("Failed to run Magpy. An instance of the app is already running.");
                }
            }
            catch (Exception e)
            {
                Log.Error(e, "Error launching app, checking for updates...");

                try
                {
                    await UpdateManager.UpdateMyApp();
                }
                catch (Exception ex)
                {
                    Log.Debug(ex, "Error while trying to update server.");
                }
#if DEBUG
                Console.ReadKey();
#endif
            }
            finally
            {
                InstanceManager.ReleaseInstance();
            }
        }
    }
}
