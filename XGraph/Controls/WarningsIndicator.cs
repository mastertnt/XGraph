using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using XGraph.Converters;

namespace XGraph.Controls
{
    /// <summary>
    /// Indicator displaying the node warnings.
    /// </summary>
    public class WarningsIndicator : Control
    {
        #region Dependencies

        /// <summary>
        /// Identifies the Warnings dependency property.
        /// </summary>
        public static readonly DependencyProperty WarningsProperty = DependencyProperty.Register("Warnings", typeof(object), typeof(WarningsIndicator), new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Identifies the WarningsDataTemplate dependency property.
        /// </summary>
        public static readonly DependencyProperty WarningsDataTemplateProperty = DependencyProperty.Register("WarningsDataTemplate", typeof(DataTemplate), typeof(WarningsIndicator), new FrameworkPropertyMetadata(null));

        #endregion // Dependencies.

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="WarningsIndicator"/> class.
        /// </summary>
        static WarningsIndicator()
        {
            WarningsIndicator.DefaultStyleKeyProperty.OverrideMetadata(typeof(WarningsIndicator), new FrameworkPropertyMetadata(typeof(WarningsIndicator)));
        }

        /// <summary>
        /// Initializes an instance of the <see cref="WarningsIndicator"/> class.
        /// </summary>
        /// <param name="pParentNode">The parent node.</param>
        public WarningsIndicator(NodeView pParentNode)
        {
            // Binding the Warnings property.
            Binding lWarningsBinding = new Binding("Warnings");
            lWarningsBinding.Source = pParentNode;
            lWarningsBinding.Mode = BindingMode.OneWay;
            this.SetBinding(WarningsIndicator.WarningsProperty, lWarningsBinding);

            // Binding the Visibility property.
            Binding lVisibilityBinding = new Binding("Warnings");
            lVisibilityBinding.Source = pParentNode;
            lVisibilityBinding.Mode = BindingMode.OneWay;
            lVisibilityBinding.Converter = new InfosTooltipContentToVisibilityConverter();
            this.SetBinding(WarningsIndicator.VisibilityProperty, lVisibilityBinding);

            // Binding the WarningsDataTemplate property.
            Binding lWarningsDataTemplateBinding = new Binding("WarningsDataTemplate");
            lWarningsDataTemplateBinding.Source = pParentNode;
            lWarningsDataTemplateBinding.Mode = BindingMode.OneWay;
            this.SetBinding(WarningsIndicator.WarningsDataTemplateProperty, lWarningsDataTemplateBinding);
        }
        
        #endregion // Constructors.

        #region Properties

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

        #endregion // Properties.
    }
}
