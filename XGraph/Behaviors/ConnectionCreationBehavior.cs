using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using XGraph.Controls;
using XGraph.ViewModels;

namespace XGraph.Behaviors
{
    /// <summary>
    /// Class handling the connection creation using a graphic line.
    /// </summary>
    public class ConnectionCreationBehavior : AGraphViewBehavior
    {
        #region Fields

        /// <summary>
        /// This field stores the source connector.
        /// </summary>
        private OutputConnector mSourceConnector;

        /// <summary>
        /// This field stores the target connector.
        /// </summary>
        private InputConnector mTargetConnector;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionCreationBehavior"/> class.
        /// </summary>
        /// <param name="pParent">The behavior parent view.</param>
        /// <param name="pWorkingCanvas">The canvas containing the connection line to work with.</param>
        public ConnectionCreationBehavior(SimpleGraphView pParent, Canvas pWorkingCanvas)
            : base(pParent)
        {
            this.WorkingCanvas = pWorkingCanvas;
            this.WorkingCanvas.Visibility = Visibility.Collapsed;
            this.ConnectingLine.Visibility = Visibility.Collapsed;

            this.mSourceConnector = null;
            this.mTargetConnector = null;
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets or sets the canvas in witch is diplayed the line used to draw a connection.
        /// </summary>
        public Canvas WorkingCanvas
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the connecting line.
        /// </summary>
        public ConnectingLine ConnectingLine
        {
            get
            {
                return this.WorkingCanvas.Children.OfType<ConnectingLine>().First();
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Starts the connection creation process from the given source connector.
        /// </summary>
        /// <param name="pSource">The source connector.</param>
        public void StartCreation(OutputConnector pSource)
        {
            // Updating the source.
            this.mSourceConnector = pSource;

            // Making the working canvas visible and updating the line position.
            this.ConnectingLine.From = this.mSourceConnector.Position;
            this.ConnectingLine.To = this.mSourceConnector.Position;
            this.WorkingCanvas.Visibility = Visibility.Visible;
            this.ConnectingLine.Visibility = Visibility.Visible;

            // Listening to the mouse move and up of the canvas to update the line.
            this.WorkingCanvas.CaptureMouse();
            this.WorkingCanvas.MouseMove -= this.OnWorkingCanvasMouseMove;
            this.WorkingCanvas.MouseMove += this.OnWorkingCanvasMouseMove;
            this.WorkingCanvas.MouseUp -= this.OnWorkingCanvasMouseUp;
            this.WorkingCanvas.MouseUp += this.OnWorkingCanvasMouseUp;
        }

        /// <summary>
        /// Delegate called when the mouse move on the working canvas.
        /// </summary>
        /// <param name="pSender">The event sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnWorkingCanvasMouseMove(object pSender, MouseEventArgs pEventArgs)
        {
            if (pEventArgs.LeftButton == MouseButtonState.Pressed)
            {
                // Create a path according to the source and the end.
                this.ConnectingLine.To = pEventArgs.GetPosition(this.WorkingCanvas);

                // Updating the cursor.
                this.mTargetConnector = this.TryHitInputConnector();
                if (this.mTargetConnector != null)
                {
                    PortViewModel lTargetViewModel = this.mTargetConnector.ParentPort.Content as PortViewModel;
                    PortViewModel lSourceViewModel = this.mSourceConnector.ParentPort.Content as PortViewModel;
                    if (lSourceViewModel.CanBeConnectedTo(lTargetViewModel))
                    {
                        this.WorkingCanvas.Cursor = Cursors.Cross;
                    }
                    else
                    {
                        this.WorkingCanvas.Cursor = Cursors.No;
                        this.mTargetConnector = null;
                    }
                }
                else
                {
                    this.WorkingCanvas.Cursor = Cursors.Cross;
                }
            }
        }

        /// <summary>
        /// Delegate called when the mouse isup on the working canvas.
        /// </summary>
        /// <param name="pSender">The event sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnWorkingCanvasMouseUp(object pSender, MouseButtonEventArgs pEventArgs)
        {
            // Release the mouse capture.
            this.WorkingCanvas.ReleaseMouseCapture();
            this.WorkingCanvas.MouseMove -= this.OnWorkingCanvasMouseMove;
            this.WorkingCanvas.MouseUp -= this.OnWorkingCanvasMouseUp;
            this.WorkingCanvas.Visibility = Visibility.Collapsed;
            this.ConnectingLine.Visibility = Visibility.Collapsed;

            // Trying to create the final connection.
            if (this.mSourceConnector != null && this.mTargetConnector != null)
            {
                // Both defined means the connection can be created. Test have been made in the mouse move event handler.
                GraphViewModel lGraphViewModel = this.ParentView.DataContext as GraphViewModel;
                if (lGraphViewModel == null)
                {
                    return;
                }

                PortViewModel lSourceViewModel = this.mSourceConnector.ParentPort.Content as PortViewModel;
                PortViewModel lTargetViewModel = this.mTargetConnector.ParentPort.Content as PortViewModel;

                ConnectionViewModel lConnectionViewModel = new ConnectionViewModel();
                lConnectionViewModel.Output = lSourceViewModel;
                lConnectionViewModel.Input = lTargetViewModel;
                lGraphViewModel.AddConnection(lConnectionViewModel);
            }

            // Forget the reference on connectors.
            this.mSourceConnector = null;
            this.mTargetConnector = null;
        }

        /// <summary>
        /// Using the To property of the connecting line to try to hit an input connector.
        /// </summary>
        /// <returns>The input connector if hit succeded.</returns>
        private InputConnector TryHitInputConnector()
        {
            return this.ParentView.HitControl<InputConnector>(this.ConnectingLine.To);
        }

        #endregion // Methods.
    }
}
