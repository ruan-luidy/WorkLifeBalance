using CommunityToolkit.Mvvm.ComponentModel;
namespace WorkLifeBalance.Shared.Navigation
{
    public abstract partial class SecondWindowPageBase : PageViewModelBase
    {
        [ObservableProperty]
        private double pageWidth = 250;
        
        [ObservableProperty]
        private double pageHeight = 300;
        
        [ObservableProperty]
        private string pageName = "Page";
    }
}
