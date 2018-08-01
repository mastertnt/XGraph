using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace XGraph.Themes.ExpressionDark.Controls
{
    /// <summary>
    /// Class defining the shape of the tooltip used to display the error and warning message.
    /// </summary>
    /// <!-- Damien Porte -->
    public class InfosIndicatorTooltipShape : Shape
    {
        #region Dependencies

        /// <summary>
        /// Identifies the CornerRadius dependency property.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(InfosIndicatorTooltipShape), new FrameworkPropertyMetadata(new CornerRadius(), FrameworkPropertyMetadataOptions.AffectsRender, OnCornerRadiusChanged));

        /// <summary>
        /// Identifies the ArrowHeight dependency property.
        /// </summary>
        public static readonly DependencyProperty ArrowHeightProperty = DependencyProperty.Register("ArrowHeight", typeof(double), typeof(InfosIndicatorTooltipShape), new FrameworkPropertyMetadata(3.0, FrameworkPropertyMetadataOptions.AffectsRender, OnArrowHeightChanged));

        #endregion // Dependencies.

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="InfosIndicatorTooltipShape"/> class.
        /// </summary>
        static InfosIndicatorTooltipShape()
        {
            Shape.StrokeThicknessProperty.OverrideMetadata(typeof(InfosIndicatorTooltipShape), new FrameworkPropertyMetadata(0.0));
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets or sets the corner radius of the shape.
        /// </summary>
        public CornerRadius CornerRadius
        {
            get
            {
                return (CornerRadius)this.GetValue(CornerRadiusProperty);
            }
            set
            {
                this.SetValue(CornerRadiusProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the arrow height of the shape.
        /// </summary>
        public double ArrowHeight
        {
            get
            {
                return (double)this.GetValue(ArrowHeightProperty);
            }
            set
            {
                this.SetValue(ArrowHeightProperty, value);
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

                // Creating the geometry.
                StreamGeometry lGeometry = new StreamGeometry();
                using (StreamGeometryContext lContext = lGeometry.Open())
                {
                    bool lIsStroked = lMargin > 0.0;
                    bool lIsFilled = this.Fill != null;
                    const bool lIsSmoothJoin = true;

                    Point lP1 = new Point(lX, this.CornerRadius.TopLeft + this.ArrowHeight);
                    lContext.BeginFigure(lP1, lIsFilled, true);

                    Point lP2 = new Point(this.CornerRadius.TopLeft, lY + this.ArrowHeight);
                    Size lTopLeftArcSize = new Size(this.CornerRadius.TopLeft, this.CornerRadius.TopLeft);
                    lContext.ArcTo(lP2, lTopLeftArcSize, 90, false, SweepDirection.Clockwise, lIsStroked, lIsSmoothJoin);

                    Point lP3 = new Point(this.CornerRadius.TopLeft * 2.0, lY + this.ArrowHeight);
                    lContext.LineTo(lP3, lIsStroked, lIsSmoothJoin);

                    Point lP4 = new Point(this.CornerRadius.TopLeft * 2.0 + this.ArrowHeight / 2.0, lY);
                    lContext.LineTo(lP4, lIsStroked, lIsSmoothJoin);

                    Point lP5 = new Point(this.CornerRadius.TopLeft * 2.0 + this.ArrowHeight, lY + this.ArrowHeight);
                    lContext.LineTo(lP5, lIsStroked, lIsSmoothJoin);

                    Point lP6 = new Point(lWidth - this.CornerRadius.TopRight, lY + this.ArrowHeight);
                    lContext.LineTo(lP6, lIsStroked, lIsSmoothJoin);

                    Point lP7 = new Point(lWidth, this.CornerRadius.TopRight + this.ArrowHeight);
                    Size lTopRightArcSize = new Size(this.CornerRadius.TopRight, this.CornerRadius.TopRight);
                    lContext.ArcTo(lP7, lTopRightArcSize, 90, false, SweepDirection.Clockwise, lIsStroked, lIsSmoothJoin);

                    Point lP8 = new Point(lWidth, lHeight - this.CornerRadius.BottomRight);
                    lContext.LineTo(lP8, lIsStroked, lIsSmoothJoin);

                    Point lP9 = new Point(lWidth - this.CornerRadius.BottomRight, lHeight);
                    Size lBottomRightArcSize = new Size(this.CornerRadius.BottomRight, this.CornerRadius.BottomRight);
                    lContext.ArcTo(lP9, lBottomRightArcSize, 90, false, SweepDirection.Clockwise, lIsStroked, lIsSmoothJoin);

                    Point lP10 = new Point(this.CornerRadius.BottomLeft, lHeight);
                    lContext.LineTo(lP10, lIsStroked, lIsSmoothJoin);

                    Point lP11 = new Point(lX, lHeight - this.CornerRadius.BottomLeft);
                    Size lBottomLeftArcSize = new Size(this.CornerRadius.BottomLeft, this.CornerRadius.BottomLeft);
                    lContext.ArcTo(lP11, lBottomLeftArcSize, 90, false, SweepDirection.Clockwise, lIsStroked, lIsSmoothJoin);

                    lContext.Close();
                }

                return lGeometry;
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Delegate called when the corner raduis changed.
        /// </summary>
        /// <param name="pObject">The modified control.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnCornerRadiusChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
        {   
            NodeViewShape lRenderer = pObject as NodeViewShape;
            if (lRenderer != null)
            {
                lRenderer.InvalidateVisual();
            }
        }

        /// <summary>
        /// Delegate called when the arrow height changed.
        /// </summary>
        /// <param name="pObject">The modified control.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnArrowHeightChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
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
