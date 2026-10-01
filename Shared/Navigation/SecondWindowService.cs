using System.Threading.Tasks;
using WorkLifeBalance.Shared.Data;
namespace WorkLifeBalance.Shared.Navigation
{
    public class SecondWindowService : WindowServiceBase<SecondWindowPageBase>, IWindowService<SecondWindowPageBase>
    {
        private readonly DataStorageFeature dataStorageFeature;
        
        public SecondWindowService(INavigationService navigation, DataStorageFeature dataStorageFeature) : base(navigation)
        {
            this.dataStorageFeature = dataStorageFeature;
        }
        
        public override async Task OpenWith<TVm>(object? args = null)
        {
            if (dataStorageFeature.IsClosingApp) return;

            SecondWindowPageBase loading = (SecondWindowPageBase)navigationService.NavigateTo<LoadingViewModel>();
            
            if(activePage != null)
            {
                loading.PageWidth = activePage.PageWidth;
                loading.PageHeight= activePage.PageHeight;
            }

            LoadedPage = loading;

            await Task.Delay(150);

            await ClearPage();

            activePage = (SecondWindowPageBase)navigationService.NavigateTo<TVm>();

            await Task.Run(async () =>
            {
                await activePage.OnPageOpeningAsync(args);
                App.Current.Dispatcher.Invoke(() =>
                {
                    LoadedPage = activePage;
                });
            });
        }

        public override async Task Close()
        {
            if (dataStorageFeature.IsClosingApp) return;
            await ClearPage();
        }
    }
}
