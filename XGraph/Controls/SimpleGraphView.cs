using System;
using System.Windows;
using System.Windows.Controls;
using XGraph.Behaviors;
using XGraph.ViewModels;

namespace XGraph.Controls
{
    /// <summary>
    /// Graph view is a list box : it then already handle the selection ... etc ...
    /// </summary>
    [TemplatePart(Name = PART_SELECTION_BOX_CANVAS, Type = typeof(Canvas))]
    public class SimpleGraphView : ListBox
    {
        #region Dependencies

        /// <summary>
        /// Identifies the IsReadOnly dependency property.
        /// </summary>
        public static readonly DependencyProperty IsReadOnlyProperty = DependencyProperty.Register("IsReadOnly", typeof(bool), typeof(SimpleGraphView), new FrameworkPropertyMetadata(false, OnIsReadOnlyChanged));

        #endregion // Dependencies.

        #region Fields

        /// <summary>
        /// Stores the box selection drag threshold.
        /// </summary>
        private const double BOX_SELECTION_DRAG_THRESHOLD = 2.0;

        /// <summary>
        /// Name of the parts that have to be in the control template.
        /// </summary>
        private const string PART_SELECTION_BOX_CANVAS = "PART_SelectionBoxCanvas";

        /// <summary>
        /// Stores the behavior handling the selection using a box in the view.
        /// </summary>
        private BoxSelectionBehavior mBoxSelectionBehavior;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="SimpleGraphView"/> class.
        /// </summary>
        static SimpleGraphView()
        {
            SimpleGraphView.DefaultStyleKeyProperty.OverrideMetadata(typeof(SimpleGraphView), new FrameworkPropertyMetadata(typeof(SimpleGraphView)));
            ListBox.SelectionModeProperty.OverrideMetadata(typeof(SimpleGraphView), new FrameworkPropertyMetadata(SelectionMode.Extended));
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets or sets the flag indicating if the view is read  only.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return (bool)this.GetValue(IsReadOnlyProperty);
            }
            set
            {
                this.SetValue(IsReadOnlyProperty, value);
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Creates or identifies the element used to display a specified item.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Windows.Controls.ListBoxItem" />.
        /// </returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new GraphItem();
        }

        /// <summary>
        /// Returns the node view containing the given view model.
        /// </summary>
        /// <param name="pItem">The item contained by the view.</param>
        /// <returns>The found view if any, null otherwise.</returns>
        public AGraphItemContainer GetContainerForViewModel(IGraphItemViewModel pItem)
        {
            GraphItem lItemView = this.ItemContainerGenerator.ContainerFromItem(pItem) as GraphItem;
            if (lItemView != null)
            {
                return lItemView.TemplateControl;
            }

            return null;
        }

        /// <summary>
        /// Returns the node view containing the given view model.
        /// </summary>
        /// <param name="pItem">The item contained by the view.</param>
        /// <returns>The found view if any, null otherwise.</returns>
        public TContainer GetContainerForViewModel<TViewModel, TContainer>(TViewModel pItem) 
            where TViewModel : IGraphItemViewModel
            where TContainer : AGraphItemContainer
        {
            return this.GetContainerForViewModel(pItem) as TContainer;
        }

        /// <summary>
        /// Method called when the control template is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Getting the parts of the control.
            Canvas lSelectionBoxCanvas = this.GetTemplateChild(PART_SELECTION_BOX_CANVAS) as Canvas;

            if (lSelectionBoxCanvas == null)
            {
                throw new Exception("GraphView control template not correctly defined.");
            }

            // Initializing the box selection behavior.
            this.mBoxSelectionBehavior = new BoxSelectionBehavior(this, lSelectionBoxCanvas);
            this.mBoxSelectionBehavior.DragThreshold = BOX_SELECTION_DRAG_THRESHOLD;

            // Updating the visual.
            this.UpdateVisualState();
        }

        /// <summary>
        /// Delegate called when the read only state changed.
        /// </summary>
        /// <param name="pObject">The modified control.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnIsReadOnlyChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
        {
            SimpleGraphView lGraphView = pObject as SimpleGraphView;
            if (lGraphView != null)
            {
                lGraphView.UpdateVisualState();
            }
        }

        /// <summary>
        /// Change the visual state according to the control state.
        /// </summary>
        private void UpdateVisualState()
        {
            if (this.IsReadOnly)
            {
                VisualStateManager.GoToState(this, "IsReadOnly", true);
            }
            else
            {
                VisualStateManager.GoToState(this, "Normal", true);
            }
        }

        #endregion // Methods
    }
}
