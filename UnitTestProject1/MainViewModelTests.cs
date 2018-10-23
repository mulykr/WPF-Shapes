using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using Polylines;
using System.Windows.Shapes;

namespace PolylinesTests
{
    [TestClass]
    public class MainViewModel_Tests
    {
        [TestMethod]
        public void Test_ViwModelCurentColorProperty()
        {
            MainViewModel viewModel = new MainViewModel();
            viewModel.CurrentColor = Colors.Gold;
            Assert.AreEqual(viewModel.CurrentColor, Colors.Gold);
        }

        [TestMethod]
        public void Test_ViewModelObseverableCollection()
        {
            MainViewModel viewModel = new MainViewModel();
            ObservableCollection<Polyline> collection = new ObservableCollection<Polyline>();
            collection.Add(new Polyline());
            collection.Add(new Polyline());
            viewModel.Shape = collection;
            Assert.AreEqual(viewModel.Shape.Count, 2);
        }

        [TestMethod]
        public void Test_ViewModelClearWindow()
        {
            MainViewModel viewModel = new MainViewModel();
            ObservableCollection<Polyline> collection = new ObservableCollection<Polyline>();
            collection.Add(new Polyline());
            collection.Add(new Polyline());
            collection.Add(new Polyline());
            collection.Add(new Polyline());
            viewModel.Shape = collection;
            viewModel.Shape.Clear();
            Assert.AreEqual(viewModel.Shape.Count, 0);
        }
    }
}
