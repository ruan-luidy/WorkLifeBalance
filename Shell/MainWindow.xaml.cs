using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace WorkLifeBalance.Shell
{
    public partial class MainWindow : Window, IDisposable
    {
        private readonly MainViewModel _viewModel;
        private readonly NotifyIcon _notifyIcon;

        public MainWindow(MainViewModel viewModel)
        {
            Topmost = true;
            DataContext = _viewModel = viewModel;

            _notifyIcon = new NotifyIcon { Icon = System.Drawing.Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location) };
            _notifyIcon.Click += OnNotifyIconClick;

            SetStartUpLocation();
            InitializeComponent();
        }

        public void Dispose()
        {
            _notifyIcon.Dispose();
            GC.SuppressFinalize(this);
        }

        // bottom left corner of the main screen
        private void SetStartUpLocation()
        {
            Left = 0;
            Top = (int)SystemParameters.PrimaryScreenHeight - 297;
        }

        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void OnNotifyIconClick(object? sender, EventArgs e)
        {
            Show();
            WindowState = WindowState.Normal;
            _notifyIcon.Visible = false;
        }

        private void HideWindow(object sender, RoutedEventArgs e)
        {
            if (_viewModel.MinimizeToTray)
            {
                Hide();
                _notifyIcon.Visible = true;
            }
            else
            {
                WindowState = WindowState.Minimized;
            }
        }
    }
}
