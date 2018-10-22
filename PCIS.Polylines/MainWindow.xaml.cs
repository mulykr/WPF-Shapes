using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

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
            if (e.Key==Key.Space)
            {
                MainViewModel.EndDrawing = true;
                MessageBox.Show("Put the last point, please");
            }
        }

        private void KeyboardClick(object sender, KeyEventArgs e)
        {
            if (MainViewModel.selectedPolyline != null)
            {
                if (e.Key == Key.Up)
                {
                    var oldPoints = MainViewModel.selectedPolyline.Points;
                    PointCollection newPoints = new PointCollection();
                    foreach (var point in oldPoints)
                    {
                        newPoints.Add(new Point(point.X, point.Y - 5));
                    }

                    MainViewModel.selectedPolyline.Points = newPoints;
                }

                if (e.Key == Key.Down)
                {
                    var oldPoints = MainViewModel.selectedPolyline.Points;
                    PointCollection newPoints = new PointCollection();
                    foreach (var point in oldPoints)
                    {
                        newPoints.Add(new Point(point.X, point.Y + 5));
                    }

                    MainViewModel.selectedPolyline.Points = newPoints;
                }

                if (e.Key == Key.Left)
                {
                    var oldPoints = MainViewModel.selectedPolyline.Points;
                    PointCollection newPoints = new PointCollection();
                    foreach (var point in oldPoints)
                    {
                        newPoints.Add(new Point(point.X - 5, point.Y));
                    }

                    MainViewModel.selectedPolyline.Points = newPoints;
                }

                if (e.Key == Key.Right)
                {
                    var oldPoints = MainViewModel.selectedPolyline.Points;
                    PointCollection newPoints = new PointCollection();
                    foreach (var point in MainViewModel.selectedPolyline.Points)
                    {
                        newPoints.Add(new Point(point.X + 5, point.Y));
                    }

                    MainViewModel.selectedPolyline.Points = newPoints;
                }
            }
        }
    }
}