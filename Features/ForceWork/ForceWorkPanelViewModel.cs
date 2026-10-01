using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;
using WorkLifeBalance.Features.Tracking;
using WorkLifeBalance.Shared.Navigation;
using WorkLifeBalance.Shared.Scheduling;
namespace WorkLifeBalance.Features.ForceWork
{
    public partial class ForceWorkPanelViewModel : MainWindowDetailsPageBase
    {

        [ObservableProperty]
        private AppState requiredAppState;

        [ObservableProperty]
        private TimeOnly currentStageTimeRemaining;

        [ObservableProperty]
        private TimeOnly totalWorkTimeRemaining;

        private readonly ForceWorkFeature forceWorkFeature;
        private readonly IWindowService<SecondWindowPageBase> secondWindowService;
        private readonly IFeaturesServices featuresServices;

        public ForceWorkPanelViewModel(ForceWorkFeature forceWorkFeature, IWindowService<SecondWindowPageBase> secondWindowService, IFeaturesServices featuresServices)
        {
            this.forceWorkFeature = forceWorkFeature;
            this.secondWindowService = secondWindowService;
            this.featuresServices = featuresServices;
        }

        public override Task OnPageOpeningAsync(object? args = null)
        {
            forceWorkFeature.OnDataUpdated += UpdateDataFromForceWork;
            UpdateDataFromForceWork();
            return Task.CompletedTask;
        }

        public override Task OnPageClosingAsync()
        {
            forceWorkFeature.OnDataUpdated -= UpdateDataFromForceWork;
            featuresServices.RemoveFeature<ForceWorkFeature>();
            return Task.CompletedTask;
        }

        private void UpdateDataFromForceWork()
        {
            RequiredAppState = forceWorkFeature.RequiredAppState;
            CurrentStageTimeRemaining = forceWorkFeature.CurrentStageTimeRemaining;
            TotalWorkTimeRemaining = forceWorkFeature.TotalWorkTimeRemaining;
        }

        [RelayCommand]
        private void EditForceWork()
        {
            secondWindowService.OpenWith<ForceWorkViewModel>();
        }
    }
}
