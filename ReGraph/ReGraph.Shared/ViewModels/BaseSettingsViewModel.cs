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

		private string _XBegin = "0";
		public string XBegin
		{
            get { return _XBegin; }
			set
			{
                _XBegin = value;
				if (XBegin != String.Empty && XEnd != String.Empty)
				{
					(IoC.GetInstance(typeof(MainViewModel), null) as MainViewModel).graphDrawer.setHorizontalRange(double.Parse(XBegin), double.Parse(XEnd)); ;

				}
                NotifyOfPropertyChange(() => XBegin);
			}
		}

        private string _XEnd = "100";
        public string XEnd
        {
            get { return _XEnd; }
            set
            {
                _XEnd = value;
                if (XBegin != String.Empty && XEnd != String.Empty)
                {
                    (IoC.GetInstance(typeof(MainViewModel), null) as MainViewModel).graphDrawer.setHorizontalRange(double.Parse(XBegin), double.Parse(XEnd));
                }
                NotifyOfPropertyChange(() => XEnd);
            }
        }
		private string _YBegin = "0";
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
        private string _YEnd = "100";
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

        public void SetMiddlePointButton_Clicked()
        {
            (IoC.GetInstance(typeof(MainViewModel), null) as MainViewModel).CurrentPointerMode = MainViewModel.PointerEventMode.MIDDLE_POINT;
        }

    }
}
