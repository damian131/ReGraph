using Caliburn.Micro;
using ReGraph.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using WinRTXamlToolkit.Imaging;
using WinRTXamlToolkit.Controls.Extensions;
using Windows.Foundation;
using Windows.UI.Xaml;
namespace ReGraph.ViewModels
{
    /// <summary>
    /// The OCRViewModel associated with CropView.
    /// </summary>
    public class OCRViewModel : Screen
    {
        /// <summary>
        /// Gets or sets the navigation service.
        /// </summary>
        /// <value>
        /// The navigation service.
        /// </value>
        private INavigationService NavigationService { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="CropViewModel"/> class.
        /// </summary>
        /// <param name="navigationService">The navigation service.</param>
        /// <param name="workspace">The workspace.</param>
        public OCRViewModel(INavigationService navigationService)
        {
            this.NavigationService = navigationService;
        }

        /// <summary>
        /// Applies the crop operation.
        /// </summary>
        public void CropButton_Clicked()
        {

            //Workspace.Image = Windows.UI.Xaml.Media.Imaging.WriteableBitmapExtensions.Crop(Workspace.Image, (int)(ZoomFactor * (RectLeft - ImageLeft)), (int)(ZoomFactor * (RectTop - ImageTop)), (int)(ZoomFactor * RectWidth), (int)(ZoomFactor * RectHeight));
            double x = RectLeft - ImageLeft;
            x *= 1.0 / ZoomFactor;

            double y = RectTop - ImageTop;
            y *= 1.0 / ZoomFactor;

            double w = RectWidth;
            w *= 1.0 / ZoomFactor;

            double h = RectHeight;
            h *= 1.0 / ZoomFactor;

            CroppedImage = Windows.UI.Xaml.Media.Imaging.WriteableBitmapExtensions.Crop(Workspace.Image, (int)x, (int)y, (int)w, (int)h);
            //Workspace.UpdateActualSize(Workspace.Image.PixelWidth, Workspace.Image.PixelHeight);
            //Workspace.Zoom(1.0, WorkspacePart.Center);
            //Workspace.RefreshUI();

			SelectedAreaVisibility = false;
			IsCropEnabled = false;
			IsRecognizeEnabled = true;

            //NavigationService.GoBack();
        }

		public void RecognizeButton_Clicked()
		{

		}

        /// <summary>
        /// The manipulation started event handler.
        /// </summary>
        /// <param name="e">The <see cref="ManipulationStartedRoutedEventArgs"/> instance containing the event data.</param>
        public void ManipulationStarted(ManipulationStartedRoutedEventArgs e)
        {

            e.Handled = true;
        }

        /// <summary>
        /// The manipulation delta event handler.
        /// </summary>
        /// <param name="e">The <see cref="ManipulationDeltaRoutedEventArgs"/> instance containing the event data.</param>
        public void ManipulationDelta(ManipulationDeltaRoutedEventArgs e)
        {
            if (e.Delta.Translation.X > 0)
                RectLeft = Math.Min(RectLeft + e.Delta.Translation.X, ImageWidth - RectWidth + ImageLeft);
            else if (e.Delta.Translation.X < 0)
                RectLeft = Math.Max(ImageLeft, RectLeft + e.Delta.Translation.X);

            if (e.Delta.Translation.Y > 0)
                RectTop = Math.Min(RectTop + e.Delta.Translation.Y, ImageHeight - RectHeight + ImageTop);
            else if (e.Delta.Translation.Y < 0)
                RectTop = Math.Max(ImageTop, RectTop + e.Delta.Translation.Y);

            e.Handled = true;
        }

		private bool _SelectedAreaVisibility = true;
		public bool SelectedAreaVisibility
		{
			get { return _SelectedAreaVisibility; }
			set
			{
				_SelectedAreaVisibility = value;
				NotifyOfPropertyChange(() => SelectedAreaVisibility);
			}
		}

        /// <summary>
        /// The manipulation completed event handler.
        /// </summary>
        /// <param name="e">The <see cref="ManipulationCompletedRoutedEventArgs"/> instance containing the event data.</param>
        public void ManipulationCompleted(ManipulationCompletedRoutedEventArgs e)
        {
            e.Handled = true;
        }

        #region ManipulationDelta

        /// <summary>
        /// The top left manipulation delta event handler.
        /// </summary>
        /// <param name="e">The <see cref="ManipulationDeltaRoutedEventArgs"/> instance containing the event data.</param>
        public void TopLeftManipulationDelta(ManipulationDeltaRoutedEventArgs e)
        {
            if (e.Delta.Translation.X < 0)
            {
                double newRectLeft = Math.Max(ImageLeft, RectLeft + e.Delta.Translation.X);
                double widthDelta = RectLeft - newRectLeft;
                RectWidth += widthDelta;
                RectLeft = newRectLeft;
            }
            else if (e.Delta.Translation.X > 0)
            {
                double newWidth = Math.Max(30, RectWidth - e.Delta.Translation.X);
                double delta = RectWidth - newWidth;
                RectLeft += delta;
                RectWidth -= delta;
            }

            if (e.Delta.Translation.Y < 0)
            {
                double newRectTop = Math.Max(ImageTop, RectTop + e.Delta.Translation.Y);
                double delta = RectTop - newRectTop;
                RectTop -= delta;
                RectHeight += delta;
            }
            else if (e.Delta.Translation.Y > 0)
            {
                double newHeight = Math.Max(30, RectHeight - e.Delta.Translation.Y);
                double delta = RectHeight - newHeight;
                RectTop += delta;
                RectHeight -= delta;
            }
            e.Handled = true;
        }

        /// <summary>
        /// The bottom left manipulation delta event handler.
        /// </summary>
        /// <param name="e">The <see cref="ManipulationDeltaRoutedEventArgs"/> instance containing the event data.</param>
        public void BottomLeftManipulationDelta(ManipulationDeltaRoutedEventArgs e)
        {
            if (e.Delta.Translation.X < 0)
            {
                double newRectLeft = Math.Max(ImageLeft, RectLeft + e.Delta.Translation.X);
                double widthDelta = RectLeft - newRectLeft;
                RectWidth += widthDelta;
                RectLeft = newRectLeft;
            }
            else if (e.Delta.Translation.X > 0)
            {
                double newWidth = Math.Max(30, RectWidth - e.Delta.Translation.X);
                double delta = RectWidth - newWidth;
                RectLeft += delta;
                RectWidth -= delta;
            }

            if (e.Delta.Translation.Y < 0)
            {
                double newHeight = Math.Max(30, RectHeight + e.Delta.Translation.Y);
                double delta = RectHeight - newHeight;
                RectHeight -= delta;
            }
            if (e.Delta.Translation.Y > 0)
            {
                double newHeight = Math.Min(ImageTop + ImageHeight - RectTop, RectHeight + e.Delta.Translation.Y);
                double delta = newHeight - RectHeight;
                RectHeight += delta;
            }
            e.Handled = true;
        }

        /// <summary>
        /// The top right manipulation delta event handler.
        /// </summary>
        /// <param name="e">The <see cref="ManipulationDeltaRoutedEventArgs"/> instance containing the event data.</param>
        public void TopRightManipulationDelta(ManipulationDeltaRoutedEventArgs e)
        {
            if (e.Delta.Translation.X < 0)
            {
                double newWidth = (Math.Max(30, RectWidth + e.Delta.Translation.X));
                double delta = RectWidth - newWidth;
                RectWidth -= delta;
            }

            else if (e.Delta.Translation.X > 0)
            {
                double newWidth = Math.Min(ImageLeft + ImageWidth - RectLeft, RectWidth + e.Delta.Translation.X);
                double delta = newWidth - RectWidth;
                RectWidth += delta;
            }
            if (e.Delta.Translation.Y < 0)
            {
                double newRectTop = Math.Max(ImageTop, RectTop + e.Delta.Translation.Y);
                double delta = RectTop - newRectTop;
                RectTop -= delta;
                RectHeight += delta;
            }
            else if (e.Delta.Translation.Y > 0)
            {
                double newHeight = Math.Max(30, RectHeight - e.Delta.Translation.Y);
                double delta = RectHeight - newHeight;
                RectTop += delta;
                RectHeight -= delta;
            }
            e.Handled = true;
        }

        /// <summary>
        /// The bottom right manipulation delta event handler.
        /// </summary>
        /// <param name="e">The <see cref="ManipulationDeltaRoutedEventArgs"/> instance containing the event data.</param>
        public void BottomRightManipulationDelta(ManipulationDeltaRoutedEventArgs e)
        {
            if (e.Delta.Translation.X < 0)
            {
                double newWidth = (Math.Max(30, RectWidth + e.Delta.Translation.X));
                double delta = RectWidth - newWidth;
                RectWidth -= delta;
            }

            else if (e.Delta.Translation.X > 0)
            {
                double newWidth = Math.Min(ImageLeft + ImageWidth - RectLeft, RectWidth + e.Delta.Translation.X);
                double delta = newWidth - RectWidth;
                RectWidth += delta;
            }

            if (e.Delta.Translation.Y < 0)
            {
                double newHeight = Math.Max(30, RectHeight + e.Delta.Translation.Y);
                double delta = RectHeight - newHeight;
                RectHeight -= delta;
            }
            if (e.Delta.Translation.Y > 0)
            {
                double newHeight = Math.Min(ImageTop + ImageHeight - RectTop, RectHeight + e.Delta.Translation.Y);
                double delta = newHeight - RectHeight;
                RectHeight += delta;
            }

            e.Handled = true;
        }

        #endregion

        /// <summary>
        /// Navigates the screen back.
        /// </summary>
        public void GoBack()
        {
            NavigationService.GoBack();
        }

        private IGraphSpace _Workspace;
        /// <summary>
        /// Gets or sets the workspace.
        /// </summary>
        /// <value>
        /// The workspace.
        /// </value>
        public IGraphSpace Workspace
        {
            get { return _Workspace; }
            set
            {
                _Workspace = value;
                NotifyOfPropertyChange(() => Workspace);
            }
        }

		private WriteableBitmap _CroppedImage;
		public WriteableBitmap CroppedImage
		{
			get { return _CroppedImage; }
			set
			{
				_CroppedImage = value;
				NotifyOfPropertyChange(() => CroppedImage);
			}
		}

		private bool _IsCropEnabled = true;
		public bool IsCropEnabled
		{
			get { return _IsCropEnabled; }
			set
			{
				_IsCropEnabled = value;
				NotifyOfPropertyChange(() => IsCropEnabled);
			}
		}

		private bool _IsRecognizeEnabled = false;
		public bool IsRecognizeEnabled
		{
			get { return _IsRecognizeEnabled; }
			set
			{
				_IsRecognizeEnabled = value;
				NotifyOfPropertyChange(() => IsRecognizeEnabled);
			}
		}

        /// <summary>
        /// Gets or sets the zoom factor.
        /// </summary>
        /// <value>
        /// The zoom factor.
        /// </value>
        private double ZoomFactor { get; set; }

        /// <summary>
        /// The Canvas size changed event handler.
        /// </summary>
        /// <param name="e">The <see cref="Windows.UI.Xaml.SizeChangedEventArgs"/> instance containing the event data.</param>
        public void CanvasSizeChanged(Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            double WorkspaceWidth = e.NewSize.Width;
            double WorkspaceHeight = e.NewSize.Height;

            double ActualWidth = Workspace.Image.PixelWidth;
            double ActualHeight = Workspace.Image.PixelHeight;

            ZoomFactor = 1.0;
            if (WorkspaceWidth < ActualWidth)
                ZoomFactor = WorkspaceWidth / ActualWidth;

            if (WorkspaceHeight < ActualHeight && WorkspaceHeight / ActualHeight < ZoomFactor)
                ZoomFactor = WorkspaceHeight / ActualHeight;

            ImageWidth = ActualWidth * ZoomFactor;
            ImageHeight = ActualHeight * ZoomFactor;

            ImageLeft = (WorkspaceWidth - ImageWidth) / 2.0;
            ImageTop = (WorkspaceHeight - ImageHeight) / 2.0;

            RectWidth = Math.Min(300, ImageWidth);
            RectHeight = Math.Min(300, ImageHeight);

            RectLeft = (WorkspaceWidth - RectWidth) / 2.0;
            RectTop = (WorkspaceHeight - RectHeight) / 2.0;

        }

        private double _RectHeight;
        /// <summary>
        /// Gets or sets the height of the rect.
        /// </summary>
        /// <value>
        /// The height of the rect.
        /// </value>
        public double RectHeight
        {
            get { return _RectHeight; }
            set
            {
                _RectHeight = value;
                NotifyOfPropertyChange(() => RectHeight);
                //NotifyOfPropertyChange(() => Rect3);
            }
        }


        private double _RectWidth;
        /// <summary>
        /// Gets or sets the width of the rect.
        /// </summary>
        /// <value>
        /// The width of the rect.
        /// </value>
        public double RectWidth
        {
            get { return _RectWidth; }
            set
            {
                _RectWidth = value;
                NotifyOfPropertyChange(() => RectWidth);
                //NotifyOfPropertyChange(() => Rect4);
            }
        }


        private double _RectTop;
        /// <summary>
        /// Gets or sets the rect top.
        /// </summary>
        /// <value>
        /// The rect top.
        /// </value>
        public double RectTop
        {
            get { return _RectTop; }
            set
            {
                _RectTop = value;
                NotifyOfPropertyChange(() => RectTop);
                //NotifyOfPropertyChange(() => Rect1);
            }
        }


        private double _RectLeft;
        /// <summary>
        /// Gets or sets the rect left.
        /// </summary>
        /// <value>
        /// The rect left.
        /// </value>
        public double RectLeft
        {
            get { return _RectLeft; }
            set
            {
                _RectLeft = value;
                NotifyOfPropertyChange(() => RectLeft);
                //NotifyOfPropertyChange(() => Rect2);
            }
        }


        private double _ImageHeight;
        /// <summary>
        /// Gets or sets the height of the image.
        /// </summary>
        /// <value>
        /// The height of the image.
        /// </value>
        public double ImageHeight
        {
            get { return _ImageHeight; }
            set
            {
                _ImageHeight = value;
                NotifyOfPropertyChange(() => ImageHeight);
            }
        }


        private double _ImageWidth;
        /// <summary>
        /// Gets or sets the width of the image.
        /// </summary>
        /// <value>
        /// The width of the image.
        /// </value>
        public double ImageWidth
        {
            get { return _ImageWidth; }
            set
            {
                _ImageWidth = value;
                NotifyOfPropertyChange(() => ImageWidth);
            }
        }


        private double _ImageTop;
        /// <summary>
        /// Gets or sets the image top.
        /// </summary>
        /// <value>
        /// The image top.
        /// </value>
        public double ImageTop
        {
            get { return _ImageTop; }
            set
            {
                _ImageTop = value;
                NotifyOfPropertyChange(() => ImageTop);
            }
        }


        private double _ImageLeft;
        /// <summary>
        /// Gets or sets the image left.
        /// </summary>
        /// <value>
        /// The image left.
        /// </value>
        public double ImageLeft
        {
            get { return _ImageLeft; }
            set
            {
                _ImageLeft = value;
                NotifyOfPropertyChange(() => ImageLeft);
            }
        }

        public void SetGraphSource( IGraphSpace graphSpace )
        {
            this.Workspace = graphSpace;
			this.CroppedImage = Workspace.Image.Copy();
        }

        //public Rect Rect1
        //{
        //    get { return new Rect(0, 0, ImageWidth, RectTop-ImageTop); }
        //}
        //public Rect Rect2
        //{
        //    get { return new Rect(0, RectTop - ImageTop, RectLeft - ImageLeft, RectHeight); }
        //}
        //public Rect Rect3
        //{
        //    get { return new Rect(ImageLeft, RectTop+RectHeight, ImageWidth, ImageTop+ImageHeight-RectTop-RectHeight); }
        //}
        //public Rect Rect4
        //{
        //    get { return new Rect(RectLeft + RectWidth-ImageLeft, RectTop - ImageTop, ImageWidth + ImageLeft - RectLeft - RectWidth, RectHeight); }
        //}

    }
}
