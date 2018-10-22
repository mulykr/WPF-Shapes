using System;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Polylines
{
    [Serializable]
    public class Shape
    {

        private PointCollection points;
        public PointCollection Points
        {

            get
            {
                return points;
            }
            set
            {
                points = value;
            }
        }

        public Color ShapeColor { get; set; }
        public Shape() { }
        public Shape(Polyline figure)
        {
            Points = figure.Points;
            ShapeColor = (figure.Stroke as SolidColorBrush).Color;
        }
    }
}
