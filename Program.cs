using Avalonia;
using Avalonia.Controls;
using Avalonia.ReactiveUI;
using System;

namespace ToastrForge;

class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        const string appName = "com.toastrforge.tasbeeh"; // Unique ID
        bool createdNew;

        using (var mutex = new System.Threading.Mutex(true, appName, out createdNew))
        {
            if (!createdNew)
            {
                Services.SingleInstanceService.NotifyExistingInstance();
                return;
            }

            BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args, ShutdownMode.OnExplicitShutdown);
        }
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
}
