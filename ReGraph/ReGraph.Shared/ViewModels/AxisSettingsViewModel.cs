using Caliburn.Micro;
using ReGraph.Models.OCR;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReGraph.ViewModels
{
    public class AxisSettingsViewModel : Screen, IHandle<OCRResultWrapper>
    {
		private INavigationService _NavigationService;
		private IEventAggregator _EventAggregator;

		public AxisSettingsViewModel(IEventAggregator eventAggregator, INavigationService navigationService)
		{
			this._EventAggregator = eventAggregator;
			this._NavigationService = navigationService;

			_EventAggregator.Subscribe(this);
		}

		private string _XAxisName = "X Name";
		public string XAxisName
		{
			get { return _XAxisName; }
			set
			{
				_XAxisName = value;
                (IoC.GetInstance(typeof(MainViewModel), null) as MainViewModel).graphDrawer.HorizontalTitle = value;
				NotifyOfPropertyChange(() => XAxisName);
			}
		}

		private string _YAxisName  = "Y Name";
		public string YAxisName
		{
			get { return _YAxisName; }
			set
			{
				_YAxisName = value;
                (IoC.GetInstance(typeof(MainViewModel), null) as MainViewModel).graphDrawer.VerticalTitle = value;
				NotifyOfPropertyChange(() => YAxisName);
			}
		}

        private string _MainTitle = "Main Title";
        public string MainTitle
        {
			get { return _MainTitle; }
            set
            {
				_MainTitle = value;
                (IoC.GetInstance(typeof(MainViewModel), null) as MainViewModel).graphDrawer.Title = value;
				NotifyOfPropertyChange(() => MainTitle);
            }
        }

		private bool _IsEnabled = false;
		public bool IsEnabled
		{
			get { return _IsEnabled; }
			set
			{
				_IsEnabled = value;
				NotifyOfPropertyChange(() => IsEnabled);
			}
		}


		public void XOCRButton_Clicked()
		{
			_EventAggregator.PublishOnCurrentThread(OCRTypeResult.XName);
		}

		public void YOCRButton_Clicked()
		{
			_EventAggregator.PublishOnCurrentThread(OCRTypeResult.YName);
		}
        public void MainOCRButton_Clicked()
		{
			_EventAggregator.PublishOnCurrentThread(OCRTypeResult.MainTitle);
		}

		public void Handle(OCRResultWrapper message)
		{
			if (message.ResultType == OCRTypeResult.XName)
				XAxisName = message.Result;
			else if (message.ResultType == OCRTypeResult.YName)
				YAxisName = message.Result;
			else if (message.ResultType == OCRTypeResult.MainTitle)
				MainTitle = message.Result;
		}
	}
}
