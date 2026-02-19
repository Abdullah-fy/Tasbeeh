using System;
using System.Text.Json.Serialization;

namespace ToastrForge.Models;

public class ToastText
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;

    [JsonPropertyName("isProtected")]
    public bool IsProtected { get; set; } = false;
}
