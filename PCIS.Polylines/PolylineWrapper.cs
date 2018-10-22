using System;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Polylines
{
    [Serializable]
    public class Hexagone
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
                if (value.Count != 6)
                {
                    //throw new ArgumentException("Number of point must be 6");
                }
                points = value;
            }
        }

        public Color HexagoneColor { get; set; }
        public Hexagone() { }
        public Hexagone(Polyline figure)
        {
            Points = figure.Points;
            HexagoneColor = (figure.Stroke as SolidColorBrush).Color;
        }
    }
}
