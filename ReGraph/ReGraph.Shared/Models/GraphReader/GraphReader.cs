using ReGraph.Models.GraphDrawer;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace ReGraph.Models.GraphReader
{
    public class GraphReader
    {
        private Point StartPoint;
        private WriteableBitmap InputImage;
        private GraphDrawer.GraphDrawer Drawer;
        public double xScale;
        public double yScale;
        public GraphReader(WriteableBitmap img, GraphDrawer.GraphDrawer drawer)
        {
            this.InputImage = img;
            this.Drawer = drawer;
        }

        public void RecognizeLine(Point clickedPoint, Color color)
        {
            Line line = new Line();

            Drawer.addSolidLine(line);
        }

        public void SetMiddlePoint(Point middlePoint)
        {
            RescalePoint(middlePoint);
            StartPoint = middlePoint;
        }

        private void RescalePoint(Point p)
        {
            p.X = p.X * xScale;
            p.Y = p.Y * yScale;
        }
    }
}
