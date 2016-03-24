using System.Windows;
using System.Windows.Controls;

namespace XGraph.Controls
{
    /// <summary>
    /// Class defining a ghost line used to define a connection.
    /// </summary>
    public class ConnectingLine : Control
    {
        #region Dependencies

        /// <summary>
        /// Identifies the From dependency property.
        /// </summary>
        public static readonly DependencyProperty FromProperty = DependencyProperty.Register("From", typeof(Point), typeof(ConnectingLine), new FrameworkPropertyMetadata(new Point()));

        /// <summary>
        /// Identifies the To dependency property.
        /// </summary>
        public static readonly DependencyProperty ToProperty = DependencyProperty.Register("To", typeof(Point), typeof(ConnectingLine), new FrameworkPropertyMetadata(new Point()));

        #endregion // Dependencies.

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="ConnectingLine"/> class.
        /// </summary>
        static ConnectingLine()
        {
            // set the key to reference the style for this control
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ConnectingLine), new FrameworkPropertyMetadata(typeof(ConnectingLine)));
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets or sets the initiale position of the connecting line.
        /// </summary>
        public Point From
        {
            get
            {
                return (Point)this.GetValue(FromProperty);
            }
            set
            {
                this.SetValue(FromProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the final position of the connecting line.
        /// </summary>
        public Point To
        {
            get
            {
                return (Point)this.GetValue(ToProperty);
            }
            set
            {
                this.SetValue(ToProperty, value);
            }
        }

        #endregion // Properties.
    }
}
