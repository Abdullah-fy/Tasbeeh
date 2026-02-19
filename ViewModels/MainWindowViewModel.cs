using Avalonia.Media;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ToastrForge.Models;
using ToastrForge.Services;
    
namespace ToastrForge.ViewModels;

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly SettingsService _settingsService;
        private readonly ToastService _toastService;
        private AppSettings _settings;

        public enum Language { Arabic, English }

        private Language _currentLanguage = Language.Arabic;
        public Language CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                if (_currentLanguage != value)
                {
                    _currentLanguage = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(FlowDirection));
                    OnPropertyChanged(nameof(LabelAppName));
                    OnPropertyChanged(nameof(LabelSettings));
                    OnPropertyChanged(nameof(LabelTexts));
                    OnPropertyChanged(nameof(LabelEnable));
                    OnPropertyChanged(nameof(LabelEvery));
                    OnPropertyChanged(nameof(LabelMinute));
                    OnPropertyChanged(nameof(LabelTest));
                    OnPropertyChanged(nameof(LabelSave));
                    OnPropertyChanged(nameof(LabelAppearance));
                    OnPropertyChanged(nameof(LabelBackgroundColor));
                    OnPropertyChanged(nameof(LabelTextColor));
                    OnPropertyChanged(nameof(LabelCornerRadius));
                    OnPropertyChanged(nameof(LabelFont));
                    OnPropertyChanged(nameof(LabelContent));
                    OnPropertyChanged(nameof(LabelShowIcon));
                    OnPropertyChanged(nameof(LabelIcon));
                    OnPropertyChanged(nameof(LabelBehavior));
                    OnPropertyChanged(nameof(LabelDuration));
                    OnPropertyChanged(nameof(LabelAnimation));
                    OnPropertyChanged(nameof(LabelPosition));
                    OnPropertyChanged(nameof(LabelCloseOnClick));
                    OnPropertyChanged(nameof(LabelShowProgressBar));
                    OnPropertyChanged(nameof(LabelStartWithSystem));
                    OnPropertyChanged(nameof(LabelLanguageToggle));
                    OnPropertyChanged(nameof(LabelHowToUse));
                    OnPropertyChanged(nameof(LabelHowToUseContent));
                    OnPropertyChanged(nameof(LabelUninstall));
                }
            }
        }

        public FlowDirection FlowDirection => _currentLanguage == Language.Arabic ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;

        public void ToggleLanguage()
        {
            CurrentLanguage = CurrentLanguage == Language.Arabic ? Language.English : Language.Arabic;
        }

        public string LabelAppName => _currentLanguage == Language.Arabic ? "تسبيح" : "Tasbeeh";
        public string LabelSettings => _currentLanguage == Language.Arabic ? "⚙ الإعدادات" : "⚙ Settings";
        public string LabelTexts => _currentLanguage == Language.Arabic ? "✦ النصوص" : "✦ Texts";
        public string LabelEnable => _currentLanguage == Language.Arabic ? "تفعيل الإشعارات" : "Enable Notifications";
        public string LabelEvery => _currentLanguage == Language.Arabic ? "كل" : "Every";
        public string LabelMinute => _currentLanguage == Language.Arabic ? "دقيقة" : "Minutes";
        public string LabelTest => _currentLanguage == Language.Arabic ? "اختبار الإشعار" : "Test Notification";
        public string LabelSave => _currentLanguage == Language.Arabic ? "💾 حفظ الإعدادات" : "💾 Save Settings";
        public string LabelAppearance => _currentLanguage == Language.Arabic ? "المظهر" : "Appearance";
        public string LabelBackgroundColor => _currentLanguage == Language.Arabic ? "لون الخلفية" : "Background Color";
        public string LabelTextColor => _currentLanguage == Language.Arabic ? "لون النص" : "Text Color";
        public string LabelCornerRadius => _currentLanguage == Language.Arabic ? "انحناء الزوايا" : "Corner Radius";
        public string LabelFont => _currentLanguage == Language.Arabic ? "الخط" : "Font";
        public string LabelContent => _currentLanguage == Language.Arabic ? "المحتوى" : "Content";
        public string LabelShowIcon => _currentLanguage == Language.Arabic ? "إظهار الأيقونة" : "Show Icon";
        public string LabelIcon => _currentLanguage == Language.Arabic ? "الأيقونة" : "Icon";
        public string LabelBehavior => _currentLanguage == Language.Arabic ? "السلوك" : "Behavior";
        public string LabelDuration => _currentLanguage == Language.Arabic ? "المدة" : "Duration";
        public string LabelAnimation => _currentLanguage == Language.Arabic ? "نوع الحركة" : "Animation Type";
        public string LabelPosition => _currentLanguage == Language.Arabic ? "المكان" : "Position";
        public string LabelCloseOnClick => _currentLanguage == Language.Arabic ? "إغلاق عند النقر" : "Close on Click";
        public string LabelShowProgressBar => _currentLanguage == Language.Arabic ? "شريط التقدم" : "Show Progress Bar";
        public string LabelStartWithSystem => _currentLanguage == Language.Arabic ? "تشغيل مع النظام" : "Start with System";
        public string LabelLanguageToggle => _currentLanguage == Language.Arabic ? "English" : "عربي";
        public string LabelHowToUse => _currentLanguage == Language.Arabic ? "📖 طريقة الاستخدام" : "📖 How to Use";
        public string LabelHowToUseContent => _currentLanguage == Language.Arabic ?
            """
            أهلاً بك في تطبيق "تسبيح"! 📿
            
            هدف التطبيق:
            تذكيرك بذكر الله والصلاة على النبي ﷺ أثناء استخدامك للكمبيوتر، بطريقة غير مزعجة وأنيقة.

            كيفية الاستخدام:
            1. الإعدادات (Settings):
               - يمكنك تفعيل أو تعطيل الإشعارات من الزر الرئيسي.
               - حدد الفترة الزمنية (بالدقائق) بين كل إشعار وآخر.
               - اختر مكان ظهور الإشعار (أسفل اليمين، أعلى اليسار، ...).

            2. المظهر (Appearance):
               - خصص ألوان الإشعار وكيف يظهر (الألوان، الخطوط، الأيقونات).
               - جرب زر "اختبار الإشعار" لترى التغييرات فوراً.

            3. النصوص (Texts):
               - يمكنك إضافة أذكار خاصة بك أو تعديل الموجود.
               - التطبيق يأتي بمجموعة كبيرة من الأذكار الجاهزة.

            4. بدأ التشغيل:
               - ننصح بتفعيل خيار "تشغيل مع النظام" ليعمل البرنامج تلقائياً عند فتح الجهاز.

            5. حذف البرنامج:
               - يمكنك حذف إعدادات البرنامج وإيقافه تماماً من زر "حذف الإعدادات والإغلاق" في تبويب الإعدادات، ثم حذف المجلد يدوياً.

            نسأل الله أن يكون هذا العمل خالصاً لوجهه الكريم، ولا تنسونا من صالح دعائكم.
            """ :
            """
            Welcome to "Tasbeeh"! 📿

            Goal:
            Remind you to remember Allah and pray upon the Prophet ﷺ while using your computer, in a subtle and elegant way.

            How to Use:
            1. Settings:
               - Enable/Disable notifications via the main toggle.
               - Set the interval (in minutes) between reminders.
               - Choose where the notification appears on screen.

            2. Appearance:
               - Customize the look (Colors, Fonts, Icons).
               - Use the "Test Notification" button to preview changes instantly.

            3. Texts:
               - Add your own Dhikr or edit existing ones.
               - The app comes with a pre-loaded collection.

            4. Startup:
               - We recommend enabling "Start with System" so the app runs automatically when you log in.


        5. Uninstall:
           - You can remove app settings and stop it completely using the "Remove App Settings & Exit" button in Settings, then delete the folder manually.

        May Allah accept this deed from us and you.
        """;

    public TextsViewModel TextsVm { get; }

    public MainWindowViewModel(SettingsService settingsService, ToastService toastService, TextsService textsService)
    {
        _settingsService = settingsService;
        _toastService = toastService;
        _settings = _settingsService.Load();
        TextsVm = new TextsViewModel(textsService);
        _startWithSystem = _settingsService.IsStartWithSystemEnabled();
        
        LoadFromSettings();

        if (!_settings.HasCreatedShortcut && 
            System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
        {
            CreateDesktopShortcut();
            _settings.HasCreatedShortcut = true;
            _settingsService.Save(_settings);
        }
    }

    public void CreateDesktopShortcut()
    {
        if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
        {
            try
            {
                string exePath = System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName ?? "";
                if (string.IsNullOrEmpty(exePath)) return;

                string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.DesktopDirectory);
                string shortcutPath = System.IO.Path.Combine(desktopPath, "Tasbeeh.lnk");

                string script = $"/c powershell -Command \"$ws = New-Object -ComObject WScript.Shell; $s = $ws.CreateShortcut('{shortcutPath}'); $s.TargetPath = '{exePath}'; $s.WorkingDirectory = '{System.IO.Path.GetDirectoryName(exePath)}'; $s.Save()\"";
                
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = script,
                    CreateNoWindow = true,
                    UseShellExecute = false
                });
            }
            catch { }
        }
    }

    private Color _backgroundColorObj = Color.Parse("#1e1e1e");
    public Color BackgroundColorObj
    {
        get => _backgroundColorObj;
        set
        {
            _backgroundColorObj = value;
            OnPropertyChanged();
            _backgroundColor = value.ToString();
            OnPropertyChanged(nameof(BackgroundColor));
        }
    }

    private string _backgroundColor = "#1e1e1e";
    public string BackgroundColor
    {
        get => _backgroundColor;
        set
        {
            _backgroundColor = value;
            OnPropertyChanged();
            if (Color.TryParse(value, out Color c))
            {
                _backgroundColorObj = c;
                OnPropertyChanged(nameof(BackgroundColorObj));
            }
        }
    }

    private Color _textColorObj = Color.Parse("#ffffff");
    public Color TextColorObj
    {
        get => _textColorObj;
        set
        {
            _textColorObj = value;
            OnPropertyChanged();
            _textColor = value.ToString();
            OnPropertyChanged(nameof(TextColor));
        }
    }

    private string _textColor = "#ffffff";
    public string TextColor
    {
        get => _textColor;
        set
        {
            _textColor = value;
            OnPropertyChanged();
            if (Color.TryParse(value, out Color c))
            {
                _textColorObj = c;
                OnPropertyChanged(nameof(TextColorObj));
            }
        }
    }

    private int _cornerRadius = 19;
    public int CornerRadius
    {
        get => _cornerRadius;
        set { _cornerRadius = Math.Clamp(value, 0, 50); OnPropertyChanged(); }
    }

    private string _selectedFont = "Default (Arws)";
    public string SelectedFont
    {
        get => _selectedFont;
        set { _selectedFont = value; OnPropertyChanged(); }
    }

    public ObservableCollection<string> AvailableFonts { get; } =
    [
        "Default (Arws)",
        "Traditional Arabic",
        "Arial",
        "Segoe UI",
        "Calibri"
    ];

    private bool _showIcon = true;
    public bool ShowIcon
    {
        get => _showIcon;
        set { _showIcon = value; OnPropertyChanged(); }
    }

    private string _selectedIcon = "Bell";
    public string SelectedIcon
    {
        get => _selectedIcon;
        set { _selectedIcon = value; OnPropertyChanged(); }
    }

    public ObservableCollection<string> AvailableIcons { get; } =
        ["Bell", "Star", "Heart", "Moon", "Sun"];

    private int _displayDurationSeconds = 5;
    public int DisplayDurationSeconds
    {
        get => _displayDurationSeconds;
        set { _displayDurationSeconds = Math.Clamp(value, 2, 30); OnPropertyChanged(); }
    }

    private string _animationType = "SlideUp";
    public string AnimationType
    {
        get => _animationType;
        set { _animationType = value; OnPropertyChanged(); }
    }

    public ObservableCollection<string> AnimationTypes { get; } =
        ["SlideUp", "Fade", "Bounce", "SlideLeft"];

    private decimal? _intervalMinutes = 30;
    public decimal? IntervalMinutes
    {
        get => _intervalMinutes;
        set
        {
            if (value.HasValue)
            {
                if (value.Value <= 0)
                {
                    ShowIntervalError();
                    _intervalMinutes = 30; 
                }
                else
                {
                    _intervalMinutes = Math.Clamp(value.Value, 1, 1440);
                    IntervalErrorText = "";
                }
            }
            else
            {
                 _intervalMinutes = null;
            }
            OnPropertyChanged();
        }
    }

    private string _intervalErrorText = "";
    public string IntervalErrorText
    {
        get => _intervalErrorText;
        set { _intervalErrorText = value; OnPropertyChanged(); }
    }

    private void ShowIntervalError()
    {
        IntervalErrorText = _currentLanguage == Language.Arabic 
            ? "⚠ لا يمكن أن يكون 0. سيتم تعيين القيمة الافتراضية 30." 
            : "⚠ Cannot be 0. Defaulting to 30.";
            

        System.Threading.Tasks.Task.Delay(3000).ContinueWith(_ => 
        {
            Avalonia.Threading.Dispatcher.UIThread.Post(() => IntervalErrorText = "");
        });
    }

    private string _position = "BottomRight";
    public string Position
    {
        get => _position;
        set { _position = value; OnPropertyChanged(); }
    }

    public ObservableCollection<string> Positions { get; } =
        ["BottomRight", "BottomLeft", "TopRight", "TopLeft", "TopCenter", "BottomCenter"];

    private bool _isEnabled = true;
    public bool IsEnabled
    {
        get => _isEnabled;
        set { _isEnabled = value; OnPropertyChanged(); }
    }

    public bool CanStartWithSystem => 
        System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows) ||
        System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux);

    private bool _startWithSystem;
    public bool StartWithSystem
    {
        get => _startWithSystem;
        set
        {
            if (_startWithSystem != value)
            {
                _startWithSystem = value;
                OnPropertyChanged();
                
                _settingsService.SetStartWithSystem(value);
            }
        }
    }

    private bool _closeOnClick = true;
    public bool CloseOnClick
    {
        get => _closeOnClick;
        set { _closeOnClick = value; OnPropertyChanged(); }
    }

    private bool _showProgressBar = true;
    public bool ShowProgressBar
    {
        get => _showProgressBar;
        set { _showProgressBar = value; OnPropertyChanged(); }
    }

    public void SaveSettings()
    {
        _settings.BackgroundColor = BackgroundColor;
        _settings.TextColor = TextColor;
        _settings.CornerRadius = CornerRadius;
        _settings.Font = SelectedFont;
        _settings.ShowIcon = ShowIcon;
        _settings.IconType = SelectedIcon;
        _settings.DisplayDurationSeconds = DisplayDurationSeconds;
        _settings.AnimationType = AnimationType;
        _settings.IntervalMinutes = (int)(IntervalMinutes ?? 30);
        _settings.Position = Position;
        _settings.IsEnabled = IsEnabled;
        _settings.StartWithSystem = StartWithSystem;
        _settings.CloseOnClick = CloseOnClick;
        _settings.ShowProgressBar = ShowProgressBar;

        _settingsService.Save(_settings);
        _toastService.Restart();
    }

    public void TestToast() => _toastService.FireNow();

    private bool _isUninstallConfirming = false;
    public string LabelUninstall => _isUninstallConfirming 
        ? (_currentLanguage == Language.Arabic ? "⚠ هل أنت متأكد؟ اضغط مرة أخرى للتنفيذ" : "⚠ Are you sure? Click again to confirm")
        : (_currentLanguage == Language.Arabic ? "⚠ حذف إعدادات التطبيق والإغلاق" : "⚠ Remove App Settings & Exit");

    public void UninstallApp()
    {
        if (!_isUninstallConfirming)
        {
            _isUninstallConfirming = true;
            OnPropertyChanged(nameof(LabelUninstall));

            System.Threading.Tasks.Task.Delay(3000).ContinueWith(_ => 
            {
                if (_isUninstallConfirming)
                {
                    _isUninstallConfirming = false;
                    Avalonia.Threading.Dispatcher.UIThread.Post(() => OnPropertyChanged(nameof(LabelUninstall)));
                }
            });
            return;
        }

        _settingsService.RemoveAppAttributes();
        
        if (Avalonia.Application.Current?.ApplicationLifetime is Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown();
        }
    }

    private void LoadFromSettings()
    {
        BackgroundColor = _settings.BackgroundColor;
        TextColor = _settings.TextColor;
        CornerRadius = _settings.CornerRadius;
        SelectedFont = _settings.Font;
        ShowIcon = _settings.ShowIcon;
        SelectedIcon = _settings.IconType;
        DisplayDurationSeconds = _settings.DisplayDurationSeconds;
        AnimationType = _settings.AnimationType;
        IntervalMinutes = _settings.IntervalMinutes;
        Position = _settings.Position;
        IsEnabled = _settings.IsEnabled;
        StartWithSystem = _settings.StartWithSystem;
        CloseOnClick = _settings.CloseOnClick;
        ShowProgressBar = _settings.ShowProgressBar;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
