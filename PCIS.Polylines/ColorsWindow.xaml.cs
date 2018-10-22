namespace Polylines
{
    using System.Windows;

    public partial class ColorsWindow : Window
    {
        public ColorsWindow(MainViewModel mainViewModel)
        {
            this.InitializeComponent();
            this.DataContext = mainViewModel;
        }
    }
}
