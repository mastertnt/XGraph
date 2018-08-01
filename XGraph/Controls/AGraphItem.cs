using System.Windows;
using System.Windows.Controls;
using XGraph.Extensions;

namespace XGraph.Controls
{
    /// <summary>
    /// Base class for the graph view item.
    /// </summary>
    public abstract class AGraphItem : ContentControl
    {
        #region Dependencies

        /// <summary>
        /// Identifies the IsSelected dependency property.
        /// </summary>
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(AGraphItem), new FrameworkPropertyMetadata(false, OnIsSelectedChanged));

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
        public Rect BoundingBox
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
            AGraphItem lItem = pObject as AGraphItem;
            if (lItem != null)
            {
                lItem.UpdateVisualState();
            }
        }

        /// <summary>
        /// Updates the visual state of the node.
        /// </summary>
        protected virtual void UpdateVisualState()
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

        /// <summary>
        /// Sets the position of the item in the parent canvas.
        /// </summary>
        /// <param name="pPos">The container position.</param>
        public void SetCanvasPosition(Point pPos)
        {
            GraphItemContainer lParentGraphItem = this.FindVisualParent<GraphItemContainer>();
            if (lParentGraphItem != null)
            {
                lParentGraphItem.PosX = pPos.X;
                lParentGraphItem.PosY = pPos.Y;
            }
        }

        /// <summary>
        /// Gets the position of the item in the parent canvas.
        /// </summary>
        /// <returns>The position of the item in the parent canvas.</returns>
        public Point? GetCanvasPosition()
        {
            GraphItemContainer lParentGraphItem = this.FindVisualParent<GraphItemContainer>();
            if (lParentGraphItem != null)
            {
                return new Point(lParentGraphItem.PosX, lParentGraphItem.PosY);
            }

            return null;
        }

        #endregion // Methods.
    }
}
