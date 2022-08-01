using CaliburnMicroAvalonDockBug.ViewModels.Panes;
using Microsoft.Extensions.Logging;

namespace CaliburnMicroAvalonDockBug.ViewModels
{
    public class Tab2ViewModel : PaneViewModel
    {
        private readonly ILogger<Tab2ViewModel>? _logger;

        public Tab2ViewModel()
        {
            Title = "TAB2";
            ContentId = "TAB2";
        }

        public Tab2ViewModel(ILogger<Tab2ViewModel> logger) : base(logger)
        {
            _logger = logger;
            Title = "TAB2";
            ContentId = "TAB2";
        }
    }
}
