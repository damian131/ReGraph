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
using Windows.UI.Xaml.Media.Imaging;

namespace ReGraph.ViewModels
{
    public class MainViewModel : Screen
#if WINDOWS_PHONE_APP
        , IFileOpenPickerContinuable
#endif
    {

        private INavigationService _NavigationService;

        public MainViewModel( INavigationService NavigationService )
        {
            this._NavigationService = NavigationService;

            InputGraph = new GraphSpace();
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

        #endregion //PROPERTIES

        #region EVENT HANDLERS

        private const string SelectImageOperationName = "SelectImage";
        private const string SelectDestinationOperationName = "SelectDestination";
        private readonly string[] _supportedImageFilePostfixes = { ".jpg", ".jpeg", ".png" };

        public async void OpenButton_Clicked()
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
                await HandleSelectedImageFileAsync(file);
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

            IRandomAccessStream stream = new InMemoryRandomAccessStream();

            //await takePhotoManager.CapturePhotoToStreamAsync(imgFormat, stream);
            await takePhotoManager.CapturePhotoToStorageFileAsync(imgFormat, file);

            await HandleSelectedImageFileAsync(file);
#else
            
            _NavigationService.NavigateToViewModel<CapturePreviewViewModel>();
            await Task.Delay(0);
#endif
        }



        #endregion //EVENT HANDLERS

        #region METHODS

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

        #endregion //METHODS

        #region CONTINUATION MANAGER IMPLEMENTATION

#if WINDOWS_PHONE_APP
        public async void ContinueFileOpenPickerAsync(FileOpenPickerContinuationEventArgs args)
        {
            if (args.Files == null || args.Files.Count == 0 || args.Files[0] == null
                || (args.ContinuationData["Operation"] as string) != SelectImageOperationName)
            {
                //System.Diagnostics.Debug.WriteLine(DebugTag + "ContinueFileOpenPicker(): Invalid arguments!");
            }
            else
            {
                StorageFile file = args.Files[0];
                await HandleSelectedImageFileAsync(file);
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
