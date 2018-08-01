using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace XGraph.Converters
{
    /// <summary>
    /// This converter allows to convert from boolean to integer type.
    /// </summary>
    /// <!-- Damien Porte -->
    [ValueConversion(typeof(bool), typeof(int))]
    public class BooleanToInt32Converter : IValueConverter
    {
        #region Properties

        /// <summary>
        /// Gets or sets the boolean equals to zero.
        /// </summary>
        public bool ZeroValue
        {
            get;
            set;
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BooleanToInt32Converter"/> class.
        /// </summary>
        public BooleanToInt32Converter()
        {
            this.ZeroValue = true;
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Convert from boolean to integer.
        /// </summary>
        /// <param name="pValue">The value to convert.</param>
        /// <param name="pTargetType">The target type.</param>
        /// <param name="pExtraParameter">The extra parameter to use (not used by the lConverter).</param>
        /// <param name="pCulture">The culture to use (not used by the lConverter).</param>
        /// <returns>The value converted.</returns>
        public object Convert(object pValue, Type pTargetType, object pExtraParameter, CultureInfo pCulture)
        {
            return Binding.DoNothing;
        }

        /// <summary>
        /// Convert from integer to boolean.
        /// </summary>
        /// <param name="pValue">The value to convert.</param>
        /// <param name="pTargetType">The target type.</param>
        /// <param name="pExtraParameter">The extra parameter to use (not used by the lConverter).</param>
        /// <param name="pCulture">The culture to use (not used by the lConverter).</param>
        /// <returns>The value converted.</returns>
        public object ConvertBack(object pValue, Type pTargetType, object pExtraParameter, CultureInfo pCulture)
        {
            // Checks if the value is valid.
            if ((pValue is Int32) == false)
            {
                return Binding.DoNothing;
            }

            Int32 lValue = (Int32)pValue;
            if (lValue == 0)
            {
                return this.ZeroValue;
            }

            return !this.ZeroValue;
        }

        #endregion // Methods.
    }
}
