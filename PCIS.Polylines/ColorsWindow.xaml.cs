using System.Windows;

namespace Polylines
{
    public partial class ColorsWindow : Window
    {
        public ColorsWindow(MainViewModel mainViewModel)
        {
            InitializeComponent();
            DataContext = mainViewModel;
        }
    }
}
