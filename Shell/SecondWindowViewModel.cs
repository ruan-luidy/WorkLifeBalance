using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;
using WorkLifeBalance.Shared.Navigation;
namespace WorkLifeBalance.Shell;

public partial class SecondWindowViewModel : NewWindowBase<SecondWindowPageBase>
{
    public Action? OnShowView { get; set; } = () => { };
    public Action? OnHideView { get; set; } = () => { };

    public SecondWindowViewModel(IWindowService<SecondWindowPageBase> windowService) : base(windowService)
    {
        windowService.OnPageLoaded += () => { OnShowView?.Invoke(); };
    }

    [RelayCommand]
    protected override async Task CloseWindow()
    {
        await WindowService.Close();
        OnHideView?.Invoke();
    }
}