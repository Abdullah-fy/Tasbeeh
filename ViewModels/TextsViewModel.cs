using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using ToastrForge.Models;
using ToastrForge.Services;

namespace ToastrForge.ViewModels;

public class TextsViewModel : INotifyPropertyChanged
{
    private readonly TextsService _textsService;

    public TextsViewModel(TextsService textsService)
    {
        _textsService = textsService;
        LoadAll();
    }

    public ObservableCollection<ToastText> CoreTexts { get; } = [];

    public ObservableCollection<ToastText> UserTexts { get; } = [];

    private string _newText = string.Empty;
    public string NewText
    {
        get => _newText;
        set { _newText = value; OnPropertyChanged(); OnPropertyChanged(nameof(CanAdd)); }
    }

    public bool CanAdd => !string.IsNullOrWhiteSpace(NewText);

    public void AddText()
    {
        if (!CanAdd) return;

        var userList = UserTexts.ToList();
        _textsService.AddUserText(NewText, userList);

        UserTexts.Clear();
        foreach (var t in userList)
            UserTexts.Add(t);

        NewText = string.Empty;
    }

    public void DeleteText(string id)
    {
        var item = UserTexts.FirstOrDefault(t => t.Id == id);
        if (item == null || item.IsProtected) return;

        var userList = UserTexts.ToList();
        _textsService.DeleteUserText(id, userList);

        UserTexts.Remove(item);
    }

    private void LoadAll()
    {
        CoreTexts.Clear();
        foreach (var t in _textsService.LoadCoreTexts())
            CoreTexts.Add(t);

        UserTexts.Clear();
        foreach (var t in _textsService.LoadUserTexts())
            UserTexts.Add(t);
    }

    public string CoreCount => $"{CoreTexts.Count} نص محمي";
    public string UserCount => $"{UserTexts.Count} نص مضاف";

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
