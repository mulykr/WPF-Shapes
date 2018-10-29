using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Polylines
{
    public partial class MainWindow : Window
    {

        /// <summary>
        /// Initialize component of main windom
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
            MVM.JustDrawn += RestoreBtn;
            MVM.NewPointAdded += NewPointAdded;
            NewPointAdded();
        }

        public static MainViewModel MVM { get; set; }

        void KeyUpFunction()
        {
            var oldPoints = MainViewModel.selectedPolyline.Points;
            PointCollection newPoints = new PointCollection();
            foreach (var point in oldPoints)
            {
                newPoints.Add(new Point(point.X, point.Y + 5));
            }

            MainViewModel.selectedPolyline.Points = newPoints;
        }

        void KeyDownFunction()
        {
            var oldPoints = MainViewModel.selectedPolyline.Points;
            PointCollection newPoints = new PointCollection();
            foreach (var point in oldPoints)
            {
                newPoints.Add(new Point(point.X, point.Y - 5));
            }

            MainViewModel.selectedPolyline.Points = newPoints;
        }

        void KeyLefFinction()
        {
            var oldPoints = MainViewModel.selectedPolyline.Points;
            PointCollection newPoints = new PointCollection();
            foreach (var point in oldPoints)
            {
                newPoints.Add(new Point(point.X - 5, point.Y));
            }

            MainViewModel.selectedPolyline.Points = newPoints;
        }

        void KeyRightFunction()
        {
            var oldPoints = MainViewModel.selectedPolyline.Points;
            PointCollection newPoints = new PointCollection();
            foreach (var point in MainViewModel.selectedPolyline.Points)
            {
                newPoints.Add(new Point(point.X + 5, point.Y));
            }

            MainViewModel.selectedPolyline.Points = newPoints;
        }

        /// <summary>
        /// Function for stop painting line
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindowName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Space)
            {
                MainViewModel.EndDrawing = true;
                MessageBox.Show("Put the last point, please");
            }
        }

        /// <summary>
        /// Function for dragging by KeyBoard
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyboardClick(object sender, KeyEventArgs e)
        {
            if (MainViewModel.selectedPolyline != null)
            {
                if (e.Key == Key.Up)
                {
                    KeyDownFunction();
                }

                if (e.Key == Key.Down)
                {
                    KeyUpFunction();
                }

                if (e.Key == Key.Left)
                {
                    KeyLefFinction();
                }

                if (e.Key == Key.Right)
                {
                    KeyRightFunction();
                }
            }
        }

        private void MainWindowName_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MVM.Polylines.Count != 0)
                MVM.SaveFile_Command.Execute(null);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.EndDrawing = true;
            FinishDrawingBTN.Content = "Put the last point";
            FinishDrawingBTN.IsEnabled = false;
        }

        private void RestoreBtn()
        {
            FinishDrawingBTN.Content = "Put some points";
            FinishDrawingBTN.IsEnabled = false;
        }

        private void NewPointAdded()
        {
            if (MVM.CurrentPolyline.Points.Count < 1)
            {
                FinishDrawingBTN.Content = "Put some points";
                FinishDrawingBTN.IsEnabled = false;
            }
            else
            {
                FinishDrawingBTN.Content = "Finish drawing";
                FinishDrawingBTN.IsEnabled = true;
            }
        }
    }
}