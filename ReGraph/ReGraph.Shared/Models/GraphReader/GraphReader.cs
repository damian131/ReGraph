using Caliburn.Micro;
using ReGraph.Models.GraphDrawer;
using ReGraph.ViewModels;
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
        private double horizontalScale;
        private double verticalScale;
        private RGB[,] ImageData;
        public GraphReader()
        {       
        }

        public void RecognizeLine(Point clickedPoint, Color color)
        {
            Line line = new Line();
            RescalePoint(clickedPoint);

            Drawer.addSolidLine(line);
        }

        public void SetMiddlePoint(Point middlePoint)
        {
            InputImage = (IoC.GetInstance(typeof(MainViewModel), null) as MainViewModel).InputGraph.Image;
            Drawer = (IoC.GetInstance(typeof(MainViewModel), null) as MainViewModel).graphDrawer;
            ImageData = new RGB[InputImage.PixelWidth, InputImage.PixelHeight];
            byte[] byteArray = InputImage.ToByteArray();
            int width = InputImage.PixelWidth;
            int height = InputImage.PixelHeight;
            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < height; ++j)
                {
                    int index = j * (width) * 4 + i * 4;
                    ImageData[i, j] = new RGB(byteArray[index + 2], byteArray[index + 1], byteArray[index]);
                }
            }

            RescalePoint(middlePoint);
            StartPoint = middlePoint;

            InputImage.FillEllipseCentered((int)middlePoint.X, (int)middlePoint.Y, 8, 8, Colors.Red);

            double graphWidth = InputImage.PixelWidth - StartPoint.X;
            double graphHeight = InputImage.PixelHeight - StartPoint.Y;
            double graphHorizontalRangeSize = (double)(Drawer.HorizontalAxis.ActualMaximum - Drawer.HorizontalAxis.ActualMinimum);
            double graphVerticalRangeSize = (double)(Drawer.VerticalAxis.ActualMaximum - Drawer.VerticalAxis.ActualMinimum);

            horizontalScale = graphHorizontalRangeSize / graphWidth;
            verticalScale = graphVerticalRangeSize / graphHeight;
        }

        private void RescalePoint(Point p)
        {
            p.X = p.X * xScale;
            p.Y = p.Y * yScale;
        }
        private void TransformatePointValue(Point p)
        {
            p.Y = StartPoint.Y - p.Y;
            p.X = p.X - StartPoint.X;
            p.Y = p.Y * verticalScale + (double)Drawer.VerticalAxis.ActualMinimum;
            p.X = p.X * horizontalScale + (double)Drawer.HorizontalAxis.ActualMinimum;
        }
    }
}
