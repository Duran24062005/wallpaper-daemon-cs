namespace wallpaper_daemon_cs.Models;

public sealed record WallpaperConfiguration(string WallpaperDirectory, TimeSpan Interval)
{ public static WallpaperConfiguration Default => new("assets", TimeSpan.FromSeconds(10)); }
