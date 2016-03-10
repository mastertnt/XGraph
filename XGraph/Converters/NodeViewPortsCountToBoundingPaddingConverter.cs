using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using XGraph.ViewModels;

namespace XGraph.Converters
{
    /// <summary>
    /// Converts the node view port count to the bounding padding. 
    /// </summary>
    public class NodeViewPortsCountToBoundingPaddingConverter : IMultiValueConverter
    {
        #region Fields

        /// <summary>
        /// Stores the port width value to take in account to return the correct pading.
        /// </summary>
        private const double PORT_WIDTH = 15.0;

        #endregion // Fields.

        #region Methods

        /// <summary>
        /// Convert from A to B.
        /// </summary>
        /// <param name="pValue">The value to convert.</param>
        /// <param name="pTargetType">The target type.</param>
        /// <param name="pExtraParameter">The extra parameter to use (not used by the converter).</param>
        /// <param name="pCulture">The culture to use (not used by the converter).</param>
        /// <returns>The value converted.</returns>
        public object Convert(object[] pValue, Type pTargetType, object pExtraParameter, CultureInfo pCulture)
        {
            if (pValue.Any(lValue => lValue == DependencyProperty.UnsetValue))
            {
                return Binding.DoNothing;
            }

            // First value is the port list count, second is the list.
            int lPortsCount = System.Convert.ToInt32(pValue[0]);
            PortViewModelCollection lPorts = pValue[1] as PortViewModelCollection;

            // Evaluating the padding using the ports.
            Thickness lPadding = new Thickness();
            if (lPorts != null)
            {
                if (lPorts.Any(lPort => lPort.Direction == PortDirection.Input))
                {
                    lPadding.Left = PORT_WIDTH;
                }

                if (lPorts.Any(lPort => lPort.Direction == PortDirection.Output))
                {
                    lPadding.Right = PORT_WIDTH;
                }
            }

            return lPadding;
        }

        /// <summary>
        /// Convert from B to A.
        /// </summary>
        /// <param name="pValue">The value to convert.</param>
        /// <param name="pTargetType">The target type.</param>
        /// <param name="pExtraParameter">The extra parameter to use (not used by the converter).</param>
        /// <param name="pCulture">The culture to use (not used by the converter).</param>
        public object[] ConvertBack(object pValue, Type[] pTargetType, object pExtraParameter, CultureInfo pCulture)
        {
            return new object[] { Binding.DoNothing };
        }

        #endregion // Methods.
    }
}
