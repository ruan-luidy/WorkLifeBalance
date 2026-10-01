using System.Threading.Tasks;
namespace WorkLifeBalance.Shared.Navigation;

public class PopupWindowService : WindowServiceBase<PopupWindowPageBase>,
    IWindowService<PopupWindowPageBase>
{
    public PopupWindowService(INavigationService navigationService) : base(navigationService)
    {
    }

    public override async Task OpenWith<TVm>(object? args = null)
    {
        await ClearPage();

        activePage = (PopupWindowPageBase)navigationService.NavigateTo<TVm>();

        await App.Current.Dispatcher.InvokeAsync(async () =>
        {
            await activePage.OnPageOpeningAsync(args);
            LoadedPage = activePage;
        });
    }

    protected virtual async Task ClearPage()
    {
        if (LoadedPage != null)
        {
            await LoadedPage.OnPageClosingAsync();
            LoadedPage = null;
        }
    }
}