using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WorkLifeBalance.Shared.Data;
using WorkLifeBalance.Shared.Navigation;

namespace WorkLifeBalance.Features.Statistics
{
    public partial class DayDetailsViewModel : SecondWindowPageBase
    {
        private readonly IWindowService<SecondWindowPageBase> _secondWindowService;
        private readonly StatisticsStore _store;

        [ObservableProperty]
        private ProcessActivityData[]? _processActivities;

        [ObservableProperty]
        private PageActivityData[]? _pageActivities;

        [ObservableProperty]
        private DayData? _loadedDayData;

        private DaysRange _range;

        public DayDetailsViewModel(IWindowService<SecondWindowPageBase> secondWindowService, StatisticsStore store)
        {
            _secondWindowService = secondWindowService;
            _store = store;
            PageHeight = 440;
            PageWidth = 630;
            PageName = "View Day Details";
        }

        public override Task OnPageOpeningAsync(object? args = null)
        {
            if (args is (DaysRange range, DayData day))
            {
                _range = range;
                LoadedDayData = day;
                PageName = $"{day.DateC:MM/dd/yyyy} Activity";
                _ = RequestData(day);
            }

            return Task.CompletedTask;
        }

        private async Task RequestData(DayData day)
        {
            var processes = await _store.ReadProcessDayActivity(day.Date);
            var pages = await _store.ReadUrlDayActivity(day.Date);
            ProcessActivities = processes.OrderByDescending(activity => activity.TimeSpentC).ToArray();
            PageActivities = pages.OrderByDescending(activity => activity.TimeSpentC).ToArray();
        }

        [RelayCommand]
        private void BackToViewDaysPage() => _secondWindowService.OpenWith<DaysViewModel>(_range);
    }
}
