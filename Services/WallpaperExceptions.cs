namespace wallpaper_daemon_cs.Services;
public abstract class WallpaperDaemonException(string message, Exception? inner=null) : Exception(message, inner);
public sealed class WallpaperScanException(string message, Exception? inner=null) : WallpaperDaemonException(message, inner);
public sealed class InvalidWallpaperUriException(string value, Exception? inner=null) : WallpaperDaemonException($"Invalid wallpaper URI: {value}", inner);
public sealed class WallpaperCommandException(string message, Exception? inner=null) : WallpaperDaemonException(message, inner);
public sealed class NoAlternativeWallpaperException() : WallpaperDaemonException("No alternative wallpaper found.");
