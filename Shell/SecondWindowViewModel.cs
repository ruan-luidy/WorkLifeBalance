using CommunityToolkit.Mvvm.Input;
using WorkLifeBalance.Shared.Navigation;

namespace WorkLifeBalance.Shell
{
    public partial class SecondWindowViewModel : NewWindowBase<SecondWindowPageBase>
    {
        public SecondWindowViewModel(IWindowService<SecondWindowPageBase> windowService)
            : base(windowService)
        {
            windowService.OnPageLoaded += () => OnShowView?.Invoke();
        }

        public Action? OnShowView { get; set; } = () => { };
        public Action? OnHideView { get; set; } = () => { };

        [RelayCommand]
        protected override async Task CloseWindow()
        {
            await WindowService.Close();
            OnHideView?.Invoke();
        }
    }
}
