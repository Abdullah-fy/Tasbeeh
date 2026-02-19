using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using ToastrForge.Models;

namespace ToastrForge.Services;

public class TextsService
{
    private static readonly string AppFolder = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "ToastrForge");

    private static readonly string UserTextsPath = Path.Combine(AppFolder, "user_texts.json");

    private static readonly string CoreTextsPath = Path.Combine(
        AppContext.BaseDirectory, "Assets", "core_texts.json");

    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    public List<ToastText> LoadCoreTexts()
    {
        try
        {
            if (!File.Exists(CoreTextsPath)) return GetHardcodedCoreTexts();

            var json = File.ReadAllText(CoreTextsPath);
            using var doc = JsonDocument.Parse(json);
            var arr = doc.RootElement.GetProperty("texts");

            return arr.EnumerateArray()
                .Select((el, i) => new ToastText
                {
                    Id = $"core_{i}",
                    Text = el.GetString() ?? string.Empty,
                    IsProtected = true
                })
                .Where(t => !string.IsNullOrWhiteSpace(t.Text))
                .ToList();
        }
        catch
        {
            return GetHardcodedCoreTexts();
        }
    }

    private static List<ToastText> GetHardcodedCoreTexts() =>
    [
        new() { Id = "core_0", Text = "صلِّ على النبي ﷺ",                                          IsProtected = true },
        new() { Id = "core_1", Text = "سبحان الله وبحمده",                                          IsProtected = true },
        new() { Id = "core_2", Text = "سبحان الله العظيم",                                          IsProtected = true },
        new() { Id = "core_3", Text = "لا إله إلا الله",                                            IsProtected = true },
        new() { Id = "core_4", Text = "الله أكبر",                                                  IsProtected = true },
        new() { Id = "core_5", Text = "الحمد لله",                                                  IsProtected = true },
        new() { Id = "core_6", Text = "أستغفر الله العظيم",                                         IsProtected = true },
        new() { Id = "core_7", Text = "لا حول ولا قوة إلا بالله",                                  IsProtected = true },
        new() { Id = "core_8", Text = "سبحان الله والحمد لله ولا إله إلا الله والله أكبر",         IsProtected = true },
        new() { Id = "core_9", Text = "اللهم صلِّ وسلم على نبينا محمد",                             IsProtected = true },
    ];

    public List<ToastText> LoadUserTexts()
    {
        try
        {
            if (!File.Exists(UserTextsPath)) return [];

            var json = File.ReadAllText(UserTextsPath);
            return JsonSerializer.Deserialize<List<ToastText>>(json, JsonOpts) ?? [];
        }
        catch
        {
            return [];
        }
    }

    public void SaveUserTexts(List<ToastText> texts)
    {
        try
        {
            Directory.CreateDirectory(AppFolder);
            var userOnly = texts.Where(t => !t.IsProtected).ToList();
            var json = JsonSerializer.Serialize(userOnly, JsonOpts);
            File.WriteAllText(UserTextsPath, json);
        }
        catch { }
    }

    public void AddUserText(string text, List<ToastText> existing)
    {
        if (string.IsNullOrWhiteSpace(text)) return;
        existing.Add(new ToastText { Text = text.Trim(), IsProtected = false });
        SaveUserTexts(existing);
    }

    public void DeleteUserText(string id, List<ToastText> existing)
    {
        var item = existing.FirstOrDefault(t => t.Id == id);
        if (item == null || item.IsProtected) return;
        existing.Remove(item);
        SaveUserTexts(existing);
    }

    public List<ToastText> GetAllTexts()
    {
        var core = LoadCoreTexts();
        var user = LoadUserTexts();
        return [.. core, .. user];
    }

    public string GetRandomText()
    {
        var all = GetAllTexts();
        if (all.Count == 0) return "سبحان الله";
        return all[Random.Shared.Next(all.Count)].Text;
    }

    public static string GetUserTextsPath() => UserTextsPath;
}
