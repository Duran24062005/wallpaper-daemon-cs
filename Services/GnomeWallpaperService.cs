using System.ComponentModel;
using System.Diagnostics;
namespace wallpaper_daemon_cs.Services;
public sealed class GnomeWallpaperService
{
    private const string GSettings="/usr/bin/gsettings", Schema="org.gnome.desktop.background", Key="picture-uri-dark";
    private readonly Func<string,string[],CancellationToken,Task<ProcessResult>> run;
    public GnomeWallpaperService() : this(RunAsync) { }
    internal GnomeWallpaperService(Func<string,string[],CancellationToken,Task<ProcessResult>> run) => this.run=run;
    public async Task<string> GetCurrentWallpaperAsync(CancellationToken token=default) { var r=await run(GSettings,["get",Schema,Key],token); Ensure("get",r); return ParseWallpaperUri(r.Output); }
    public async Task SetWallpaperAsync(string image,CancellationToken token=default)
    {
        string path; try { path=Path.GetFullPath(image); if(!File.Exists(path)) throw new FileNotFoundException("Wallpaper image does not exist.",path); }
        catch(Exception ex) when(ex is IOException or ArgumentException or NotSupportedException) { throw new WallpaperCommandException("Could not resolve wallpaper image path.",ex); }
        var r=await run(GSettings,["set",Schema,Key,new Uri(path).AbsoluteUri],token); Ensure("set",r);
    }
    public static string ParseWallpaperUri(string value)
    { var s=value.Trim().Trim('\''); if(!s.StartsWith("file://",StringComparison.Ordinal)) throw new InvalidWallpaperUriException(s); try{return new Uri(s,UriKind.Absolute).LocalPath;}catch(UriFormatException ex){throw new InvalidWallpaperUriException(s,ex);} }
    private static void Ensure(string op,ProcessResult r){if(r.ExitCode!=0)throw new WallpaperCommandException($"gsettings {op} failed with exit code {r.ExitCode}: {r.Error.Trim()}");}
    private static async Task<ProcessResult> RunAsync(string file,string[] args,CancellationToken token)
    { using var p=new Process{StartInfo=new ProcessStartInfo{FileName=file,UseShellExecute=false,RedirectStandardOutput=true,RedirectStandardError=true,CreateNoWindow=true}}; foreach(var a in args)p.StartInfo.ArgumentList.Add(a); try{if(!p.Start())throw new WallpaperCommandException($"Could not start {file}.");var o=p.StandardOutput.ReadToEndAsync(token);var e=p.StandardError.ReadToEndAsync(token);await p.WaitForExitAsync(token);return new(p.ExitCode,await o,await e);}catch(Exception ex)when(ex is Win32Exception or InvalidOperationException){throw new WallpaperCommandException($"Could not execute {file}.",ex);}}
}
internal sealed record ProcessResult(int ExitCode,string Output,string Error);
