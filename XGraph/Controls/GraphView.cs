using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using XGraph.ViewModels;

namespace XGraph.Controls
{
    /// <summary>
    /// Class defining a graph view having the zoom and pan capability.
    /// </summary>
    public class GraphView : Control
    {
        #region Dependencies

        /// <summary>
        /// Identifies the GraphWidth dependency property.
        /// </summary>
        public static readonly DependencyProperty GraphWidthProperty = DependencyProperty.Register("GraphWidth", typeof(double), typeof(GraphView), new FrameworkPropertyMetadata(2000.0));

        /// <summary>
        /// Identifies the GraphHeight dependency property.
        /// </summary>
        public static readonly DependencyProperty GraphHeightProperty = DependencyProperty.Register("GraphHeight", typeof(double), typeof(GraphView), new FrameworkPropertyMetadata(1500.0));
        
        /// <summary>
        /// Identifies the OverviewWidth dependency property.
        /// </summary>
        public static readonly DependencyProperty OverviewWidthProperty = DependencyProperty.Register("OverviewWidth", typeof(double), typeof(GraphView), new FrameworkPropertyMetadata(200.0));

        /// <summary>
        /// Identifies the OverviewHeight dependency property.
        /// </summary>
        public static readonly DependencyProperty OverviewHeightProperty = DependencyProperty.Register("OverviewHeight", typeof(double), typeof(GraphView), new FrameworkPropertyMetadata(200.0));

        /// <summary>
        /// Identifies the OverviewDefaultOpacity attached dependency property.
        /// </summary>
        public static readonly DependencyProperty OverviewDefaultOpacityProperty = DependencyProperty.RegisterAttached("OverviewDefaultOpacity", typeof(double), typeof(GraphView), new FrameworkPropertyMetadata(0.3));

        /// <summary>
        /// Identifies the OverviewVisibility attached dependency property.
        /// </summary>
        public static readonly DependencyProperty OverviewVisibilityProperty = DependencyProperty.RegisterAttached("OverviewVisibility", typeof(Visibility), typeof(GraphView), new FrameworkPropertyMetadata(Visibility.Visible));

        #endregion // Dependencies.

        #region Fields

        /// <summary>
        /// Stores the box selection drag threshold.
        /// </summary>
        private const double BOX_SELECTION_DRAG_THRESHOLD = 2.0;

        /// <summary>
        /// Name of the parts that have to be in the control template.
        /// </summary>
        private const string PART_SIMPLE_GRAPH_VIEW = "PART_SimpleGraphView";
        private const string PART_OVERVIEW = "PART_Overview";

        /// <summary>
        /// Stores the inner simple graph view.
        /// </summary>
        private SimpleGraphView mSimpleGraphView;

        /// <summary>
        /// Stores the overview.
        /// </summary>
        private SimpleGraphView mOverview;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="GraphView"/> class.
        /// </summary>
        static GraphView()
        {
            GraphView.DefaultStyleKeyProperty.OverrideMetadata(typeof(GraphView), new FrameworkPropertyMetadata(typeof(GraphView)));
        }

        #endregion // Constructors.

        #region Events

        /// <summary>
        /// Event raised when the selection is modified.
        /// </summary>
        public event SelectionChangedEventHandler SelectionChanged;

        #endregion // Events.

        #region Properties

        /// <summary>
        /// Gets the selected items view model.
        /// </summary>
        public IGraphItemViewModel[] SelectedViewModels
        {
            get
            {
                if (this.mSimpleGraphView != null)
                {
                    return this.mSimpleGraphView.SelectedItems.Cast<IGraphItemViewModel>().ToArray();
                }

                return new IGraphItemViewModel[] { };
            }
        }


        /// <summary>
        /// Gets or sets the width of the content.
        /// </summary>
        public double GraphWidth
        {
            get
            {
                return (double)GetValue(GraphWidthProperty);
            }
            set
            {
                SetValue(GraphWidthProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the height of the content.
        /// </summary>
        public double GraphHeight
        {
            get
            {
                return (double)GetValue(GraphHeightProperty);
            }
            set
            {
                SetValue(GraphHeightProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the width of the overview.
        /// </summary>
        public double OverviewWidth
        {
            get
            {
                return (double)GetValue(OverviewWidthProperty);
            }
            set
            {
                SetValue(OverviewWidthProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the height of the overview.
        /// </summary>
        public double OverviewHeight
        {
            get
            {
                return (double)GetValue(OverviewHeightProperty);
            }
            set
            {
                SetValue(OverviewHeightProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the opacity of the overview when it is displayed and the mouse is not over.
        /// </summary>
        public double OverviewDefaultOpacity
        {
            get
            {
                return (double)GetValue(OverviewDefaultOpacityProperty);
            }
            set
            {
                SetValue(OverviewDefaultOpacityProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the overview visibility.
        /// </summary>
        public Visibility OverviewVisibility
        {
            get
            {
                return (Visibility)GetValue(OverviewVisibilityProperty);
            }
            set
            {
                SetValue(OverviewVisibilityProperty, value);
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Method called when the control template is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Getting the parts of the control.
            this.mSimpleGraphView = this.GetTemplateChild(PART_SIMPLE_GRAPH_VIEW) as SimpleGraphView;
            this.mOverview = this.GetTemplateChild(PART_OVERVIEW) as SimpleGraphView;

            if (this.mSimpleGraphView == null || this.mOverview == null)
            {
                throw new Exception("GraphView control template not correctly defined.");
            }

            this.mSimpleGraphView.SelectionChanged += this.OnSimpleGraphViewSelectionChanged;
        }

        /// <summary>
        /// Delegate called when the inner graph view selection changed.
        /// </summary>
        /// <param name="pSender">The modified control.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnSimpleGraphViewSelectionChanged(object pSender, SelectionChangedEventArgs pEventArgs)
        {
            if (this.SelectionChanged != null)
            {
                this.SelectionChanged(pSender, pEventArgs);
            }
        }

        #endregion // Methods.
    }
}
