using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReGraph.ViewModels
{
    public class BaseSettingsViewModel : Screen 
    {
		private INavigationService _NavigationService;
		private IEventAggregator _EventAggregator;

		public BaseSettingsViewModel( IEventAggregator eventAggregator, INavigationService navigationService)
		{
			this._EventAggregator = eventAggregator;
			this._NavigationService = navigationService;
		}

		private string _XScale;
		public string XScale
		{
			get { return _XScale; }
			set
			{
				_XScale = value;
				NotifyOfPropertyChange(() => XScale);
			}
		}

		private string _YScale;
		public string YScale
		{
			get { return _YScale; }
			set
			{
				_YScale = value;
				NotifyOfPropertyChange(() => YScale);
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

    }
}
