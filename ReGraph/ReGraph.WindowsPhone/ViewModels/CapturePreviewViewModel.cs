using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;

namespace ReGraph.ViewModels
{
    public class CapturePreviewViewModel : Screen
    {
        private INavigationService _NavigationService;
        private IEventAggregator _EventAggregator;
        private MediaCapture _CaptureManager;

        public CapturePreviewViewModel( INavigationService NavigationService , IEventAggregator EventAggregator)
        {
            this._NavigationService = NavigationService;
            this._EventAggregator = EventAggregator;

            //InitializeCamera();
        }

        #region PROPERTIES

        MediaCapture _Source;
        MediaCapture Source
        {
            get { return _Source; }
            set
            {
                _Source = value;
                NotifyOfPropertyChange(() => Source);
            }
        }

        #endregion // PROPERTIES

        #region EVENTS HANDLERS

        async public void CaptureButton_Clicked()
        {
            //TEST
            _CaptureManager = new MediaCapture();
            await _CaptureManager.InitializeAsync();
            //ENDTEST

            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync("Photo.jpg", CreationCollisionOption.ReplaceExisting);

            if (file == null)
                return;

            ImageEncodingProperties imgFormat = ImageEncodingProperties.CreateJpeg();

            await _CaptureManager.CapturePhotoToStorageFileAsync(imgFormat, file);

            // PUBLISH FiLE USING EVENT AGGREGATOR
            _EventAggregator.PublishOnCurrentThread(file);
        }

        #endregion //EVENTS HANDLERS

        #region METHODS

        private async void InitializeCamera()
        {
            _CaptureManager = new MediaCapture();
            await _CaptureManager.InitializeAsync();

            Source = _CaptureManager;
            await _CaptureManager.StartPreviewAsync();
        }

        #endregion //METHODS
    }
}
