using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AvalonDock;
using AvalonDock.Layout.Serialization;
using Caliburn.Micro;
using CaliburnMicroAvalonDockBug.Properties;
using CaliburnMicroAvalonDockBug.ViewModels.Panes;
using CaliburnMicroAvalonDockBug.Views;
using Microsoft.Extensions.Logging;

namespace CaliburnMicroAvalonDockBug.ViewModels
{
    public class DockingHostViewModel : Conductor<Screen>.Collection.OneActive
    {
        private readonly ILogger<DockingHostViewModel> logger_;
        private readonly DesignSurfaceViewModel designSurfaceViewModel_;
        private readonly Tab1ViewModel tab1ViewModel_;
        private readonly Tab2ViewModel tab2ViewModel_;

        private string _lastLayout = "";

        ObservableCollection<ToolViewModel> _tools = new();
        public ObservableCollection<ToolViewModel> Tools
        {
            get => _tools;
            set
            {
                _tools = value;
                NotifyOfPropertyChange(() => Tools);
            }
        }

        ObservableCollection<PaneViewModel> _documents = new();

        public ObservableCollection<PaneViewModel> Documents
        {
            get => _documents;
            set
            {
                _documents = value;
                NotifyOfPropertyChange(() => Documents);
            }
        }

        private DockingManager _dockingManager = new();


        // Required for design-time binding.
        public DockingHostViewModel()
        {
            //no-op
        }
// define the conditional compilation symbol to enabled this path
#if USE_CONSTRUCTOR_DI
        public DockingHostViewModel(ILogger<DockingHostViewModel> logger, DesignSurfaceViewModel designSurfaceViewModel, Tab1ViewModel tab1ViewModel, Tab2ViewModel tab2ViewModel)
        {
            logger_ = logger;
            designSurfaceViewModel_ = designSurfaceViewModel;
            tab1ViewModel_ = tab1ViewModel;
            tab2ViewModel_ = tab2ViewModel;
        }
#else
        public DockingHostViewModel(ILogger<DockingHostViewModel> logger)
        {
            logger_ = logger;
        }
#endif

        

        /// <summary>
        /// Binds the viewmodel to its view prior to activating so that the OnViewAttached method of the
        /// child viewmodel are called.
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <returns></returns>
        private async Task ActivateItemAsync<TViewModel>(CancellationToken cancellationToken = default)
            where TViewModel : Screen
        {

            // NOTE:  This is the hack to get OnViewAttached and OnViewReady methods to be called on conducted ViewModels.  Also note
            //   OnViewLoaded is not called.

            var viewModel = IoC.Get<TViewModel>();
            viewModel.Parent = this;
            viewModel.ConductWith(this);
            var view = ViewLocator.LocateForModel(viewModel, null, null);
            ViewModelBinder.Bind(viewModel, view, null);
            await ActivateItemAsync(viewModel, cancellationToken);
        }

        public Task ActivateOrCreate<T>(string displayName)
            where T : Screen
        {
            var item = Items.OfType<T>().FirstOrDefault(x => x.DisplayName == displayName);
            if (item == null)
            {
                var typeToActivate = typeof(T);
                item = (T)Activator.CreateInstance(typeToActivate)!;
                if (item == null)
                {
                    throw new NullReferenceException($"Cannot activate {typeToActivate.Name}");
                }
                item.Parent = this;
                item.ConductWith(this);
                item.DisplayName = displayName;
            }
            return ActivateItemAsync(item, CancellationToken.None);
        }

  

        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            if (_lastLayout == "")
            {
                SaveLayout();
            }

            return base.OnDeactivateAsync(close, cancellationToken);
        }

        public void SaveLayout()
        {
            var layoutSerializer = new XmlLayoutSerializer(this._dockingManager);
            var filePath = Path.Combine(Environment.CurrentDirectory, @"Resources\Layouts\Project.Layout.config");
            layoutSerializer.Serialize(filePath);
        }

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);

            // hook up a reference to the windows dock manager
            if (view is DockingHostView currentView)
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                _dockingManager = (DockingManager)currentView.FindName("dockManager");
            }


        }

        protected override async Task OnInitializeAsync(CancellationToken cancellationToken)
        {
            await Initialize();
            await base.OnInitializeAsync(cancellationToken);
        }


        private async Task Initialize()
        {
            Items.Clear();

// define the conditional compilation symbol to enabled this path
#if USE_CONSTRUCTOR_DI

            // NOTE the hack to get OnViewAttached and OnViewReady methods to be called on conducted ViewModels.  Also note
            //   OnViewLoaded is not called.

            await base.ActivateItemAsync(tab1ViewModel_);
            tab1ViewModel_.Parent = this;
            tab1ViewModel_.ConductWith(this);
            var tab1View = ViewLocator.LocateForModel(tab1ViewModel_, null, null);
            ViewModelBinder.Bind(tab1ViewModel_, tab1View, null);

            await base.ActivateItemAsync(tab2ViewModel_);
            tab2ViewModel_.Parent = this;
            tab2ViewModel_.ConductWith(this);
            var tab2View = ViewLocator.LocateForModel(tab2ViewModel_, null, null);
            ViewModelBinder.Bind(tab2ViewModel_, tab2View, null);

            await base.ActivateItemAsync(designSurfaceViewModel_);
            designSurfaceViewModel_.Parent = this;
            designSurfaceViewModel_.ConductWith(this);
            var designSurfaceView = ViewLocator.LocateForModel(designSurfaceViewModel_, null, null);
            ViewModelBinder.Bind(designSurfaceViewModel_, designSurfaceView, null);

#else
            await ActivateItemAsync<Tab1ViewModel>();
            await ActivateItemAsync<Tab2ViewModel>();
            await ActivateItemAsync<DesignSurfaceViewModel>();
#endif

            LoadWindows();

            var layoutSerializer = new XmlLayoutSerializer(_dockingManager);
            var filePath = Path.Combine(Environment.CurrentDirectory, @"Resources\Layouts\Project.Layout.config");
            LoadLayout(layoutSerializer, filePath);



        }

        private void LoadWindows()
        {
            // build a layout
            _tools.Clear();
            _documents.Clear();

            foreach (var t in Items)
            {
                var type = t;
                switch (type)
                {
                    case Tab1ViewModel:
                    case Tab2ViewModel:
                        _documents.Add((PaneViewModel)t);
                        break;
                    case DesignSurfaceViewModel:
                        _tools.Add((ToolViewModel)t);
                        break;
                }
            }

            NotifyOfPropertyChange(() => Documents);
            NotifyOfPropertyChange(() => Tools);
        }

   
        private void LoadLayout(XmlLayoutSerializer layoutSerializer, string filePath)
        {

            layoutSerializer.LayoutSerializationCallback += async (_, e) =>
            {
                if (e.Model.ContentId is not null)
                {


                    var item = Items.Cast<IAvalonDockWindow>()
                        .FirstOrDefault(item => item.ContentId == e.Model.ContentId);

                    // attempt to reactivate the view model to see if we can get CM to wire up the model to the view model
                    if (item is Screen screen)
                    {
                        await base.ActivateItemAsync(screen);
                    }
                   
                    e.Content = item;

                }
            };

            try
            {
                layoutSerializer.Deserialize(filePath);
            }
            catch (Exception e)
            {
                logger_.LogError(e.Message);
                filePath = Path.Combine(Environment.CurrentDirectory, @"Resources\Layouts\Project.Layout.config");
                if (File.Exists(filePath))
                {
                    // load in the default layout
                    layoutSerializer.Deserialize(filePath);
                }
                else
                {
                    LoadWindows();
                }
            }

            // save to settings
            Settings.Default.LastProjectLayout = filePath;
            _lastLayout = filePath;
        }
    }
}
