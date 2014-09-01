using Caliburn.Micro;
using ReGraph.Common;
using ReGraph.Models;
using ReGraph.Models.GraphDrawer;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Input;
using ReGraph.Models.GraphReader;

namespace ReGraph.ViewModels
{

	public enum AppCycleState
	{
		NoImageLoaded,
		ImageLoaded,
		SettingsEntered
	}

    public class MainViewModel : Screen, IHandle<StorageFile>, IHandle<String>, IHandle<AppCycleState>
    {
        public enum FileAccess { NONE, READ_IMAGE, WRITE_IMAGE, READ_CSV, WRITE_CSV };
        public FileAccess CurrentFileAccess;
        public enum PointerEventMode { NONE, MIDDLE_POINT, RECOGNITION }
        public PointerEventMode CurrentPointerMode;
        private INavigationService _NavigationService;

        public MainViewModel(INavigationService NavigationService, IEventAggregator EventAggregator)
        {
            EventAggregator.Subscribe(this);

            this._NavigationService = NavigationService;

            InputGraph = new GraphSpace();
            graphDrawer = new GraphDrawer();
            NewImageVM = new NewImageViewModel(EventAggregator, _NavigationService);
			RotationVM = new RotationViewModel(EventAggregator, _NavigationService);
            BaseSettingsVM = new BaseSettingsViewModel(EventAggregator, _NavigationService);
            ReGraphVM = new ReGraphModeViewModel(EventAggregator, _NavigationService);
            AxisSettingsVM = new AxisSettingsViewModel(EventAggregator, _NavigationService);
            ExtrasSettingsVM = new ExtrasSettingsViewModel(EventAggregator, _NavigationService);

            CurrentFileAccess = FileAccess.NONE;
            CurrentPointerMode = PointerEventMode.NONE;

			CurrentState = AppCycleState.NoImageLoaded;

            SaveChartVM = new SaveChartViewModel(EventAggregator, _NavigationService);
            graphReader = new GraphReader();
        }

        #region PROPERTIES

		public AppCycleState CurrentState { get; private set; }

        private GraphDrawer _graphDrawer;
        public GraphDrawer graphDrawer
        {
            get { return _graphDrawer; }
            set
            {
                _graphDrawer = value;
                NotifyOfPropertyChange(() => graphDrawer);
            }
        }

        private GraphReader _graphReader;
        public GraphReader graphReader
        {
            get { return _graphReader; }
            set
            {
                _graphReader = value;
                NotifyOfPropertyChange(() => graphReader);
            }
        }
        private IGraphSpace _InputGraph;
        public IGraphSpace InputGraph
        {
            get { return _InputGraph; }
            set
            {
                _InputGraph = value;
                NotifyOfPropertyChange(() => InputGraph);
            }
        }

        public NewImageViewModel NewImageVM { get; private set; }
		public RotationViewModel RotationVM { get; private set; }
        public BaseSettingsViewModel BaseSettingsVM { get; private set; }
        public ReGraphModeViewModel ReGraphVM { get; private set; }
        public AxisSettingsViewModel AxisSettingsVM { get; private set; }
        public ExtrasSettingsViewModel ExtrasSettingsVM { get; private set; }
        public SaveChartViewModel SaveChartVM { get; private set; }
        public Chart ChartView { get; set; }

        private bool _IsSaveEnabled = false;
        public bool IsSaveEnabled
        {
            get { return _IsSaveEnabled; }
            set
            {
                _IsSaveEnabled = value;
                NotifyOfPropertyChange(() => IsSaveEnabled);
            }
        }

        #endregion //PROPERTIES

        #region EVENT HANDLERS

#if WINDOWS_PHONE_APP
		public void FromFileButton_Clicked() //TODO: change data context and bind event to newImageVM
		{
			NewImageVM.FromFileButton_Clicked();
		}

        public void FromCSVFileButton_Clicked()
        {
            NewImageVM.FromCSVButton_Clicked();
        }
        public void SaveAsImageFileButton_Clicked()
        {
            SaveChartVM.SaveAsImage_Clicked();
        }

        public void SaveAsCSVFileButton_Clicked()
        {
            SaveChartVM.SaveAsCSV_Clicked();
        }

#endif
		public async void GraphImage_PointerPressed(PointerRoutedEventArgs args)
        {
            if (CurrentPointerMode == PointerEventMode.NONE)
            {
                return;
            }
            if (CurrentPointerMode == PointerEventMode.MIDDLE_POINT)
            {
                await HandleSetStartPointAsync(args);
            }
            if (CurrentPointerMode == PointerEventMode.RECOGNITION)
            {
                await HandleRecognizeLineAsync(args);
            }
            CurrentPointerMode = PointerEventMode.NONE;
        }

        private async Task HandleRecognizeLineAsync(PointerRoutedEventArgs args)
        {
            FrameworkElement element = args.OriginalSource as Image;
            PointerPoint p = args.GetCurrentPoint(args.OriginalSource as Image);
            ReGraph.Models.GraphDrawer.Point clickedPoint = new ReGraph.Models.GraphDrawer.Point() { X = p.Position.X, Y = p.Position.Y };
            graphReader.RecognizeLine(clickedPoint, ReGraphVM.SelectedColor);
        }
        private async Task HandleSetStartPointAsync(PointerRoutedEventArgs args)
        {
            PointerPoint p = args.GetCurrentPoint(args.OriginalSource as Image);
            FrameworkElement element = args.OriginalSource as Image;
            graphReader.xScale = InputGraph.Image.PixelWidth / element.ActualWidth;
            graphReader.yScale = InputGraph.Image.PixelHeight / element.ActualHeight;
            ReGraph.Models.GraphDrawer.Point StartPoint = new ReGraph.Models.GraphDrawer.Point() { X = p.Position.X, Y = p.Position.Y };
            graphReader.SetMiddlePoint(StartPoint);

			Handle(AppCycleState.SettingsEntered);
        }

        #endregion //EVENT HANDLERS

        private async Task<WriteableBitmap> GetWritableBitmapFromStream(IRandomAccessStream stream, StorageFile file)
        {
            var bitmap = new BitmapImage();
            bitmap.SetSource(stream);

            var wb = new WriteableBitmap(bitmap.PixelWidth, bitmap.PixelHeight);

            using (IRandomAccessStream strm = await file.OpenReadAsync())
            {
                wb.SetSource(strm);
            }

            return wb;
        }

        private async Task HandleSelectedImageFileAsync(StorageFile file)
        {

            var fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            try
            {
                InputGraph.Image = await GetWritableBitmapFromStream(fileStream, file);
                ChartView = new Chart();
                graphDrawer.CleanGraph();
                graphDrawer.PrepareGraph();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("ERROR: " + e.Message);
            }
        }

        private void NavigationServiceOnNavigated(object sender, NavigationEventArgs args)
        {
            FrameworkElement view;
            OCRViewModel cropViewModel;
            if ((view = args.Content as FrameworkElement) == null ||
                (cropViewModel = view.DataContext as OCRViewModel) == null ||
                 args.Parameter == null) return;

            cropViewModel.SetGraphSource(args.Parameter as IGraphSpace);
        }


        #region EVENT AGGREGATOR IMPLEMENTATION

        async public void Handle(StorageFile message)
        {
            if (CurrentFileAccess == FileAccess.READ_IMAGE)
            {
                await HandleSelectedImageFileAsync(message);       
            }
            if (CurrentFileAccess == FileAccess.WRITE_IMAGE)
            {
                await HandleSaveChartAsImageAsync(message);
            }
            if(CurrentFileAccess == FileAccess.WRITE_CSV)
            {
                await HandleSaveChartAsCSVAsync(message);
            }
            if (CurrentFileAccess == FileAccess.READ_CSV)
            {
                await HandleOpenChartFromCSVAsync(message);
            }
            CurrentFileAccess = FileAccess.NONE;
        }

        private async Task HandleOpenChartFromCSVAsync(StorageFile message)
        {
            graphDrawer.PrepareGraph();
            GraphFileReader writer = new GraphFileReader(graphDrawer);
            writer.readFromFile(message);
        }

        private async Task HandleSaveChartAsCSVAsync(StorageFile message)
        {
            GraphFileWriter writer = new GraphFileWriter(graphDrawer);
            writer.writeToFile(message);
        }

        private async Task HandleSaveChartAsImageAsync(StorageFile file)
        {
           using (IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                RenderTargetBitmap render = new RenderTargetBitmap();
                // Encode the image into JPG format,reading for saving
                BitmapEncoder encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, stream);
                await render.RenderAsync(ChartView);
                var data = await render.GetPixelsAsync();
                Stream pixelStream = data.AsStream();
                byte[] pixels = new byte[pixelStream.Length];
                await pixelStream.ReadAsync(pixels, 0, pixels.Length);
                encoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, (uint)ChartView.ActualWidth, (uint)ChartView.ActualHeight, 96.0, 96.0, pixels);
                await encoder.FlushAsync();
            }
        }

        public void Handle(String recognizedText)
        {
            _NavigationService.Navigated += NavigationServiceOnNavigated;
            _NavigationService.NavigateToViewModel<OCRViewModel>(InputGraph);
            _NavigationService.Navigated -= NavigationServiceOnNavigated;
        }

        public void Handle(AppCycleState state)
        {
			if (CurrentState == state)
				return;

			CurrentState = state;

			if (CurrentState == AppCycleState.ImageLoaded)
			{
				RotationVM.IsEnabled = true;
				BaseSettingsVM.IsEnabled = true;
				AxisSettingsVM.IsEnabled = true;
				IsSaveEnabled = true;

				ReGraphVM.IsEnabled = false;
				ExtrasSettingsVM.IsEnabled = false;
			}
			else if (CurrentState == AppCycleState.SettingsEntered)
			{
				RotationVM.IsEnabled = false;
				BaseSettingsVM.IsEnabled = false;
				AxisSettingsVM.IsEnabled = false;

				ReGraphVM.IsEnabled = true;
				ExtrasSettingsVM.IsEnabled = true;
			}
			
        }

        #endregion //EVENT AGGREGATOR IMPLEMENTATION
    }
}
