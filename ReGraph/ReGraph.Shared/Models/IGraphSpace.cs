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
        double WorkspaceWidth { get; set; }
        /// <summary>
        /// Gets the height of the workspace (usually it's equal to screen's resolution)
        /// </summary>
        double WorkspaceHeight { get; set; }
        /// <summary>
        /// Gets the actual width of the image.
        /// </summary>
        double ActualWidth { get; set; }
        /// <summary>
        /// Gets the actual height of the image.
        /// </summary>
        double ActualHeight { get; set; }

        /// <summary>
        /// Gets or sets the image processing tool.
        /// </summary>
        IImageProcessing ImageProcessing { get; set; }


        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>
        /// The image.
        /// </value>
        WriteableBitmap Image { get; set; }

        
        /// <summary>
        /// Occurs when [workspace size changed].
        /// </summary>
        event EventHandler<SizeChangedEventArgs> WorkspaceSizeChanged;
        /// <summary>
        /// Occurs when [actual size changed].
        /// </summary>
        event EventHandler<SizeChangedEventArgs> ActualSizeChanged;
        /// <summary>
        /// Occurs when [UI refreshed].
        /// </summary>
        event EventHandler<EventArgs> UIRefreshed;

        /// <summary>
        /// Gets or sets the Canvas's children UIElements (images, drawings, texts, etc.)
        /// </summary>
        UIElementCollection Children { get; set; }
        

        /// <summary>
        /// Updates the Workspace's size.
        /// </summary>
        /// <param name="workspaceWidth">Width of the workspace.</param>
        /// <param name="workspaceHeight">Height of the workspace.</param>
        void UpdateWorkspaceSize(double workspaceWidth, double workspaceHeight);
        /// <summary>
        /// Invalidates UI (ScrollViewer and Canvas)
        /// </summary>
        void RefreshUI();
        /// <summary>
        /// Updates the actual size of the image.
        /// </summary>
        /// <param name="actualWidth">The actual image's width.</param>
        /// <param name="actualHeight">The actual image's height.</param>
        void UpdateActualSize(double actualWidth, double actualHeight);
    }
}
