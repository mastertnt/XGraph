using System;
using System.Windows.Data;

namespace XGraph.Converters
{
    /// <summary>
    /// This converter is only used to check if a binding works.
    /// </summary>
    /// <!-- NBY -->
    public class DebugConverter : IValueConverter
    {
        #region Methods

        /// <summary>
        /// Convert from B to A.
        /// </summary>
        /// <param name="pValue">The value to convert.</param>
        /// <param name="pTargetType">The target type.</param>
        /// <param name="pExtraParameter">The extra parameter to use (not used by the converter).</param>
        /// <param name="pCulture">The culture to use (not used by the converter).</param>
        public object ConvertBack(object pValue, Type pTargetType, object pExtraParameter, System.Globalization.CultureInfo pCulture)
        {
            return pValue;
        }

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
            return pValue;
        }

        #endregion // Methods.
    }
}
