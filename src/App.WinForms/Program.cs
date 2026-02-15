using App.Core.Interfaces;
using App.Core.Services;
using App.Data.Infrastructure;
using App.Data.Repositories;
using Serilog;

namespace App.WinForms;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var appDirectory = Path.Combine(localAppData, "YSM-GMTool");
        var logsDirectory = Path.Combine(appDirectory, "logs");
        Directory.CreateDirectory(logsDirectory);

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File(Path.Combine(logsDirectory, "gmtool-.log"), rollingInterval: RollingInterval.Day)
            .CreateLogger();

        try
        {
            ApplicationConfiguration.Initialize();
            ApplyNativeDarkMode();
            RegisterUnhandledExceptionHandlers();

            var queryFile = Path.Combine(AppContext.BaseDirectory, "Config", "queries.json");
            var luaFile = Path.Combine(AppContext.BaseDirectory, "Config", "lua_commands.json");
            var settingsFile = Path.Combine(appDirectory, "settings.json");

            IQueryStore queryStore = new FileQueryStore(queryFile);
            ILuaCommandTemplateStore templateStore = new FileLuaCommandTemplateStore(luaFile);
            IAppSettingsService settingsService = new JsonAppSettingsService(settingsFile);
            INameNormalizer normalizer = new SearchNameNormalizer();
            ILuaCommandBuilder commandBuilder = new LuaCommandBuilder(templateStore);
            ICommandHistoryService commandHistory = new CommandHistoryService();
            IGameDataRepository repository = new GameDataRepository(queryStore, new DbConnectionFactory());

            Application.Run(new MainForm(repository, settingsService, commandBuilder, commandHistory, normalizer));
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly.");
            MessageBox.Show($"Fatal error:{Environment.NewLine}{ex.Message}", "YSM GM Tool", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    private static void RegisterUnhandledExceptionHandlers()
    {
        Application.ThreadException += (_, args) =>
        {
            Log.Error(args.Exception, "Unhandled UI thread exception.");
            MessageBox.Show($"Unhandled error:{Environment.NewLine}{args.Exception.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        };

        AppDomain.CurrentDomain.UnhandledException += (_, args) =>
        {
            if (args.ExceptionObject is Exception ex)
            {
                Log.Fatal(ex, "Unhandled non-UI exception.");
            }
        };

        TaskScheduler.UnobservedTaskException += (_, args) =>
        {
            Log.Error(args.Exception, "Unobserved task exception.");
            args.SetObserved();
        };
    }

    private static void ApplyNativeDarkMode()
    {
#if NET10_0_OR_GREATER
        // Variant A (.NET 10 GA/current SDK): official built-in dark mode API.
        Application.SetColorMode(SystemColorMode.Dark);
#else
        // Variant B (older previews): no stable SetColorMode API.
#endif
    }
}
