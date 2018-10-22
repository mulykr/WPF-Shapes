namespace Polylines
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using System.Xml;
    using System.Xml.Serialization;
    using Microsoft.Win32;

    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Polyline> Polylines { get; set; }

        private Polyline CurrentPolyline { get; set; }

        private uint CountEdges { get; set; }

        public static Polyline selectedPolyline = null;

        private Color currentColor;

        public Color CurrentColor
        {
            get
            {
                return currentColor;
            }
            set
            {
                this.currentColor = value;
                this.OnPropertyChanged("CurrentColor");
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
            this.Polylines = new ObservableCollection<Polyline>();
            this.CountEdges = 0;
            this.CurrentColor = Colors.Red;
            this.CurrentPolyline = new Polyline();
            this.ClearWindow_Command = new RelayCommand(this.ClearWindow);
            this.OpenFile_Command = new RelayCommand(this.OpenFile);
            this.SaveFile_Command = new RelayCommand(this.SaveFile);
            this.CloseWindow_Command = new RelayCommand(this.CloseWindow);
            this.DrawClick_Command = new RelayCommand(this.DrawClick);
            this.ApplyColor_Command = new RelayCommand(this.ApplyColor);

            this.SelectPolyline_Command = new RelayCommand(this.SelectPolyline);
            this.Drag_Command = new RelayCommand(this.Drag);
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
            if(EndDrawing && CurrentPolyline.Points.Count < 2)
            {
                throw new InvalidOperationException("Not enough points");
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
            Polylines.Clear();
            OnPropertyChanged("Polylines");
        }

        private void OpenFile(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".xml";
            openFileDialog.Filter = "XML documents (.xml)|*.xml";
            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = openFileDialog.FileName;
                List<Hexagone> polylines = new List<Hexagone>();
                XmlSerializer serializer = new XmlSerializer(typeof(List<Hexagone>));
                using (XmlReader reader = XmlReader.Create(fileName))
                {
                    polylines = (List<Hexagone>)serializer.Deserialize(reader);
                }
                Polylines.Clear();
                for (int i = 0; i < polylines.Count; ++i)
                {
                    Polylines.Add(new Polyline() { Name = String.Format("Polyline_{0}", i + 1), Stroke = new SolidColorBrush(polylines[i].HexagoneColor), Points = polylines[i].Points });
                }
                OnPropertyChanged("Polylines");
            }
        }

        private void SaveFile(object obj)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = ".xml";
            saveFileDialog.FileName = "New_shapes.xml";
            saveFileDialog.Filter = "XML documents (.xml)|*.xml";
            if (saveFileDialog.ShowDialog() == true)
            {
                string fileName = saveFileDialog.FileName;
                List<Hexagone> polylines = new List<Hexagone>();
                foreach (var elem in this.Polylines)
                {
                    polylines.Add(new Hexagone(elem));
                }
                using (Stream outputFile = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Hexagone>));
                    serializer.Serialize(outputFile, polylines);
                }
            }
        }

        private void CloseWindow(object obj)
        {
            (obj as MainWindow).Close();
        }

        //Selecting and draging hexogones
        private void SelectPolyline(object obj)
        {
            Polyline curHexagone = obj as Polyline;
            selectedPolyline = curHexagone;
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
            this.AllowDragging = true;
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
                Canvas.SetLeft(SelectedHexagone, e.GetPosition(sender as IInputElement).X - this.MousePosition.X);
                Canvas.SetTop(SelectedHexagone, e.GetPosition(sender as IInputElement).Y - this.MousePosition.Y);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
