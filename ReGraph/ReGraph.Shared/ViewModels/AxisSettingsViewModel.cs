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

		private string _XAxisName;
		public string XAxisName
		{
			get { return _XAxisName; }
			set
			{
				_XAxisName = value;
				NotifyOfPropertyChange(() => XAxisName);
			}
		}

		private string _YAxisName;
		public string YAxisName
		{
			get { return _YAxisName; }
			set
			{
				_YAxisName = value;
				NotifyOfPropertyChange(() => YAxisName);
			}
		}


		public void XOCRButton_Clicked()
		{

		}

		public void YOCRButton_Clicked()
		{

		}
    }
}
