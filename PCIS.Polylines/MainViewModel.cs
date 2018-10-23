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

    /// <summary>
    /// Main view model class
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Collection of shapes
        /// </summary>
        public ObservableCollection<Polyline> Polylines { get; set; }

        /// <summary>
        /// Current shape
        /// </summary>
        private Polyline CurrentPolyline { get; set; }

        /// <summary>
        /// Count of points in shape
        /// </summary>
        private uint CountEdges { get; set; }

        /// <summary>
        /// Selected shape
        /// </summary>
        public static Polyline selectedPolyline = null;

        /// <summary>
        /// Current color of shape
        /// </summary>
        private Color currentColor;
        public ObservableCollection<Polyline> Shape;

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

        
        /// <summary>
        /// Commands for painting
        /// </summary>
        public ICommand DrawClick_Command { get; private set; }

        public ICommand ApplyColor_Command { get; set; }

        
        /// <summary>
        /// File menu commands
        /// </summary>
        public ICommand ClearWindow_Command { get; private set; }

        public ICommand OpenFile_Command { get; private set; }

        public ICommand SaveFile_Command { get; private set; }

        public ICommand CloseWindow_Command { get; private set; }


        /// <summary>
        /// Commands for selecting and draging shapes
        /// </summary>
        public ICommand SelectPolyline_Command { get; private set; }

        public ICommand Drag_Command { get; private set; }

        private bool AllowDragging { get; set; }

        private Point MousePosition { get; set; }

        private Polyline SelectedShape { get; set; }


        /// <summary>
        /// The variable responsible for the end of drawing
        /// </summary>
        public static bool EndDrawing { get; set; } = false;


        /// <summary>
        /// Initialize main view model
        /// </summary>
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

        
        /// <summary>
        /// Method for painting shape
        /// </summary>
        /// <param name="obj"></param>
        private void DrawClick(object obj)
        {
            try
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
                if (EndDrawing && CurrentPolyline.Points.Count < 2)
                {
                    throw new InvalidOperationException("Not enough points");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error: {e.Message}");
            }
        }

        /// <summary>
        /// Methid for apply color for shape
        /// </summary>
        /// <param name="obj"></param>
        private void ApplyColor(object obj)
        {
            ColorsWindow colorsWindow = (ColorsWindow)obj;
            colorsWindow.DialogResult = true;
            colorsWindow.Close();
        }

        
        /// <summary>
        /// Clear window
        /// </summary>
        /// <param name="obj"></param>
        private void ClearWindow(object obj)
        {
            Polylines.Clear();
            OnPropertyChanged("Polylines");
        }

        /// <summary>
        /// Open file with shape
        /// </summary>
        /// <param name="obj"></param>
        private void OpenFile(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = ".xml";
            openFileDialog.Filter = "XML documents (.xml)|*.xml";
            if (openFileDialog.ShowDialog() == true)
            {
                string fileName = openFileDialog.FileName;
                List<Shape> polylines = new List<Shape>();
                XmlSerializer serializer = new XmlSerializer(typeof(List<Shape>));
                using (XmlReader reader = XmlReader.Create(fileName))
                {
                    polylines = (List<Shape>)serializer.Deserialize(reader);
                }
                Polylines.Clear();
                for (int i = 0; i < polylines.Count; ++i)
                {
                    Polylines.Add(new Polyline() { Name = String.Format("Polyline_{0}", i + 1), Stroke = new SolidColorBrush(polylines[i].ShapeColor), Points = polylines[i].Points });
                }
                OnPropertyChanged("Polylines");
            }
        }

        /// <summary>
        /// Save shapes
        /// </summary>
        /// <param name="obj"></param>
        private void SaveFile(object obj)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = ".xml";
            saveFileDialog.FileName = "New_shapes.xml";
            saveFileDialog.Filter = "XML documents (.xml)|*.xml";
            if (saveFileDialog.ShowDialog() == true)
            {
                string fileName = saveFileDialog.FileName;
                List<Shape> polylines = new List<Shape>();
                foreach (var elem in Polylines)
                {
                    polylines.Add(new Shape(elem));
                }
                using (Stream outputFile = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Shape>));
                    serializer.Serialize(outputFile, polylines);
                }
            }
        }

        /// <summary>
        /// Close window
        /// </summary>
        /// <param name="obj"></param>
        private void CloseWindow(object obj)
        {
            (obj as MainWindow).Close();
        }

        /// <summary>
        /// Select a figure from an existing list
        /// </summary>
        /// <param name="obj"></param>
        private void SelectPolyline(object obj)
        {
            Polyline curShape = obj as Polyline;
            selectedPolyline = curShape;
            curShape.MouseDown += new MouseButtonEventHandler(Shape_MouseDown);
            OnPropertyChanged("Polylines");
        }

        /// <summary>
        /// Drag shape by mouse
        /// </summary>
        /// <param name="obj"></param>
        private void Drag(object obj)
        {
            Canvas plane = (obj as Canvas);
            plane.MouseMove += new MouseEventHandler(Canvas_MouseMove);
            plane.MouseUp += new MouseButtonEventHandler(Canvas_MouseUp);
        }

        //Events
        void Shape_MouseDown(object sender, MouseButtonEventArgs e)
        {
            AllowDragging = true;
            SelectedShape = sender as Polyline;
            MousePosition = e.GetPosition(SelectedShape);
        }

        void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            AllowDragging = false;
        }

        void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (AllowDragging)
            {
                Canvas.SetLeft(SelectedShape, e.GetPosition(sender as IInputElement).X - MousePosition.X);
                Canvas.SetTop(SelectedShape, e.GetPosition(sender as IInputElement).Y - MousePosition.Y);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
