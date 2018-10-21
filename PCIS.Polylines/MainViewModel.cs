using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Controls;

namespace Polylines
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Polyline> Polylines { get; set; }
        private Polyline CurrentPolyline { get; set; }
        private uint CountEdges { get; set; }
        private Color currentColor;
        public Color CurrentColor
        {
            get
            {
                return currentColor;
            }
            set
            {
                currentColor = value;
                OnPropertyChanged("CurrentColor");
            }
        }

        //Painting
        public ICommand DrawClick_Command { get; private set; }
        public ICommand ApplyColor_Command { get; set; }

        //File Menu
        public ICommand ClearWindow_Command { get; private set; }
        public ICommand OpenFile_Command { get; private set; }
        public ICommand SaveFile_Command { get; private set; }
        public ICommand CloseWindow_Command { get; private set; }

        //Selecting and draging polylines
        public ICommand SelectPolyline_Command { get; private set; }
        public ICommand Drag_Command { get; private set; }
        private bool AllowDragging { get; set; }
        private Point MousePosition { get; set; }
        private Polyline SelectedHexagone { get; set; }

        // End Drawing Polyline
        public static bool EndDrawing { get; set; } = false;

        public MainViewModel()
        {
            Polylines = new ObservableCollection<Polyline>();
            CountEdges = 0;
            CurrentColor = Colors.Red;
            CurrentPolyline = new Polyline();
            ClearWindow_Command = new RelayCommand(ClearWindow);
            OpenFile_Command = new RelayCommand(OpenFile);
            SaveFile_Command = new RelayCommand(SaveFile);
            CloseWindow_Command = new RelayCommand(CloseWindow);
            DrawClick_Command = new RelayCommand(DrawClick);
            ApplyColor_Command = new RelayCommand(ApplyColor);

            SelectPolyline_Command = new RelayCommand(SelectPolyline);
            Drag_Command = new RelayCommand(Drag);
        }

        //Painting
        private void DrawClick(object obj)
        {
            Point mousePoint = Mouse.GetPosition((IInputElement)obj);
            CurrentPolyline.Stroke = Brushes.Black;
            CurrentPolyline.Points.Add(mousePoint);
            if (EndDrawing && CurrentPolyline.Points.Count >= 2)
            {
                ColorsWindow colorWin = new ColorsWindow(this);
                if (colorWin.ShowDialog() == true)
                {
                    CurrentPolyline.Stroke = new SolidColorBrush(CurrentColor);
                }
                CurrentPolyline.Name = String.Format("Polyline_{0}", Polylines.Count + 1);
                Polylines.Add(CurrentPolyline);
                CurrentPolyline = new Polyline();
                OnPropertyChanged("Polylines");
                CountEdges = 0;
                EndDrawing = false;
            }
        }

        private void ApplyColor(object obj)
        {
            ColorsWindow colorsWindow = (ColorsWindow)obj;
            colorsWindow.DialogResult = true;
            colorsWindow.Close();
        }

        //File Menu
        private void ClearWindow(object obj)
        {
            throw new NotImplementedException();
        }

        private void OpenFile(object obj)
        {
            throw new NotImplementedException();
        }

        private void SaveFile(object obj)
        {
            throw new NotImplementedException();
        }

        private void CloseWindow(object obj)
        {
            throw new NotImplementedException();
        }

        //Selecting and draging hexogones
        private void SelectPolyline(object obj)
        {
            Polyline curHexagone = obj as Polyline;
            curHexagone.MouseDown += new MouseButtonEventHandler(Hexagone_MouseDown);
            OnPropertyChanged("Polylines");
        }

        private void Drag(object obj)
        {
            Canvas plane = (obj as Canvas);
            plane.MouseMove += new MouseEventHandler(Canvas_MouseMove);
            plane.MouseUp += new MouseButtonEventHandler(Canvas_MouseUp);
        }

        //Events
        void Hexagone_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AllowDragging = true;
            SelectedHexagone = sender as Polyline;
            MousePosition = e.GetPosition(SelectedHexagone);
        }

        void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            AllowDragging = false;
        }

        void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (AllowDragging)
            {
                Canvas.SetLeft(SelectedHexagone, e.GetPosition(sender as IInputElement).X - MousePosition.X);
                Canvas.SetTop(SelectedHexagone, e.GetPosition(sender as IInputElement).Y - MousePosition.Y);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
