using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using XGraph.Controls;
using XGraph.ViewModels;

namespace XGraph.Behaviors
{
    /// <summary>
    /// Class handling the selection using a box in the graph view.
    /// </summary>
    public class BoxSelectionBehavior : AGraphViewBehavior
    {
        #region Fields

        /// <summary>
        /// Stores the flag indicating if the left mouse button is down on the view, that is, not on one of its item.
        /// </summary>
        private bool mIsLeftMouseButtonDownOnView;

        /// <summary>
        /// Stores the flag indincating if the selection box is creating.
        /// </summary>
        private bool mIsDraggingSelectionBox;

        /// <summary>
        /// Stores the initial position of the selection box.
        /// </summary>
        private Point mOrigMouseDownPoint;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BoxSelectionBehavior"/> class.
        /// </summary>
        /// <param name="pParent">The behavior parent view.</param>
        /// <param name="pSelectionBoxCanvas">The canvas containing the box.</param>
        public BoxSelectionBehavior(SimpleGraphView pParent, Canvas pSelectionBoxCanvas)
            : base(pParent)
        {
            this.SelectionBoxCanvas = pSelectionBoxCanvas;
            this.SelectionBoxCanvas.Visibility = Visibility.Collapsed;
            this.SelectionBox.Visibility = Visibility.Collapsed;

            this.mIsLeftMouseButtonDownOnView = false;
            this.mIsDraggingSelectionBox = false;
            this.DragThreshold = 0.1;

            // Registering on the event used to draw the box.
            this.ParentView.MouseDown += this.OnParentViewMouseDown;
            this.ParentView.MouseMove += this.OnParentViewMouseMove;
            this.ParentView.MouseUp += this.OnParentViewMouseUp;
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets or sets the canvas in witch is diplayed the box used for multi selection.
        /// </summary>
        public Canvas SelectionBoxCanvas
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the flag indicating the threshold used to define the selection box creation.
        /// </summary>
        public double DragThreshold
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the selection box.
        /// </summary>
        public SelectionBox SelectionBox
        {
            get
            {
                return this.SelectionBoxCanvas.Children.OfType<SelectionBox>().First();
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Delegate called when the mouse is down on the view.
        /// </summary>
        /// <param name="pSender">The graph view sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnParentViewMouseDown(object pSender, MouseButtonEventArgs pEventArgs)
        {
            if (this.ParentView.IsReadOnly == false && pEventArgs.ChangedButton == MouseButton.Left)
            {
                this.mIsLeftMouseButtonDownOnView = true;
                this.mOrigMouseDownPoint = pEventArgs.GetPosition(this.ParentView);

                this.ParentView.CaptureMouse();

                pEventArgs.Handled = true;
            }
        }

        /// <summary>
        /// Delegate called when the mouse moves on the view.
        /// </summary>
        /// <param name="pSender">The graph view sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnParentViewMouseMove(object pSender, MouseEventArgs pEventArgs)
        {
            if (this.mIsDraggingSelectionBox)
            {
                // Drag selection already initiated, handle drag selection in progress.
                Point lCurMouseDownPoint = pEventArgs.GetPosition(this.ParentView);
                this.UpdateSelectionBox(this.mOrigMouseDownPoint, lCurMouseDownPoint);

                pEventArgs.Handled = true;
            }
            else if (this.mIsLeftMouseButtonDownOnView)
            {
                // The user is left-dragging the mouse, but don't initiate drag selection until they have dragged past the threshold value.
                Point lCurMouseDownPoint = pEventArgs.GetPosition(this.ParentView);
                Vector lDragDelta = lCurMouseDownPoint - this.mOrigMouseDownPoint;
                double lDragDistance = Math.Abs(lDragDelta.Length);
                if (lDragDistance > this.DragThreshold)
                {
                    // When the mouse has been dragged more than the threshold value commence drag selection.
                    this.mIsDraggingSelectionBox = true;
                    this.InitSelectionBox(this.mOrigMouseDownPoint, lCurMouseDownPoint);
                }

                pEventArgs.Handled = true;
            }
        }

        /// <summary>
        /// Initializes the selection box using the the given points.
        /// </summary>
        /// <param name="pPt1">The first point.</param>
        /// <param name="pPt2">The second point.</param>
        private void InitSelectionBox(Point pPt1, Point pPt2)
        {
            // Updating the box.
            this.UpdateSelectionBox(pPt1, pPt2);

            // Showing the canvas and then the box.
            this.SelectionBoxCanvas.Visibility = Visibility.Visible;
            this.SelectionBox.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// Updates the selection box using the the given points.
        /// </summary>
        /// <param name="pPt1">The first point.</param>
        /// <param name="pPt2">The second point.</param>
        private void UpdateSelectionBox(Point pPt1, Point pPt2)
        {
            // Determine x, y, width and height of the rect inverting the points if necessary.
            double lX = Math.Min(pPt1.X, pPt2.X);
            double lY = Math.Min(pPt1.Y, pPt2.Y);
            double lWidth = Math.Abs(pPt2.X - pPt1.X);
            double lHeight = Math.Abs(pPt2.Y - pPt1.Y);

            // Update the coordinates of the rectangle used for drag selection.
            Canvas.SetLeft(this.SelectionBox, lX);
            Canvas.SetTop(this.SelectionBox, lY);
            this.SelectionBox.Width = lWidth;
            this.SelectionBox.Height = lHeight;
        }

        /// <summary>
        /// Delegate called when the mouse is up on the view.
        /// </summary>
        /// <param name="pSender">The graph view sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnParentViewMouseUp(object pSender, MouseButtonEventArgs pEventArgs)
        {
            if (pEventArgs.ChangedButton == MouseButton.Left)
            {
                if (this.mIsDraggingSelectionBox)
                {
                    // Drag selection has ended, apply the 'selection box'.
                    this.mIsDraggingSelectionBox = false;
                    this.ApplySelectionBox();
                }

                if (this.mIsLeftMouseButtonDownOnView)
                {
                    this.mIsLeftMouseButtonDownOnView = false;
                    this.ParentView.ReleaseMouseCapture();
                }
            }
        }

        /// <summary>
        /// Applies the selection box on the view to know the selected items.
        /// </summary>
        private void ApplySelectionBox()
        {
            // Clear selection.
            this.ParentView.SelectedItems.Clear();

            // Hidding the canvas hosting the selection box.
            this.SelectionBoxCanvas.Visibility = Visibility.Collapsed;
            this.SelectionBox.Visibility = Visibility.Collapsed;

            double lX = Canvas.GetLeft(this.SelectionBox);
            double lY = Canvas.GetTop(this.SelectionBox);
            double lWidth = this.SelectionBox.Width;
            double lHeight = this.SelectionBox.Height;
            Rect lSelectionRect = new Rect(lX, lY, lWidth, lHeight);

            // Inflate the drag selection-rectangle by 1/10 of its size to make sure the intended item is selected.
            lSelectionRect.Inflate(lWidth / 10, lHeight / 10);

            // Selecting the items contained in the selection rect.
            if (this.ParentView.ItemsSource != null)
            {
                foreach (IGraphItemViewModel lItem in this.ParentView.ItemsSource)
                {
                    // Getting the corresponding container.
                    AGraphItem lContainer = this.ParentView.GetContainerForViewModel(lItem);
                    if (lContainer != null)
                    {
                        if (lSelectionRect.Contains(lContainer.BoundingBox))
                        {
                            this.ParentView.SelectedItems.Add(lItem);
                        }
                    }
                }
            }
        }

        #endregion // Methods.
    }
}
