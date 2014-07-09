using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace ReGraph.ViewModels
{
    public class MainViewModel : Screen
    {
        public MainViewModel()
        {

        }

        private WriteableBitmap _Result;

        /// <summary>
        /// Stores image result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        public WriteableBitmap Result
        {
            get { return _Result; }
            set
            {
                _Result = value;
                NotifyOfPropertyChange(() => Result);
            }
        }

        private const string SelectImageOperationName = "SelectImage";
        private const string SelectDestinationOperationName = "SelectDestination";
        private readonly string[] _supportedImageFilePostfixes = { ".jpg", ".jpeg", ".png" };

        public async void OpenButton_Clicked()
        {
            //bool success = false;

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
            picker.ContinuationData["Operation"] = SelectImageOperationName;
            picker.PickSingleFileAndContinue();
            //success = true;
#else
            
            Windows.Storage.StorageFile file = await picker.PickSingleFileAsync();

            if (file != null)
            {
                await HandleSelectedImageFileAsync(file);
            }
#endif
            //return success;
        }

#if WINDOWS_PHONE_APP
        public async void ContinueFileOpenPickerAsync(FileOpenPickerContinuationEventArgs args)
        {
            //System.Diagnostics.Debug.WriteLine(DebugTag + "ContinueFileOpenPicker()");
            bool success = false;

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

            //NotifyLoadedResult(success);
            //App.ContinuationManager.MarkAsStale();
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
#endif


        private async Task HandleSelectedImageFileAsync(StorageFile file)
        {
            //System.Diagnostics.Debug.WriteLine(DebugTag + "HandleSelectedImageFile(): " + file.Name);
            var fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);
            //DataContext dataContext = DataContext.Instance;

            // Reset the streams
            //dataContext.ResetStreams();

            //var image = new BitmapImage();
            //image.SetSource(fileStream);
            //int width = image.PixelWidth;
            //int height = image.PixelHeight;
            ////dataContext.SetFullResolution(width, height);

            //int previewWidth = (int)FilterEffects.DataContext.DefaultPreviewResolutionWidth;
            //int previewHeight = 0;
            //AppUtils.CalculatePreviewResolution(width, height, ref previewWidth, ref previewHeight);
            //dataContext.SetPreviewResolution(previewWidth, previewHeight);

            //bool success = false;

            try
            {
                // JPEG images can be used as such
                //var stream = fileStream.AsStream();
                var bitmap = new BitmapImage();

                bitmap.SetSource(fileStream);
                //stream.Position = 0;

                var image = new Image();
                image.Source = bitmap;
                image.Width = 200;
                image.Height = 200;

                Canvas.SetLeft(image, 10);
                Canvas.SetTop(image, 10);
                
                //stream.CopyTo(dataContext.FullResolutionStream);
                //success = true;
            }
            catch (Exception e)
            {
                //System.Diagnostics.Debug.WriteLine(DebugTag
                //    + "Cannot use stream as such (not probably in JPEG format): " + e.Message);
            }

            //if (!success)
            //{
            //    try
            //    {
            //        await AppUtils.FileStreamToJpegStreamAsync(fileStream,
            //            (IRandomAccessStream)dataContext.FullResolutionStream.AsInputStream());
            //        success = true;
            //    }
            //    catch (Exception e)
            //    {
            //        System.Diagnostics.Debug.WriteLine(DebugTag
            //            + "Failed to convert the file stream content into JPEG format: "
            //            + e.ToString());
            //    }
            //}

            //if (success)
            //{
            //    await AppUtils.ScaleImageStreamAsync(
            //        dataContext.FullResolutionStream,
            //        dataContext.FullResolution,
            //        dataContext.PreviewResolutionStream,
            //        dataContext.PreviewResolution);

            //    dataContext.WasCaptured = false;
            //}

            //return success;
        }

        public async void CameraButton_Clicked()
        {
            //CameraCaptureUI dialog = new CameraCaptureUI();
            //Size aspectRatio = new Size(16, 9);
            //dialog.PhotoSettings.CroppedAspectRatio = aspectRatio;
            //StorageFile file = await dialog.CaptureFileAsync(CameraCaptureUIMode.Photo);

            //if (file == null)
            //    return;

            //Result = await Extensions.CreateWriteableBitmapFromFile(file);

            Windows.Media.Capture.MediaCapture takePhotoManager = new Windows.Media.Capture.MediaCapture();
            await takePhotoManager.InitializeAsync();

            ImageEncodingProperties imgFormat = ImageEncodingProperties.CreateJpeg();

            // a file to save a photo
            //StorageFile file = await ApplicationData.Current.LocalFolder.(
            //        "Photo.jpg", CreationCollisionOption.ReplaceExisting);

            IRandomAccessStream stream = new InMemoryRandomAccessStream();

            await takePhotoManager.CapturePhotoToStreamAsync(imgFormat, stream);

            //Result = new WriteableBitmap(originalResolutionWidth, originalResolutionHeight);
            //stream.Seek(0, SeekOrigin.Begin);
            //await bitmap.SetSourceAsync(stream.AsRandomAccessStream());
        }
    }
}
