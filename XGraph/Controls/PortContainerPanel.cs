using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using XGraph.ViewModels;

namespace XGraph.Controls
{
    /// <summary>
    /// Panel used to organize the <see cref="PortView"/> elements in the <see cref="PortContainer"/> in two columns.
    /// </summary>
    /// <remarks>Left column contains the input ports, right column contains the output ports.</remarks>
    /// <!-- Damien Porte -->
    public class PortContainerPanel : Grid
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PortContainerPanel"/> class.
        /// </summary>
        public PortContainerPanel()
        {
            // Setting two columns to the panel so that the width computed for each port view in the base.ArrangeOverride() method is coherent.
            this.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            this.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Computes the size of the panel viewport.
        /// </summary>
        /// <param name="pAvailableSize">The initial available size.</param>
        /// <returns>The viewport desired size.</returns>
        protected override Size MeasureOverride(Size pAvailableSize)
        {
            Size lColumnSize = base.MeasureOverride(pAvailableSize);

            Size lInputPortsSize = new Size();
            Size lOutputPortsSize = new Size();

            // Iterating threw the ports to have the highest column.
            for (int i = 0, count = this.InternalChildren.Count; i < count; ++i)
            {
                PortView lPort = this.InternalChildren[i] as PortView;
                if (lPort != null)
                {
                    lPort.Measure(pAvailableSize);

                    if (lPort.Direction == PortDirection.Input)
                    {
                        lInputPortsSize.Height += lPort.DesiredSize.Height;
                        lInputPortsSize.Width = Math.Max(lInputPortsSize.Width, lColumnSize.Width);
                    }
                    else
                    {
                        lOutputPortsSize.Height += lPort.DesiredSize.Height;
                        lOutputPortsSize.Width = Math.Max(lOutputPortsSize.Width, lColumnSize.Width);
                    }
                }
            }

            // Computing the size of the panel.
            Size lAvailableSize = new Size
            {
                Height = Math.Max(lInputPortsSize.Height, lOutputPortsSize.Height),
                Width = lInputPortsSize.Width + lOutputPortsSize.Width
            };

            // This is the minimum size the control must have.
            return lAvailableSize;
        }

        /// <summary>
        /// Arranges the port views by taking in account the computed size of the panel viewport.
        /// </summary>
        /// <param name="pFinalSize">The available size.</param>
        /// <returns>The size used (here equals to the available size).</returns>
        protected override Size ArrangeOverride(Size pFinalSize)
        {
            // (1) Calling this base method is the key for the width of each port view to be well computed !!!
            base.ArrangeOverride(pFinalSize);

            int lInputPortsCount = 0;
            int lOutputPortsCount = 0;

            if (this.InternalChildren.Count > 0)
            {
                // Using the ActualWidth computed by the base arrange (1) method to have the goog column width.
                double lInputPortsWidth = this.InternalChildren.Cast<PortView>().Where(pPort => pPort.Direction == PortDirection.Input).Max(pPort => pPort.ActualWidth);
                lInputPortsWidth = Math.Round(lInputPortsWidth);
                double lOutputPortsWidth = this.InternalChildren.Cast<PortView>().Where(pPort => pPort.Direction == PortDirection.Output).Max(pPort => pPort.ActualWidth);
                lOutputPortsWidth = Math.Round(lOutputPortsWidth);

                for (int i = 0, lCount = this.InternalChildren.Count; i < lCount; ++i)
                {
                    PortView lPort = this.InternalChildren[i] as PortView;
                    if (lPort != null)
                    {
                        if (lPort.Direction == PortDirection.Input)
                        {
                            // Input ports are in the first column.
                            lPort.Arrange(new Rect(0.0, lInputPortsCount * lPort.DesiredSize.Height, lInputPortsWidth, lPort.DesiredSize.Height));
                            lInputPortsCount++;
                        }
                        else
                        {
                            // Output ports are in the second column.
                            lPort.Arrange(new Rect(lInputPortsWidth, lOutputPortsCount * lPort.DesiredSize.Height, lOutputPortsWidth, lPort.DesiredSize.Height));
                            lOutputPortsCount++;
                        }
                    }
                }
            }

            return pFinalSize;
        }

        #endregion // Methods.
    }
}
