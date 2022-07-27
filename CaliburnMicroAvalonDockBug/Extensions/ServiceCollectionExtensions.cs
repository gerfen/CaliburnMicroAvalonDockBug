using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Microsoft.Extensions.DependencyInjection;

namespace CaliburnMicroAvalonDockBug.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        public static FrameSet AddCaliburnMicro(this IServiceCollection serviceCollection)
        {
            var frameSet = new FrameSet();
            // wire up the interfaces required by Caliburn.Micro
            serviceCollection.AddSingleton<IWindowManager, WindowManager>();
            serviceCollection.AddSingleton<IEventAggregator, EventAggregator>();

            // Register the FrameAdapter which wraps a Frame as INavigationService
            serviceCollection.AddSingleton<INavigationService>(sp => frameSet.NavigationService);

            // wire up all of the view models in the project.
            typeof(Bootstrapper).Assembly.GetTypes()
                .Where(type => type.IsClass)
                .Where(type => type.IsAbstract == false) // ignore any view models which are abstract!
                .Where(type => type.Name.EndsWith("ViewModel"))
                .ToList()
                .ForEach(viewModelType => serviceCollection.AddTransient(viewModelType));

            return frameSet;
        }
    }

}
