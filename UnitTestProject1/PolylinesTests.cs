namespace PolylinesTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Windows.Media;
    using System.Windows;
    using System.Windows.Media.Imaging;
    using Polylines;
    using System.Collections.Generic;

    [TestClass]
    public class Shape_Tests
    {
        [TestMethod]
        public void Test_PointsProperty()
        {
          
            Shape Shape = new Shape();
            PointCollection points = new PointCollection();
            points.Add(new Point(67, 89));
            points.Add(new Point(67, 45));
            points.Add(new Point(56, 23));
            points.Add(new Point(10, 9));
            points.Add(new Point(100, 145));
            points.Add(new Point(23, 76));
            Shape.Points = points;
            Assert.AreEqual(Shape.Points, points);
        }

        [TestMethod]
        public void Test_ShapeColorProperty()
        {
            Shape Shape = new Shape();
            Shape.ShapeColor = Colors.Red;
            Assert.AreEqual(Shape.ShapeColor, Colors.Red);
        }
    }
}

