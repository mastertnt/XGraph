using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace XGraph.Controls
{
    /// <summary>
    /// Classe defining the adorner displaying the informations like errors, warnings...
    /// </summary>
    /// <!-- Damien Porte -->
    public class InfosAdorner : AAdorner<NodeView>
    {
        #region Fields

        /// <summary>
        /// Stores the space in pixel between two indicators.
        /// </summary>
        private const double SPACE_BETWEEN_INDICATORS = 2.0;

        #endregion // Fields.
        
        #region Constructors

        /// <summary>
        /// Innitializes a new instance of the <see cref="InfosAdorner"/> class.
        /// </summary>
        /// <param name="pAdornedElement">The orned node view.</param>
        public InfosAdorner(NodeView pAdornedElement)
            : base(pAdornedElement)
        {
            // Filling the visual children.
            this.VisualChildren.Add(new WarningsIndicator(pAdornedElement));
            this.VisualChildren.Add(new ErrorsIndicator(pAdornedElement));
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Computes the size of the adorner.
        /// </summary>
        /// <param name="pAvailableSize">The initial available size.</param>
        /// <returns>The viewport desired size.</returns>
        protected override Size MeasureOverride(Size pAvailableSize)
        {
            // Getting the size of the adorned node view.
            Size lNodeViewSize = new Size(this.ConcreteAdornedElement.ActualWidth, this.ConcreteAdornedElement.ActualHeight);

            // Compting the final size.
            Size lAdornerSize = new Size();
            foreach (UIElement lChild in this.VisualChildren)
            {
                lChild.Measure(pAvailableSize);
                lAdornerSize.Height = Math.Max(lAdornerSize.Height, lChild.DesiredSize.Height);
            }
            lAdornerSize.Height += lNodeViewSize.Height;
            lAdornerSize.Width += lNodeViewSize.Width;

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
            double lAdornedElementWidth = this.ConcreteAdornedElement.ActualWidth;
            double lAdornedElementHeight = this.ConcreteAdornedElement.ActualHeight;

            // Computing the size of the indicator place.
            Size lAdornerSize = new Size();
            lAdornerSize.Width += (this.VisualChildren.Count - 1) * SPACE_BETWEEN_INDICATORS;
            foreach (UIElement lChild in this.VisualChildren)
            {
                lAdornerSize.Height = Math.Max(lAdornerSize.Height, lChild.DesiredSize.Height);
                lAdornerSize.Width += lChild.DesiredSize.Width;
            }
            
            // Computing the start position of indicators.
            double lCurrentIndicatorPos = Math.Round((lAdornedElementWidth - lAdornerSize.Width) / 2.0);

            // Computing the position of each information indicator.
            foreach (UIElement lChild in this.VisualChildren)
            {
                lChild.Arrange(new Rect(lCurrentIndicatorPos, lAdornedElementHeight + SPACE_BETWEEN_INDICATORS, lChild.DesiredSize.Width, lChild.DesiredSize.Height));
                lCurrentIndicatorPos += SPACE_BETWEEN_INDICATORS + lChild.DesiredSize.Width;
            }

            // Return the final size.
            return pFinalSize;
        }

        #endregion // Methods.
    }
}
