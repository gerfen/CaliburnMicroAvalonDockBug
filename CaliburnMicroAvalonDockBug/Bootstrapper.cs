using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Caliburn.Micro;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using CaliburnMicroAvalonDockBug.Extensions;
using CaliburnMicroAvalonDockBug.ViewModels;
using Serilog;
using Serilog.Events;

namespace CaliburnMicroAvalonDockBug
{
    internal class Bootstrapper : BootstrapperBase
    {
        protected FrameSet FrameSet { get; private set; }
        public static IHost Host { get; private set; }
        protected ILogger<Bootstrapper> Logger { get; private set; }

        public Bootstrapper()
        {
            Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    ConfigureServices(services);
                })
                .Build();

            SetupLogging();
            Initialize();
        }

        protected void ConfigureServices(IServiceCollection serviceCollection)
        {
            FrameSet = serviceCollection.AddCaliburnMicro();
          
            serviceCollection.AddLogging();

        }

        private void SetupLogging()
        {

            var fullPath = Path.Combine(Environment.CurrentDirectory, "Logs\\CaliburnMicroAvalonDockBug.log");
            var level = LogEventLevel.Information;
#if DEBUG
            level = LogEventLevel.Verbose;
#endif
            const string outputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] [{SourceContext}] {Message}{NewLine}{Exception}";

            var log = new LoggerConfiguration()
                .MinimumLevel.Is(level)
                .WriteTo.File(fullPath, outputTemplate: outputTemplate, rollingInterval: RollingInterval.Day)
                .WriteTo.Debug(outputTemplate: outputTemplate)
                .CreateLogger();

            var loggerFactory = Host.Services.GetService<ILoggerFactory>();
            loggerFactory.AddSerilog(log);

            Logger = Host.Services.GetService<ILogger<Bootstrapper>>();
        }

        protected override async void OnStartup(object sender, StartupEventArgs e)
        {
            Logger.LogInformation("Application is starting.");

            // Allow the ShellView to be created.
            await DisplayRootViewForAsync<DockingHostViewModel>();

            // Now add the Frame to be added to the Grid in ShellView
           // AddFrameToMainWindow(FrameSet.Frame);

            // Navigate to the LandingView.
           // FrameSet.NavigationService.NavigateToViewModel(typeof(DockingHostViewModel));
        }

        /// <summary>
        /// Adds the Frame to the Grid control on the ShellView
        /// </summary>
        /// <param name="frame"></param>
        /// <exception cref="NullReferenceException"></exception>
        private void AddFrameToMainWindow(Frame frame)
        {
            Logger.LogInformation("Adding Frame to ShellView grid control.");

            var mainWindow = Application.MainWindow;
            if (mainWindow == null)
            {
                throw new NullReferenceException("'Application.MainWindow' is null.");
            }


            if (mainWindow.Content is not Grid grid)
            {
                throw new NullReferenceException("The grid on 'Application.MainWindow' is null.");
            }

            Grid.SetRow(frame, 1);
            Grid.SetColumn(frame, 0);
            grid.Children.Add(frame);
        }

        protected override object GetInstance(Type service, string key)
        {
            return Host.Services.GetService(service);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return Host.Services.GetServices(service);
        }

    
        protected override void OnExit(object sender, EventArgs e)
        {
            Logger.LogInformation("Application is exiting.");
            base.OnExit(sender, e);
        }
      

        /// <summary>
        /// Handle the system wide exceptions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;

            Logger.LogError(e.Exception, "An unhandled error as occurred");
            MessageBox.Show(e.Exception.Message, "An error as occurred", MessageBoxButton.OK);
        }

    }
}
