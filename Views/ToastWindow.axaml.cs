using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Styling;
using Avalonia.Threading;
using System;
using ToastrForge.Models;
using System.Runtime.InteropServices;

namespace ToastrForge.Views;

public partial class ToastWindow : Window
{
    private DispatcherTimer? _closeTimer;
    private DispatcherTimer? _progressTimer;
    private double _totalDurationMs;
    private double _elapsed;
    private AppSettings? _settings;

    private const int GWL_EXSTYLE = -20;
    private const int WS_EX_NOACTIVATE = 0x08000000;
    private const int WS_EX_TOPMOST = 0x00000008;
    private const int WS_EX_TOOLWINDOW = 0x00000080;

    [DllImport("user32.dll")]
    private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    public ToastWindow()
    {
        InitializeComponent();
    }

    public void Show(string text, AppSettings settings)
    {
        _settings = settings;
        _totalDurationMs = settings.DisplayDurationSeconds * 1000.0;

        ApplySettings(text, settings);
        PositionWindow(settings.Position);

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
             Opened += (s, e) =>
             {
                 var handle = TryGetPlatformHandle()?.Handle;
                 if (handle != null)
                 {
                     var exStyle = GetWindowLong(handle.Value, GWL_EXSTYLE);
                     SetWindowLong(handle.Value, GWL_EXSTYLE, exStyle | WS_EX_NOACTIVATE | WS_EX_TOPMOST | WS_EX_TOOLWINDOW);
                 }
             };
        }

        base.Show();

        if (settings.ShowProgressBar)
            StartProgress();

        if (settings.CloseOnClick)
            PointerPressed += (_, _) => CloseToast();

        _closeTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(_totalDurationMs)
        };
        _closeTimer.Tick += (_, _) => CloseToast();
        _closeTimer.Start();

        AnimateIn(settings.AnimationType, settings.Position);
    }

    private void ApplySettings(string text, AppSettings settings)
    {
        MessageBlock.Text = text;

        try
        {
            ToastRoot.Background = new SolidColorBrush(Color.Parse(settings.BackgroundColor));
            MessageBlock.Foreground = new SolidColorBrush(Color.Parse(settings.TextColor));
        }
        catch { }

        ToastRoot.CornerRadius = new CornerRadius(settings.CornerRadius);

        IconBorder.IsVisible = settings.ShowIcon;
        IconText.Text = settings.IconType switch
        {
            "Bell"  => "🔔",
            "Star"  => "⭐",
            "Heart" => "❤",
            "Moon"  => "🌙",
            "Sun"   => "☀",
            _       => "🔔"
        };

        ProgressBar.Background = new SolidColorBrush(Color.Parse(settings.BackgroundColor == "#ffffff" ? "#7c6af5" : "#7c6af5"));
        ProgressBarContainer.IsVisible = settings.ShowProgressBar;
    }

    private void PositionWindow(string position)
    {
        var screen = Screens.Primary;
        if (screen == null) return;

        var workArea = screen.WorkingArea;
        const int margin = 16;

        var width  = 380 + 24;
        var height = 90;

        double x, y;

        switch (position)
        {
            case "TopLeft":
                x = workArea.X + margin;
                y = workArea.Y + margin;
                break;
            case "TopRight":
                x = workArea.X + workArea.Width  - width  - margin;
                y = workArea.Y + margin;
                break;
            case "BottomLeft":
                x = workArea.X + margin;
                y = workArea.Y + workArea.Height - height - margin;
                break;
            case "TopCenter":
                x = workArea.X + (workArea.Width - width) / 2;
                y = workArea.Y + margin;
                break;
            case "BottomCenter":
                x = workArea.X + (workArea.Width - width) / 2;
                y = workArea.Y + workArea.Height - height - margin;
                break;
            default:
                x = workArea.X + workArea.Width  - width  - margin;
                y = workArea.Y + workArea.Height - height - margin;
                break;
        }

        Position = new PixelPoint((int)x, (int)y);
    }

    private void AnimateIn(string animationType, string position)
    {
        var transform = new TranslateTransform(0, 0);
        ToastRoot.RenderTransform = transform;

        double startX = 0, startY = 0;

        switch (animationType)
        {
            case "SlideUp":
                startY = 40;
                break;
            case "SlideLeft":
                startX = position.Contains("Right") ? 60 : -60;
                break;
            case "Bounce":
                startY = 30;
                break;
            default:
                Opacity = 0;
                break;
        }

        if (animationType == "Fade")
        {
            var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(16) };
            double op = 0;
            timer.Tick += (_, _) =>
            {
                op = Math.Min(1.0, op + 0.08);
                Opacity = op;
                if (op >= 1.0) timer.Stop();
            };
            timer.Start();
            return;
        }

        transform.X = startX;
        transform.Y = startY;
        Opacity = 0;

        var animTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(16) };
        double progress = 0;
        animTimer.Tick += (_, _) =>
        {
            progress = Math.Min(1.0, progress + 0.07);
            double eased = EaseOutBack(progress);

            transform.X = startX * (1 - eased);
            transform.Y = startY * (1 - eased);
            Opacity = progress;

            if (progress >= 1.0) animTimer.Stop();
        };
        animTimer.Start();
    }

    private static double EaseOutBack(double t)
    {
        const double c1 = 1.70158;
        const double c3 = c1 + 1;
        return 1 + c3 * Math.Pow(t - 1, 3) + c1 * Math.Pow(t - 1, 2);
    }

    private void StartProgress()
    {
        _elapsed = 0;
        const double tickMs = 50;

        _progressTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(tickMs)
        };

        _progressTimer.Tick += (_, _) =>
        {
            _elapsed += tickMs;
            double ratio = Math.Clamp(1.0 - _elapsed / _totalDurationMs, 0, 1);

            double containerWidth = ProgressBarContainer.Bounds.Width;
            if (containerWidth > 0)
                ProgressBar.Width = containerWidth * ratio;

            if (_elapsed >= _totalDurationMs)
                _progressTimer.Stop();
        };

        _progressTimer.Start();
    }

    private void CloseToast()
    {
        _closeTimer?.Stop();
        _progressTimer?.Stop();

        var timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(16) };
        timer.Tick += (_, _) =>
        {
            Opacity = Math.Max(0, Opacity - 0.1);
            if (Opacity <= 0)
            {
                timer.Stop();
                Close();
            }
        };
        timer.Start();
    }
}
