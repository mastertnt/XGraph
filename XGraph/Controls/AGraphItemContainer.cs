using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using XGraph.Extensions;

namespace XGraph.Controls
{
    /// <summary>
    /// Base class for the graph view item container.
    /// </summary>
    public abstract class AGraphItemContainer : ContentControl
    {
        #region Dependencies

        /// <summary>
        /// Identifies the IsSelected dependency property.
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(AGraphItemContainer), new FrameworkPropertyMetadata(false, OnIsSelectedChanged));

        #endregion // Dependencies.

        #region Properties

        /// <summary>
        /// Gets or sets the selection state of the node.
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return (bool)this.GetValue(IsSelectedProperty);
            }
            set
            {
                this.SetValue(IsSelectedProperty, value);
            }
        }

        /// <summary>
        /// Gets the bounding box of this container.
        /// </summary>
        public virtual Rect BoundingBox
        {
            get
            {
                AdornerLayeredCanvas lParentCanvas = this.FindVisualParent<AdornerLayeredCanvas>();
                if (lParentCanvas != null)
                {
                    return this.BoundsRelativeTo(lParentCanvas);
                }

                return Rect.Empty;
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Delegate called when the selection state changed.
        /// </summary>
        /// <param name="pObject">The modified control.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnIsSelectedChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
        {
            AGraphItemContainer lNodeView = pObject as AGraphItemContainer;
            if (lNodeView != null)
            {
                lNodeView.UpdateVisualState();
            }
        }

        /// <summary>
        /// Updates the visual state of the node.
        /// </summary>
        private void UpdateVisualState()
        {
            if (this.IsSelected)
            {
                VisualStateManager.GoToState(this, "Selected", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "Unselected", true);
            }
        }

        #endregion // Methods.
    }
}
