using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace XGraph.Extensions
{
    /// <summary>
    /// Class extending the <see cref="FrameworkElement"/> class.
    /// </summary>
    public static class FrameworkElementExtensions
    {
        /// <summary>
        /// Computes the bound of a framework element relatively to a given visual host.
        /// </summary>
        /// <param name="pElement">The element of interest.</param>
        /// <param name="pRelativeTo">The visual host.</param>
        /// <returns>The computed bound.</returns>
        public static Rect BoundsRelativeTo(this FrameworkElement pElement, Visual pRelativeTo)
        {
            return pElement.TransformToVisual(pRelativeTo).TransformBounds(LayoutInformation.GetLayoutSlot(pElement));
        }
    }
}
