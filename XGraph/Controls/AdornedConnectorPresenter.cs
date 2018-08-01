using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using XGraph.Extensions;
using XGraph.ViewModels;

namespace XGraph.Controls
{
    /// <summary>
    /// Class used as proxy to add adorning connectors to the <see cref="PortView"/> using XAML.
    /// </summary>
    /// <!-- Damien Porte -->
    public class AdornedConnectorPresenter : AConnectorPresenter
    {
        #region Fields

        /// <summary>
        /// Stores the presenter adorner.
        /// </summary>
        private ConnectorAdorner mAdorner;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes the <see cref="AdornedConnectorPresenter"/> class.
        /// </summary>
        static AdornedConnectorPresenter()
        {
            Control.BackgroundProperty.OverrideMetadata(typeof(AdornedConnectorPresenter), new FrameworkPropertyMetadata(null, OnBackgroundChanged));
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets the connector.
        /// </summary>
        public override AConnector Connector
        {
            get
            {
                return this.mAdorner.Connector;
            }
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Delegate called when the control is initialized.
        /// </summary>
        /// <param name="pEventArgs">The event arguments.</param>
        protected override void OnInitialized(EventArgs pEventArgs)
        {
            PortViewModel lPortViewModel = this.DataContext as PortViewModel;
            if (lPortViewModel == null)
            {
                return;
            }

            PortContainer lParentContainer = this.FindVisualParent<PortContainer>();
            if (lParentContainer == null)
            {
                return;
            }

            PortView lPortView = lParentContainer.GetContainerForViewModel(lPortViewModel);
            if (lPortView != null)
            {
                AdornerLayeredCanvas lCanvas = this.FindVisualParent<AdornerLayeredCanvas>();
                if (lCanvas != null)
                {
                    // Creating the adorner layer.
                    AdornerLayer lLayer = lCanvas.AdornerLayer;

                    // Creating the adorner and propagating this control background.
                    this.mAdorner = new ConnectorAdorner(lPortView);
                    this.UpdateConnectorsBackground();

                    // Adding the adorner to the layer.
                    lLayer.Add(this.mAdorner);
                }
            }
        }

        /// <summary>
        /// Updates the connectors background brush by applying the background of this container.
        /// </summary>
        private void UpdateConnectorsBackground()
        {
            if (this.mAdorner != null)
            {
                this.mAdorner.Connector.Background = this.Background;
            }
        }

        /// <summary>
        /// Delegate called when the background brush changed.
        /// </summary>
        /// <param name="pObject">The modified control.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnBackgroundChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
        {
            AdornedConnectorPresenter lContainer = pObject as AdornedConnectorPresenter;
            if (lContainer != null)
            {
                lContainer.UpdateConnectorsBackground();
            }
        }

        #endregion // Methods.
    }
}
