using System.Windows;
using PropertyChanged;
using XGraph.Extensions;
using XGraph.ViewModels;
using System;
using System.Windows.Data;

namespace XGraph.Controls
{
    /// <summary>
    /// This class represents a connection.
    /// </summary>
    /// <!-- Nicolas Baudrey -->
    [ImplementPropertyChanged]
    public class Connection : AGraphItem
    {
        #region Dependencies

        /// <summary>
        /// Identifies the OutputConnector dependency property.
        /// </summary>
        public static readonly DependencyProperty OutputConnectorProperty = DependencyProperty.Register("OutputConnector", typeof(OutputConnector), typeof(Connection), new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Identifies the InputConnector dependency property.
        /// </summary>
        public static readonly DependencyProperty InputConnectorProperty = DependencyProperty.Register("InputConnector", typeof(InputConnector), typeof(Connection), new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Identifies the RelativeFrom dependency property.
        /// </summary>
        public static readonly DependencyProperty RelativeFromProperty = DependencyProperty.Register("RelativeFrom", typeof(Point), typeof(Connection), new FrameworkPropertyMetadata(new Point()));

        /// <summary>
        /// Identifies the RelativeTo dependency property.
        /// </summary>
        public static readonly DependencyProperty RelativeToProperty = DependencyProperty.Register("RelativeTo", typeof(Point), typeof(Connection), new FrameworkPropertyMetadata(new Point()));

        /// <summary>
        /// Identifies the LineWidth dependency property.
        /// </summary>
        public static readonly DependencyProperty LineWidthProperty = DependencyProperty.Register("LineWidth", typeof(double), typeof(Connection), new FrameworkPropertyMetadata(1.0, OnLineWidthChanged));

        #endregion // Dependencies.

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="Connection"/> class.
        /// </summary>
        static Connection()
        {
            Connection.DefaultStyleKeyProperty.OverrideMetadata(typeof(Connection), new FrameworkPropertyMetadata(typeof(Connection)));
            Connection.PaddingProperty.OverrideMetadata(typeof(Connection), new FrameworkPropertyMetadata(new Thickness(0.0), OnPaddingChanged));
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets or sets the output connector of the connection.
        /// </summary>
        public OutputConnector OutputConnector
        {
            get
            {
                return (OutputConnector)this.GetValue(OutputConnectorProperty);
            }
            set
            {
                this.SetValue(OutputConnectorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the input connector of the connection.
        /// </summary>
        public InputConnector InputConnector
        {
            get
            {
                return (InputConnector)this.GetValue(InputConnectorProperty);
            }
            set
            {
                this.SetValue(InputConnectorProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the initiale position of the connection relative to this control bounding box.
        /// </summary>
        public Point RelativeFrom
        {
            get
            {
                return (Point)this.GetValue(RelativeFromProperty);
            }
            set
            {
                this.SetValue(RelativeFromProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the final position of the connection relative to this control bounding box.
        /// </summary>
        public Point RelativeTo
        {
            get
            {
                return (Point)this.GetValue(RelativeToProperty);
            }
            set
            {
                this.SetValue(RelativeToProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the connection line width.
        /// </summary>
        public double LineWidth
        {
            get
            {
                return (double)this.GetValue(LineWidthProperty);
            }
            set
            {
                this.SetValue(LineWidthProperty, value);
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Method called when the control content changed.
        /// </summary>
        /// <param name="pOldContent">The previous content.</param>
        /// <param name="pNewContent">The new content.</param>
        protected override void OnContentChanged(object pOldContent, object pNewContent)
        {
            // Calling ancestor methods.
            base.OnContentChanged(pOldContent, pNewContent);

            BindingOperations.ClearAllBindings(this);

            // The content is the view model.
            ConnectionViewModel lViewModel = pNewContent as ConnectionViewModel;
            if (lViewModel == null)
            {
                // Unreferencing the connectors to avoid memory leaks.
                this.OutputConnector.ConnectionsCount--;
                this.InputConnector.ConnectionsCount--;
                this.OutputConnector.PositionChanged -= this.OnConnectorPositionChanged;
                this.InputConnector.PositionChanged -= this.OnConnectorPositionChanged;
                this.OutputConnector = null;
                this.InputConnector = null;
            }
            else
            {
                // Binding the Background property.
                Binding lBackgroundBinding = new Binding("Brush");
                lBackgroundBinding.Source = lViewModel;
                lBackgroundBinding.Mode = BindingMode.OneWay;
                this.SetBinding(Connection.BackgroundProperty, lBackgroundBinding);

                // Filling the output and input connectors.
                SimpleGraphView lParentCanvas = this.FindVisualParent<SimpleGraphView>();
                if (lViewModel != null && lParentCanvas != null)
                {
                    NodeView lOutputNode = lParentCanvas.GetContainerForViewModel<NodeViewModel, NodeView>(lViewModel.Output.ParentNode);
                    if (lOutputNode != null)
                    {
                        PortView lOutputPort = lOutputNode.GetContainerForPortViewModel(lViewModel.Output);
                        if (lOutputPort != null)
                        {
                            this.OutputConnector = lOutputPort.Connector as OutputConnector;
                            if (this.OutputConnector != null)
                            {
                                this.OutputConnector.PositionChanged += this.OnConnectorPositionChanged;
                                this.OutputConnector.ConnectionsCount++;
                            }
                        }
                    }

                    NodeView lInputNode = lParentCanvas.GetContainerForViewModel<NodeViewModel, NodeView>(lViewModel.Input.ParentNode);
                    if (lInputNode != null)
                    {
                        PortView lInputPort = lInputNode.GetContainerForPortViewModel(lViewModel.Input);
                        if (lInputPort != null)
                        {
                            this.InputConnector = lInputPort.Connector as InputConnector;
                            if (this.InputConnector != null)
                            {
                                this.InputConnector.PositionChanged += this.OnConnectorPositionChanged;
                                this.InputConnector.ConnectionsCount++;
                            }
                        }
                    }
                }
            }

            this.UpdateRendering();
        }

        /// <summary>
        /// Computes the bounding of the connection using the connectors positions.
        /// </summary>
        /// <param name="pTakeInAcountMargin">Flag indicating if the margin have to be taken in account during the bounding computation.</param>
        /// <returns>The computed bounding box.</returns>
        private Rect ComputeBoundingBox(bool pTakeInAcountMargin)
        {
            if (this.OutputConnector != null && this.InputConnector != null)
            {
                Point lStartPos = this.OutputConnector.Position;
                Point lEndPos = this.InputConnector.Position;

                double lX = Math.Min(lStartPos.X, lEndPos.X);
                if (pTakeInAcountMargin)
                {
                    lX -= (this.LineWidth / 2.0 + this.Padding.Left);
                }
                double lY = Math.Min(lStartPos.Y, lEndPos.Y);
                if (pTakeInAcountMargin)
                {
                    lY -= (this.LineWidth / 2.0 + this.Padding.Top);
                }
                double lWidth = Math.Abs(lStartPos.X - lEndPos.X);
                if (pTakeInAcountMargin)
                {
                    lWidth += this.LineWidth + this.Padding.Left + this.Padding.Right;
                }
                double lHeight = Math.Abs(lStartPos.Y - lEndPos.Y);
                if (pTakeInAcountMargin)
                {
                    lHeight += this.LineWidth + this.Padding.Top + this.Padding.Bottom;
                }

                return new Rect(lX, lY, lWidth, lHeight);
            }

            return new Rect();
        }

        /// <summary>
        /// Updates the position and size of the control.
        /// </summary>
        /// <param name="pNewBounding">The bounding box to tke in account.</param>
        private void UpdateBoundingBox(Rect pNewBounding)
        {
            this.SetCanvasPosition(pNewBounding.TopLeft);

            this.Width = pNewBounding.Width;
            this.Height = pNewBounding.Height;
        }

        /// <summary>
        /// Updates the relative position of the connection limits using the new bounding.
        /// </summary>
        /// <param name="pNewBounding">The bounding box to take in account.</param>
        private void UpdateRelativePos(Rect pNewBounding)
        {
            if (this.OutputConnector != null && this.InputConnector != null)
            {
                if (this.OutputConnector.Position.X <= this.InputConnector.Position.X)
                {
                    if (this.OutputConnector.Position == pNewBounding.TopLeft)
                    {
                        this.RelativeFrom = new Point(this.LineWidth / 2.0 + this.Padding.Left, this.LineWidth / 2.0 + this.Padding.Top);
                    }
                    else
                    {
                        this.RelativeFrom = new Point(this.LineWidth / 2.0 + this.Padding.Left, pNewBounding.Height + this.LineWidth / 2.0 + this.Padding.Top);
                    }

                    if (this.InputConnector.Position == pNewBounding.TopRight)
                    {
                        this.RelativeTo = new Point(pNewBounding.Width + this.LineWidth / 2.0 + this.Padding.Left, this.LineWidth / 2.0 + this.Padding.Top);
                    }
                    else
                    {
                        this.RelativeTo = new Point(pNewBounding.Width + this.LineWidth / 2.0 + this.Padding.Left, pNewBounding.Height + this.LineWidth / 2.0 + this.Padding.Top);
                    }
                }
                else
                {
                    if (this.InputConnector.Position == pNewBounding.TopLeft)
                    {
                        this.RelativeTo = new Point(this.LineWidth / 2.0 + this.Padding.Left, this.LineWidth / 2.0 + this.Padding.Top);
                    }
                    else
                    {
                        this.RelativeTo = new Point(this.LineWidth / 2.0 + this.Padding.Left, pNewBounding.Height + this.LineWidth / 2.0 + this.Padding.Top);
                    }

                    if (this.OutputConnector.Position == pNewBounding.TopRight)
                    {
                        this.RelativeFrom = new Point(pNewBounding.Width + this.LineWidth / 2.0 + this.Padding.Left, this.LineWidth / 2.0 + this.Padding.Top);
                    }
                    else
                    {
                        this.RelativeFrom = new Point(pNewBounding.Width + this.LineWidth / 2.0 + this.Padding.Left, pNewBounding.Height + this.LineWidth / 2.0 + this.Padding.Top);
                    }
                }
            }
        }

        /// <summary>
        /// Delegate called when one of the connector position is modified.
        /// </summary>
        /// <param name="pOldPos">The old connector position.</param>
        /// <param name="pNewPos">The new connector position.</param>
        private void OnConnectorPositionChanged(Point pOldPos, Point pNewPos)
        {
            this.UpdateRendering();
        }

        /// <summary>
        /// Delegate called when the line width changed.
        /// </summary>
        /// <param name="pObject">The modified control.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnLineWidthChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
        {
            Connection lConnection = pObject as Connection;
            if (lConnection != null)
            {
                lConnection.UpdateRendering();
            }
        }

        /// <summary>
        /// Delegate called when the padding changed.
        /// </summary>
        /// <param name="pObject">The modified control.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnPaddingChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
        {
            Connection lConnection = pObject as Connection;
            if (lConnection != null)
            {
                lConnection.UpdateRendering();
            }
        }

        /// <summary>
        /// Updates the connection rendering properties.
        /// </summary>
        private void UpdateRendering()
        {
            // Computes the new bounding box taking in account the line width.
            Rect lFullBoundingBox = this.ComputeBoundingBox(true);

            // Updating it.
            this.UpdateBoundingBox(lFullBoundingBox);

            // Updating the relative position of the connection rendering without taking in account the line width.
            Rect lLazyBoundingBox = this.ComputeBoundingBox(false);
            this.UpdateRelativePos(lLazyBoundingBox);
        }

        #endregion // Methods.
    }
}
