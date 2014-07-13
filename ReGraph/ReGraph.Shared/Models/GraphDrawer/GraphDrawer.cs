using System;
using System.Collections.Generic;
using System.Text;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Media;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

namespace ReGraph.Models.GraphDrawer
{

    /// <summary>
    /// Class for generating Graph. Before drawing starts Canvas need to be placed on screen (must have ActualWith and ActualHeight).
    /// </summary>
    public class GraphDrawer
    {
        private List<Line> solidLines;
        private List<Line> dottedLines;
        private Chart Graph;
        private LinearAxis x_Axis;
        private LinearAxis y_Axis;
        public GraphDrawer(Chart chart)
        {
            this.Graph = chart;
            x_Axis = new LinearAxis();
            x_Axis.Orientation=AxisOrientation.X;
            x_Axis.ShowGridLines = true;
            Graph.Axes.Add(x_Axis);

            y_Axis = new LinearAxis();
            y_Axis.Orientation = AxisOrientation.Y;
            y_Axis.ShowGridLines = true;
            Graph.Axes.Add(y_Axis);

            solidLines = new List<Line>();
            dottedLines = new List<Line>();
            setHorizontalRange(0, 100);
            setVerticalRange(0, 100);
            _Width = 800;
            _Height = 600;
            GraphTitle = "Main Title";
            HorizontalTitle = "OŚ X";
            VerticalTitle = "OŚ Y";
        }

        public void addSolidLine(Line line)
        {
            LineSeries series = new LineSeries();
            series.Title = line.Name;
            series.Foreground = new SolidColorBrush(line.Color);
            series.Margin = new Windows.UI.Xaml.Thickness(0, 0, 0, 0);
            series.IndependentValuePath = "X";
            series.DependentValuePath = "Y";
            series.IsSelectionEnabled = true;
            series.ItemsSource = line.Points;
            Graph.Series.Add(series);

            solidLines.Add(line);
        }

        public void CleanGraph()
        {
            Graph.Series.Clear();
            solidLines.Clear();
            dottedLines.Clear();
        }

        public void ReDraw()
        {
            Graph.Series.Clear();
            foreach (var line in solidLines)
                addSolidLine(line);
            foreach (var line in dottedLines)
                addDottedLine(line);
        }

        private void addDottedLine(Line line)
        {
            throw new NotImplementedException();
        }

        


       
  
    
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

        public void setHorizontalRange(double begin, double end)
        {
            x_Axis.Minimum = begin;
            x_Axis.Maximum = end;
        }


        public void setVerticalRange(double begin, double end)
        {
            y_Axis.Minimum = begin;
            y_Axis.Maximum = end;
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
                Graph.Title = value;
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
                y_Axis.Title = _VerticalTitle;
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
                x_Axis.Title = _HorizontalTitle;
            }
        }
        #endregion
        #region SaveToFile
        public void SaveAsCsv()
        {
            throw new NotImplementedException();
        }
        #endregion
        #region ReadFromFile
        public static GraphDrawer GetFromCsv()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
