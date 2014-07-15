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
        /// The  width
        /// </summary>
        private double _Width;
        /// <summary>
        /// Gets the  width of the image.
        /// </summary>
        public double Width
        {
            get { return _Width; }
            set
            {
                _Width = value;
                NotifyOfPropertyChange(() => Width);
            }
        }

        /// <summary>
        /// The  height
        /// </summary>
        private double _Height;
        /// <summary>
        /// Gets the  height of the image.
        /// </summary>
        public double Height
        {
            get { return _Height; }
            set
            {
                _Height = value;
                NotifyOfPropertyChange(() => Height);
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


    }
}
