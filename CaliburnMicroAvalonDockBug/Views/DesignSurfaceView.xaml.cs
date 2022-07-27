using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CaliburnMicroAvalonDockBug.Views
{
    /// <summary>
    /// Interaction logic for DesignSurfaceView.xaml
    /// </summary>
    public partial class DesignSurfaceView : UserControl
    {
        public DesignSurfaceView()
        {
            InitializeComponent();
        }

        public void AddControl(FrameworkElement control)
        {
            DesignSurfaceCanvas.Dispatcher.Invoke(() => { DesignSurfaceCanvas.Children.Add(control); });

        }
    }
}
