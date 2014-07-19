using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace ReGraph.Models
{
    /// <summary>
    /// Workspace interface
    /// </summary>
    public interface IGraphSpace
    {
        /// <summary>
        /// Gets the actual width of the image.
        /// </summary>
        double Width { get; set; }
        /// <summary>
        /// Gets the actual height of the image.
        /// </summary>
        double Height { get; set; }


        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        WriteableBitmap Image { get; set; }

       
    }
}
