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
    public class NodeView : AGraphItemContainer
    {
        #region Dependencies

        /// <summary>
        /// Identifies the IsActive dependency property.
        /// </summary>
        public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register("IsActive", typeof(bool), typeof(NodeView), new FrameworkPropertyMetadata(false, OnIsActiveChanged));

        /// <summary>
        /// Identifies the IsActiveBackground dependency property.
        /// </summary>
        public static readonly DependencyProperty IsActiveBackgroundProperty = DependencyProperty.Register("IsActiveBackground", typeof(Brush), typeof(NodeView), new FrameworkPropertyMetadata(null));

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

        /// <summary>
        /// Stores the backup default background brush.
        /// </summary>
        private Brush mBackgroundBackup;

        /// <summary>
        /// Stores the flag indicating if the 
        /// </summary>
        private bool mIsBackgroundBackupUpdateLocked;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="NodeView"/> class.
        /// </summary>
        static NodeView()
        {
            NodeView.DefaultStyleKeyProperty.OverrideMetadata(typeof(NodeView), new FrameworkPropertyMetadata(typeof(NodeView)));
            NodeView.BackgroundProperty.OverrideMetadata(typeof(NodeView), new FrameworkPropertyMetadata(null, OnBackgroundChanged));
        }

        /// <summary>
        /// Initializes an instance of the <see cref="NodeView"/> class.
        /// </summary>
        public NodeView()
        {
            this.mInnerPortContainer = null;
            this.mBackgroundBackup = null;
            this.mIsBackgroundBackupUpdateLocked = false;
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
        /// Gets or sets the brush to use as background when the node is active.
        /// </summary>
        public Brush IsActiveBackground
        {
            get
            {
                return (Brush)this.GetValue(IsActiveBackgroundProperty);
            }
            set
            {
                this.SetValue(IsActiveBackgroundProperty, value);
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

        /// <summary>
        /// Delegate called when the selection state changed.
        /// </summary>
        /// <param name="pObject">The modified control.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnIsActiveChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
        {
            NodeView lNodeView = pObject as NodeView;
            if (lNodeView != null)
            {
                lNodeView.UpdateVisualState();
            }
        }

        /// <summary>
        /// Updates the visual state of the node.
        /// </summary>
        protected override void UpdateVisualState()
        {
            base.UpdateVisualState();

            // The active state only affect the background color.
            if (this.IsActiveBackground != null)
            {
                if (this.IsActive)
                {
                    this.mIsBackgroundBackupUpdateLocked = true;
                    this.Background = this.IsActiveBackground;
                    this.mIsBackgroundBackupUpdateLocked = false;
                }
                else
                {
                    this.Background = this.mBackgroundBackup;
                }
            }
        }

        /// <summary>
        /// Tries to backup the backgroud.
        /// </summary>
        private void TryToBackupBackground()
        {
            if (this.mIsBackgroundBackupUpdateLocked == false)
            {
                this.mBackgroundBackup = this.Background;
            }
        }

        /// <summary>
        /// Delegate called when the background brush changed.
        /// </summary>
        /// <param name="pObject">The modified control.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnBackgroundChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
        {
            NodeView lContainer = pObject as NodeView;
            if (lContainer != null)
            {
                lContainer.TryToBackupBackground();
            }
        }

        #endregion // Methods.
    }
}
