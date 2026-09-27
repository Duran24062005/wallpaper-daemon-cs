namespace wallpaper_daemon_cs.Services;
public sealed class WallpaperScanner
{
    private static readonly HashSet<string> Extensions = new(StringComparer.OrdinalIgnoreCase) { ".jpg", ".jpeg", ".png", ".webp" };
    public IReadOnlyList<string> ScanImages(string directory)
    {
        if (!Directory.Exists(directory)) throw new WallpaperScanException($"Wallpaper directory does not exist: {directory}");
        try { return Directory.EnumerateFiles(directory, "*", SearchOption.TopDirectoryOnly).Where(p => Extensions.Contains(Path.GetExtension(p))).ToArray(); }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException) { throw new WallpaperScanException($"Failed to scan wallpaper directory: {directory}", ex); }
    }
}
