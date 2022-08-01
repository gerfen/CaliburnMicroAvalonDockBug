using System.Windows;
using System.Windows.Controls;
using AvalonDock.Layout;
using CaliburnMicroAvalonDockBug.ViewModels;

namespace CaliburnMicroAvalonDockBug.Views.Panes
{
    internal class DockingHostPanesTemplateSelector : DataTemplateSelector
    {

        public DataTemplate Tab1ViewDataTemplate
        {
            get;
            set;
        }

        public DataTemplate Tab2ViewDataTemplate
        {
            get;
            set;
        }

        // ====================
        //        TOOLS
        // ====================
        public DataTemplate DesignSurfaceDataTemplate
        {
            get;
            set;
        }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var itemAsLayoutContent = item as LayoutContent;

            if (item is Tab1ViewModel)
                return Tab1ViewDataTemplate;

            if (item is Tab2ViewModel)
                return Tab2ViewDataTemplate;

            if (item is DesignSurfaceViewModel)
            {
                return DesignSurfaceDataTemplate;
            }

            return base.SelectTemplate(item, container);
        }
    }
}
