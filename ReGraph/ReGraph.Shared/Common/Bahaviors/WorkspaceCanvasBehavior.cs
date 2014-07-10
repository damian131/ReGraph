using Microsoft.Xaml.Interactivity;
using System;
using Windows.UI;
using Windows.UI.Input;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Shapes;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;
using Caliburn.Micro;
using Windows.Storage.Streams;
using System.IO;
using Windows.Graphics.Imaging;
using Windows.Foundation;
using Windows.Storage.Pickers;
using Windows.Storage;
using System.Diagnostics;
using Windows.UI.Core;
using Windows.ApplicationModel.Core;
using Windows.UI.Xaml.Input;
using ReGraph.Models;

namespace ReGraph.Common
{
    /// <summary>
    /// Extends Canvas control allowing it to add drawing functionality.
    /// </summary>
    public class WorkspaceCanvasBehavior : DependencyObject, IBehavior
    {
        /// <summary>
        /// Gets or sets the workspace.
        /// </summary>
        /// <value>
        /// The workspace.
        /// </value>
        public IGraphSpace Workspace
        {
            get { 
                return (IGraphSpace)GetValue(WorkspaceProperty); }
            set { 
                SetValue(WorkspaceProperty, value); }
        }
        /// <summary>
        /// The workspace property
        /// </summary>
        public static readonly DependencyProperty WorkspaceProperty =
            DependencyProperty.Register("Workspace", typeof(object), typeof(WorkspaceCanvasBehavior), new PropertyMetadata(null, WorkspaceChanged));
        /// <summary>
        /// Workspaces the changed.
        /// </summary>
        /// <param name="d">The d.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void WorkspaceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WorkspaceCanvasBehavior wcb = d as WorkspaceCanvasBehavior;
            if (e.OldValue != null)
            {
                (e.OldValue as IGraphSpace).ActualSizeChanged -= wcb.ActualSizeChanged;
                (e.OldValue as IGraphSpace).WorkspaceSizeChanged -= wcb.WorkspaceSizeChanged;
                (e.OldValue as IGraphSpace).UIRefreshed -= wcb.UIRefreshed;

            }

            if (e.NewValue != null)
            {
                (e.NewValue as IGraphSpace).ActualSizeChanged += wcb.ActualSizeChanged;
                (e.NewValue as IGraphSpace).WorkspaceSizeChanged += wcb.WorkspaceSizeChanged;
                (e.NewValue as IGraphSpace).Children = wcb.WorkspaceCanvas.Children;
                (e.NewValue as IGraphSpace).UIRefreshed += wcb.UIRefreshed;
                //(e.NewValue as IWorkspace).ImageProcessing = wcb;
            }
        }

        /// <summary>
        /// Refreshes bounds of the Canvas.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void UIRefreshed(object sender, EventArgs e)
        {
            WorkspaceCanvas.Width = Workspace.ActualWidth;
            WorkspaceCanvas.Height = Workspace.ActualHeight;

            WorkspaceCanvas.InvalidateMeasure();

        }

        /// <summary>
        /// Refreshes UI - called when Image's size has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Models.SizeChangedEventArgs"/> instance containing the event data.</param>
        private void ActualSizeChanged(object sender, Models.SizeChangedEventArgs e)
        {
            WorkspaceCanvas.Width = e.NewWidth;
            WorkspaceCanvas.Height = e.NewHeight;

            WorkspaceCanvas.InvalidateMeasure();
        }

        /// <summary>
        /// Updates UI when size of the workspace has changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Models.SizeChangedEventArgs"/> instance containing the event data.</param>
        private void WorkspaceSizeChanged(object sender, Models.SizeChangedEventArgs e)
        {
            WorkspaceCanvas.Width = e.NewWidth;
            WorkspaceCanvas.Height = e.NewHeight;

            WorkspaceCanvas.InvalidateMeasure();
        }

        /// <summary>
        /// Gets the associated object.
        /// </summary>
        /// <value>
        /// The associated object.
        /// </value>
        public DependencyObject AssociatedObject { get; private set; }
        /// <summary>
        /// Gets or sets the workspace canvas.
        /// </summary>
        /// <value>
        /// The workspace canvas.
        /// </value>
        private Canvas WorkspaceCanvas { get; set; }

        /// <summary>
        /// Attaches the specified associated object.
        /// </summary>
        /// <param name="associatedObject">The associated object.</param>
        public void Attach(DependencyObject associatedObject)
        {
            AssociatedObject = associatedObject;
            WorkspaceCanvas = AssociatedObject as Canvas;
            WorkspaceCanvas.PointerPressed += WorkspaceCanvas_PointerPressed;
            WorkspaceCanvas.SizeChanged += WorkspaceCanvas_SizeChanged;
        }

        void WorkspaceCanvas_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            if (Workspace != null)
            {
                Workspace.WorkspaceWidth = e.NewSize.Width;
                Workspace.WorkspaceHeight = e.NewSize.Height;
            }
        }

        /// <summary>
        /// Handles the PointerPressed event of the WorkspaceCanvas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PointerRoutedEventArgs"/> instance containing the event data.</param>
        void WorkspaceCanvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            //pointerArgs = e;
            //if (Workspace != null && Workspace.ActiveDrawingTool != null)
            //{
            //    if (Workspace.ActiveDrawingTool is PointerTool)
            //        WorkspaceCanvas.ManipulationMode = ManipulationModes.System;
            //    else
            //    {
            //        WorkspaceCanvas.ManipulationMode = ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            //        WorkspaceCanvas.ManipulationDelta += WorkspaceCanvas_ManipulationDelta;
            //        WorkspaceCanvas.PointerReleased += WorkspaceCanvas_PointerReleased;
            //    }
            //}
        }

        /// <summary>
        /// The pointer arguments
        /// </summary>
        private PointerRoutedEventArgs pointerArgs = null;

        /// <summary>
        /// Handles the ManipulationDelta event of the WorkspaceCanvas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ManipulationDeltaRoutedEventArgs"/> instance containing the event data.</param>
        void WorkspaceCanvas_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            //if (WorkspaceCanvas.CapturePointer(pointerArgs.Pointer) || Workspace.ActiveDrawingTool.IsRecordingOutOfBounds)
            //    Workspace.ActiveDrawingTool.Update(pointerArgs.GetCurrentPoint(WorkspaceCanvas));
        }

        /// <summary>
        /// Handles the PointerReleased event of the WorkspaceCanvas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="PointerRoutedEventArgs"/> instance containing the event data.</param>
        void WorkspaceCanvas_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            WorkspaceCanvas.ManipulationMode = ManipulationModes.None;
            WorkspaceCanvas.ManipulationDelta -= WorkspaceCanvas_ManipulationDelta;
            WorkspaceCanvas.PointerReleased -= WorkspaceCanvas_PointerReleased;
        }

        /// <summary>
        /// Handles the PointerMoved event of the WorkspaceCanvas control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.Input.PointerRoutedEventArgs"/> instance containing the event data.</param>
        void WorkspaceCanvas_PointerMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            //if (WorkspaceCanvas.CapturePointer(e.Pointer) || Workspace.ActiveDrawingTool.IsRecordingOutOfBounds)
            //    Workspace.ActiveDrawingTool.Update(e.GetCurrentPoint(WorkspaceCanvas));
        }

        /// <summary>
        /// Detaches this instance.
        /// </summary>
        public void Detach()
        {
            AssociatedObject = null;
            WorkspaceCanvas = null;
        }


        /// <summary>
        /// Merges all UIElements in a workspace canvas into IImageWrapper instance.
        /// It default takes true value which induce removing text tool elements from the workspace.
        /// </summary>
        /// <returns></returns>
        //public async Task MergeWorkspaceAsync( bool IfRemoveTextToolElements = true )
        //{
        //    if (Workspace.Children.Count <= 1)
        //        return;

        //    if (IfRemoveTextToolElements)
        //    {
        //        var textToolElements = Workspace.Children.OfType<Grid>().ToList();
        //        foreach (var element in textToolElements)
        //        {
        //            Workspace.Children.Remove(element);
        //        }
        //    }
               

        //    Workspace.Image = await GetImageAsync();
            
        //    while (Workspace.Children.Count > 1)
        //        Workspace.Children.RemoveAt(1);
        //}

        /// <summary>
        /// Gets the current state of the Canvas and returns the result as the Image representation.
        /// </summary>
        /// <returns>
        /// WriteableBitmap instance.
        /// </returns>
        //public async Task<WriteableBitmap> GetImageAsync()
        //{
        //    Rect r = new Rect(new Point(0, 0), new Point(Workspace.ActualWidth, Workspace.ActualHeight));
        //    for (int i = 1; i < WorkspaceCanvas.Children.Count && !(WorkspaceCanvas.Children[i] is TextBox); ++i)
        //        WorkspaceCanvas.Children[i].Clip = new RectangleGeometry() { Rect = r };

        //    RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap();
        //    await renderTargetBitmap.RenderAsync(WorkspaceCanvas, (int)WorkspaceCanvas.Width, (int)WorkspaceCanvas.Height);

        //    IBuffer pixels = await renderTargetBitmap.GetPixelsAsync();
        //    byte[] bytes = pixels.ToArray();

        //    WriteableBitmap wb = new WriteableBitmap((int)WorkspaceCanvas.Width, (int)WorkspaceCanvas.Height);
        //    wb. FromByteArray(bytes);

        //    return wb;
        //}


    }
}
