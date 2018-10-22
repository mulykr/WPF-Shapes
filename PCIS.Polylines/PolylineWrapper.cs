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
