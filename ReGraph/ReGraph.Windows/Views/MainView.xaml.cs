﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Caliburn.Micro;
using ReGraph.Common;
using ReGraph.Models.GraphDrawer;
using ReGraph.ViewModels;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ReGraph.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainView : Page
    {
        public MainView()
        {
            this.InitializeComponent();
            MainViewModel mvm = IoC.GetInstance(typeof(MainViewModel), null) as MainViewModel;
            mvm.graphDrawer.Axes = Graph.Axes;
            mvm.graphDrawer.Series = Graph.Series;
            mvm.ChartView = Graph;
        }
    }
}
