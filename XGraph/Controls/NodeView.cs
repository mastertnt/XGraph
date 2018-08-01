using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using XGraph.ViewModels;

namespace XGraph.Controls
{
    /// <summary>
    /// Class defining a node in the graph view.
    /// </summary>
    [TemplatePart(Name = PART_PORT_CONTAINER, Type = typeof(PortContainer))]
    public class NodeView : AGraphItem
    {
        #region Dependencies

        /// <summary>
        /// Identifies the IsActive dependency property.
        /// </summary>
        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(NodeView), new FrameworkPropertyMetadata(false));

        /// <summary>
        /// Identifies the HeaderBackground dependency property.
        /// </summary>
        public static readonly DependencyProperty HeaderBackgroundProperty = DependencyProperty.Register("HeaderBackground", typeof(Brush), typeof(NodeView), new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Identifies the Warnings dependency property.
        /// </summary>
        public static readonly DependencyProperty WarningsProperty = DependencyProperty.Register("Warnings", typeof(object), typeof(NodeView), new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Identifies the Errors dependency property.
        /// </summary>
        public static readonly DependencyProperty ErrorsProperty = DependencyProperty.Register("Errors", typeof(object), typeof(NodeView), new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Identifies the WarningsDataTemplate dependency property.
        /// </summary>
        public static readonly DependencyProperty WarningsDataTemplateProperty = DependencyProperty.Register("WarningsDataTemplate", typeof(DataTemplate), typeof(NodeView), new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Identifies the ErrorsDataTemplate dependency property.
        /// </summary>
        public static readonly DependencyProperty ErrorsDataTemplateProperty = DependencyProperty.Register("ErrorsDataTemplate", typeof(DataTemplate), typeof(NodeView), new FrameworkPropertyMetadata(null));

        #endregion // Dependencies.

        #region Fields

        /// <summary>
        /// Name of the parts that have to be in the control template.
        /// </summary>
        private const string PART_PORT_CONTAINER = "PART_PortContainer";

        /// <summary>
        /// Stores the inner port container
        /// </summary>
        private PortContainer mInnerPortContainer;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="NodeView"/> class.
        /// </summary>
        static NodeView()
        {
            NodeView.DefaultStyleKeyProperty.OverrideMetadata(typeof(NodeView), new FrameworkPropertyMetadata(typeof(NodeView)));
        }

        /// <summary>
        /// Initializes an instance of the <see cref="NodeView"/> class.
        /// </summary>
        public NodeView()
        {
            this.mInnerPortContainer = null;
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets or sets the flag indicating if the node is active.
        /// </summary>
        public bool IsActive
        {
            get
            {
                return (bool)this.GetValue(IsActiveProperty);
            }
            set
            {
                this.SetValue(IsActiveProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the brush to use as background in the header if the node has one.
        /// </summary>
        public Brush HeaderBackground
        {
            get
            {
                return (Brush)this.GetValue(HeaderBackgroundProperty);
            }
            set
            {
                this.SetValue(HeaderBackgroundProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the warnings list.
        /// </summary>
        public object Warnings
        {
            get
            {
                return this.GetValue(WarningsProperty);
            }
            set
            {
                this.SetValue(WarningsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the errors list.
        /// </summary>
        public object Errors
        {
            get
            {
                return this.GetValue(ErrorsProperty);
            }
            set
            {
                this.SetValue(ErrorsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the warnings list.
        /// </summary>
        public DataTemplate WarningsDataTemplate
        {
            get
            {
                return (DataTemplate)this.GetValue(WarningsProperty);
            }
            set
            {
                this.SetValue(WarningsProperty, value);
            }
        }

        /// <summary>
        /// Gets or sets the errors list.
        /// </summary>
        public DataTemplate ErrorsDataTemplate
        {
            get
            {
                return (DataTemplate)this.GetValue(ErrorsProperty);
            }
            set
            {
                this.SetValue(ErrorsProperty, value);
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Method called when the control content changed.
        /// </summary>
        /// <param name="pOldContent">The previous content.</param>
        /// <param name="pNewContent">The new content.</param>
        protected override void OnContentChanged(object pOldContent, object pNewContent)
        {
            base.OnContentChanged(pOldContent, pNewContent);

            BindingOperations.ClearAllBindings(this);

            // The content is the view model.
            NodeViewModel lNewContent = pNewContent as NodeViewModel;
            if (lNewContent != null)
            {
                // Setting the content data template.
                this.ContentTemplate = lNewContent.DataTemplate;

                // Binding the IsActive property.
                Binding lIsActiveBinding = new Binding("IsActive");
                lIsActiveBinding.Source = lNewContent;
                lIsActiveBinding.Mode = BindingMode.TwoWay;
                this.SetBinding(NodeView.IsActiveProperty, lIsActiveBinding);

                // Binding the Warnings property.
                Binding lWarningsBinding = new Binding("Warnings");
                lWarningsBinding.Source = lNewContent;
                lWarningsBinding.Mode = BindingMode.TwoWay;
                this.SetBinding(NodeView.WarningsProperty, lWarningsBinding);

                // Binding the Errors property.
                Binding lErrorsBinding = new Binding("Errors");
                lErrorsBinding.Source = lNewContent;
                lErrorsBinding.Mode = BindingMode.TwoWay;
                this.SetBinding(NodeView.ErrorsProperty, lErrorsBinding);

                // Binding the WarningsDataTemplate property.
                Binding lWarningsDataTemplateBinding = new Binding("WarningsDataTemplate");
                lWarningsDataTemplateBinding.Source = lNewContent;
                lWarningsDataTemplateBinding.Mode = BindingMode.OneWay;
                this.SetBinding(NodeView.WarningsDataTemplateProperty, lWarningsDataTemplateBinding);

                // Binding the ErrorsDataTemplate property.
                Binding lErrorsDataTemplateBinding = new Binding("ErrorsDataTemplate");
                lErrorsDataTemplateBinding.Source = lNewContent;
                lErrorsDataTemplateBinding.Mode = BindingMode.OneWay;
                this.SetBinding(NodeView.ErrorsDataTemplateProperty, lErrorsDataTemplateBinding);
            }
        }

        /// <summary>
        /// Method called when the control template is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Getting the parts of the control.
            this.mInnerPortContainer = this.GetTemplateChild(PART_PORT_CONTAINER) as PortContainer;

            if (this.mInnerPortContainer == null)
            {
                throw new Exception("NodeView control template not correctly defined.");
            }

            // Binding the Ports property.
            Binding lNodesBinding = new Binding("Ports");
            lNodesBinding.Source = this.Content;
            lNodesBinding.Mode = BindingMode.OneWay;
            this.mInnerPortContainer.SetBinding(ItemsControl.ItemsSourceProperty, lNodesBinding);
        }
        
        /// <summary>
        /// Returns the node view containing the given view model.
        /// </summary>
        /// <param name="pItem">The item contained by the view.</param>
        /// <returns>The found view if any, null otherwise.</returns>
        public PortView GetContainerForPortViewModel(PortViewModel pItem)
        {
            return this.mInnerPortContainer.ItemContainerGenerator.ContainerFromItem(pItem) as PortView;
        }

        #endregion // Methods.
    }
}
