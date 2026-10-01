using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using WorkLifeBalance.Features.ForceWork;
using WorkLifeBalance.Features.Settings;
using WorkLifeBalance.Features.WorkApps;
using WorkLifeBalance.Shared.Native;
using WorkLifeBalance.Shared.Navigation;
namespace WorkLifeBalance.Features.Options
{
    public partial class OptionsViewModel : SecondWindowPageBase
    {
        private readonly IWindowService<SecondWindowPageBase> secondWindowService;
        private readonly LowLevelHandler LowLevelHandler;
        public OptionsViewModel(IWindowService<SecondWindowPageBase> secondWindowService, LowLevelHandler lowLevelHandler)
        {
            this.secondWindowService = secondWindowService;
            PageHeight = 320;
            PageWidth = 250;
            PageName = "Options";
            LowLevelHandler = lowLevelHandler;
        }
        public override Task OnPageClosingAsync() => Task.CompletedTask;

        public override Task OnPageOpeningAsync(object? args = null) => Task.CompletedTask;

        [RelayCommand]
        private void OpenSettings()
        {
            secondWindowService.OpenWith<SettingsViewModel>();
        }

        [RelayCommand]
        private void ConfigureAutoDetect()
        {
            secondWindowService.OpenWith<WorkAppsViewModel>();
        }

        [RelayCommand]
        private void OpenDonations()
        {
            LowLevelHandler.OpenLink("https://buymeacoffee.com/RoberBot");
        }

        [RelayCommand]
        private void OpenFeedback()
        {

            LowLevelHandler.OpenLink(@"https://docs.google.com/forms/d/e/1FAIpQLSfkPDHOLysWAPLZc9pdLFyRmiFxlVBN0xefXFcZ7XACOnnPhw/viewform?usp=sf_link");
        }

        [RelayCommand]
        private void OpenForceWork()
        {
            secondWindowService.OpenWith<ForceWorkViewModel>();
        }
    }
}
