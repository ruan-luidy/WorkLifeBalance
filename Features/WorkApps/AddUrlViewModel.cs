using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using WorkLifeBalance.Shared.Navigation;
namespace WorkLifeBalance.Features.WorkApps;

public partial class AddUrlViewModel: PopupWindowPageBase
{
    private readonly IWindowService<PopupWindowPageBase> windowService;

    [ObservableProperty] 
    private string urls = string.Empty;

    public AddUrlViewModel(IWindowService<PopupWindowPageBase> windowService)
    {
        PageHeight = 320;
        PageWidth = 300;
        PageName = "Enter \"working\" URLs";
        this.windowService = windowService;
    }

    public override Task OnPageOpeningAsync(object? args = null)
    {
        if (args is string urlStr)
        {
            Urls = urlStr;
        }
        
        return Task.CompletedTask;
    }

    public override Task OnPageClosingAsync()
    {
        WeakReferenceMessenger.Default.Send(new UrlsMessage(urls));
        WeakReferenceMessenger.Default.Send(new PopupCloseMessage());
        return Task.CompletedTask;
    }

    public void Receive(UrlsMessage? message)
    {
       if(message != null)
       {
            Urls = message.Value;
       }
    }

    [RelayCommand]
    private async Task ClosePage()
    {
        await windowService.Close();
    }
}