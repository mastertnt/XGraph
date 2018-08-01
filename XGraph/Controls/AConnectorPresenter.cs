using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using XGraph.Extensions;

namespace XGraph.Controls
{
    /// <summary>
    /// Class used as proxy to add connectors to the <see cref="PortView"/> using XAML.
    /// </summary>
    /// <!-- Damien Porte -->
    public abstract class AConnectorPresenter : Control
    {
        #region Constructors

        /// <summary>
        /// Initializes the <see cref="AConnectorPresenter"/> class.
        /// </summary>
        protected AConnectorPresenter()
        {
        }

        #endregion // Constructors.

        #region Events

        /// <summary>
        /// Event raised when the control template is applied.
        /// </summary>
        public Action TemplateApplied;

        #endregion // Events.

        #region Properties

        /// <summary>
        /// Gets the connector.
        /// </summary>
        public abstract AConnector Connector
        {
            get;
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Notifies the controle template application.
        /// </summary>
        protected void NotifiyTemplateApplied()
        {
            if (this.TemplateApplied != null)
            {
                this.TemplateApplied();
            }
        }

        #endregion // Methods.
    }
}
