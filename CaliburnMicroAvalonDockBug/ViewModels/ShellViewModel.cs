using Caliburn.Micro;
using Microsoft.Extensions.Logging;

namespace CaliburnMicroAvalonDockBug.ViewModels
{
    public class ShellViewModel : Screen
    {
        private readonly Logger<ShellViewModel> logger_;

        public ShellViewModel()
        {
            
        }

        public ShellViewModel(Logger<ShellViewModel> logger)
        {
            logger_ = logger;
        }
    }
}
