using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReGraph.Common
{
    /// <summary>
    /// Manages handling of the size changed event args.
    /// </summary>
    public sealed class CustomSizeChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the new width.
        /// </summary>
        /// <value>
        /// The new width.
        /// </value>
        public double NewWidth { get; private set; }
        /// <summary>
        /// Gets the new height.
        /// </summary>
        /// <value>
        /// The new height.
        /// </value>
        public double NewHeight { get; private set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomSizeChangedEventArgs"/> class.
        /// </summary>
        /// <param name="newWidth">The new width.</param>
        /// <param name="newHeight">The new height.</param>
        public CustomSizeChangedEventArgs(double newWidth, double newHeight)
        {
            this.NewWidth = newWidth;
            this.NewHeight = newHeight;
        }
    }
}
