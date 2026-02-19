using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using System.Threading.Tasks;
using ToastrForge.ViewModels;

namespace ToastrForge.Views;

public partial class MainWindow : Window
{
    private MainWindowViewModel _vm = null!;

    public MainWindow()
    {
        InitializeComponent();
    }

    public MainWindow(MainWindowViewModel vm) : this()
    {
        _vm = vm;
        DataContext = vm;
    }

    private void ShowSettingsTab(object? sender, RoutedEventArgs e)
    {
        SettingsTab.IsVisible = true;
        TextsTab.IsVisible = false;
        HowToUseTab.IsVisible = false;
        
        BtnTabSettings.Classes.Add("Active");
        BtnTabTexts.Classes.Remove("Active");
        BtnTabHowToUse.Classes.Remove("Active");
    }

    private void ShowTextsTab(object? sender, RoutedEventArgs e)
    {
        SettingsTab.IsVisible = false;
        TextsTab.IsVisible = true;
        HowToUseTab.IsVisible = false;
        
        BtnTabSettings.Classes.Remove("Active");
        BtnTabTexts.Classes.Add("Active");
        BtnTabHowToUse.Classes.Remove("Active");
    }

    private void ShowHowToUseTab(object? sender, RoutedEventArgs e)
    {
        SettingsTab.IsVisible = false;
        TextsTab.IsVisible = false;
        HowToUseTab.IsVisible = true;
        
        BtnTabSettings.Classes.Remove("Active");
        BtnTabTexts.Classes.Remove("Active");
        BtnTabHowToUse.Classes.Add("Active");
    }

    private void CreateShortcut_Click(object? sender, RoutedEventArgs e)
    {
        _vm.CreateDesktopShortcut();
        if (sender is Button btn) btn.Content = "✓ Done / تم";
    }

    private void SaveSettings_Click(object? sender, RoutedEventArgs e)
    {
        _vm.SaveSettings();
        if (sender is Button btn)
        {
            var original = btn.Content;
            btn.Content = "✓ تم الحفظ!";
            _ = Task.Delay(1500).ContinueWith(_ =>
                Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                    btn.Content = original));
        }
    }

    private void TestToast_Click(object? sender, RoutedEventArgs e)
    {
        _vm.TestToast();
    }

    private void AddText_Click(object? sender, RoutedEventArgs e)
    {
        _vm.TextsVm.AddText();
    }

    private void DeleteText_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is string id)
            _vm.TextsVm.DeleteText(id);
    }

    private void TitleBar_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            BeginMoveDrag(e);
    }

    private void MinimizeWindow(object? sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void CloseWindow(object? sender, RoutedEventArgs e)
    {
        Hide();
    }
}
