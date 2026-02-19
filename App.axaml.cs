using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using ToastrForge.Services;
using ToastrForge.ViewModels;
using ToastrForge.Views;

namespace ToastrForge;

public partial class App : Application
{
    private SettingsService _settingsService = null!;
    private TextsService _textsService = null!;
    private ToastService _toastService = null!;
    private MainWindowViewModel _mainVm = null!;
    private MainWindow? _mainWindow;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        _settingsService = new SettingsService();
        _textsService    = new TextsService();
        _toastService    = new ToastService(_settingsService, _textsService);

        _mainVm = new MainWindowViewModel(_settingsService, _toastService, _textsService);

        _toastService.OnShowToast += (text, settings) =>
        {
            Dispatcher.UIThread.Post(() => ShowToast(text, settings));
        };

        _toastService.Start();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            SetupTrayIcon();

            SingleInstanceService.StartServer(() =>
            {
                Dispatcher.UIThread.Post(() =>
                {
                    OpenMainWindow();
                });
            });

            OpenMainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }

    private void SetupTrayIcon()
    {
        var trayIcon = new TrayIcon
        {
            ToolTipText = "Tasbeeh — تسبيح",
            Icon = CreateTrayIcon(),
            IsVisible = true
        };

        var menu = new NativeMenu();

        var openItem = new NativeMenuItem(" فتح الإعدادات");
        openItem.Click += (_, _) => OpenMainWindow();

        var testItem = new NativeMenuItem(" اختبار الإشعار");
        testItem.Click += (_, _) => _toastService.FireNow();

        var separator = new NativeMenuItemSeparator();

        var exitItem = new NativeMenuItem("✕ إنهاء التطبيق");
        exitItem.Click += (_, _) => ExitApp();

        menu.Add(openItem);
        menu.Add(testItem);
        menu.Add(separator);
        menu.Add(exitItem);

        trayIcon.Menu = menu;

        trayIcon.Clicked += (_, _) => OpenMainWindow();

        TrayIcon.SetIcons(this, new TrayIcons { trayIcon });
    }

    private void OpenMainWindow()
    {
        if (_mainWindow == null || !_mainWindow.IsVisible)
        {
            _mainWindow = new MainWindow(_mainVm);
            _mainWindow.Closing += (_, e) =>
            {
                e.Cancel = true;
                _mainWindow.Hide();
            };
            _mainWindow.Show();
        }
        else
        {
            _mainWindow.Activate();
            _mainWindow.WindowState = WindowState.Normal;
        }
    }

    private void ShowToast(string text, Models.AppSettings settings)
    {
        var toast = new ToastWindow();
        toast.Show(text, settings);
    }

    private void ExitApp()
    {
        _toastService.Stop();
        _toastService.Dispose();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.Shutdown();
    }

    private static WindowIcon? CreateTrayIcon()
    {
        try
        {
            var uri = new Uri("avares://Tasbeeh/Assets/tasbeeh.png");
            if (Avalonia.Platform.AssetLoader.Exists(uri))
            {
                using var stream = Avalonia.Platform.AssetLoader.Open(uri);
                return new WindowIcon(stream);
            }
            
            uri = new Uri("avares://ToastrForge/Assets/tasbeeh.png");
            if (Avalonia.Platform.AssetLoader.Exists(uri))
            {
                using var stream = Avalonia.Platform.AssetLoader.Open(uri);
                return new WindowIcon(stream);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading tray icon: {ex.Message}");
        }
        
        return null;
    }
}
