using Caliburn.Micro;
using ReGraph.Models.GraphDrawer;
using ReGraph.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Windows.UI;
using Windows.UI.Popups;
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
        private int width;
        private int height;
        private const int TOLERANCE = 30;
        private int LIMIT;
        public GraphReader()
        {       
        }

        public async void RecognizeLine(Point clickedPoint, Color color)
        {
            Line line = new Line();
            RescalePoint(clickedPoint);
            clickedPoint = ValidClickedPoint(clickedPoint);
            if (clickedPoint != null)
            {
                System.Diagnostics.Debug.WriteLine(ImageData[(int)clickedPoint.X, (int)clickedPoint.Y].ToString());
                InputImage.FillEllipseCentered((int)clickedPoint.X, (int)clickedPoint.Y, 5, 5, Colors.Pink);
                List<Point> points = GetLinePoints((int)clickedPoint.X, (int)clickedPoint.Y);
                foreach (var p in points)
                {
                    TransformatePointValue(p);
                }
                line.Points = points;
                Drawer.addSolidLine(line);
            }
            else
            {
                MessageDialog msg = new MessageDialog("Wrong point clicked");
                await msg.ShowAsync();
            }
        }
        public void SetMiddlePoint(Point middlePoint)
        {
            InputImage = (IoC.GetInstance(typeof(MainViewModel), null) as MainViewModel).InputGraph.Image;
            Drawer = (IoC.GetInstance(typeof(MainViewModel), null) as MainViewModel).graphDrawer;
            ImageData = new RGB[InputImage.PixelWidth, InputImage.PixelHeight];
            byte[] byteArray = InputImage.ToByteArray();
            width = InputImage.PixelWidth;
            height = InputImage.PixelHeight;
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
            LIMIT = (int)(0.2 * width * height);
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

        public Point ValidClickedPoint(Point clickedPoint)
        {
            if (CountSimilarPoints((int)clickedPoint.X, (int)clickedPoint.Y) > LIMIT)
            {
                clickedPoint = FoundNewPoint((int)clickedPoint.X, (int)clickedPoint.Y);
            }
            return clickedPoint;
        }

        private int CountSimilarPoints(int x, int y)
        {
            int result = 0;
            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < height; ++j)
                {
                    if (ImageData[i, j].getDifference(ImageData[x, y]) < TOLERANCE)
                    {
                        ++result;
                    }
                }
            }
            return result;
        }
        private Point FoundNewPoint(int x, int y)
        {
            for (int i = 1; i <= 5; ++i)
            {
                Point p = SearchInRegion(x, y, i);
                if (p != null)
                {
                    return p;
                }
            }
            return null;
        }
        private Point SearchInRegion(int x, int y, int areaSize)
        {
            for (int i = -areaSize; i <= areaSize; ++i)
            {
                for (int j = -areaSize; j <= areaSize; ++j)
                {
                    if (ImageData[x + i, y + j].getDifference(ImageData[x, y]) > TOLERANCE)
                    {
                        if (CountSimilarPoints(x + i, y + j) < LIMIT)
                        {
                            Point p = new Point() { X = x + i, Y = y + j };
                            return p;
                        }
                    }
                }
            }
         return null;
        }

        private List<Point> GetLinePoints(int x, int y)
        {
            List<Point> points = new List<Point>();
            RGB currentColor = ImageData[x, y].Clone();
            Dictionary<int, List<int>> readedValues = new Dictionary<int, List<int>>();
            for (int i = x; i < width; ++i)
            {
                List<int> list = new List<int>();
                for (int j = 0; j < height; ++j)
                {
                    if (ImageData[i, j].PixelRecognitionStatus == RGB.RecognitionStasus.NONE && currentColor.getDifference(ImageData[i, j]) < TOLERANCE)
                    {
                        list.Add(j);
                        ImageData[i, j].PixelRecognitionStatus = RGB.RecognitionStasus.SELECTED;
                        currentColor.Average(ImageData[i, j]);
                    }
                }
                readedValues.Add(i, list);

            }
            currentColor = ImageData[x, y].Clone();
            for (int i = x - 1; i >= 0; --i)
            {
                List<int> list = new List<int>();
                for (int j = 0; j < height; ++j)
                {
                    if (ImageData[i, j].PixelRecognitionStatus == RGB.RecognitionStasus.NONE && currentColor.getDifference(ImageData[i, j]) < TOLERANCE)
                    {
                        list.Add(j);
                        ImageData[i, j].PixelRecognitionStatus = RGB.RecognitionStasus.SELECTED;
                        currentColor.Average(ImageData[i, j]);
                    }
                }
                readedValues.Add(i, list);

            }
            foreach (var element in readedValues)
            {
                int i = element.Key;
                List<int> list = element.Value;
                if (list.Count > 0)
                {
                    if (list.Count < 3)
                    {
                        points.Add(new Point() { X = i, Y = list[0] });
                    }
                    else
                    {
                        list.Sort();
                        int middle = list.Count / 2;
                        points.Add(new Point() { X = i, Y = list[middle] });

                    }
                }
            }
            return points;
        }
    }
}
