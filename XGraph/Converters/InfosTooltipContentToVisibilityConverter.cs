using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace XGraph.Converters
{
    /// <summary>
    /// Converter used to define the visibility of a tooltip depending on its content.
    /// </summary>
    public class InfosTooltipContentToVisibilityConverter : IValueConverter
    {
        #region Methods

        /// <summary>
        /// Convert from A to B.
        /// </summary>
        /// <param name="pValue">The value to convert.</param>
        /// <param name="pTargetType">The target type.</param>
        /// <param name="pExtraParameter">The extra parameter to use (not used by the converter).</param>
        /// <param name="pCulture">The culture to use (not used by the converter).</param>
        /// <returns>The value converted.</returns>
        public object Convert(object pValue, Type pTargetType, object pExtraParameter, System.Globalization.CultureInfo pCulture)
        {
            IEnumerable lList = pValue as IEnumerable;
            if (lList != null)
            {
                // It's a list. Verifying if there is at least one item.
                IEnumerator lEnumerator = lList.GetEnumerator();
                if (lEnumerator.MoveNext())
                {
                    return Visibility.Visible;
                }
            }
            else
            {
                // Not a list.
                if (pValue != null)
                {
                    return Visibility.Visible;
                }
            }

            return Visibility.Hidden;
        }

        /// <summary>
        /// Convert from B to A.
        /// </summary>
        /// <param name="pValue">The value to convert.</param>
        /// <param name="pTargetType">The target type.</param>
        /// <param name="pExtraParameter">The extra parameter to use (not used by the converter).</param>
        /// <param name="pCulture">The culture to use (not used by the converter).</param>
        public object ConvertBack(object pValue, Type pTargetType, object pExtraParameter, System.Globalization.CultureInfo pCulture)
        {
            return Binding.DoNothing;
        }

        #endregion // Methods.
    }
}
