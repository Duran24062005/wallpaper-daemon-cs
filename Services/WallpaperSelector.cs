namespace wallpaper_daemon_cs.Services;
public sealed class WallpaperSelector
{
    public string SelectRandom(IReadOnlyList<string> images, string? current)
    {
        var available=images.Where(p => current is null || !string.Equals(Path.GetFullPath(p), Path.GetFullPath(current), StringComparison.Ordinal)).ToArray();
        if (available.Length == 0) throw new NoAlternativeWallpaperException();
        return available[Random.Shared.Next(available.Length)];
    }
}
