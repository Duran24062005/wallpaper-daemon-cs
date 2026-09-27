using wallpaper_daemon_cs.Models;
using wallpaper_daemon_cs.Services;

var config = WallpaperConfiguration.Default;
var scanner = new WallpaperScanner();
var selector = new WallpaperSelector();
var gnome = new GnomeWallpaperService();
var scheduler = new Scheduler();
Console.WriteLine($"Wallpaper daemon started. Directory: {config.WallpaperDirectory}");
while (true)
{
    try
    {
        var images = scanner.ScanImages(config.WallpaperDirectory);
        var current = await gnome.GetCurrentWallpaperAsync();
        var selected = selector.SelectRandom(images, current);
        Console.WriteLine($"Current wallpaper: {current}");
        Console.WriteLine($"Selected wallpaper: {selected}");
        await gnome.SetWallpaperAsync(selected);
    }
    catch (WallpaperDaemonException ex) { Console.Error.WriteLine($"Wallpaper error: {ex.Message}"); }
    catch (Exception ex) { Console.Error.WriteLine($"Unexpected wallpaper error: {ex.Message}"); }
    await scheduler.WaitAsync(config.Interval);
}
