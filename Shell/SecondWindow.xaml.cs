using System.Windows;
using System.Windows.Input;

namespace WorkLifeBalance.Shell
{
    public partial class SecondWindow : Window
    {
        private readonly SecondWindowViewModel _viewModel;

        public SecondWindow(SecondWindowViewModel viewModel)
        {
            Topmost = true;
            DataContext = _viewModel = viewModel;
            viewModel.OnShowView += Show;
            viewModel.OnHideView += Hide;
            InitializeComponent();
        }

        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        // The back button on the side of the mouse
        private void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton != MouseButton.XButton1)
                return;

            _viewModel.GoBackCommand.Execute(null);
            e.Handled = true;
        }
    }
}
