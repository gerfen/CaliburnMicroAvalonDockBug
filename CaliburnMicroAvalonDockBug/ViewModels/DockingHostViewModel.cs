using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
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
        private ILogger<DockingHostViewModel> logger_;

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

        public DockingHostViewModel()
        {
            
        }

        public DockingHostViewModel(ILogger<DockingHostViewModel> logger)
        {
            logger_ = logger;
        }

        /// <summary>
        /// Binds the viewmodel to it's view prior to activating so that the OnViewAttached method of the
        /// child viewmodel are called.
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <returns></returns>
        private async Task ActivateItemAsync<TViewModel>(CancellationToken cancellationToken = default)
            where TViewModel : Screen
        {
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
                item = (T)Activator.CreateInstance(typeof(T));
                item.Parent = this;
                item.ConductWith(this);
                item.DisplayName = displayName;
                //item.IsDirty = ++_createCount % 2 > 0;
            }
            return ActivateItemAsync(item, CancellationToken.None);
        }

        protected override Task OnActivateAsync(CancellationToken cancellationToken)
        {
            // subscribe to the event aggregator so that we can listen to messages
            //EventAggregator.SubscribeOnUIThread(this);

            return base.OnActivateAsync(cancellationToken);
        }

        protected override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
        {
            if (_lastLayout == "")
            {
                SaveLayout();
            }

            // unsubscribe to the event aggregator
            //EventAggregator.Unsubscribe(this);
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
            // documents
            await ActivateItemAsync<Tab1ViewModel>();
            await ActivateItemAsync<Tab2ViewModel>();

            // tools
            await ActivateItemAsync<DesignSurfaceViewModel>();

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
