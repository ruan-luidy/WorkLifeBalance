namespace WorkLifeBalance.Shared.Navigation
{
    public interface INavigationService
    {
        ViewModelBase NavigateTo<T>() where T : ViewModelBase;
    }
}
