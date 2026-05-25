using Microsoft.Extensions.DependencyInjection;
using Terminal.Gui.App;
using Terminal.Gui.Configuration;

namespace CommunityToolkitExample;

public static class Program
{
    public static IServiceProvider? Services { get; private set; }

    private static async Task Main (string[] args)
    {
        var smokeTest = args.Length > 0 && args [0] == "--smoke-test";

        ConfigurationManager.Enable (ConfigLocations.All);
        Services = ConfigureServices ();
        using IApplication app = Application.Create ();
        app.Init ();

        if (smokeTest)
        {
            using CancellationTokenSource cts = new (TimeSpan.FromSeconds (2));
            using LoginView smokeLoginView = Services.GetRequiredService<LoginView> ();
            await app.RunAsync (smokeLoginView, cts.Token);
            Console.WriteLine ("Smoke test passed.");

            return;
        }

        using LoginView loginView = Services.GetRequiredService<LoginView> ();
        app.Run (loginView);
    }

    private static IServiceProvider ConfigureServices ()
    {
        ServiceCollection services = new ();
        services.AddTransient<LoginView> ();
        services.AddTransient<LoginViewModel> ();

        return services.BuildServiceProvider ();
    }
}
