﻿using Caliburn.Micro;
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
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace ReGraph.ViewModels
{
    public class MainViewModel : Screen, IFileOpenPickerContinuable
    {

        public MainViewModel()
        {
            InputGraph = new GraphSpace();
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
            var fileStream = await file.OpenAsync(Windows.Storage.FileAccessMode.Read);

            try
            {
                var bitmap = new BitmapImage();

                bitmap.SetSource(fileStream);

                WriteableBitmap img1 = new WriteableBitmap(bitmap.PixelWidth, bitmap.PixelHeight);

                using (IRandomAccessStream strm = await file.OpenReadAsync())
                {
                    img1.SetSource(strm);
                }

                double scale = InputGraph.WorkspaceWidth/bitmap.PixelWidth;
                img1 = img1.Resize((int)InputGraph.WorkspaceWidth, (int)(bitmap.PixelHeight * scale), WriteableBitmapExtensions.Interpolation.Bilinear);

                InputGraph.WorkspaceMargin = new Thickness(0, 0, 0, 0);

                InputGraph.Image = img1;
                InputGraph.WorkspaceMargin = new Thickness(0, InputGraph.WorkspaceHeight / 2 - img1.PixelHeight/2, 0, 0);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("ERROR: " + e.Message);
            }
        }

#if WINDOWS_PHONE_APP
        public void ContinueFileOpenPicker(FileOpenPickerContinuationEventArgs args)
        {
            ContinueFileOpenPickerAsync(args);
            //_resumingFromFile = true;
        }
#endif

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
