using System.Windows;
using System.Windows.Controls;
using XGraph.ViewModels;

namespace XGraph.Controls
{
    /// <summary>
    /// This class stores all the ports.
    /// </summary>
    public class PortContainer : ItemsControl
    {
        #region Constructors

        /// <summary>
        /// Initializes the <see cref="PortContainer"/> class.
        /// </summary>
        static PortContainer()
        {
            PortContainer.DefaultStyleKeyProperty.OverrideMetadata(typeof(PortContainer), new FrameworkPropertyMetadata(typeof(PortContainer)));
            PortContainer.BackgroundProperty.OverrideMetadata(typeof(PortContainer), new FrameworkPropertyMetadata(null, OnBackgroundChanged));
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Creates or identifies the element that is used to display the given item.
        /// </summary>
        /// <returns>
        /// The element that is used to display the given item.
        /// </returns>
        protected override System.Windows.DependencyObject GetContainerForItemOverride()
        {
            return new PortView();
        }

        /// <summary>
        /// Prespares the container for the given item.
        /// </summary>
        /// <param name="pElement">The item container.</param>
        /// <param name="pItem">The contained item.</param>
        protected override void PrepareContainerForItemOverride(DependencyObject pElement, object pItem)
        {
            base.PrepareContainerForItemOverride(pElement, pItem);

            PortView lContainer = pElement as PortView;
            if (lContainer != null)
            {
                // Updating the background.
                lContainer.Background = this.Background;
            }
        }

        /// <summary>
        /// Updates the items background brush by applying the background of this container.
        /// </summary>
        private void UpdateItemsBackground()
        {
            if (this.ItemsSource != null)
            {
                foreach (PortViewModel lItem in this.ItemsSource)
                {
                    PortView lPortView = this.ItemContainerGenerator.ContainerFromItem(lItem) as PortView;
                    if (lPortView != null)
                    {
                        lPortView.Background = this.Background;
                    }
                }
            }
        }

        /// <summary>
        /// Delegate called when the background brush changed.
        /// </summary>
        /// <param name="pObject">The modified control.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private static void OnBackgroundChanged(DependencyObject pObject, DependencyPropertyChangedEventArgs pEventArgs)
        {
            PortContainer lContainer = pObject as PortContainer;
            if (lContainer != null)
            {
                lContainer.UpdateItemsBackground();
            }
        }

        #endregion // Methods.
    }
}
