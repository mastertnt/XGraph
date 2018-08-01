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
        #region Dependencies

        /// <summary>
        /// Identifies the LineWidth dependency property.
        /// </summary>
        public static readonly DependencyProperty LineWidthProperty = DependencyProperty.Register("LineWidth", typeof(double), typeof(PortContainerPanel), new FrameworkPropertyMetadata(1.0));

        #endregion // Dependencies.

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

        #region Properties

        /// <summary>
        /// Gets or sets the grid line width.
        /// </summary>
        public double LineWidth
        {
            get
            {
                return (double)this.GetValue(LineWidthProperty);
            }
            set
            {
                this.SetValue(LineWidthProperty, value);
            }
        }

        #endregion // Properties.

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

            int lInputPortsIndex = 0;
            int lOutputPortsIndex = 0;

            int lInputPortsCount = this.InternalChildren.Cast<PortView>().Where(lPort => lPort.Direction == PortDirection.Input).Count();
            int lOutputPortsCount = this.InternalChildren.Cast<PortView>().Where(lPort => lPort.Direction == PortDirection.Output).Count();

            // Width is the column width.
            lInputPortsSize.Width = lColumnSize.Width + this.LineWidth / 2.0;
            lOutputPortsSize.Width = lColumnSize.Width + this.LineWidth / 2.0;

            // Iterating threw the ports to compute the height.
            for (int i = 0, lCount = this.InternalChildren.Count; i < lCount; ++i)
            {
                PortView lPort = this.InternalChildren[i] as PortView;
                if (lPort != null)
                {
                    lPort.Measure(pAvailableSize);

                    if (lPort.Direction == PortDirection.Input)
                    {
                        if (lInputPortsIndex == 0 || lInputPortsIndex == lInputPortsCount - 1)
                        {
                            lInputPortsSize.Height += lPort.DesiredSize.Height + this.LineWidth / 2.0;
                        }
                        else
                        {
                            lInputPortsSize.Height += lPort.DesiredSize.Height + this.LineWidth;
                        }
                        lInputPortsIndex++;
                    }
                    else
                    {
                        if (lOutputPortsIndex == 0 || lOutputPortsIndex == lOutputPortsCount - 1)
                        {
                            lOutputPortsSize.Height += lPort.DesiredSize.Height + this.LineWidth / 2.0;
                        }
                        else
                        {
                            lOutputPortsSize.Height += lPort.DesiredSize.Height + this.LineWidth;
                        }
                        lOutputPortsIndex++;
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

            int lInputPortsIndex = 0;
            int lOutputPortsIndex = 0;

            if (this.InternalChildren.Count > 0)
            {
                // Using the ActualWidth computed by the base arrange (1) method to have the good column width.
                double lInputPortsWidth = -1;
                PortView[] lInputPorts = this.InternalChildren.Cast<PortView>().Where(pPort => pPort.Direction == PortDirection.Input).ToArray();
                if (lInputPorts.Any())
                {
                    lInputPortsWidth = Math.Round(lInputPorts.Max(pPort => pPort.ActualWidth) - this.LineWidth / 2.0);
                }

                double lOutputPortsWidth = -1;
                PortView[] lOutputPorts = this.InternalChildren.Cast<PortView>().Where(pPort => pPort.Direction == PortDirection.Output).ToArray();
                if (lOutputPorts.Any())
                {
                    lOutputPortsWidth = Math.Round(lOutputPorts.Max(pPort => pPort.ActualWidth) - this.LineWidth / 2.0);
                }

                if (lInputPortsWidth == -1 && lOutputPortsWidth != -1)
                {
                    lInputPortsWidth = lOutputPortsWidth;
                }

                if (lInputPortsWidth != -1 && lOutputPortsWidth == -1)
                {
                    lOutputPortsWidth = lInputPortsWidth;
                }

                for (int i = 0, lCount = this.InternalChildren.Count; i < lCount; ++i)
                {
                    PortView lPort = this.InternalChildren[i] as PortView;
                    if (lPort != null)
                    {
                        if (lPort.Direction == PortDirection.Input)
                        {
                            // Input ports are in the first column.
                            lPort.Arrange(new Rect(0.0, lInputPortsIndex * (lPort.DesiredSize.Height + this.LineWidth), lInputPortsWidth, lPort.DesiredSize.Height));
                            lInputPortsIndex++;
                        }
                        else
                        {
                            // Output ports are in the second column.
                            lPort.Arrange(new Rect(lInputPortsWidth + this.LineWidth, lOutputPortsIndex * (lPort.DesiredSize.Height + this.LineWidth), lOutputPortsWidth, lPort.DesiredSize.Height));
                            lOutputPortsIndex++;
                        }
                    }
                }
            }

            return pFinalSize;
        }

        #endregion // Methods.
    }
}
