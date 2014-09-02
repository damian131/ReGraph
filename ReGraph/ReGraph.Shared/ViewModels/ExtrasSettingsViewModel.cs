using Caliburn.Micro;
using ReGraph.Models.GraphDrawer;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReGraph.ViewModels
{
    public class ExtrasSettingsViewModel : Screen
    {
		private INavigationService _NavigationService;
		private IEventAggregator _EventAggregator;

		public ExtrasSettingsViewModel(IEventAggregator eventAggregator, INavigationService navigationService)
		{
			this._EventAggregator = eventAggregator;
			this._NavigationService = navigationService;
		}

		private string _XCoordinate;
		public string XCoordinate
		{
			get { return _XCoordinate; }
			set
			{
				_XCoordinate = value;
				NotifyOfPropertyChange(() => XCoordinate);
			}
		}

		private string _YCoordinate;
		public string YCoordinate
		{
			get { return _YCoordinate; }
			set
			{
				_YCoordinate = value;
				NotifyOfPropertyChange(() => YCoordinate);
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

		public void AddPoint_Clicked()
		{
            Point p = new Point() { X = Double.Parse(_XCoordinate), Y = Double.Parse(YCoordinate) };
            (IoC.GetInstance(typeof(MainViewModel), null) as MainViewModel).graphDrawer.AddPoint(p);
		}
    }
}
