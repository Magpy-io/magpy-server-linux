using Serilog;
using Serilog.Extensions.Logging;

namespace MagpyServerLinux
{
    class LoggingManager
    {
        public const string APP_LOGGING_CHANNEL = "app";
        public const string INSTALLER_LOGGING_CHANNEL = "installer";
        public const string NODE_LOGGING_CHANNEL = "node";


        public static ILogger? LoggerApp { get; private set; }
        public static ILogger? LoggerInstaller { get; private set; }
        public static ILogger? LoggerNode { get; private set; }

        public static void InitEarly()
        {
            LoggerApp = CreateConsoleLogger(APP_LOGGING_CHANNEL);
            LoggerInstaller = CreateConsoleLogger(INSTALLER_LOGGING_CHANNEL);
            LoggerNode = CreateNodeConsoleLogger(NODE_LOGGING_CHANNEL);

            Log.Logger = LoggerApp;
        }

        public static void Init()
        {
#if DEBUG
            LoggerApp = CreateConsoleLogger(APP_LOGGING_CHANNEL);
            LoggerInstaller = CreateConsoleLogger(INSTALLER_LOGGING_CHANNEL);
            LoggerNode = CreateNodeConsoleLogger(NODE_LOGGING_CHANNEL);
#else
            LoggerApp = CreateFileLoggerInFolder(APP_LOGGING_CHANNEL);
            LoggerInstaller = CreateFileLoggerInFolder(INSTALLER_LOGGING_CHANNEL);
            LoggerNode = CreateNodeFileLoggerInFolder(NODE_LOGGING_CHANNEL);
#endif

            Log.Logger = LoggerApp;
        }

        private static ILogger CreateFileLoggerInFolder(string channel)
        {
            return new LoggerConfiguration()
               .MinimumLevel.Verbose()
               .WriteTo.File(PathManager.RelativeAppDataToAbsolute(Path.Combine("LogFiles", channel, "Log.txt")),
                   rollingInterval: RollingInterval.Day,
                   retainedFileTimeLimit: TimeSpan.FromDays(5),
                   outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}] [{Level}] {Message}{NewLine}{Exception}")
               .CreateLogger();
        }

        private static ILogger CreateConsoleLogger(string channel)
        {
            return new LoggerConfiguration()
               .MinimumLevel.Verbose()
               .WriteTo.Console(outputTemplate: $"[{channel}] " + "[{Level}] {Message}{NewLine}{Exception}")
               .CreateLogger();
        }

        private static ILogger CreateNodeFileLoggerInFolder(string channel)
        {
            return new LoggerConfiguration()
               .MinimumLevel.Verbose()
               .WriteTo.File(PathManager.RelativeAppDataToAbsolute(Path.Combine("LogFiles", channel, "Log.txt")),
                   rollingInterval: RollingInterval.Day,
                   retainedFileTimeLimit: TimeSpan.FromDays(7),
                   outputTemplate: "{Message}{NewLine}")
               .CreateLogger();
        }

        private static ILogger CreateNodeConsoleLogger(string channel)
        {
            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console(outputTemplate: "{Message}{NewLine}")
                .CreateLogger();
        }

        public static Microsoft.Extensions.Logging.ILogger? SerilogToMicrosoftLogger(ILogger? logger)
        {
            if (logger == null)
            {
                return null;
            }
            return new SerilogLoggerFactory(logger).CreateLogger("Serilog");
        }

        public static void DisposeLoggers()
        {
            IDisposable? disposable1 = (IDisposable?)LoggerInstaller;
            IDisposable? disposable2 = (IDisposable?)LoggerApp;
            IDisposable? disposable3 = (IDisposable?)LoggerNode;

            disposable1?.Dispose();
            disposable2?.Dispose();
            disposable3?.Dispose();
        }
    }
}
