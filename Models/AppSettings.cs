using System.Text.Json.Serialization;

namespace ToastrForge.Models;

public class AppSettings
{
    [JsonPropertyName("backgroundColor")]
    public string BackgroundColor { get; set; } = "#1e1e1e";

    [JsonPropertyName("textColor")]
    public string TextColor { get; set; } = "#ffffff";

    [JsonPropertyName("cornerRadius")]
    public int CornerRadius { get; set; } = 19;

    [JsonPropertyName("font")]
    public string Font { get; set; } = "Default (Arws)";

    [JsonPropertyName("showIcon")]
    public bool ShowIcon { get; set; } = true;

    [JsonPropertyName("iconType")]
    public string IconType { get; set; } = "Bell";

    [JsonPropertyName("displayDurationSeconds")]
    public int DisplayDurationSeconds { get; set; } = 5;

    [JsonPropertyName("animationType")]
    public string AnimationType { get; set; } = "SlideUp";

    [JsonPropertyName("intervalMinutes")]
    public int IntervalMinutes { get; set; } = 30;

    [JsonPropertyName("position")]
    public string Position { get; set; } = "BottomRight";

    [JsonPropertyName("isEnabled")]
    public bool IsEnabled { get; set; } = true;

    [JsonPropertyName("startWithSystem")]
    public bool StartWithSystem { get; set; } = true;

    [JsonPropertyName("closeOnClick")]
    public bool CloseOnClick { get; set; } = true;

    [JsonPropertyName("showProgressBar")]
    public bool ShowProgressBar { get; set; } = true;

    [JsonPropertyName("hasCreatedShortcut")]
    public bool HasCreatedShortcut { get; set; } = false;
}
