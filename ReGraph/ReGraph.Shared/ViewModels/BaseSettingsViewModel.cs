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

		private string _XBegin;
		public string XBegin
		{
            get { return _XBegin; }
			set
			{
                _XBegin = value;
                if (XBegin != null && XEnd != null)
                {
                    (IoC.GetInstance(typeof(MainViewModel), null) as MainViewModel).graphDrawer.setHorizontalRange(double.Parse(XBegin), double.Parse(XEnd)); ;

                }
                NotifyOfPropertyChange(() => XBegin);
			}
		}

        private string _XEnd;
        public string XEnd
        {
            get { return _XEnd; }
            set
            {
                _XEnd = value;
                if (XBegin != null && XEnd != null)
                {
                    (IoC.GetInstance(typeof(MainViewModel), null) as MainViewModel).graphDrawer.setHorizontalRange(double.Parse(XBegin), double.Parse(XEnd));
                }
                NotifyOfPropertyChange(() => XEnd);
            }
        }
		private string _YBegin;
		public string YBegin
		{
            get { return _YBegin; }
			set
			{
                _YBegin = value;
                if (YBegin != null && YEnd != null)
                {
                    (IoC.GetInstance(typeof(MainViewModel), null) as MainViewModel).graphDrawer.setVerticalRange(double.Parse(YBegin), double.Parse(YEnd));
                }
                NotifyOfPropertyChange(() => YBegin);
			}
		}
        private string _YEnd;
        public string YEnd
        {
            get { return _YEnd; }
            set
            {
                _YEnd = value;
                if (YBegin != null && YEnd != null)
                {
                    (IoC.GetInstance(typeof(MainViewModel), null) as MainViewModel).graphDrawer.setVerticalRange(double.Parse(YBegin), double.Parse(YEnd));
                } NotifyOfPropertyChange(() => YEnd);
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
