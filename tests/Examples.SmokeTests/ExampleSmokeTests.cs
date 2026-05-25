using System.Diagnostics;

namespace Examples.SmokeTests;

/// <summary>
///     Smoke tests that verify each example starts, renders, and exits cleanly.
///     Each test launches the example as a subprocess with DisableRealDriverIO=1
///     and a timeout, verifying exit code 0 and "Smoke test passed." output.
/// </summary>
// Copilot - Claude Opus 4.6
public class ExampleSmokeTests
{
    private static readonly TimeSpan Timeout = TimeSpan.FromSeconds (30);

    private static readonly string SolutionRoot = FindSolutionRoot ();

    [Theory]
    [InlineData ("Example")]
    [InlineData ("CommunityToolkitExample")]
    [InlineData ("FluentExample")]
    [InlineData ("InlineColorPicker")]
    [InlineData ("InlineSelect")]
    [InlineData ("ShortcutTest")]
    [InlineData ("SelfContained")]
    [InlineData ("InlineCLI")]
    [InlineData ("PromptExample")]
    public async Task Example_StartsAndExitsCleanly (string projectName)
    {
        var projectPath = Path.Combine (SolutionRoot, projectName);

        Assert.True (Directory.Exists (projectPath), $"Project directory not found: {projectPath}");

        ProcessStartInfo psi = new ()
        {
            FileName = "dotnet",
            Arguments = $"run --project \"{projectPath}\" -- --smoke-test",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = SolutionRoot
        };

        // DisableRealDriverIO prevents the driver from probing the terminal
        psi.Environment ["DisableRealDriverIO"] = "1";

        using Process? process = Process.Start (psi);

        Assert.NotNull (process);

        var stdout = await process.StandardOutput.ReadToEndAsync (TestContext.Current.CancellationToken);
        var stderr = await process.StandardError.ReadToEndAsync (TestContext.Current.CancellationToken);

        using CancellationTokenSource cts = new (Timeout);

        try
        {
            await process.WaitForExitAsync (TestContext.Current.CancellationToken);
        }
        catch (OperationCanceledException)
        {
            process.Kill (true);

            Assert.Fail (
                $"{projectName} did not exit within {Timeout.TotalSeconds}s.\nStdout: {stdout}\nStderr: {stderr}");
        }

        Assert.True (
            process.ExitCode == 0,
            $"{projectName} exited with code {process.ExitCode}.\nStdout: {stdout}\nStderr: {stderr}");

        Assert.Contains ("Smoke test passed.", stdout);
    }

    private static string FindSolutionRoot ()
    {
        var dir = AppContext.BaseDirectory;

        while (dir is not null)
        {
            if (File.Exists (Path.Combine (dir, "Examples.sln")))
            {
                return dir;
            }

            dir = Path.GetDirectoryName (dir);
        }

        // Fallback: try relative to the test assembly
        var assemblyDir = Path.GetDirectoryName (typeof (ExampleSmokeTests).Assembly.Location);

        if (assemblyDir is not null)
        {
            // Walk up from bin/Debug/net10.0 -> tests/Examples.SmokeTests -> tests -> root
            var candidate = Path.GetFullPath (Path.Combine (assemblyDir, "..", "..", "..", "..", ".."));

            if (File.Exists (Path.Combine (candidate, "Examples.sln")))
            {
                return candidate;
            }
        }

        throw new InvalidOperationException ("Could not find Examples.sln in parent directories.");
    }
}
