using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Windows.Media.Capture;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ReGraph.Common
{
    public static class CaptureElementSourceBinding
    {
        public static MediaCapture GetCaptureSource(DependencyObject obj)
        {
            return (MediaCapture)obj.GetValue(CaptureSourceProperty);
        }

        public static void SetCaptureSource(DependencyObject obj,
                MediaCapture value)
        {
            obj.SetValue(CaptureSourceProperty, value);
        }

        public static readonly DependencyProperty CaptureSourceProperty
            = DependencyProperty.RegisterAttached("VideoSource",
            typeof(MediaCapture),
            typeof(CaptureElementSourceBinding),
            new PropertyMetadata(null, onVideoSourcePropertyChanged));

        private static async void onVideoSourcePropertyChanged(
                DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debug.Assert(d is CaptureElement);
            Debug.Assert(e.Property == CaptureElementSourceBinding.CaptureSourceProperty);

            CaptureElement preview = d as CaptureElement;

            if (d != null)
            {
                if (preview.Source != null)
                {
                    // If another camera was attached before, stop it.
                    await preview.Source.StopPreviewAsync();
                }

                try
                {
                    preview.Source = (MediaCapture)e.NewValue;
                }
                catch
                {
                    preview.Source = null;
                }

                if (preview.Source != null)
                {
                    // Start the preview stream.
                    await preview.Source.StartPreviewAsync();
                }
            }
        }
    }
}
