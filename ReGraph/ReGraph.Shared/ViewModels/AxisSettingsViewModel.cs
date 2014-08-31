using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReGraph.ViewModels
{
    public class AxisSettingsViewModel : Screen
    {
		private INavigationService _NavigationService;
		private IEventAggregator _EventAggregator;

		public AxisSettingsViewModel(IEventAggregator eventAggregator, INavigationService navigationService)
		{
			this._EventAggregator = eventAggregator;
			this._NavigationService = navigationService;
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
                _XAxisName = value;
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
			_EventAggregator.PublishOnCurrentThread(XAxisName);
		}

		public void YOCRButton_Clicked()
		{
			_EventAggregator.PublishOnCurrentThread(YAxisName);
		}
        public void MainOCRButton_Clicked()
		{
			_EventAggregator.PublishOnCurrentThread(MainTitle);
		}
    }
}
