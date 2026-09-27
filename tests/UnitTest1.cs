using wallpaper_daemon_cs.Services;

namespace wallpaper_daemon_cs.Tests;

public class WallpaperTests
{
    [Fact]
    public void Scanner_filters_supported_extensions_case_insensitively()
    {
        var dir = Directory.CreateTempSubdirectory();
        try { File.WriteAllText(Path.Combine(dir.FullName, "a.JPG"), "x"); File.WriteAllText(Path.Combine(dir.FullName, "b.txt"), "x"); Directory.CreateDirectory(Path.Combine(dir.FullName, "nested.png")); var result = new WallpaperScanner().ScanImages(dir.FullName); Assert.Single(result); Assert.EndsWith("a.JPG", result[0]); }
        finally { dir.Delete(true); }
    }

    [Fact]
    public void Scanner_throws_for_missing_directory() => Assert.Throws<WallpaperScanException>(() => new WallpaperScanner().ScanImages(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString())));

    [Fact]
    public void Selector_excludes_current_wallpaper()
    {
        var result = new WallpaperSelector().SelectRandom(["/tmp/a.jpg", "/tmp/b.jpg"], "/tmp/a.jpg");
        Assert.Equal("/tmp/b.jpg", result);
    }

    [Fact]
    public void Selector_allows_single_image_without_current() => Assert.Equal("/tmp/a.jpg", new WallpaperSelector().SelectRandom(["/tmp/a.jpg"], "/tmp/old.jpg"));

    [Fact]
    public void Selector_throws_when_no_alternative_exists() => Assert.Throws<NoAlternativeWallpaperException>(() => new WallpaperSelector().SelectRandom(["/tmp/a.jpg"], "/tmp/a.jpg"));

    [Fact]
    public void Uri_parser_accepts_quoted_file_uri() => Assert.Equal("/tmp/example.jpg", GnomeWallpaperService.ParseWallpaperUri("'file:///tmp/example.jpg'"));

    [Fact]
    public async Task Gsettings_failure_becomes_domain_error()
    {
        var service = new GnomeWallpaperService((_, _, _) => Task.FromResult(new ProcessResult(1, "", "permission denied")));
        await Assert.ThrowsAsync<WallpaperCommandException>(() => service.GetCurrentWallpaperAsync());
    }

    [Fact]
    public void Uri_parser_rejects_non_file_uri() => Assert.Throws<InvalidWallpaperUriException>(() => GnomeWallpaperService.ParseWallpaperUri("'/tmp/example.jpg'"));
}
