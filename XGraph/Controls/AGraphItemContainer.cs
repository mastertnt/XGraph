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
            AGraphItemContainer lItem = pObject as AGraphItemContainer;
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
        /// Sets the position of the container in the parent canvas.
        /// </summary>
        /// <param name="pPos">The container position.</param>
        public void SetCanvasPosition(Point pPos)
        {
            GraphItem lParentGraphItem = this.FindVisualParent<GraphItem>();
            if (lParentGraphItem != null)
            {
                lParentGraphItem.PosX = pPos.X;
                lParentGraphItem.PosY = pPos.Y;
            }
        }

        #endregion // Methods.
    }
}
