namespace Los4Carnales.Services;

public class PageTitleService
{
    public string CurrentTitle { get; private set; } = "Mi aplicación";

    public event Action OnChange;

    public void SetTitle(string title)
    {
        CurrentTitle = title;
        OnChange?.Invoke();
    }
}
