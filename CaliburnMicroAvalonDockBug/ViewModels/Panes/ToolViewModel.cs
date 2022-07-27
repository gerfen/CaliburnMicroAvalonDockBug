using Microsoft.Extensions.Logging;

namespace CaliburnMicroAvalonDockBug.ViewModels.Panes;

public class ToolViewModel : PaneViewModel, IAvalonDockWindow
{

    private bool _isVisible = true;
   
    public string Name { get; private set; }
        
    public bool IsVisible
    {
        get => _isVisible;
        set
        {
            if (_isVisible != value)
            {
                _isVisible = value;
                NotifyOfPropertyChange(() => IsVisible);
            }
        }
    }
    
    public ToolViewModel()
    {

    }

    public ToolViewModel(ILogger logger)
    {
             
    }
     
}