using Caliburn.Micro;
using Microsoft.Extensions.Logging;

namespace CaliburnMicroAvalonDockBug.ViewModels
{
    public class ShellViewModel : Screen
    {
        private readonly ILogger<ShellViewModel> logger_;

        public ShellViewModel()
        {
            
        }

        public ShellViewModel(ILogger<ShellViewModel> logger)
        {
            logger_ = logger;
        }
    }
}
