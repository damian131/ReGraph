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
    public class SaveChartViewModel : Screen
#if WINDOWS_PHONE_APP
        , IFileOpenPickerContinuable
#endif
    {
        private IEventAggregator _EventAggregator;
        private INavigationService _NavigationService;

        public SaveChartViewModel(IEventAggregator eventAggregator, INavigationService navigationService)
		{
			this._EventAggregator = eventAggregator;
			this._NavigationService = navigationService;
		}

        private readonly string SelectImageOperationName = "SelectFile";
        private readonly string SelectDestinationOperationName = "SelectDestination";
        private readonly string[] _supportedImageFilePostfixes = { ".jpg"};
        private readonly string[] _supportedCSVFilePostfixes = { ".csv" };

        public async void SaveAsImage_Clicked()
        {
            var picker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,
            };
            picker.FileTypeChoices.Add(SelectImageOperationName, _supportedImageFilePostfixes);
            (IoC.GetInstance(typeof(MainViewModel), null) as MainViewModel).CurrentFileAccess = ReGraph.ViewModels.MainViewModel.FileAccess.WRITE_IMAGE;
#if WINDOWS_PHONE_APP
            await Task.Delay(0);
            picker.ContinuationData["Operation"] = SelectImageOperationName;
            picker.PickSaveFileAndContinue();

#else
            var file = await picker.PickSaveFileAsync();
            if (file != null)
            {
                (IoC.GetInstance(typeof(MainViewModel), null) as MainViewModel).CurrentFileAccess = ReGraph.ViewModels.MainViewModel.FileAccess.WRITE_IMAGE;
                _EventAggregator.PublishOnCurrentThread(file);
            }
#endif
        }

        public async void SaveAsCSV_Clicked()
        {
            var picker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
            };
            picker.FileTypeChoices.Add(SelectImageOperationName, _supportedCSVFilePostfixes);
            (IoC.GetInstance(typeof(MainViewModel), null) as MainViewModel).CurrentFileAccess = ReGraph.ViewModels.MainViewModel.FileAccess.WRITE_CSV;
#if WINDOWS_PHONE_APP
            await Task.Delay(0);
            picker.ContinuationData["Operation"] = SelectImageOperationName;
            picker.PickSaveFileAndContinue();
#else
            var file = await picker.PickSaveFileAsync();
            if (file != null)
            {
                _EventAggregator.PublishOnCurrentThread(file);
            }

#endif
        }


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


    }
}
