using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace XGraph.Themes
{
    /// <summary>
    /// Class defining a theme manager.
    /// </summary>
    public class ThemeManager
    {
        #region Fields

        /// <summary>
        /// Stores the unique instance of the singleton.
        /// </summary>
        private static ThemeManager msInstance;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ThemeManager"/> class.
        /// </summary>
        private ThemeManager()
        {
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets the unique instance of the singleton.
        /// </summary>
        public static ThemeManager Instance
        {
            get
            {
                if (msInstance == null)
                {
                    msInstance = new ThemeManager();
                }

                return msInstance;
            }
        }

        /// <summary>
        /// Tries to find the resource identified by the given key in the loaded theme.
        /// </summary>
        /// <param name="pResourceKey">The resource key</param>
        /// <returns>The resource if found, null otherwise.</returns>
        public object FindResource(object pResourceKey)
        {
            // Try to find a specific resource.
            object lResource = Application.Current.TryFindResource(pResourceKey);
            if (lResource != null)
            {
                return lResource;
            }

            // If not using the default style.
            return Themes.Modern.Instance[pResourceKey];
        }

        #endregion // Properties.
    }
}
