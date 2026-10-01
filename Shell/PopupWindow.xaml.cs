using System.Windows;
using System.Windows.Input;
namespace WorkLifeBalance.Shell
{
    /// <summary>
    /// Interaction logic for PopupWindow.xaml
    /// </summary>
    public partial class PopupWindow : Window
    {
        private readonly PopupWindowViewModel ViewModel;

        public PopupWindow(PopupWindowViewModel viewModel)
        {
            Topmost = true;
            ViewModel = viewModel;
            DataContext = ViewModel;
            ViewModel.OnShowView += Show;
            ViewModel.OnHideView += Hide;
            InitializeComponent();
        }

        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
