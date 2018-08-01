using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using XGraph.ViewModels;
using XGraph.Extensions;
using System.Windows.Documents;

namespace XGraph.Controls
{
    /// <summary>
    /// Class used as proxy to add adorning infomations to the <see cref="NodeView"/> using XAML.
    /// </summary>
    /// <!-- Damien Porte -->
    public class AdornedInfosPresenter : ContentControl
    {
        #region Methods

        /// <summary>
        /// Delegate called when the control is initialized.
        /// </summary>
        /// <param name="pEventArgs">The event arguments.</param>
        protected override void OnInitialized(EventArgs pEventArgs)
        {
            NodeViewModel lNodeViewModel = this.DataContext as NodeViewModel;
            if (lNodeViewModel == null)
            {
                return;
            }

            SimpleGraphView lParentGraphView = this.FindVisualParent<SimpleGraphView>();
            if (lParentGraphView == null)
            {
                return;
            }

            NodeView lNodeView = lParentGraphView.GetContainerForViewModel<NodeViewModel, NodeView>(lNodeViewModel);
            if (lNodeView != null)
            {
                AdornerLayeredCanvas lCanvas = this.FindVisualParent<AdornerLayeredCanvas>();
                if (lCanvas != null)
                {
                    // Creating the adorner layer.
                    AdornerLayer lLayer = lCanvas.AdornerLayer;

                    // Adding the adorner to the layer.
                    lLayer.Add(new InfosAdorner(lNodeView));
                }
            }
        }

        #endregion // Methods.
    }
}
