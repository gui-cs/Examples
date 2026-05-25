// A simple Terminal.Gui example in C# - using C# 9.0 Top-level statements

// This is a simple example application.  For the full range of functionality
// see the UICatalog project

using Terminal.Gui.App;
using Terminal.Gui.Configuration;
using Terminal.Gui.Input;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;

// Override the default configuration for the application to use the Amber Phosphor theme
ConfigurationManager.RuntimeConfig = """{ "Theme": "Amber Phosphor" }""";
ConfigurationManager.Enable (ConfigLocations.All);

IApplication app = Application.Create ().Init ();

var smokeTest = args.Length > 0 && args [0] == "--smoke-test";

if (smokeTest)
{
    using CancellationTokenSource cts = new (TimeSpan.FromSeconds (2));
    await app.RunAsync<ExampleWindow> (cts.Token);
    Console.WriteLine ("Smoke test passed.");
    app.Dispose ();

    return;
}

var userName = app.Run<ExampleWindow> ().GetResult<string> ();
app.Dispose ();

// To see this output on the screen it must be done after Dispose,
// which restores the previous screen.
if (string.IsNullOrEmpty (userName))
{
    Console.WriteLine (@"Login cancelled");
}
else
{
    Console.WriteLine ($@"Username: {userName}");
}

// Defines a top-level window with border and title
public sealed class ExampleWindow : Runnable<string?>
{
    public ExampleWindow ()
    {
        Title = $"Example App ({Application.GetDefaultKey (Command.Quit)} to quit)";

        // Create input components and labels
        Label usernameLabel = new() { Text = "Username:" };

        TextField userNameText = new()
        {
            // Position text field adjacent to the label
            X = Pos.Right (usernameLabel) + 1,

            // Fill remaining horizontal space
            Width = Dim.Fill ()
        };

        Label passwordLabel = new()
            { Text = "Password:", X = Pos.Left (usernameLabel), Y = Pos.Bottom (usernameLabel) + 1 };

        TextField passwordText = new()
        {
            Secret = true,

            // align with the text box above
            X = Pos.Left (userNameText),
            Y = Pos.Top (passwordLabel),
            Width = Dim.Fill ()
        };

        // Create login button
        Button btnLogin = new()
        {
            Text = "Login",
            Y = Pos.Bottom (passwordLabel) + 1,

            // center the login button horizontally
            X = Pos.Center (),
            IsDefault = true
        };

        // When login button is clicked display a message popup
        btnLogin.Accepting += (s, e) =>
        {
            if (userNameText.Text == "admin" && passwordText.Text == "password")
            {
                MessageBox.Query (App!, "Logging In", "Login Successful", "Ok");
                Result = userNameText.Text;
                App!.RequestStop ();
            }
            else
            {
                MessageBox.ErrorQuery ((s as View)?.App!, "Logging In", "Incorrect username or password", "Ok");
            }

            // When Accepting is handled, set e.Handled to true to prevent further processing.
            e.Handled = true;
        };

        // Add the views to the Window
        Add (usernameLabel, userNameText, passwordLabel, passwordText, btnLogin);
    }
}
