using System.Windows.Media;
using Caliburn.Micro;
using Microsoft.Extensions.Logging;

namespace CaliburnMicroAvalonDockBug.ViewModels.Panes
{
    public interface IAvalonDockWindow
    {
        string ContentId { get; }
    }

    public enum DockSide
    {
        Left,
        Bottom,
    }

    /// <summary>
    /// 
    /// </summary>
    public class PaneViewModel : Screen, IAvalonDockWindow
    {
        private string _title = null;
        private string _contentId = null;
        private bool _isSelected = false;
        private bool _isActive = false;


        public DockSide DockSide = DockSide.Bottom;

        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    NotifyOfPropertyChange(() => Title);
                }
            }
        }

        public ImageSource IconSource { get; protected set; }

        public string ContentId
        {
            get => _contentId;
            set
            {
                if (_contentId != value)
                {
                    _contentId = value;
                    NotifyOfPropertyChange(() => ContentId);
                }
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    NotifyOfPropertyChange(() => IsSelected);
                }
            }
        }

        public new bool IsActive
        {
            get => _isActive;
            set
            {
                if (_isActive != value)
                {
                    _isActive = value;
                    NotifyOfPropertyChange(() => IsActive);
                }
            }
        }


        public PaneViewModel()
        {
        }

        public PaneViewModel(ILogger logger)
        {

        }
    }
}
