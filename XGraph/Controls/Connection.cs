using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PropertyChanged;
using XGraph.Extensions;
using XGraph.ViewModels;
using System;

namespace XGraph.Controls
{
    /// <summary>
    /// This class represents a connection.
    /// </summary>
    /// <!-- Nicolas Baudrey -->
    [ImplementPropertyChanged]
    public class Connection : AGraphItemContainer
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

        #endregion // Dependencies.

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="Connection"/> class.
        /// </summary>
        static Connection()
        {
            Connection.DefaultStyleKeyProperty.OverrideMetadata(typeof(Connection), new FrameworkPropertyMetadata(typeof(Connection)));
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

            // The content is the view model.
            ConnectionViewModel lViewModel = pNewContent as ConnectionViewModel;
            if (lViewModel == null)
            {
                // Unreferencing the connectors to avoid memory leaks.
                this.OutputConnector = null;
                this.InputConnector = null;

                this.OutputConnector.PositionChanged -= this.OnConnectorPositionChanged;
                this.InputConnector.PositionChanged -= this.OnConnectorPositionChanged;
            }
            else
            {
                // Filling the output and input connectors.
                GraphView lParentCanvas = this.FindVisualParent<GraphView>();
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
                            }
                            else
                            {
                                this.OutputConnector.PositionChanged -= this.OnConnectorPositionChanged;
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
                            }
                            else
                            {
                                this.InputConnector.PositionChanged -= this.OnConnectorPositionChanged;
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
        /// <returns>The computed bounding box.</returns>
        private Rect ComputeBoundingBox()
        {
            if (this.OutputConnector != null && this.InputConnector != null)
            {
                Point lStartPos = this.OutputConnector.Position;
                Point lEndPos = this.InputConnector.Position;

                double lX = Math.Min(lStartPos.X, lEndPos.X);
                double lY = Math.Min(lStartPos.Y, lEndPos.Y);
                double lWidth = Math.Abs(lStartPos.X - lEndPos.X);
                double lHeight = Math.Abs(lStartPos.Y - lEndPos.Y);

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
        /// <param name="pNewBounding">The bounding box to tke in account.</param>
        private void UpdateRelativePos(Rect pNewBounding)
        {
            if (this.OutputConnector != null && this.InputConnector != null)
            {
                if (this.OutputConnector.Position.X <= this.InputConnector.Position.X)
                {
                    if (this.OutputConnector.Position == pNewBounding.TopLeft)
                    {
                        this.RelativeFrom = new Point(0.0, 0.0);
                    }
                    else
                    {
                        this.RelativeFrom = new Point(0.0, pNewBounding.Height);
                    }

                    if (this.InputConnector.Position == pNewBounding.TopRight)
                    {
                        this.RelativeTo = new Point(pNewBounding.Width, 0.0);
                    }
                    else
                    {
                        this.RelativeTo = new Point(pNewBounding.Width, pNewBounding.Height);
                    }
                }
                else
                {
                    if (this.InputConnector.Position == pNewBounding.TopLeft)
                    {
                        this.RelativeTo = new Point(0.0, 0.0);
                    }
                    else
                    {
                        this.RelativeTo = new Point(0.0, pNewBounding.Height);
                    }

                    if (this.OutputConnector.Position == pNewBounding.TopRight)
                    {
                        this.RelativeFrom = new Point(pNewBounding.Width, 0.0);
                    }
                    else
                    {
                        this.RelativeFrom = new Point(pNewBounding.Width, pNewBounding.Height);
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

        private void UpdateRendering()
        {
            // Computes the new bounding box.
            Rect lBoundingBox = this.ComputeBoundingBox();

            // Updating it.
            this.UpdateBoundingBox(lBoundingBox);

            // Updating the relative position of the connection rendering.
            this.UpdateRelativePos(lBoundingBox);
        }

        #endregion // Methods.
    }
}
