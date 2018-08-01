using System;
using System.Windows;
using System.Windows.Controls;
using PropertyChanged;
using XGraph.Extensions;
using XGraph.ViewModels;
using System.Windows.Data;
using XGraph.Converters;

namespace XGraph.Controls
{
    /// <summary>
    /// This class represents a connector.
    /// A connector is used to anchor a connection.
    /// </summary>
    /// <!-- Nicolas Baudrey -->
    [ImplementPropertyChanged]
    public abstract class AConnector : Control
    {
        #region Dependencies

        /// <summary>
        /// Identifies the Position dependency property.
        /// </summary>
        public static readonly DependencyProperty PositionProperty = DependencyProperty.Register("Position", typeof(Point), typeof(AConnector), new FrameworkPropertyMetadata(new Point(), OnPositionChanged));

        /// <summary>
        /// Identifies the ConnectionsCount dependency property.
        /// </summary>
        public static readonly DependencyProperty ConnectionsCountProperty = DependencyProperty.Register("ConnectionsCount", typeof(int), typeof(AConnector), new FrameworkPropertyMetadata(0));

        #endregion // Dependencies.

        #region Fields

        /// <summary>
        /// Stores the connector parent port.
        /// </summary>
        private PortView mParentPort;

        #endregion // Fields.

        #region Properties

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        public Point Position 
        {
            get
            {
                return (Point)this.GetValue(PositionProperty);
            }
            set
            {
                this.SetValue(PositionProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the flag indicating if the connector is connected at least once.
        /// </summary>
        public bool IsConnected
        {
            get
            {
                return (this.ConnectionsCount > 0);
            }
        }

        /// <summary>
        /// Gets or sets the number of connections connected to this connector.
        /// </summary>
        public int ConnectionsCount
        {
            get
            {
                return (int)this.GetValue(ConnectionsCountProperty);
            }
            set
            {
                this.SetValue(ConnectionsCountProperty, value);
            }
        }

        /// <summary>
        /// Gets the connector parent port.
        /// </summary>
        public PortView ParentPort
        {
            get
            {
                if (this.mParentPort == null)
                {
                    this.mParentPort = this.FindVisualParent<PortView>();
                }

                return this.mParentPort;
            }
        }

        #endregion // Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AConnector"/> class.
        /// </summary>
        protected AConnector()
        {
            this.LayoutUpdated += this.OnLayoutUpdated;
        }

        #endregion // Constructors.

        #region Events

        /// <summary>
        /// Event raised when the position has changed.
        /// </summary>
        public event Action<Point, Point> PositionChanged;

        #endregion // Events.

        #region Methods

        /// <summary>
        /// Called when the control is initialized.
        /// </summary>
        /// <param name="pEventArgs">The event arguments.</param>
        protected override void OnInitialized(EventArgs pEventArgs)
        {
            base.OnInitialized(pEventArgs);

            // Updating the bindings.
            if (this.ParentPort != null)
            {
                PortViewModel lPortViewModel = this.ParentPort.Content as PortViewModel;

                // Binding the IsConnected property.
                Binding lIsConnectedBinding = new Binding("IsConnected");
                lIsConnectedBinding.Source = lPortViewModel;
                lIsConnectedBinding.Mode = BindingMode.OneWayToSource;
                lIsConnectedBinding.Converter = new BooleanToInt32Converter() { ZeroValue = false };
                this.SetBinding(AConnector.ConnectionsCountProperty, lIsConnectedBinding);
            }
        }

        /// <summary>
        /// This method is called when the layout changes.
        /// </summary>
        /// <param name="pSender">The event sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnLayoutUpdated(object pSender, EventArgs pEventArgs)
        {
            AdornerLayeredCanvas lParentCanvas = this.FindVisualParent<AdornerLayeredCanvas>();
            if (lParentCanvas != null)
            {
                // Get centre position of this Connector relative to the DesignerCanvas.
                this.Position = this.TransformToVisual(lParentCanvas.AdornerLayer).Transform(new Point(this.ActualWidth / 2, this.ActualHeight / 2));
            }
        }

        /// <summary>
        /// Delegate called when the position changed.
        /// </summary>
        /// <param name="pObject">The modified control.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnPositionChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
        {
            AConnector lConnector = pObject as AConnector;
            if (lConnector != null)
            {
                lConnector.NotifyPositionChanged((Point)pEventArgs.OldValue, (Point)pEventArgs.NewValue);
            }
        }

        /// <summary>
        /// Notifies the position has changed.
        /// </summary>
        /// <param name="pOldPos">The old position.</param>
        /// <param name="pNewPos">The new position.</param>
        private void NotifyPositionChanged(Point pOldPos, Point pNewPos)
        {
            if (this.PositionChanged != null)
            {
                this.PositionChanged(pOldPos, pNewPos);
            }
        }

        #endregion // Methods.
    }
}
