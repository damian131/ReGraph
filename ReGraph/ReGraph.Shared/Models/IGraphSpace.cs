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
    /// Represents part of the workspace, which (will be calculated via Actual&Workspace sizes)
    /// </summary>
    public enum WorkspacePart
    {
        /// <summary>
        /// Center of the image will be automatically calculated and applied i.e in ZoomToFactor functionality
        /// </summary>
        Center,
        /// <summary>
        /// Represents upper-left part of the image, which is equal to point (0, 0)
        /// </summary>
        UpperLeft
    }

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
