using Caliburn.Micro;
using ReGraph.Common;
using ReGraph.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
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

namespace ReGraph.ViewModels
{
	public class MainViewModel : Screen, IHandle<StorageFile>, IHandle<String>, IHandle<bool>
	{

		private INavigationService _NavigationService;

		public MainViewModel(INavigationService NavigationService, IEventAggregator EventAggregator)
		{
			EventAggregator.Subscribe(this);

			this._NavigationService = NavigationService;

			InputGraph = new GraphSpace();

			NewImageVM = new NewImageViewModel(EventAggregator, _NavigationService);
			BaseSettingsVM = new BaseSettingsViewModel(EventAggregator, _NavigationService);
			ReGraphVM = new ReGraphModeViewModel(EventAggregator, _NavigationService);
			AxisSettingsVM = new AxisSettingsViewModel(EventAggregator, _NavigationService);
			ExtrasSettingsVM = new ExtrasSettingsViewModel(EventAggregator, _NavigationService);
		}

		#region PROPERTIES

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
		public BaseSettingsViewModel BaseSettingsVM { get; private set; }
		public ReGraphModeViewModel ReGraphVM { get; private set; }
		public AxisSettingsViewModel AxisSettingsVM { get; private set; }
		public ExtrasSettingsViewModel ExtrasSettingsVM { get; private set; }

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
#endif

		public void SaveFileButton_Clicked()
		{

		}

		public void GraphImage_PointerPressed(PointerRoutedEventArgs args)
		{

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
			await HandleSelectedImageFileAsync(message);
		}

		public void Handle(String recognizedText)
		{
			_NavigationService.Navigated += NavigationServiceOnNavigated;
			_NavigationService.NavigateToViewModel<OCRViewModel>(InputGraph);
			_NavigationService.Navigated -= NavigationServiceOnNavigated;
		}

		public void Handle(bool unblockButtons)
		{
			BaseSettingsVM.IsEnabled = true;
			ReGraphVM.IsEnabled = true;
			AxisSettingsVM.IsEnabled = true;
			ExtrasSettingsVM.IsEnabled = true;

			IsSaveEnabled = true;
		}

		#endregion //EVENT AGGREGATOR IMPLEMENTATION
	}
}
