using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Text;
using WinRTXamlToolkit.Imaging;
using Windows.UI.Xaml.Media.Imaging;

namespace ReGraph.ViewModels
{
    public class RotationViewModel : Screen
    {
		private INavigationService _NavigationService;
		private IEventAggregator _EventAggregator;

		public RotationViewModel(IEventAggregator eventAggregator, INavigationService navigationService)
		{
			this._EventAggregator = eventAggregator;
			this._NavigationService = navigationService;

		}

		private int _Angle = 0;
		public string Angle
		{
			get { return _Angle.ToString(); }
			set
			{
				try
				{
					_Angle = int.Parse(value);
				}
				catch(FormatException ex )
				{
					_Angle = 0;
				}
				NotifyOfPropertyChange(() => Angle);
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

		public void ApplyButton_Clicked()
		{
			if (_Angle % 360 == 0)
                return;

			var mainVM = IoC.Get<MainViewModel>();

			mainVM.InputGraph.Image = mainVM.InputGraph.Image.RotateFree(_Angle);
		}
    }
}
