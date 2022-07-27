using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CaliburnMicroAvalonDockBug.Views.Panes
{
    internal class PanesTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Tab1ViewTemplate
        {
            get;
            set;
        }

        public DataTemplate Tab2ViewTemplate
        {
            get;
            set;
        }
    }
}
