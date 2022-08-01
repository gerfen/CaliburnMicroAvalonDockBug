
using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;
using CaliburnMicroAvalonDockBug.ViewModels;
using Microsoft.Extensions.Logging;

namespace CaliburnMicroAvalonDockBug.Views.Panes
{
    internal class PanesTemplateSelector : DataTemplateSelector
    {
        
        public DataTemplate Tab1ViewDataTemplate { get; set; }

        public DataTemplate Tab2ViewDataTemplate { get; set; }

        public DataTemplate DesignSurfaceDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item,
            DependencyObject container)
        {

            //var presenter = (item as ContentPresenter);
            //if (presenter != null)
            //{

            //    if (presenter.Content is Tab1ViewModel)
            //        return Tab1ViewDataTemplate;

            //    if (presenter.Content is Tab2ViewModel)
            //        return Tab2ViewDataTemplate;

            //    if (presenter.Content is DesignSurfaceViewModel)
            //        return DesignSurfaceDataTemplate;

            //}

            var logger = IoC.Get<ILogger<PanesTemplateSelector>>();

            if (item is Tab1ViewModel)
            {
                //logger.LogInformation("PanesTemplateSelector - returning Tab1DataTemplate");
                return Tab1ViewDataTemplate;
            }

            if (item is Tab2ViewModel)
            {
                //logger.LogInformation("PanesTemplateSelector - returning Tab2DataTemplate");
                return Tab2ViewDataTemplate;
            }

            if (item is DesignSurfaceViewModel)
            {
                //logger.LogInformation("PanesTemplateSelector - returning DesignSurfaceViewModel");
                return DesignSurfaceDataTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}
