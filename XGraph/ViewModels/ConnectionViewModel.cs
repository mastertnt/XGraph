using PropertyChanged;
using System.Windows;
using System.Windows.Media;

namespace XGraph.ViewModels
{
    /// <summary>
    /// Class defining the connection view model.
    /// </summary>
    [ImplementPropertyChanged]
    public class ConnectionViewModel : IGraphItemViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionViewModel"/> class.
        /// </summary>
        public ConnectionViewModel()
        {
            this.Brush = new SolidColorBrush(Color.FromRgb(157, 157, 157));
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether this instance is selected.
        /// </summary>
        public virtual bool IsSelected
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        public virtual bool IsActive
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the input.
        /// </summary>
        public PortViewModel Input
        {
            get; 
            set;
        }

        /// <summary>
        /// Gets or sets the output.
        /// </summary>
        public PortViewModel Output
        {
            get; 
            set;
        }

        /// <summary>
        /// Gets the style to apply to the container.
        /// </summary>
        public virtual Style ContainerStyle
        {
            get
            {
                return Themes.ThemeManager.Instance.FindResource("GraphItemConnectionDefaultStyleKey") as Style;
            }
        }

        /// <summary>
        /// Gets the Z index ordering the associated control.
        /// </summary>
        public virtual int ZIndex
        {
            get
            {
                return 1;
            }
        }

        /// <summary>
        /// Gets or sets the brush used to color the connection.
        /// </summary>
        public virtual Brush Brush
        {
            get;
            set;
        }

        #endregion // Properties.
    }
}
