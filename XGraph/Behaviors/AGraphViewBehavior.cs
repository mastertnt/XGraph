using XGraph.Controls;

namespace XGraph.Behaviors
{
    /// <summary>
    /// Base class for the graph view behaviors.
    /// </summary>
    public abstract class AGraphViewBehavior
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AGraphViewBehavior"/> class.
        /// </summary>
        /// <param name="pParent">The behavior parent view.</param>
        protected AGraphViewBehavior(SimpleGraphView pParent)
        {
            this.ParentView = pParent;
        }

        #endregion // Constructors.

        #region Properties

        /// <summary>
        /// Gets the behavior parent view.
        /// </summary>
        protected SimpleGraphView ParentView
        {
            get;
            private set;
        }

        #endregion // Properties.
    }
}
