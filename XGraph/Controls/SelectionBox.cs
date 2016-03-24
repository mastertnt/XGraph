using System.Windows;
using System.Windows.Controls;

namespace XGraph.Controls
{
    /// <summary>
    /// Class defining a selection box.
    /// </summary>
    public class SelectionBox : Control
    {
        #region Constructors

        /// <summary>
        /// Initializes the <see cref="SelectionBox"/> class.
        /// </summary>
        static SelectionBox()
        {
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(SelectionBox), new FrameworkPropertyMetadata(typeof(SelectionBox)));
        }

        #endregion // Constructors.
    }
}
