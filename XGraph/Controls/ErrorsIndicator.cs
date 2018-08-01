using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using XGraph.Converters;

namespace XGraph.Controls
{
    /// <summary>
    /// Indicator displaying the node errors.
    /// </summary>
    public class ErrorsIndicator : Control
    {
        #region Dependencies

        /// <summary>
        /// Identifies the Errors dependency property.
        /// </summary>
        public static readonly DependencyProperty ErrorsProperty = DependencyProperty.Register("Errors", typeof(object), typeof(ErrorsIndicator), new FrameworkPropertyMetadata(null));

        /// <summary>
        /// Identifies the ErrorsDataTemplate dependency property.
        /// </summary>
        public static readonly DependencyProperty ErrorsDataTemplateProperty = DependencyProperty.Register("ErrorsDataTemplate", typeof(DataTemplate), typeof(ErrorsIndicator), new FrameworkPropertyMetadata(null));

        #endregion // Dependencies.

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="ErrorsIndicator"/> class.
        /// </summary>
        static ErrorsIndicator()
        {
            ErrorsIndicator.DefaultStyleKeyProperty.OverrideMetadata(typeof(ErrorsIndicator), new FrameworkPropertyMetadata(typeof(ErrorsIndicator)));
        }

        /// <summary>
        /// Initializes an instance of the <see cref="ErrorsIndicator"/> class.
        /// </summary>
        /// <param name="pParentNode">The parent node.</param>
        public ErrorsIndicator(NodeView pParentNode)
        {
            // Binding the Errors property.
            Binding lErrorsBinding = new Binding("Errors");
            lErrorsBinding.Source = pParentNode;
            lErrorsBinding.Mode = BindingMode.OneWay;
            this.SetBinding(ErrorsIndicator.ErrorsProperty, lErrorsBinding);

            // Binding the Visibility property.
            Binding lVisibilityBinding = new Binding("Errors");
            lVisibilityBinding.Source = pParentNode;
            lVisibilityBinding.Mode = BindingMode.OneWay;
            lVisibilityBinding.Converter = new InfosTooltipContentToVisibilityConverter();
            this.SetBinding(WarningsIndicator.VisibilityProperty, lVisibilityBinding);

            // Binding the ErrorsDataTemplate property.
            Binding lErrorsDataTemplateBinding = new Binding("ErrorsDataTemplate");
            lErrorsDataTemplateBinding.Source = pParentNode;
            lErrorsDataTemplateBinding.Mode = BindingMode.OneWay;
            this.SetBinding(ErrorsIndicator.ErrorsDataTemplateProperty, lErrorsDataTemplateBinding);
        }

        #endregion // Constructors.

        #region Properties

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
    }
}
