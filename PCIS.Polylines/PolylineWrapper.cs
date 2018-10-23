using System;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Polylines
{

    /// <summary>
    /// Shape class
    /// </summary>
    [Serializable]
    public class Shape
    {

        /// <summary>
        /// Private collection of points
        /// </summary>
        private PointCollection points;

        /// <summary>
        /// Public collection of points
        /// </summary>
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

        /// <summary>
        /// Color of lines
        /// </summary>
        public Color ShapeColor { get; set; }

        public Shape() { }

        /// <summary>
        /// Initialize our lines and it color
        /// </summary>
        /// <param name="figure"></param>
        public Shape(Polyline figure)
        {
            Points = figure.Points;
            ShapeColor = (figure.Stroke as SolidColorBrush).Color;
        }
    }
}
