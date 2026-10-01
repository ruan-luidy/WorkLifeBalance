using CommunityToolkit.Mvvm.ComponentModel;
namespace WorkLifeBalance.Shared.Navigation;

public abstract partial class PopupWindowPageBase : PageViewModelBase
{
    [ObservableProperty]
    private double pageWidth = 260;
        
    [ObservableProperty]
    private double pageHeight = 360;
        
    [ObservableProperty]
    private string pageName = "Page";
}