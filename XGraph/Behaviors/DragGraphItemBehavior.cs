using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using XGraph.Controls;
using XGraph.Extensions;

namespace XGraph.Behaviors
{
    /// <summary>
    /// Class defining a drag node specific behavior.
    /// </summary>
    public static class DragGraphItemBehavior
    {
        #region Dependencies

        /// <summary>
        /// Identifies the CanDrag attached property.
        /// </summary>
        public static readonly DependencyProperty CanBeDraggedProperty = DependencyProperty.RegisterAttached("CanBeDragged", typeof(bool), typeof(DragGraphItemBehavior), new PropertyMetadata(false, OnCanBeDraggedPropertyChanged));

        #endregion // Dependencies.

        #region Fields

        /// <summary>
        /// Stores the position where the drag process begins.
        /// </summary>
        private static Point? sDragStartPos = null;

        /// <summary>
        /// Stores the currently dragged item.
        /// </summary>
        private static AGraphItem sDraggedItem = null;

        #endregion // Fields.

        #region Methods

        /// <summary>
        /// Returns the property value for the given item.
        /// </summary>
        /// <param name="pItem">The item of interest.</param>
        /// <returns>True if it can be dragged, false otherwise.</returns>
        public static bool GetCanBeDragged(AGraphItem pItem)
        {
            return (bool)pItem.GetValue(CanBeDraggedProperty);
        }

        /// <summary>
        /// Sets the property value for the given item.
        /// </summary>
        /// <param name="pItem">The item of interest.</param>
        /// <param name="pValue">Flag indicating if the item can be dragged.</param>
        public static void SetCanBeDragged(AGraphItem pItem, bool pValue)
        {
            pItem.SetValue(CanBeDraggedProperty, pValue);
        }

        /// <summary>
        /// Delegate called when the CanBeDragged property is modified.
        /// </summary>
        /// <param name="pSender">The item of interest.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnCanBeDraggedPropertyChanged(object pSender, DependencyPropertyChangedEventArgs pEventArgs)
        {
            AGraphItem lItem = pSender as AGraphItem;
            bool lCanBeDragged = Convert.ToBoolean(pEventArgs.NewValue);
            if (lItem != null)
            {
                if (lCanBeDragged)
                {
                    lItem.MouseDown += OnItemMouseDown;
                    lItem.MouseMove += OnItemMouseMove;
                    lItem.MouseUp += OnItemMouseUp;
                }
                else
                {
                    lItem.MouseDown -= OnItemMouseDown;
                    lItem.MouseMove -= OnItemMouseMove;
                    lItem.MouseUp -= OnItemMouseUp;
                }
            }
        }

        /// <summary>
        /// Delegate called when the mouse is down on a graph view item.
        /// </summary>
        /// <param name="pSender">The item of interest.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnItemMouseDown(object pSender, MouseButtonEventArgs pEventArgs)
        {
            AGraphItem lItem = pSender as AGraphItem;
            if (lItem == null)
            {
                return;
            }

            SimpleGraphView lParentCanvas = lItem.FindVisualParent<SimpleGraphView>();
            if (lParentCanvas == null)
            {
                return;
            }

            // Sometimes the mouse goes faster than the item. Its up to the canvas then to continue the drag
            lParentCanvas.MouseMove += OnParentCanvasMouseMove;
            lParentCanvas.MouseUp += OnParentCanvasMouseUp;

            sDraggedItem = lItem;
            sDragStartPos = pEventArgs.GetPosition(lItem);
            lItem.CaptureMouse();
        }

        /// <summary>
        /// Delegate called when the mouse move on the dragged item parent canvas.
        /// </summary>
        /// <param name="pSender">The canvas containing the dragged item.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnParentCanvasMouseMove(object pSender, MouseEventArgs pEventArgs)
        {
            DragGraphItemBehavior.HandleDragItem(pEventArgs);
        }

        /// <summary>
        /// Delegate called when the mouse move on a graph view item.
        /// </summary>
        /// <param name="pSender">The item of interest.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnItemMouseMove(object pSender, MouseEventArgs pEventArgs)
        {
            DragGraphItemBehavior.HandleDragItem(pEventArgs);
        }

        /// <summary>
        /// Delegate called when the mouse is up on the dragged item parent canvas.
        /// </summary>
        /// <param name="pSender">The canvas containing the dragged item.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnParentCanvasMouseUp(object pSender, MouseButtonEventArgs pEventArgs)
        {
            DragGraphItemBehavior.ReleaseDraggedItem();
        }

        /// <summary>
        /// Delegate called when the mouse is up on a graph view item.
        /// </summary>
        /// <param name="pSender">The item of interest.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnItemMouseUp(object pSender, MouseButtonEventArgs pEventArgs)
        {
            DragGraphItemBehavior.ReleaseDraggedItem();
        }

        /// <summary>
        /// Handles the mouse move event when the drag process is started.
        /// </summary>
        /// <param name="pEventArgs">The mouse move event arguments.</param>
        private static void HandleDragItem(MouseEventArgs pEventArgs)
        {
            if (sDraggedItem == null)
            {
                return;
            }

            SimpleGraphView lParentCanvas = sDraggedItem.FindVisualParent<SimpleGraphView>();
            if (lParentCanvas == null)
            {
                return;
            }

            if (sDragStartPos != null && sDraggedItem != null && pEventArgs.LeftButton == MouseButtonState.Pressed)
            {
                Point lCanvasPos = pEventArgs.GetPosition(lParentCanvas);
                Point lNewItemPos = new Point(lCanvasPos.X - sDragStartPos.Value.X, lCanvasPos.Y - sDragStartPos.Value.Y);
                sDraggedItem.SetCanvasPosition(lNewItemPos);
            }
        }

        /// <summary>
        /// Releases the dragged item as the drag process ends.
        /// </summary>
        private static void ReleaseDraggedItem()
        {
            if (sDraggedItem == null)
            {
                return;
            }

            SimpleGraphView lParentCanvas = sDraggedItem.FindVisualParent<SimpleGraphView>();
            if (lParentCanvas == null)
            {
                return;
            }

            lParentCanvas.MouseMove -= OnParentCanvasMouseMove;
            lParentCanvas.MouseUp -= OnParentCanvasMouseUp;

            sDraggedItem.ReleaseMouseCapture();
            sDraggedItem = null;
            sDragStartPos = null;
        }

        #endregion // Methods.
    }
}
