using Caliburn.Micro;
using System;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace ReGraph.Models
{
    /// <summary>
    /// Manages the workspace components and specified operations associated with the workspace processing.
    /// </summary>
    public class GraphSpace : PropertyChangedBase, IGraphSpace
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GraphSpace"/> class.
        /// </summary>
        public GraphSpace()
        {

        }


        /// <summary>
        /// The workspace width
        /// </summary>
        private double _WorkspaceWidth;
        /// <summary>
        /// Gets the width of the workspace (usually it's equal to screen's resolution)
        /// </summary>
        public double WorkspaceWidth
        {
            get { return _WorkspaceWidth; }
            set
            {
                _WorkspaceWidth = value;
                NotifyOfPropertyChange(() => WorkspaceWidth);

            }
        }

        /// <summary>
        /// The workspace height
        /// </summary>
        private double _WorkspaceHeight;
        /// <summary>
        /// Gets the height of the workspace (usually it's equal to screen's resolution)
        /// </summary>
        public double WorkspaceHeight
        {
            get { return _WorkspaceHeight; }
            set
            {
                _WorkspaceHeight = value;
                NotifyOfPropertyChange(() => WorkspaceHeight);
            }
        }

        /// <summary>
        /// The actual width
        /// </summary>
        private double _ActualWidth;
        /// <summary>
        /// Gets the actual width of the image.
        /// </summary>
        public double ActualWidth
        {
            get { return _ActualWidth; }
            set
            {
                _ActualWidth = value;
                NotifyOfPropertyChange(() => ActualWidth);
                // Send event by Caliburn.Micro's EventAggregator to inform objects interested in size change
                IoC.Get<IEventAggregator>().PublishOnCurrentThread(new SizeChangedEventArgs(ActualWidth, ActualHeight));
            }
        }

        /// <summary>
        /// The actual height
        /// </summary>
        private double _ActualHeight;
        /// <summary>
        /// Gets the actual height of the image.
        /// </summary>
        public double ActualHeight
        {
            get { return _ActualHeight; }
            set
            {
                _ActualHeight = value;
                NotifyOfPropertyChange(() => ActualHeight);
                // Send event by Caliburn.Micro's EventAggregator to inform objects interested in size change
                IoC.Get<IEventAggregator>().PublishOnCurrentThread(new SizeChangedEventArgs(ActualWidth, ActualHeight));
            }
        }

        /// <summary>
        /// The children
        /// </summary>
        private UIElementCollection _Children;
        /// <summary>
        /// Gets or sets the Canvas's children UIElements (images, drawings, texts, etc.)
        /// </summary>
        public UIElementCollection Children
        {
            get { return _Children; }
            set
            {
                _Children = value;
                NotifyOfPropertyChange(() => Children);
            }
        }

        /// <summary>
        /// The image processing
        /// </summary>
        private IImageProcessing _ImageProcessing;
        /// <summary>
        /// Gets or sets the image processing tool.
        /// </summary>
        public IImageProcessing ImageProcessing
        {
            get { return _ImageProcessing; }
            set
            {
                _ImageProcessing = value;
                NotifyOfPropertyChange(() => ImageProcessing);
            }
        }


        /// <summary>
        /// The image
        /// </summary>
        private WriteableBitmap _Image;
        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        public WriteableBitmap Image
        {
            get { return _Image; }
            set
            {
                _Image = value;
                NotifyOfPropertyChange(() => Image);
            }
        }


        /// <summary>
        /// Occurs when [workspace size changed].
        /// </summary>
        public event EventHandler<SizeChangedEventArgs> WorkspaceSizeChanged;
        /// <summary>
        /// Raises the <see cr="E:WorkspaceSizeChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="SizeChangedEventArgs"/> instance containing the event data.</param>
        protected void OnWorkspaceSizeChanged(SizeChangedEventArgs e)
        {
            if (WorkspaceSizeChanged != null)
                WorkspaceSizeChanged(this, e);
        }

        /// <summary>
        /// Occurs when [actual size changed].
        /// </summary>
        public event EventHandler<SizeChangedEventArgs> ActualSizeChanged;
        /// <summary>
        /// Raises the <see cref="E:ActualSizeChanged" /> event.
        /// </summary>
        /// <param name="e">The <see cref="SizeChangedEventArgs"/> instance containing the event data.</param>
        protected void OnActualSizeChanged(SizeChangedEventArgs e)
        {
            if (ActualSizeChanged != null)
            {
                ActualSizeChanged(this, e);
            }
        }

        /// <summary>
        /// Occurs when [UI refreshed].
        /// </summary>
        public event EventHandler<EventArgs> UIRefreshed;

        
        /// <summary>
        /// Updates the Workspace's size.
        /// </summary>
        /// <param name="workspaceWidth">Width of the workspace.</param>
        /// <param name="workspaceHeight">Height of the workspace.</param>
        public void UpdateWorkspaceSize(double workspaceWidth, double workspaceHeight)
        {
            if (WorkspaceWidth != workspaceWidth || WorkspaceHeight != workspaceHeight)
            {
                WorkspaceWidth = workspaceWidth;
                WorkspaceHeight = workspaceHeight;
                OnWorkspaceSizeChanged(new SizeChangedEventArgs(WorkspaceWidth, WorkspaceHeight));
            }

        }

        /// <summary>
        /// Invalidates UI (ScrollViewer and Canvas)
        /// </summary>
        public void RefreshUI()
        {
            if (UIRefreshed != null)
                UIRefreshed(this, new EventArgs());
        }

        /// <summary>
        /// Updates the actual size of the image.
        /// </summary>
        /// <param name="actualWidth">The actual image's width.</param>
        /// <param name="actualHeight">The actual image's height.</param>
        public void UpdateActualSize(double actualWidth, double actualHeight)
        {
            if (ActualWidth != actualWidth || ActualHeight != actualHeight)
            {
                ActualWidth = actualWidth;
                ActualHeight = actualHeight;
                OnActualSizeChanged(new SizeChangedEventArgs(ActualWidth, ActualHeight));
            }

        }

    }
}
