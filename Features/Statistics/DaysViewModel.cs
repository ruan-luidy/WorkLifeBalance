using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WorkLifeBalance.Shared.Data;
using WorkLifeBalance.Shared.Navigation;

namespace WorkLifeBalance.Features.Statistics
{
    public enum DaysRange
    {
        All,
        CurrentMonth,
        PreviousMonth,
    }

    public partial class DaysViewModel : SecondWindowPageBase
    {
        private readonly IWindowService<SecondWindowPageBase> _secondWindowService;
        private readonly StatisticsStore _store;
        private readonly DataStorageFeature _dataStorage;

        [ObservableProperty]
        private int[]? _filterDays;

        [ObservableProperty]
        private int _selectedDay;

        [ObservableProperty]
        private int[]? _filterMonths;

        [ObservableProperty]
        private int _selectedMonth;

        [ObservableProperty]
        private int[]? _filterYears;

        [ObservableProperty]
        private int _selectedYear;

        private DayData[] _allDays = [];

        public DaysViewModel(IWindowService<SecondWindowPageBase> secondWindowService, StatisticsStore store, DataStorageFeature dataStorage)
        {
            _secondWindowService = secondWindowService;
            _store = store;
            _dataStorage = dataStorage;
            PageWidth = 600;
            PageHeight = 500;
            SetFilterValues();
        }

        public ObservableCollection<DayData> LoadedData { get; set; } = new();

        public override async Task OnPageOpeningAsync(object? args = null)
        {
            if (args is DaysRange range)
                await RequestData(range);
        }

        // 0 means "any" in the three filters
        private void SetFilterValues()
        {
            FilterDays = Enumerable.Range(0, 31).ToArray();
            FilterMonths = Enumerable.Range(0, 13).ToArray();
            FilterYears = Enumerable.Range(2021, _dataStorage.TodayData.DateC.Year - 2020).Reverse().Append(0).ToArray();
        }

        private async Task RequestData(DaysRange range)
        {
            var currentDate = _dataStorage.TodayData.DateC;
            var previousDate = currentDate.AddMonths(-1);

            var days = new List<DayData>();
            switch (range)
            {
                case DaysRange.All:
                    days = await _store.ReadMonth();
                    PageName = "All Months Days";
                    break;
                case DaysRange.CurrentMonth:
                    days = await _store.ReadMonth(currentDate.ToString("MM"), currentDate.ToString("yyyy"));
                    PageName = "Current Month Days";
                    break;
                case DaysRange.PreviousMonth:
                    days = await _store.ReadMonth(previousDate.ToString("MM"), previousDate.ToString("yyyy"));
                    PageName = "Previous Month Days";
                    break;
            }

            SelectedMonth = 0;
            SelectedDay = 0;
            SelectedYear = 0;
            days.Reverse();
            LoadedData = new ObservableCollection<DayData>(days);
            _allDays = days.ToArray();
        }

        [RelayCommand]
        private void ViewDay(DayData day)
        {
            day.ConvertSaveDataToUsableData();
            _secondWindowService.OpenWith<DayDetailsViewModel>(day);
        }

        [RelayCommand]
        private void ApplyFilters()
        {
            IEnumerable<DayData> days = _allDays;
            if (SelectedMonth != 0)
                days = days.Where(day => day.DateC.Month == SelectedMonth);

            if (SelectedDay != 0)
                days = days.Where(day => day.DateC.Day == SelectedDay);

            if (SelectedYear != 0)
                days = days.Where(day => day.DateC.Year == SelectedYear);

            LoadedData.Clear();
            foreach (var day in days)
                LoadedData.Add(day);
        }
    }
}
