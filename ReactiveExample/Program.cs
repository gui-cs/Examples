#nullable disable
using System.Reactive.Concurrency;
using ReactiveUI.Builder;
using Terminal.Gui.App;
using Terminal.Gui.Configuration;

namespace ReactiveExample;

public static class Program
{
    internal static ReactiveUIBuilder _rxApp;

    private static async Task Main (string[] args)
    {
        var smokeTest = args.Length > 0 && args [0] == "--smoke-test";

        ConfigurationManager.Enable (ConfigLocations.All);
        using IApplication app = Application.Create ();
        app.Init ();
        _rxApp = RxAppBuilder.CreateReactiveUIBuilder ();
        _rxApp.WithMainThreadScheduler (new TerminalScheduler (app));
        _rxApp.WithTaskPoolScheduler (TaskPoolScheduler.Default);

        if (smokeTest)
        {
            using CancellationTokenSource cts = new (TimeSpan.FromSeconds (2));
            using LoginView smokeLoginView = new (new LoginViewModel ());
            await app.RunAsync (smokeLoginView, cts.Token);
            Console.WriteLine ("Smoke test passed.");

            return;
        }

        LoginView loginView = new (new LoginViewModel ());
        app.Run (loginView);
        loginView.Dispose ();
    }
}
