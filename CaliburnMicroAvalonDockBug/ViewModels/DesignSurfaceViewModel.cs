using CaliburnMicroAvalonDockBug.ViewModels.Panes;
using CaliburnMicroAvalonDockBug.Views;
using Microsoft.Extensions.Logging;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Linq;
//using AvalonDock.Controls;
using CaliburnMicroAvalonDockBug.Extensions;

namespace CaliburnMicroAvalonDockBug.ViewModels
{
    public class DesignSurfaceViewModel : ToolViewModel
    {
        private readonly ILogger<DesignSurfaceViewModel>? logger_;

        public DesignSurfaceViewModel()
        {
            Title = "Design Surface";
            ContentId = "DESIGNSURFACE";
        }

        public DesignSurfaceViewModel(ILogger<DesignSurfaceViewModel> logger) : base(logger)
        {
            logger_ = logger;
            Title = "Design Surface";
            ContentId = "DESIGNSURFACE";
        }

        protected DesignSurfaceView View { get; set; }
        public Canvas DesignSurfaceCanvas { get; set; }

        protected override void OnViewAttached(object view, object context)
        {
            logger_.LogInformation("DesignSurfaceViewModel - OnViewAttached Called.");
            SetCanvasReference(view);
            base.OnViewAttached(view, context);
        }

        protected override void OnViewLoaded(object view)
        {
            logger_.LogInformation("DesignSurfaceViewModel - OnViewLoaded Called.");
            base.OnViewLoaded(view);
        }



        protected override void OnViewReady(object view)
        {
            logger_.LogInformation("DesignSurfaceViewModel - OnViewReady Called.");
            SetCanvasReference(view);
            base.OnViewReady(view);
        }

        private void SetCanvasReference(object view)
        {
            if (View == null)
            {
                if (view is DesignSurfaceView projectDesignSurfaceView)
                {
                    View = projectDesignSurfaceView;
                    var canvases = View.FindVisualChildren<Canvas>();
                    DesignSurfaceCanvas = (Canvas)projectDesignSurfaceView.FindName("DesignSurfaceCanvas");
                }

                DrawShape();
            }
        }

        public void DrawShape()
        {
              logger_.LogInformation("DesignSurfaceViewModel.DrawShape called.");
              var border = new Border
              {
                  Height = 75,
                  Width = 150,
                  Background = Brushes.Blue,
                  CornerRadius = new CornerRadius(5)
              };

              var textBlock = new TextBlock
              {
                  Text = "From ViewModel",
                  FontSize = 20,
                  VerticalAlignment = VerticalAlignment.Center,
                  HorizontalAlignment = HorizontalAlignment.Center

              };

              border.Child = textBlock;


            Canvas.SetTop(border, 10.0);
            Canvas.SetLeft(border, 210.0);
            //DesignSurfaceCanvas.Children.Add(rectangle);

            View.AddControl(border);
        }
    }
}
