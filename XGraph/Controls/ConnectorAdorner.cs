using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace XGraph.Controls
{
    /// <summary>
    /// Classe defining the connector adorner.
    /// </summary>
    /// <!-- Damien Porte -->
    public class ConnectorAdorner : AAdorner<PortView>
    {
        #region Constructors

        /// <summary>
        /// Innitializes a new instance of the <see cref="ConnectorAdorner"/> class.
        /// </summary>
        /// <param name="pAdornedElement">The orned port view.</param>
        public ConnectorAdorner(PortView pAdornedElement)
            : base(pAdornedElement)
        {
            // Creating the connectors contained in the adorner.
            if (this.ConcreteAdornedElement.Direction == ViewModels.PortDirection.Input)
            {
                this.VisualChildren.Add(new InputConnector());
            }
            else
            {
                this.VisualChildren.Add(new OutputConnector());
            }
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets the connector.
        /// </summary>
        public AConnector Connector
        {
            get
            {
                return this.GetVisualChild(0) as AConnector;
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Computes the size of the adorner.
        /// </summary>
        /// <param name="pAvailableSize">The initial available size.</param>
        /// <returns>The viewport desired size.</returns>
        protected override Size MeasureOverride(Size pAvailableSize)
        {
            // Getting the size of the adorned port view.
            Size lPortViewSize = new Size(this.ConcreteAdornedElement.ActualWidth, this.ConcreteAdornedElement.ActualHeight);

            // Computing the final size by adding the width of the wanted connector.
            this.Connector.Measure(pAvailableSize);
            Size lAdornerSize = lPortViewSize;
            lAdornerSize.Width += this.Connector.DesiredSize.Width;

            // Returning the final adorner size.
            return lAdornerSize;
        }

        /// <summary>
        /// Arranges the connectors by taking in account the computed size of the panel viewport.
        /// </summary>
        /// <param name="pFinalSize">The available size.</param>
        /// <returns>The size used (here equals to the available size).</returns>
        protected override Size ArrangeOverride(Size pFinalSize)
        {
            // Getting the size of the adorned port view.
            double lPortViewWidth = this.ConcreteAdornedElement.ActualWidth;
            double lPortViewHeight = this.ConcreteAdornedElement.ActualHeight;

            // Getting the size of the connector.
            double lConnectorWidth = this.Connector.DesiredSize.Width;
            double lConnectorHeight = this.Connector.DesiredSize.Height;

            // Displaying connector by calling the arrange method depending on the port direction.
            if (this.ConcreteAdornedElement.Direction == ViewModels.PortDirection.Input)
            {
                this.Connector.Arrange(new Rect(-lConnectorWidth, lPortViewHeight / 2.0 - lConnectorHeight / 2.0, lConnectorWidth, lConnectorHeight));
            }
            else
            {
                this.Connector.Arrange(new Rect(lPortViewWidth, lPortViewHeight / 2.0 - lConnectorHeight / 2.0, lConnectorWidth, lConnectorHeight));
            }

            // Return the final size.
            return pFinalSize;
        }

        #endregion // Methods.
    }
}
