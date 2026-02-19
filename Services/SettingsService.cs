using System;
using System.IO;
using System.Text.Json;
using ToastrForge.Models;

namespace ToastrForge.Services;

public class SettingsService
{
    private static readonly string AppFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "ToastrForge");

    private static readonly string SettingsPath = Path.Combine(AppFolder, "settings.json");

    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    public AppSettings Load()
    {
        try
        {
            if (!File.Exists(SettingsPath))
                return new AppSettings();

            var json = File.ReadAllText(SettingsPath);
            return JsonSerializer.Deserialize<AppSettings>(json, JsonOpts) ?? new AppSettings();
        }
        catch
        {
            return new AppSettings();
        }
    }

    public void Save(AppSettings settings)
    {
        try
        {
            Directory.CreateDirectory(AppFolder);
            var json = JsonSerializer.Serialize(settings, JsonOpts);
            File.WriteAllText(SettingsPath, json);
        }
        catch
        {
        }
    }

    public static string GetSettingsPath() => SettingsPath;

    public void SetStartWithSystem(bool enable)
    {
        if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
        {
            SetStartWithWindows(enable);
        }
        else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
        {
            SetStartWithLinux(enable);
        }
    }

    public bool IsStartWithSystemEnabled()
    {
        if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
        {
            return IsStartWithWindowsEnabled();
        }
        else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
        {
            return IsStartWithLinuxEnabled();
        }
        return false;
    }

    private void SetStartWithWindows(bool enable)
    {
        try
        {
            using var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            if (key == null) return;

            string appName = "ToastrForge";
            if (enable)
            {
                string? exePath = Environment.ProcessPath;
                if (!string.IsNullOrEmpty(exePath))
                {
                    if (exePath.EndsWith(".dll"))
                    {
                        exePath = exePath.Replace(".dll", ".exe");
                    }
                    
                    key.SetValue(appName, $"\"{exePath}\"");
                }
            }
            else
            {
                key.DeleteValue(appName, false);
            }
        }
        catch { }
    }

    private bool IsStartWithWindowsEnabled()
    {
        try
        {
            using var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", false);
            return key?.GetValue("ToastrForge") != null;
        }
        catch { return false; }
    }

    private void SetStartWithLinux(bool enable)
    {
        try
        {
            string autostartDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config", "autostart");
            string desktopFilePath = Path.Combine(autostartDir, "toastrforge.desktop");

            if (enable)
            {
                if (!Directory.Exists(autostartDir))
                {
                    Directory.CreateDirectory(autostartDir);
                }

                string? exePath = Environment.ProcessPath;
                if (string.IsNullOrEmpty(exePath)) return;

                string content = $"""
                                  [Desktop Entry]
                                  Type=Application
                                  Name=ToastrForge
                                  Exec={exePath}
                                  Terminal=false
                                  Hidden=false
                                  NoDisplay=false
                                  X-GNOME-Autostart-enabled=true
                                  """;

                File.WriteAllText(desktopFilePath, content);
            }
            else
            {
                if (File.Exists(desktopFilePath))
                {
                    File.Delete(desktopFilePath);
                }
            }
        }
        catch { }
    }

    private bool IsStartWithLinuxEnabled()
    {
        try
        {
            string desktopFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config", "autostart", "toastrforge.desktop");
            return File.Exists(desktopFilePath);
        }
        catch { return false; }
    }
    public void RemoveAppAttributes()
    {
        SetStartWithSystem(false);

        if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
        {
            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                string shortcutPath = Path.Combine(desktopPath, "Tasbeeh.lnk");
                if (File.Exists(shortcutPath))
                {
                    File.Delete(shortcutPath);
                }
            }
            catch { }
        }
    }
}
