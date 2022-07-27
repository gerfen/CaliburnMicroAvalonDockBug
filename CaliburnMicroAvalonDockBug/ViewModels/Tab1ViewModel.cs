using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaliburnMicroAvalonDockBug.ViewModels.Panes;
using Microsoft.Extensions.Logging;

namespace CaliburnMicroAvalonDockBug.ViewModels
{
    public class Tab1ViewModel : PaneViewModel
    {

        private readonly ILogger<Tab1ViewModel>? _logger;

        public Tab1ViewModel()
        {
            Title = "TAB1";
            ContentId = "TAB1";
        }

        public Tab1ViewModel(ILogger<Tab1ViewModel> logger) : base(logger)
        {
            _logger = logger;
            Title = "TAB1";
            ContentId = "TAB1";
        }
    }
}
