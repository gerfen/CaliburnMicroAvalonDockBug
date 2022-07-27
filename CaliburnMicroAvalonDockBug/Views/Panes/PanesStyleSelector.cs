
using System.Windows;
using System.Windows.Controls;
using CaliburnMicroAvalonDockBug.ViewModels;

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
            if (item is Tab1ViewModel)
            {
                return DocumentStyle;
            }

            if (item is Tab2ViewModel)
            {
                return DocumentStyle;
            }

            return base.SelectStyle(item, container);
        }
    }
}
