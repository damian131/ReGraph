using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

namespace ReGraph.Models.GraphDrawer
{

    /// <summary>
    /// Class for generating Graph. Before drawing starts Canvas need to be placed on screen (must have ActualWith and ActualHeight).
    /// </summary>
    public class GraphDrawer : PropertyChangedBase
    {
        private List<Line> solidLines;
        private List<Line> dottedLines;
        private LinearAxis x_Axis;
        private LinearAxis y_Axis;
        public GraphDrawer()
        {
           
        }

        public void PrepareGraph()
        {


            y_Axis = new LinearAxis();
            y_Axis.Orientation = AxisOrientation.Y;
            y_Axis.ShowGridLines = true;
            Axes.Add(y_Axis);

            x_Axis = new LinearAxis();
            x_Axis.Orientation = AxisOrientation.X;
            x_Axis.ShowGridLines = true;
            Axes.Add(x_Axis);


            solidLines = new List<Line>();
            dottedLines = new List<Line>();
            setHorizontalRange(0, 100);
            setVerticalRange(0, 100);
            _Width = 800;
            _Height = 600;
            Title = "Main Title";
            HorizontalTitle = "OŚ X";
            VerticalTitle = "OŚ Y";
        }

        private String _Title;
        public String Title
        {
            get
            {
                return _Title;
            }
            set
            {
                _Title = value;
                NotifyOfPropertyChange(() => Title);
            }
        }

        private Collection<ISeries> _Series;
        public Collection<ISeries> Series
        {
            get
            {
                return _Series;
            }
            set
            {
                _Series = value;
                NotifyOfPropertyChange(() => Series);
            }
        }

        private Collection<IAxis> _Axes;
        public Collection<IAxis> Axes
        {
            get
            {
                return _Axes;
            }
            set
            {
                _Axes = value;
                NotifyOfPropertyChange(() => Axes);
            }
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
            Series.Add(series);

            solidLines.Add(line);
        }

        public void TestAddingLines()
        {
            Random rnd = new Random();
            Line l = new Line();
            for(int i =0;i<100;++i)
            {
                l.Points.Add(new Point() { X=rnd.NextDouble()*100, Y=rnd.NextDouble()*100});
            }

            addSolidLine(l);
        }

        public void CleanGraph()
        {
            Series.Clear();
            solidLines.Clear();
            dottedLines.Clear();
            //System.Diagnostics.Debug.WriteLine(Series.Count);
        }

        public void ReDraw()
        {
            Series.Clear();
            foreach (var line in solidLines)
                addSolidLine(line);
            foreach (var line in dottedLines)
                addDottedLine(line);
        }

        public void addDottedLine(Line line)
        {
            throw new NotImplementedException();
        }



        public LinearAxis HorizontalAxis
        {
            get { return x_Axis; }
            private set { }
        }
        public LinearAxis VerticalAxis
        {
            get { return y_Axis; }
            private set { }
        }
        public List<Line> SolidLines
        {
            get { return solidLines; }
            private set { }
        }

        public List<Line> DottedLines
        {
            get { return dottedLines; }
            private set { }
        }
    
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

        public void setHorizontalRange(double begin, double end)
        {
            if(begin < end && begin >= 0)
            {
                x_Axis.Minimum = begin;
                x_Axis.Maximum = end;
            }
        }


        public void setVerticalRange(double begin, double end)
        {
            if (begin < end && begin >= 0)
            {
                y_Axis.Minimum = begin;
                y_Axis.Maximum = end;
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
        public async  void SaveAsCsv()
        {
            GraphFileWriter writer = new GraphFileWriter(this);
            FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            savePicker.FileTypeChoices.Add("CSV", new List<string>() { ".csv" });
            savePicker.SuggestedFileName = "Graph";
            var file = await savePicker.PickSaveFileAsync();

            writer.writeToFile(file);
        }
        public async void ReadFromCsv()
        {
            CleanGraph();
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add(".csv");
            /*
            #if WINDOWS_PHONE_APP
            openPicker.ContinuationData["Operation"] = "UpdateProfilePicture";
            openPicker.PickSingleFileAndContinue();
            #else*/
            GraphFileReader reader = new GraphFileReader(this);
            //var file = await openPicker.PickSingleFileAsync();
            //reader.readFromFile(file);
            //#endif

        }
        /*
        #if WINDOWS_PHONE_APP
        public async void ContinueFileOpenPicker(FileOpenPickerContinuationEventArgs args)
        {
            if ((args.ContinuationData["Operation"] as string) == "UpdateProfilePicture" &&
                args.Files.Count > 0)
            {
                StorageFile file = args.Files[0];
                GraphFileReader reader = new GraphFileReader(this);
                reader.readFromFile(file);
            }
        }
        #endif*/
    }
}
