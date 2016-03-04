using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using XGraph.Extensions;
using XGraph.ViewModels;

namespace XGraph.Controls
{
    /// <summary>
    /// This class represents a connection during the building (when the user drags it).
    /// </summary>
    /// <!-- Nicolas Baudrey -->
    public class ConnectingLineAdorner : Adorner
    {
        #region Fields

        /// <summary>
        /// This field stores the source connector of the adorner.
        /// </summary>
        private OutputConnector mSourceConnector;

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
        /// Initializes a new instance of the <see cref="ConnectingLineAdorner"/> class.
        /// </summary>
        /// <param name="pElement">The parent element.</param>
        /// <param name="pSourceConnector">The source connector.</param>
        public ConnectingLineAdorner(UIElement pElement, OutputConnector pSourceConnector)
            : base(pElement)
        {
            this.Cursor = Cursors.Cross;
            this.mSourceConnector = pSourceConnector;

            // Creating the line contained in the adorner.
            this.mVisualChildren = new VisualCollection(this);
            this.mVisualChildren.Add(new ConnectingLine() { From = pSourceConnector.Position });
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets the connecting line.
        /// </summary>
        public ConnectingLine ConnectingLine
        {
            get
            {
                return this.GetVisualChild(0) as ConnectingLine;
            }
        }

        /// <summary>
        /// Gets the visual children count.
        /// </summary>
        protected override Int32 VisualChildrenCount
        {
            get
            {
                return this.mVisualChildren.Count;
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// This method is called to render the adorner.
        /// </summary>
        /// <param name="pDC">The current drawing context.</param>
        protected override void OnRender(DrawingContext pDC)
        {
            base.OnRender(pDC);

            // Without a background the OnMouseMove event would not be fired
            // Alternative: implement a Canvas as a child of this adorner, like
            // the ConnectionAdorner does.
            pDC.DrawRectangle(Brushes.Transparent, null, new Rect(this.RenderSize));
        }

        /// <summary>
        /// Computes the size of the adorner.
        /// </summary>
        /// <param name="pAvailableSize">The initial available size.</param>
        /// <returns>The viewport desired size.</returns>
        protected override Size MeasureOverride(Size pAvailableSize)
        {
            this.ConnectingLine.Measure(pAvailableSize);
            return pAvailableSize;
        }

        /// <summary>
        /// Arranges the connectors by taking in account the computed size of the panel viewport.
        /// </summary>
        /// <param name="pFinalSize">The available size.</param>
        /// <returns>The size used (here equals to the available size).</returns>
        protected override Size ArrangeOverride(Size pFinalSize)
        {
            Rect lFinalSize = new Rect(pFinalSize);
            this.ConnectingLine.Arrange(lFinalSize);
            return pFinalSize;
        }

        /// <summary>
        /// This method is called when mouse move occured on the adorner.
        /// </summary>
        /// <param name="pEventArgs">The event arguments</param>
        protected override void OnMouseMove(MouseEventArgs pEventArgs)
        {
            if (pEventArgs.LeftButton == MouseButtonState.Pressed)
            {
                if (this.IsMouseCaptured == false)
                {
                    this.CaptureMouse();
                }

                // Create a path according to the source and the end.
                this.ConnectingLine.To = pEventArgs.GetPosition(this);

                // Redraw it.
                this.InvalidateVisual();
            }
            else
            {
                if (this.IsMouseCaptured)
                {
                    this.ReleaseMouseCapture();
                }
            }
        }

        /// <summary>
        /// This method is called when a mouse button up occured on the adorner.
        /// </summary>
        /// <param name="pEventArgs">The event arguments</param>
        protected override void OnMouseUp(MouseButtonEventArgs pEventArgs)
        {
            // Release the mouse capture.
            if (this.IsMouseCaptured)
            {
                this.ReleaseMouseCapture();
            }

            // Getting the position.
            Point lHitPoint = pEventArgs.GetPosition(this);

            AdornerLayeredCanvas lParentCanvas = this.AdornedElement as AdornerLayeredCanvas;
            if (lParentCanvas != null)
            {
                // Remove the adorner.
                AdornerLayer lLayer = lParentCanvas.AdornerLayer;
                if (lLayer != null)
                {
                    lLayer.Remove(this);
                }

                // Hitting the target connector.
                InputConnector lTargetConnector = lParentCanvas.HitControl<InputConnector>(lHitPoint);
                if (lTargetConnector != null)
                {
                    GraphViewModel lGraphViewModel = lParentCanvas.DataContext as GraphViewModel;
                    if (lGraphViewModel != null)
                    {
                        PortViewModel lTargetViewModel = lTargetConnector.ParentPort.Content as PortViewModel;
                        PortViewModel lSourceViewModel = this.mSourceConnector.ParentPort.Content as PortViewModel;
                        if (lTargetViewModel != null && lSourceViewModel.CanBeConnectedTo(lTargetViewModel))
                        {
                            ConnectionViewModel lConnectionViewModel = new ConnectionViewModel();
                            lConnectionViewModel.Output= lSourceViewModel;
                            lConnectionViewModel.Input = lTargetViewModel;
                            lGraphViewModel.AddConnection(lConnectionViewModel);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Returns the indexed visual child.
        /// </summary>
        /// <param name="pIndex">The index of the visual child.</param>
        /// <returns>The found visual child.</returns>
        protected override Visual GetVisualChild(Int32 pIndex)
        {
            return this.mVisualChildren[pIndex];
        }

        #endregion // Methods.
    }
}

