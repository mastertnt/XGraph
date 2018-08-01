using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XGraph.ViewModels;

namespace XGraphTestApp.ViewModels
{
    /// <summary>
    /// Graph view model used for demo purpose.
    /// </summary>
    public class GraphViewModel : AGraphViewModel
    {
        #region Methods

        /// <summary>
        /// Adds a node to the view model.
        /// </summary>
        /// <param name="pConnection">The connection to add.</param>
        public new void AddConnection(ConnectionViewModel pConnection)
        {
            base.AddConnection(pConnection);
        }

        /// <summary>
        /// Removes a connection from the view model.
        /// </summary>
        /// <param name="pNode">The node to remove.</param>
        public new void RemoveNode(NodeViewModel pNode)
        {
            base.RemoveNode(pNode);
        }

        /// <summary>
        /// Removes a connection from the view model.
        /// </summary>
        /// <param name="pConnection">The connection to remove.</param>
        public new void RemoveConnection(ConnectionViewModel pConnection)
        {
            base.RemoveConnection(pConnection);
        }

        /// <summary>
        /// Adds a connection to view model.
        /// </summary>
        /// <param name="pNode">The node to add.</param>
        public new void AddNode(NodeViewModel pNode)
        {
            base.AddNode(pNode);
        }

        /// <summary>
        /// Fires the request connection creation.
        /// </summary>
        /// <param name="pOutput">The output.</param>
        /// <param name="pInput">The input.</param>
        protected override void RequestConnectionCreation(PortViewModel pOutput, PortViewModel pInput)
        {
            ConnectionViewModel lConnectionViewModel = new ConnectionViewModel
            {
                Output = pOutput,
                Input = pInput
            };
            this.AddConnection(lConnectionViewModel);
        }

        /// <summary>
        /// Fires the request connection creation.
        /// </summary>
        /// <param name="pConnectionToRemove">The connection to remove.</param>
        protected override void RequestConnectionRemoval(ConnectionViewModel pConnectionToRemove)
        {
            // Nothing to do.
        }

        #endregion // Methods.
    }
}
