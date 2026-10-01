using CommunityToolkit.Mvvm.ComponentModel;

namespace WorkLifeBalance.Shared.Navigation
{
    public abstract partial class SecondWindowPageBase : PageViewModelBase
    {
        [ObservableProperty]
        private string _pageName = "Page";
    }
}
