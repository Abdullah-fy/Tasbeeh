using System;
using System.Timers;
using ToastrForge.Models;

namespace ToastrForge.Services;

public class ToastService : IDisposable
{
    private System.Timers.Timer? _timer;
    private readonly SettingsService _settingsService;
    private readonly TextsService _textsService;
    private AppSettings _settings;

    public event Action<string, AppSettings>? OnShowToast;

    public ToastService(SettingsService settingsService, TextsService textsService)
    {
        _settingsService = settingsService;
        _textsService = textsService;
        _settings = _settingsService.Load();
    }

    public void Start()
    {
        Stop();
        _settings = _settingsService.Load();

        if (!_settings.IsEnabled) return;

        double intervalMs = _settings.IntervalMinutes * 60_000.0;
        intervalMs = Math.Clamp(intervalMs, 10_000, 86_400_000);

        _timer = new System.Timers.Timer(intervalMs);
        _timer.Elapsed += OnTimerElapsed;
        _timer.AutoReset = true;
        _timer.Start();
    }

    public void Stop()
    {
        _timer?.Stop();
        _timer?.Dispose();
        _timer = null;
    }

    public void Restart()
    {
        Start();
    }

    public void FireNow()
    {
        _settings = _settingsService.Load();
        var text = _textsService.GetRandomText();
        OnShowToast?.Invoke(text, _settings);
    }

    private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
    {
        _settings = _settingsService.Load();
        if (!_settings.IsEnabled) return;

        var text = _textsService.GetRandomText();
        OnShowToast?.Invoke(text, _settings);
    }

    public void Dispose()
    {
        Stop();
        GC.SuppressFinalize(this);
    }
}
