using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XGraph.Controls;
using XGraph.ViewModels;
using XGraph.Extensions;
using System.Windows;

namespace XGraph.Controls
{
    /// <summary>
    /// Class defining the simple connector presenter adding the connector in the same visual tree.
    /// </summary>
    [TemplatePart(Name = PART_CONNECTOR, Type = typeof(AConnector))]
    public class ConnectorPresenter : AConnectorPresenter
    {
        #region Dependencies

        /// <summary>
        /// Identifies the PortDirection dependency property.
        /// </summary>
        public static readonly DependencyProperty PortDirectionProperty = DependencyProperty.Register("PortDirection", typeof(PortDirection), typeof(ConnectorPresenter), new FrameworkPropertyMetadata(PortDirection.Output));

        #endregion // Dependencies.

        #region Fields

        /// <summary>
        /// Name of the parts that have to be in the control template.
        /// </summary>
        private const string PART_CONNECTOR = "PART_Connector";

        /// <summary>
        /// Gets the connector.
        /// </summary>
        private AConnector mConnector;

        #endregion // Fields.

        #region Properties

        /// <summary>
        /// Gets or sets direction.
        /// </summary>
        public PortDirection PortDirection
        {
            get
            {
                return (PortDirection)this.GetValue(PortDirectionProperty);
            }
            set
            {
                this.SetValue(PortDirectionProperty, value);
            }
        }

        /// <summary>
        /// Gets the connector.
        /// </summary>
        public override AConnector Connector
        {
            get
            {
                return this.mConnector;
            }
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes an instance of the <see cref="PortContainer"/> class.
        /// </summary>
        public ConnectorPresenter()
        {
            this.mConnector = null;
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Method called when the control template is applied.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // Getting the parts of the control.
            this.mConnector = this.GetTemplateChild(PART_CONNECTOR) as AConnector;

            if (this.mConnector == null)
            {
                throw new Exception("ConnectorPresenter control template not correctly defined.");
            }

            this.NotifiyTemplateApplied();
        }

        #endregion // Methods.
    }
}
