using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using MahApps.Metro.IconPacks;
using WorkLifeBalance.Shared.Icons;

namespace WorkLifeBalance.Features.Tracking
{
    // State.WorkingBrush, State.RestingBrush or State.IdleBrush from the design system
    public class AppStateToBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is AppState or string ? Application.Current.FindResource($"State.{value}Brush") : Brushes.Transparent;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotSupportedException();
    }

    public class AppStateToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value switch
        {
            AppState.Working => PhosphorExtension.Get(PackIconPhosphorIconsKind.BriefcaseBold),
            AppState.Resting => PhosphorExtension.Get(PackIconPhosphorIconsKind.CoffeeBold),
            AppState.Idle => PhosphorExtension.Get(PackIconPhosphorIconsKind.MoonBold),
            _ => Geometry.Empty,
        };

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotSupportedException();
    }
}
