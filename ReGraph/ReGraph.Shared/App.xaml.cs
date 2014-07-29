using System;
using System.Collections.Generic;
using Caliburn.Micro;
using ReGraph.Views;
using ReGraph.ViewModels;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
// The Blank Application template is documented at http://go.microsoft.com/fwlink/?LinkId=234227

namespace ReGraph
{
    public sealed partial class App
    {
        private WinRTContainer container;
        private INavigationService navigationService;

#if WINDOWS_PHONE_APP
        ContinuationManager continuationManager;
#endif

        public App()
        {
            InitializeComponent();
        }

        protected override void Configure()
        {
            container = new WinRTContainer();

            container.RegisterWinRTServices();

            container.Singleton<MainViewModel>()
                     .PerRequest<CropViewModel>()
					 .PerRequest<NewImageViewModel>()
					 .PerRequest<BaseSettingsViewModel>();

#if WINDOWS_PHONE_APP
            container.PerRequest<CapturePreviewViewModel>();
#endif

            PrepareViewFirst();
        }

        protected override void PrepareViewFirst(Frame rootFrame)
        {
            container.RegisterNavigationService(rootFrame);
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            Initialize();

            var resumed = false;

            if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
            {
                resumed = navigationService.ResumeState();
            }

            if (!resumed)
                DisplayRootView<MainView>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            container.BuildUp(instance);
        }

#if WINDOWS_PHONE_APP
        /// <summary>
        /// Handle OnActivated event to deal with File Open/Save continuation activation kinds
        /// </summary>
        /// <param name="e">Application activated event arguments, it can be casted to proper sub-type based on ActivationKind</param>
        protected override void OnActivated(IActivatedEventArgs e)
        {
            base.OnActivated(e);

            continuationManager = new ContinuationManager();

            var continuationEventArgs = e as IContinuationActivatedEventArgs;
            if (continuationEventArgs != null)
            {
                 // Call ContinuationManager to handle continuation activation
                 continuationManager.Continue(continuationEventArgs);
            }

            Window.Current.Activate();
        }
#endif
    }
}