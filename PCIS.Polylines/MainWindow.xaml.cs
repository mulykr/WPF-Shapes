using System.Windows;
using System.Windows.Input;

namespace Polylines
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private void MainWindowName_KeyDown(object sender, KeyEventArgs e)
        {
            MainViewModel.EndDrawing = true;
            MessageBox.Show("Put the last point, please");
        }
    }
}