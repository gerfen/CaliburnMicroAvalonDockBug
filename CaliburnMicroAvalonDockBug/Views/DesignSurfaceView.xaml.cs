using System;
using Caliburn.Micro;
using Microsoft.Extensions.Logging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

        protected override void OnInitialized(EventArgs e)
        {
            DrawControl();
            base.OnInitialized(e);
        }

        public void AddControl(FrameworkElement control)
        {
            logger_.LogInformation($"DesignSurfaceView.AddControl called - adding {control.GetType().Name} to DesignSurfaceCanvas.");

            DesignSurfaceCanvas.Children.Add(control);
            //DesignSurfaceCanvas.Dispatcher.Invoke(() => { DesignSurfaceCanvas.Children.Add(control); });

        }

        private void DrawControl()
        {
            logger_.LogInformation("Drawing `From View` shape from the view's OnInitialized method.");
            var border = new Border
            {
                Height = 75,
                Width = 150,
                Background = Brushes.DarkOrange,
                CornerRadius = new CornerRadius(5)
            };

            var textBlock = new TextBlock
            {
                Text = "From View",
                FontSize = 20,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center

            };

            border.Child = textBlock;


            Canvas.SetTop(border, 95.0);
            Canvas.SetLeft(border, 10.0);

            AddControl(border);
        }
    }
}
