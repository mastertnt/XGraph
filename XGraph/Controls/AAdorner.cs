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
    /// Base class for the application adorner.
    /// </summary>
    public abstract class AAdorner<TAdornedElement> : Adorner where TAdornedElement : FrameworkElement
    {
        #region Fields

        /// <summary>
        /// Stores the adorner children.
        /// </summary>
        /// <remarks>
        /// Using a visual collection to keep the visual parenting aspect.
        /// </remarks>
        private VisualCollection mVisualChildren;

        #endregion // Fields.

         #region Constructors

        /// <summary>
        /// Innitializes a new instance of the <see>
        ///         <cref>AAdorner</cref>
        ///     </see>
        ///     class.
        /// </summary>
        /// <param name="pAdornedElement">The orned element.</param>
        protected AAdorner(TAdornedElement pAdornedElement)
            : base(pAdornedElement)
        {
            // Creating the elements contained in the adorner.
            this.mVisualChildren = new VisualCollection(this);

            // Ensuring the measure is well computed.
            this.ConcreteAdornedElement.SizeChanged += new SizeChangedEventHandler(this.OnAdornedElementSizeChanged);
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets the visual children list.
        /// </summary>
        protected VisualCollection VisualChildren
        {
            get
            {
                return this.mVisualChildren;
            }
        }

        /// <summary>
        /// Gets the concrete adorned element.
        /// </summary>
        public TAdornedElement ConcreteAdornedElement
        {
            get
            {
                return this.AdornedElement as TAdornedElement;
            }
        }

        /// <summary>
        /// Gets the visual children count.
        /// </summary>
        protected override sealed int VisualChildrenCount
        {
            get
            {
                return this.mVisualChildren.Count;
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Delegate called when the adorned element size changed.
        /// </summary>
        /// <param name="pSender">The modified adorned element.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        protected virtual void OnAdornedElementSizeChanged(object pSender, SizeChangedEventArgs pEventArgs)
        {
            this.InvalidateMeasure();
        }

        /// <summary>
        /// Returns the indexed visual child.
        /// </summary>
        /// <param name="pIndex">The index of the visual child.</param>
        /// <returns>The found visual child.</returns>
        protected override sealed Visual GetVisualChild(int pIndex)
        {
            return this.mVisualChildren[pIndex];
        }

        #endregion // Methods.
    }
}
