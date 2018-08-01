using System;
using System.Windows;

namespace XGraph.Themes
{
    /// <summary>
    /// Class defining the modern theme.
    /// </summary>
    public class ExpressionDarkTheme : ResourceDictionary
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionDarkTheme"/> class.
        /// </summary>
        public ExpressionDarkTheme()
        {
            this.Source = new System.Uri(@"/XGraph.Themes.ExpressionDark;component/Themes/ExpressionDarkTheme.xaml", UriKind.Relative);
        }

        #endregion // Constructors.
    }
}
