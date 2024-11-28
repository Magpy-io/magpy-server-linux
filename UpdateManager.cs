using Serilog;
using Velopack;

using static MagpyServerLinux.Utils;

namespace MagpyServerLinux
{
    class UpdateManager
    {
        static readonly string UPDATE_URL = "https://magpy-update-linux.s3.eu-west-3.amazonaws.com";

        static bool isUpdateRunning = false;

        public static void Init()
        {
            VelopackApp.Build()
               .WithFirstRun((v) =>
               {
                   ServerManager.OpenWebInterface();
               })
           .Run(LoggingManager.SerilogToMicrosoftLogger(LoggingManager.LoggerInstaller));
        }

        public static bool IsAppInstalled()
        {
            var mgr = new Velopack.UpdateManager(UPDATE_URL);

            return mgr.IsInstalled;
        }
        public static async Task UpdateMyAppAndExit()
        {
            if (isUpdateRunning)
            {
                return;
            }

            isUpdateRunning = true;

            try
            {
                var mgr = new Velopack.UpdateManager(UPDATE_URL);

                Log.Debug("Checking for updates.");

                // check is app installed
                if (!mgr.IsInstalled)
                {
                    Log.Debug("App is not installed.");
                    Console.WriteLine("App is not installed.");
                    return; // app not installed
                }

                // check for new version
                var newVersion = await mgr.CheckForUpdatesAsync();
                if (newVersion == null)
                {
                    Log.Debug("No new version.");
                    Console.WriteLine("No available new versions.");
                    return; // no update available
                }

                Log.Debug("Downloading new version.");
                Console.WriteLine($"New version found ({newVersion.TargetFullRelease.Version}). Downloading update...");
                // download new version
                await mgr.DownloadUpdatesAsync(newVersion);

                Log.Debug("Restarting and applying new version.");
                Console.WriteLine("Applying update...");

                // install new version and stop app
                mgr.ApplyUpdatesAndExit(newVersion);
            }
            finally
            {
                isUpdateRunning = false;
            }
        }

        public static void SetupPeriodicUpdate()
        {
            // 12 hours
            int secondsDelay = 12 * 60 * 60;

            SchedulePeriodicTask(async () =>
            {
                try
                {
                    Console.WriteLine("Checking for app updates...");
                    await UpdateMyAppAndExit();
                }
                catch (Exception ex)
                {
                    Log.Debug(ex, "Error while trying to update server.");
                }
            }, secondsDelay);
        }
    }
}
