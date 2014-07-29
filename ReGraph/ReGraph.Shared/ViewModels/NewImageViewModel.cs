using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace ReGraph.ViewModels
{
	public class NewImageViewModel : Screen
#if WINDOWS_PHONE_APP
        , IFileOpenPickerContinuable
#endif
	{
		private IEventAggregator _EventAggregator;
		private INavigationService _NavigationService;

		public NewImageViewModel(IEventAggregator eventAggregator, INavigationService navigationService )
		{
			this._EventAggregator = eventAggregator;
			this._NavigationService = navigationService;
		}

		private readonly string SelectImageOperationName = "SelectImage";
		private readonly string SelectDestinationOperationName = "SelectDestination";
		private readonly string[] _supportedImageFilePostfixes = { ".jpg", ".jpeg", ".png" };

		public async void FromFileButton_Clicked()
		{

			var picker = new FileOpenPicker
			{
				SuggestedStartLocation = PickerLocationId.PicturesLibrary,
				ViewMode = PickerViewMode.Thumbnail
			};

			// Filter to include a sample subset of file types
			picker.FileTypeFilter.Clear();

			foreach (string postfix in _supportedImageFilePostfixes)
			{
				picker.FileTypeFilter.Add(postfix);
			}

#if WINDOWS_PHONE_APP
            await Task.Delay(0);
            picker.ContinuationData["Operation"] = SelectImageOperationName;
            picker.PickSingleFileAndContinue();
#else

			Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();

			if (file != null)
			{
				_EventAggregator.PublishOnCurrentThread(file);
			}
#endif
		}

		public async void CameraButton_Clicked()
		{
#if WINDOWS_APP
			CameraCaptureUI dialog = new CameraCaptureUI();
			Size aspectRatio = new Size(16, 9);
			dialog.PhotoSettings.CroppedAspectRatio = aspectRatio;

			StorageFile file = await dialog.CaptureFileAsync(CameraCaptureUIMode.Photo);

			if (file == null)
				return;

			Windows.Media.Capture.MediaCapture takePhotoManager = new Windows.Media.Capture.MediaCapture();
			await takePhotoManager.InitializeAsync();

			ImageEncodingProperties imgFormat = ImageEncodingProperties.CreateJpeg();

			//await takePhotoManager.CapturePhotoToStreamAsync(imgFormat, stream);
			await takePhotoManager.CapturePhotoToStorageFileAsync(imgFormat, file);

			_EventAggregator.PublishOnCurrentThread(file);
#else
            
            _NavigationService.NavigateToViewModel<CapturePreviewViewModel>();
            await Task.Delay(0);
#endif
		}


		#region CONTINUATION MANAGER IMPLEMENTATION

#if WINDOWS_PHONE_APP
        public void ContinueFileOpenPickerAsync(FileOpenPickerContinuationEventArgs args)
        {
            if (args.Files == null || args.Files.Count == 0 || args.Files[0] == null
                || (args.ContinuationData["Operation"] as string) != SelectImageOperationName)
            {
                //System.Diagnostics.Debug.WriteLine(DebugTag + "ContinueFileOpenPicker(): Invalid arguments!");
            }
            else
            {
                StorageFile file = args.Files[0];
                _EventAggregator.PublishOnCurrentThread(file);
            }

        }

        //public async void ContinueFileSavePickerAsync(FileSavePickerContinuationEventArgs args)
        //{
        //    //System.Diagnostics.Debug.WriteLine(DebugTag + "ContinueFileSavePicker()");
        //    //bool success = false;
        //    StorageFile file = args.File;

        //    if (file != null && (args.ContinuationData["Operation"] as string) == SelectDestinationOperationName)
        //    {
        //        success = await SaveImageFileAsync(file);
        //        NameOfSavedFile = file.Name;
        //    }

        //    //NotifySavedResult(success);
        //    //App.ContinuationManager.MarkAsStale();
        //}

        public void ContinueFileOpenPicker(FileOpenPickerContinuationEventArgs args)
        {
            ContinueFileOpenPickerAsync(args);
        }
#endif

		#endregion //CONTINUATION MANAGER IMPLEMENTATION
	}
}
