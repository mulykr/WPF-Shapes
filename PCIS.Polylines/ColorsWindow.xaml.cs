using System.Windows;

namespace Polylines
{
    public partial class ColorsWindow : Window
    {

        /// <summary>
        /// Initialize color component
        /// </summary>
        /// <param name="mainViewModel"></param>
        public ColorsWindow(MainViewModel mainViewModel)
        {
            InitializeComponent();
            DataContext = mainViewModel;
        }
    }
}
