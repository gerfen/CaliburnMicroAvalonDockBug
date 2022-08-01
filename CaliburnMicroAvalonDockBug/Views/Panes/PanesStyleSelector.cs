
using System.Windows;
using System.Windows.Controls;
using Caliburn.Micro;
using CaliburnMicroAvalonDockBug.ViewModels;
using Microsoft.Extensions.Logging;

namespace CaliburnMicroAvalonDockBug.Views.Panes
{
    internal class PanesStyleSelector : StyleSelector
    {
        public Style DocumentStyle
        {
            get;
            set;
        }

        public Style ToolStyle
        {
            get;
            set;
        }

        public override Style SelectStyle(object item, System.Windows.DependencyObject container)
        {
            var logger = IoC.Get<ILogger<PanesStyleSelector>>();
            if (item is Tab1ViewModel)
            {
                logger.LogInformation("PanesStyleSelector.SelectStyle called for Tab1ViewModel, - returning DocumentStyle");
                return DocumentStyle;
            }

            if (item is Tab2ViewModel)
            {
                logger.LogInformation("PanesStyleSelector.SelectStyle called for Tab2ViewModel, - returning DocumentStyle");
                return DocumentStyle;
            }

            if (item is DesignSurfaceViewModel)
            {
                logger.LogInformation("PanesStyleSelector.SelectStyle called for DesignSurfaceViewModel, - returning DocumentStyle");
                return DocumentStyle;
            }
            return base.SelectStyle(item, container);
        }
    }
}
