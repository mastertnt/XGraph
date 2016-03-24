using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using XGraph.Extensions;

namespace XGraph.Controls
{
    /// <summary>
    /// Class used as proxy to add adorning connectors to the <see cref="PortView"/> using XAML.
    /// </summary>
    /// <!-- Damien Porte -->
    public class ConnectorsPresenter : Control
    {
        #region Constructors

        /// <summary>
        /// Initializes the <see cref="PortContainer"/> class.
        /// </summary>
        static ConnectorsPresenter()
        {
            Control.BackgroundProperty.OverrideMetadata(typeof(ConnectorsPresenter), new FrameworkPropertyMetadata(null, OnBackgroundChanged));
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets the presenter adorner.
        /// </summary>
        public ConnectorsAdorner Adorner
        {
            get;
            private set;
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Delegate called when the control is initialized.
        /// </summary>
        /// <param name="pEventArgs">The event arguments.</param>
        protected override void OnInitialized(EventArgs pEventArgs)
        {
            // The adorned port view is given to the control by the DataContext property.
            PortView lPortView = this.DataContext as PortView;
            if (lPortView != null)
            {
                AdornerLayeredCanvas lCanvas = this.FindVisualParent<AdornerLayeredCanvas>();
                if (lCanvas != null)
                {
                    // Creating the adorner layer.
                    AdornerLayer lLayer = lCanvas.AdornerLayer;

                    // Creating the adorner and propagating this control background.
                    this.Adorner = new ConnectorsAdorner(lPortView);
                    this.UpdateConnectorsBackground();

                    // Adding the adorner to the layer.
                    lLayer.Add(this.Adorner);
                }
            }
        }

        /// <summary>
        /// Updates the connectors background brush by applying the background of this container.
        /// </summary>
        private void UpdateConnectorsBackground()
        {
            if (this.Adorner != null)
            {
                this.Adorner.InputConnector.Background = this.Background;
                this.Adorner.OutputConnector.Background = this.Background;
            }
        }

        /// <summary>
        /// Delegate called when the background brush changed.
        /// </summary>
        /// <param name="pObject">The modified control.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnBackgroundChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
        {
            ConnectorsPresenter lContainer = pObject as ConnectorsPresenter;
            if (lContainer != null)
            {
                lContainer.UpdateConnectorsBackground();
            }
        }

        #endregion // Methods.
    }
}
