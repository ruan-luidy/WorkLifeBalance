using System.Windows;
using WorkLifeBalance.Shared.Data;

namespace WorkLifeBalance.Shared.Navigation
{
    public class SecondWindowService : WindowServiceBase<SecondWindowPageBase>, IWindowService<SecondWindowPageBase>
    {
        private readonly DataStorageFeature _dataStorage;

        public SecondWindowService(INavigationService navigationService, DataStorageFeature dataStorage)
            : base(navigationService)
        {
            _dataStorage = dataStorage;
        }

        public override async Task OpenWith<TViewModel>(object? args = null)
        {
            if (_dataStorage.IsClosingApp)
                return;

            var loading = (SecondWindowPageBase)NavigationService.NavigateTo<LoadingViewModel>();
            if (ActivePage != null)
            {
                loading.PageWidth = ActivePage.PageWidth;
                loading.PageHeight = ActivePage.PageHeight;
            }

            LoadedPage = loading;
            await Task.Delay(150);
            await ClearPage();

            var page = (SecondWindowPageBase)NavigationService.NavigateTo<TViewModel>();
            ActivePage = page;
            await Task.Run(async () =>
            {
                await page.OnPageOpeningAsync(args);
                Application.Current.Dispatcher.Invoke(() => LoadedPage = page);
            });
        }

        public override async Task Close()
        {
            if (_dataStorage.IsClosingApp)
                return;

            await ClearPage();
        }
    }
}
