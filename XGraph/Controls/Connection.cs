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

        #endregion // Dependencies.

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="Connection"/> class.
        /// </summary>
        static Connection()
        {
            // set the key to reference the style for this control
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Connection), new FrameworkPropertyMetadata(typeof(Connection)));
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
        /// Gets the bounding box of this container.
        /// </summary>
        public override Rect BoundingBox
        {
            get
            {
                Point lP1 = this.OutputConnector.Position;
                Point lP2 = this.InputConnector.Position;

                double lX = Math.Min(lP1.X, lP2.X);
                double lY = Math.Min(lP1.Y, lP2.Y);
                double lWidth = Math.Abs(lP1.X - lP2.X);
                double lHeight = Math.Abs(lP1.Y - lP2.Y);

                return new Rect(lX, lY, lWidth, lHeight);
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
                        }
                    }

                    NodeView lInputNode = lParentCanvas.GetContainerForViewModel<NodeViewModel, NodeView>(lViewModel.Input.ParentNode);
                    if (lInputNode != null)
                    {
                        PortView lInputPort = lInputNode.GetContainerForPortViewModel(lViewModel.Input);
                        if (lInputPort != null)
                        {
                            this.InputConnector = lInputPort.Connector as InputConnector;
                        }
                    }
                }
            }
        }

        #endregion // Methods.
    }
}
