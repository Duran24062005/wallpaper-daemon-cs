namespace wallpaper_daemon_cs.Services;
public sealed class Scheduler { public Task WaitAsync(TimeSpan interval, CancellationToken token=default) => Task.Delay(interval, token); }
