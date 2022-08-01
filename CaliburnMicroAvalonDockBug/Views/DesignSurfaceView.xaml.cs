using System;
using System.Collections.Generic;
using System.Configuration;
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
using Caliburn.Micro;
using Microsoft.Extensions.Logging;

namespace CaliburnMicroAvalonDockBug.Views
{
    /// <summary>
    /// Interaction logic for DesignSurfaceView.xaml
    /// </summary>
    public partial class DesignSurfaceView : UserControl
    {
        private ILogger<DesignSurfaceView> logger_;
        public DesignSurfaceView()
        {
            logger_ = IoC.Get<ILogger<DesignSurfaceView>>();
            InitializeComponent();
        }

        public void AddControl(FrameworkElement control)
        {
            logger_.LogInformation($"DesignSurfaceView.AddControl called - adding {control.GetType().Name} to DesignSurfaceCanvas.");

            DesignSurfaceCanvas.Children.Add(control);
            //DesignSurfaceCanvas.Dispatcher.Invoke(() => { DesignSurfaceCanvas.Children.Add(control); });

        }
    }
}
