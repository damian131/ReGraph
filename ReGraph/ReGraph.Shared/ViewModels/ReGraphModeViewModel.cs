﻿using Caliburn.Micro;
using ReGraph.Models.OCR;
using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI;

namespace ReGraph.ViewModels
{
    public class ReGraphModeViewModel : Screen, IHandle<OCRResultWrapper>
    {
		private INavigationService _NavigationService;
		private IEventAggregator _EventAggregator;

		public ReGraphModeViewModel(IEventAggregator eventAggregator, INavigationService navigationService)
		{
			this._EventAggregator = eventAggregator;
			this._NavigationService = navigationService;

			_EventAggregator.Subscribe(this);
		}


        private string _LegendName = String.Empty;
        public string LegendName
        {
            get { return _LegendName; }
            set
            {
                _LegendName = value;
                NotifyOfPropertyChange(() => LegendName);
            }
        }

		private Color _SelectedColor;

        private void BeginRecognition()
        {
            (IoC.GetInstance(typeof(MainViewModel), null) as MainViewModel).CurrentPointerMode = MainViewModel.PointerEventMode.RECOGNITION;

        }
		public Color SelectedColor
		{
			get { return _SelectedColor; }
			set
			{
				_SelectedColor = value;
                BeginRecognition();
				NotifyOfPropertyChange(() => SelectedColor);
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

		public void WhiteButton_Clicked()
		{
			SelectedColor = Colors.White;
		}

		public void GoldButton_Clicked()
		{
			SelectedColor = Colors.Gold;
		}

		public void CrimsonButton_Clicked()
		{
			SelectedColor = Colors.Crimson;
		}

		public void LimeGreenButton_Clicked()
		{
			SelectedColor = Colors.LimeGreen;
		}

		public void MediumVioletRedButton_Clicked()
		{
			SelectedColor = Colors.MediumVioletRed;
		}

		public void CyanButton_Clicked()
		{
			SelectedColor = Colors.Cyan;
		}

		public void DodgerBlueButton_Clicked()
		{
			SelectedColor = Colors.DodgerBlue;
		}

		public void BlackButton_Clicked()
		{
			SelectedColor = Colors.Black;
		}
        public void LegendOCRButton_Clicked()
        {
            _EventAggregator.PublishOnCurrentThread(OCRTypeResult.Legend);
        }

		public void Handle(OCRResultWrapper message)
		{
			if (message.ResultType == OCRTypeResult.Legend)
				LegendName = message.Result;
		}
	}
}
