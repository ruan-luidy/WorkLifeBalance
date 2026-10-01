using CommunityToolkit.Mvvm.ComponentModel;

namespace WorkLifeBalance.Shared.Navigation
{
    public abstract partial class SecondWindowPageBase : PageViewModelBase
    {
        [ObservableProperty]
        private double _pageWidth = 250;

        [ObservableProperty]
        private double _pageHeight = 300;

        [ObservableProperty]
        private string _pageName = "Page";
    }
}
