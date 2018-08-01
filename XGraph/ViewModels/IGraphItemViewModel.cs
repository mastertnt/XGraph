using System.Windows;

namespace XGraph.ViewModels
{
    /// <summary>
    /// This object represents a graph item view model.
    /// </summary>
    public interface IGraphItemViewModel
    {
        #region Properties

        /// <summary>
        /// Gets the style to apply to the container.
        /// </summary>
        Style ContainerStyle
        {
            get;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is selected.
        /// </summary>
        bool IsSelected
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the Z index ordering the associated control.
        /// </summary>
        int ZIndex
        {
            get;
        }

        #endregion // Properties.
    }
}
