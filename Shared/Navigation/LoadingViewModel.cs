using System.Threading.Tasks;

namespace WorkLifeBalance.Shared.Navigation
{
    public class LoadingViewModel : SecondWindowPageBase
    {
        public LoadingViewModel()
        {
            PageName = "Loading...";
        }

        public override Task OnPageClosingAsync()
        {
            return Task.CompletedTask;
        }

        public override Task OnPageOpeningAsync(object? args = null)
        {
            return Task.CompletedTask;
        }
    }
}
