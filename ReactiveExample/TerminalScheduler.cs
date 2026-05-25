using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using Terminal.Gui.App;

namespace ReactiveExample;

public class TerminalScheduler : LocalScheduler
{
    private readonly IApplication? _application;
    public TerminalScheduler (IApplication? application) { _application = application; }

    public override IDisposable Schedule<TState> (
        TState state,
        TimeSpan dueTime,
        Func<IScheduler, TState, IDisposable> action
    )
    {
        IDisposable PostOnMainLoop ()
        {
            CompositeDisposable composite = new (2);
            CancellationDisposable cancellation = new ();

            _application?.Invoke (_ =>
                {
                    if (!cancellation.Token.IsCancellationRequested)
                    {
                        composite.Add (action (this, state));
                    }
                }
            );
            composite.Add (cancellation);

            return composite;
        }

        IDisposable PostOnMainLoopAsTimeout ()
        {
            CompositeDisposable composite = new (2);

            var timeout = _application?.AddTimeout (
                dueTime,
                () =>
                {
                    composite.Add (action (this, state));

                    return false;
                }
            );
            composite.Add (Disposable.Create (() =>
            {
                if (timeout is not null)
                {
                    _application?.RemoveTimeout (timeout);
                }
            }));

            return composite;
        }

        return dueTime == TimeSpan.Zero
            ? PostOnMainLoop ()
            : PostOnMainLoopAsTimeout ();
    }
}
