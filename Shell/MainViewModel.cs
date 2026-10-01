using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using WorkLifeBalance.Features.CloseApp;
using WorkLifeBalance.Features.ForceState;
using WorkLifeBalance.Features.Options;
using WorkLifeBalance.Features.Statistics;
using WorkLifeBalance.Features.Tracking;
using WorkLifeBalance.Shared.Data;
using WorkLifeBalance.Shared.Native;
using WorkLifeBalance.Shared.Navigation;
using WorkLifeBalance.Shared.Scheduling;
namespace WorkLifeBalance.Shell
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private string? dateText;

        [ObservableProperty]
        private TimeOnly elapsedWorkTime;

        [ObservableProperty]
        private TimeOnly elapsedRestTime;

        [ObservableProperty]
        private TimeOnly elapsedIdleTime;

        [ObservableProperty]
        private AppState appState = AppState.Resting;

        public bool MinimizeToTray 
        {
            get 
            { 
                return dataStorageFeature.Settings.MinimizeToTrayC;
            } 
        }

        public IWindowService<MainWindowDetailsPageBase> MainWindowDetailsService { get; set; }

        private readonly AppStateHandler appStateHandler;
        private readonly LowLevelHandler lowLevelHandler;
        private readonly DataStorageFeature dataStorageFeature;
        private readonly TimeTrackerFeature timeTrackerFeature;
        private readonly IFeaturesServices featuresServices;
        private readonly IWindowService<SecondWindowPageBase> secondWindowService;

        public MainViewModel(AppTimer mainTimer, LowLevelHandler lowLevelHandler, DataStorageFeature dataStorageFeature, TimeTrackerFeature timeTrackerFeature, IWindowService<SecondWindowPageBase> secondWindowService, AppStateHandler appStateHandler, IWindowService<MainWindowDetailsPageBase> mainWindowDetailsService, IFeaturesServices featuresServices)
        {
            this.lowLevelHandler = lowLevelHandler;
            this.dataStorageFeature = dataStorageFeature;
            this.timeTrackerFeature = timeTrackerFeature;
            this.secondWindowService = secondWindowService;
            this.appStateHandler = appStateHandler;
            this.MainWindowDetailsService = mainWindowDetailsService;
            this.featuresServices = featuresServices;

            DateText = $"Today: {dataStorageFeature.TodayData.DateC:MM/dd/yyyy}";

            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            appStateHandler.OnStateChanges += OnStateChanged;

            dataStorageFeature.OnSaving += OnSavingData;
            dataStorageFeature.OnSaved += OnDataSaved;

            timeTrackerFeature.OnSpentTimeChange += OnTimeSpentChanged;
        }

        private void OnStateChanged(AppState state)
        {
            AppState = state;
        }

        private void OnSavingData()
        {
            DateText = "Saving data...";
        }

        private void OnDataSaved()
        {
            DateText = $"Today: {dataStorageFeature.TodayData.DateC:MM/dd/yyyy}";
        }

        private void OnTimeSpentChanged()
        {
            ElapsedWorkTime = dataStorageFeature.TodayData.WorkedAmmountC;
            ElapsedRestTime = dataStorageFeature.TodayData.RestedAmmountC;
            ElapsedIdleTime = dataStorageFeature.TodayData.IdleAmmountC;
        }

        [RelayCommand]
        private void ToggleForceState()
        {
            if (featuresServices.IsFeaturePresent<ForceStateFeature>())
            {
                featuresServices.RemoveFeature<ForceStateFeature>();
            }
            else
            {
                featuresServices.AddFeature<ForceStateFeature>();
            }
        }

        [RelayCommand]
        private void OpenViewDataWindow()
        {
            secondWindowService.OpenWith<StatisticsViewModel>();
        }

        [RelayCommand]
        private void OpenOptionsWindow()
        {
            secondWindowService.OpenWith<OptionsViewModel>();
        }

        [RelayCommand]
        private void CloseApp()
        {
            secondWindowService.OpenWith<CloseWarningViewModel>();
        }
    }
}
