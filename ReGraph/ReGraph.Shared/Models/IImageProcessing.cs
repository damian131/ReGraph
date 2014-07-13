using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace ReGraph.Models
{
    /// <summary>
    /// Image processing interface
    /// </summary>
    public interface IImageProcessing
    {
        /// <summary>
        /// Merges all UIElements in a workspace canvas into IImageWrapper instance.
        /// It default takes true value which induce removing text tool elements from the workspace.
        /// </summary>
        /// <returns></returns>
        Task MergeWorkspaceAsync(bool IfRemoveTextToolElements = true);
        /// <summary>
        /// Gets the image of a Workspace
        /// </summary>
        /// <returns>
        /// IImageWrapper instance
        /// </returns>
        Task<WriteableBitmap> GetImageAsync();
    }
}
