using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace XGraph.Controls
{
    /// <summary>
    /// Class defining the main part shape of the node view.
    /// </summary>
    /// <!-- Damien Porte -->
    public class NodeViewShape : Shape
    {
        #region Dependencies

        /// <summary>
        /// Identifies the BevelLength dependency property.
        /// </summary>
        public static readonly DependencyProperty BevelLengthProperty = DependencyProperty.Register("BevelLength", typeof(double), typeof(NodeViewShape), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.AffectsRender, OnBevelLengthChanged));

        #endregion // Dependencies.

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="NodeViewShape"/> class.
        /// </summary>
        static NodeViewShape()
        {
            Shape.StrokeThicknessProperty.OverrideMetadata(typeof(NodeViewShape), new FrameworkPropertyMetadata(0.0));
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets or sets the bevel length of the shape.
        /// </summary>
        public double BevelLength
        {
            get
            {
                return (double)this.GetValue(BevelLengthProperty);
            }
            set
            {
                this.SetValue(BevelLengthProperty, value);
            }
        }

        /// <summary>
        /// Gets the geometry of the shape.
        /// </summary>
        protected override Geometry DefiningGeometry
        {
            get
            {
                // Margin computed for the border not to be drawn aliased.
                double lPenThickness = this.StrokeThickness;
                double lMargin = lPenThickness / 2;

                double lX = lMargin;
                double lY = lMargin;
                double lWidth = Math.Max(0, this.ActualWidth - lMargin);
                double lHeight = Math.Max(0, this.ActualHeight - lMargin);

                // Defining the points.
                Point lP1 = new Point(lX, this.BevelLength);
                Point lP2 = new Point(this.BevelLength, lY);
                Point lP3 = new Point(lWidth, lY);
                Point lP4 = new Point(lWidth, lHeight - this.BevelLength);
                Point lP5 = new Point(lWidth - this.BevelLength, lHeight);
                Point lP6 = new Point(lX, lHeight);

                // Building the path.
                PathSegment[] lPath =
                {
                    new LineSegment(lP1, true),
                    new LineSegment(lP2, true),
                    new LineSegment(lP3, true),
                    new LineSegment(lP4, true),
                    new LineSegment(lP5, true),
                    new LineSegment(lP6, true)
                };

                // Building the figure using the path.
                PathFigure[] lFigures = { new PathFigure(lP1, lPath, true) };

                // Building the final geometry.
                return new PathGeometry(lFigures);
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Delegate called when the bevel length changed.
        /// </summary>
        /// <param name="pObject">The modified control.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnBevelLengthChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
        {   
            NodeViewShape lRenderer = pObject as NodeViewShape;
            if (lRenderer != null)
            {
                lRenderer.InvalidateVisual();
            }
        }

        #endregion // Methods.
    }
}
