﻿using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace UnitConverter
{
    [Microsoft.Xaml.Interactivity.TypeConstraint(typeof(Windows.UI.Xaml.Controls.Panel))]
    public class OrientationBehavior : DependencyObject, IBehavior
    {
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public Windows.UI.Xaml.DependencyObject AssociatedObject { get; set; }

        public void Attach(Windows.UI.Xaml.DependencyObject associatedObject)
        {
            this.AssociatedObject = associatedObject;
            Window.Current.SizeChanged += Current_SizeChanged;
            Window.Current.Activated += Current_Activated;
        }

        public void Detach()
        {
            Window.Current.SizeChanged -= Current_SizeChanged;
            Window.Current.Activated -= Current_Activated;
        }

        void Current_Activated(object sender, Windows.UI.Core.WindowActivatedEventArgs e) { UpdateOrientation(); }

        void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e) { UpdateOrientation(); }

        private void UpdateOrientation()
        {
            var control = VisualStateUtilities.FindNearestStatefulControl(this.AssociatedObject as FrameworkElement);
            if (control == null)
            {
                //System.Diagnostics.Debugger.Break();
                return;
            }

            if (Window.Current.Bounds.Width <= this.NarrowWidth)
            {
                // narrow
                if (!string.IsNullOrEmpty(this.NarrowStateName))
                    VisualStateManager.GoToState(control, this.NarrowStateName, this.Transitions);
            }
            else
            {
                switch (Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().Orientation)
                {
                    // landscape
                    case Windows.UI.ViewManagement.ApplicationViewOrientation.Landscape:
                        if (!string.IsNullOrEmpty(this.LandscapeStateName))
                            VisualStateManager.GoToState(control, this.LandscapeStateName, this.Transitions);
                        break;
                    // portrait
                    case Windows.UI.ViewManagement.ApplicationViewOrientation.Portrait:
                        if (!string.IsNullOrEmpty(this.PortraitStateName))
                            VisualStateManager.GoToState(control, this.PortraitStateName, this.Transitions);
                        break;
                }
            }
        }

        [CustomPropertyValueEditor(Microsoft.Xaml.Interactivity.CustomPropertyValueEditor.StateName)]
        public string LandscapeStateName
        {
            get { return (string)GetValue(LandscapeStateNameProperty); }
            set { SetValue(LandscapeStateNameProperty, value); }
        }
        public static readonly DependencyProperty LandscapeStateNameProperty =
            DependencyProperty.Register("LandscapeStateName", typeof(string), typeof(OrientationBehavior), new PropertyMetadata(null));

        [CustomPropertyValueEditor(Microsoft.Xaml.Interactivity.CustomPropertyValueEditor.StateName)]
        public string PortraitStateName
        {
            get { return (string)GetValue(PortraitStateNameProperty); }
            set { SetValue(PortraitStateNameProperty, value); }
        }
        public static readonly DependencyProperty PortraitStateNameProperty =
            DependencyProperty.Register("PortraitStateName", typeof(string), typeof(OrientationBehavior), new PropertyMetadata(null));

        [CustomPropertyValueEditor(Microsoft.Xaml.Interactivity.CustomPropertyValueEditor.StateName)]
        public string NarrowStateName
        {
            get { return (string)GetValue(NarrowStateNameProperty); }
            set { SetValue(NarrowStateNameProperty, value); }
        }
        public static readonly DependencyProperty NarrowStateNameProperty =
            DependencyProperty.Register("NarrowStateName", typeof(string), typeof(OrientationBehavior), new PropertyMetadata(null));

        public int NarrowWidth
        {
            get { return (int)GetValue(NarrowWidthProperty); }
            set { SetValue(NarrowWidthProperty, value); }
        }
        public static readonly DependencyProperty NarrowWidthProperty =
            DependencyProperty.Register("NarrowWidth", typeof(int), typeof(OrientationBehavior), new PropertyMetadata(320));

        public bool Transitions
        {
            get { return (bool)GetValue(TransitionsProperty); }
            set { SetValue(TransitionsProperty, value); }
        }
        public static readonly DependencyProperty TransitionsProperty =
            DependencyProperty.Register("Transitions", typeof(bool), typeof(OrientationBehavior), new PropertyMetadata(true));
    }
}
