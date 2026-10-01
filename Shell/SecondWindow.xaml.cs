using System.Windows;
using System.Windows.Input;

namespace WorkLifeBalance.Shell
{
    public partial class SecondWindow : Window
    {
        public SecondWindow(SecondWindowViewModel viewModel)
        {
            Topmost = true;
            DataContext = viewModel;
            viewModel.OnShowView += Show;
            viewModel.OnHideView += Hide;
            InitializeComponent();
        }

        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
