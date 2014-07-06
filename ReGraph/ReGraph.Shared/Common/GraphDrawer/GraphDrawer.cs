using System;
using System.Collections.Generic;
using System.Text;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

namespace ReGraph.Common.GraphDrawer
{
    /*
     * TODO:
     * 1. Test this class
     * 2. Implement AddLegent method
     * 3. Implement CreateBitmap method
     * 4. Implement SaveToFile methods
     * 5. Implement ReadFromFile methods
     * I will do in on my own(@jezior)
     */
    /// <summary>
    /// Class for generating Graph. Before drawing starts Canvas need to be placed on screen (must have ActualWith and ActualHeight).
    /// </summary>
    public class GraphDrawer
    {
        private Canvas result;
        private List<Line> solidLines;
        private List<Line> dottedLines;
        public GraphDrawer()
        {
            result = new Canvas();
            solidLines = new List<Line>();
            dottedLines = new List<Line>();
            verticalBegin = 0;
            verticalEnd = 100;
            horizontalBegin = 0;
            verticalEnd = 100;
            _Width = 800;
            _Height = 600;
            _GraphTitle = "";
            _HorizontalTitle = "";
            _VerticalTitle = "";
        }

        public Canvas GetCanvas()
        {
            return result;
        }

        public WriteableBitmap CreateBitmap()
        {
            WriteableBitmap bitmap = new WriteableBitmap(_Width, _Height); ;
            throw new NotImplementedException();
        }
        public void reDrawAll()
        {
            result.Children.Clear();
            reCountScale();
            foreach (Line line in solidLines)
            {
                drawSolidLine(line);
            }

            foreach (Line line in dottedLines)
            {
                drawDottedLine(line,1);
            }
            drawAxis();
            addTitles();
        }
        private void AddLegend()
        {
            throw new NotImplementedException();
        }

     
        #region AddingLine
        public void addSolidLine(List<Point> points, Color color, String name)
        {
            reCountScale();
            Line currentLine = new Line(points, color, name);
            solidLines.Add(currentLine);
            drawSolidLine(currentLine);
        }
        public void addDottedLine(List<Point> points, Color color, String name)
        {
            reCountScale();
            Line currentLine = new Line(points, color, name);
            dottedLines.Add(currentLine);
            drawDottedLine(currentLine, 1);
        }
        #endregion
        #region TitlesDrawing
        private void addTitles()
        {
            addGraphTitle();
            addVerticalTitle();
            addHorizontalTitle();
        }

        private void addGraphTitle()
        {
            TextBlock tb = new TextBlock();
            tb.Text = _GraphTitle;
            tb.Foreground = new SolidColorBrush(Colors.Black);
            tb.FontSize = 24;
            Canvas.SetTop(tb, 10);
            double left = (result.ActualWidth-tb.ActualWidth)/2;
            Canvas.SetLeft(tb, left);
            result.Children.Add(tb);
        }
        private void addVerticalTitle()
        {
            RotateTransform rotation = new RotateTransform();
            rotation.Angle = 90;
            TextBlock tb = new TextBlock();
            tb.Text = _VerticalTitle;
            tb.Text = _GraphTitle;
            tb.FontSize = 18;
            tb.RenderTransform = rotation;

            Canvas.SetTop(tb, (result.ActualHeight-tb.ActualHeight)/2);
            Canvas.SetLeft(tb, 2);
            result.Children.Add(tb);
        }

        private void addHorizontalTitle()
        {
            TextBlock tb = new TextBlock();
            tb.Text = _VerticalTitle;
            tb.Text = _GraphTitle;
            tb.FontSize = 18;
            Canvas.SetTop(tb, 2);
            Canvas.SetLeft(tb, (result.ActualWidth - tb.ActualWidth) / 2);
            result.Children.Add(tb);
        }
        #endregion
        #region AxisDrawing
        private void drawAxis()
        {
            drawHorizontalAxis(5);
            drawVerticalAxis(5);
        }

        private void drawHorizontalAxis(int margin)
        {
            int begin = margin;
            int end = (int)(result.ActualWidth - margin);
            int y_position = (int)(result.ActualHeight - margin);

            PointCollection collection = new PointCollection();
            collection.Add(new Point(begin, y_position));
            collection.Add(new Point(end, y_position));

            drawSolidLine(collection, Colors.Black);
        }

        private void drawVerticalAxis(int margin)
        {
            int begin = margin;
            int end = (int)(result.ActualHeight - margin);
            int x_position = margin;

            PointCollection collection = new PointCollection();
            collection.Add(new Point(x_position, begin));
            collection.Add(new Point(x_position, end));

            drawSolidLine(collection, Colors.Black);
        }
        #endregion
        #region LineDrawing
        private void drawSolidLine(Line line)
        {
            PointCollection collection = new PointCollection();
            foreach (Point p in line.points)
            {
                collection.Add(new Point(p.X / horizontalScale, p.Y / verticalScale));
            }
            Polyline linia = new Polyline();
            linia.Stroke = new SolidColorBrush(line.color);
            linia.Points = collection;
            result.Children.Add(linia);
        }
        private void drawSolidLine(PointCollection points, Color color)
        {
            Polyline linia = new Polyline();
            linia.Stroke = new SolidColorBrush(color);
            linia.Points = points;
            result.Children.Add(linia);
        }

        private void drawDottedLine(Line line, int Radius)
        {
            foreach (Point p in line.points)
            {
                Ellipse dot = new Ellipse();
                dot.Width = Radius;
                dot.Height = Radius;
                dot.Fill = new SolidColorBrush(line.color);

                Canvas.SetLeft(dot, p.X/horizontalScale);
                Canvas.SetTop(dot, p.Y/verticalScale);
                result.Children.Add(dot);
            }
        }
        #endregion
        #region Scale
        private double verticalScale;
        private double horizontalScale;
        private void reCountScale()
        {
            verticalScale = (verticalEnd - verticalBegin) / result.ActualHeight;
            horizontalScale = (horizontalEnd - horizontalEnd) / result.ActualWidth;
        }
        #endregion
        #region OutputImageSize
        private int _Width;
        public int Width
        {
            get 
            { 
                return _Width; 
            }
            set
            {
                _Width = value;
            }

        }
        private int _Height;
        public int Height
        {
            get 
            { 
                return _Height; 
            }
            set
            {
                _Height = value;
            }

        }
        #endregion
        #region Ranges
        private double horizontalBegin;
        private double horizontalEnd;
        public void setHorizontalRange(double begin, double end)
        {
            horizontalBegin = begin;
            horizontalEnd = end;
        }

        private double verticalBegin;
        private double verticalEnd;
        public void setVerticalRange(double begin, double end)
        {
            verticalBegin = begin;
            verticalEnd = end;
        }
        #endregion
        #region Titles
        private String _GraphTitle;
        public String GraphTitle
        {
            get
            {
                return _GraphTitle;
            }
            set
            {
                _GraphTitle = value;
            }
        }
        private String _VerticalTitle;
        public String VerticalTitle
        {
            get
            {
                return _VerticalTitle;
            }
            set
            {
                _VerticalTitle = value;
            }
        }
        private String _HorizontalTitle;
        public String HorizontalTitle
        {
            get
            {
                return _HorizontalTitle;
            }
            set
            {
                _HorizontalTitle = value;
            }
        }
        #endregion
        #region SaveToFile
        public void SaveAsTxt()
        {
            throw new NotImplementedException();
        }

        public void SaveAsCsv()
        {
            throw new NotImplementedException();
        }
        #endregion
        #region ReadFromFile
        public static GraphDrawer GetFromTxt()
        {
            throw new NotImplementedException();
        }
        public static GraphDrawer GetFromCsv()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
