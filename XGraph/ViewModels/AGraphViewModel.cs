using System.Collections.Generic;
using System.Collections.ObjectModel;
using PropertyChanged;
using System.ComponentModel;
using System;

namespace XGraph.ViewModels
{
    /// <summary>
    /// This class represents a view model of the graph.
    /// </summary>
    public abstract class AGraphViewModel : INotifyPropertyChanged
    {
        #region Fields

        /// <summary>
        /// Stores all the graph items.
        /// </summary>
        private ObservableCollection<IGraphItemViewModel> mGraphItems;

        /// <summary>
        /// Stores the nodes collection.
        /// </summary>
        private ObservableCollection<NodeViewModel> mNodes;

        /// <summary>
        /// Stores the connection collection.
        /// </summary>
        private ObservableCollection<ConnectionViewModel> mConnections;

        #endregion // Fields.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AGraphViewModel"/> class.
        /// </summary>
        protected AGraphViewModel()
        {
            this.mGraphItems = new ObservableCollection<IGraphItemViewModel>();
            this.mNodes = new ObservableCollection<NodeViewModel>();
            this.mConnections = new ObservableCollection<ConnectionViewModel>();

            this.ViewScale = 1.0;
            this.ViewOffsetX = 0.0;
            this.ViewOffsetY = 0.0;
        }

        #endregion // Constructors.

        #region Events

        /// <summary>
        /// Event raised when a property is modified.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion // Events.

        #region Properties

        /// <summary>
        /// Gets the items to display into the graph.
        /// </summary>
        public IEnumerable<IGraphItemViewModel> Items
        {
            get
            {
                return this.mGraphItems;
            }
        }

        /// <summary>
        /// Gets or sets the nodes.
        /// </summary>
        /// <value>
        /// The nodes.
        /// </value>
        public IEnumerable<NodeViewModel> Nodes
        {
            get
            {
                return this.mNodes;
            }
        }

        /// <summary>
        /// Gets or sets the connections.
        /// </summary>
        /// <value>
        /// The connections.
        /// </value>
        public IEnumerable<ConnectionViewModel> Connections
        {
            get
            {
                return this.mConnections;
            }
        }

        /// <summary>
        /// Get or set the current scale (or zoom factor) of the view.
        /// </summary>
        public double ViewScale
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the X offset (in content coordinates) of the view on the content.
        /// </summary>
        public double ViewOffsetX
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the Y offset (in content coordinates) of the view on the content.
        /// </summary>
        public double ViewOffsetY
        {
            get;
            set;
        }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Adds a node to the view model.
        /// </summary>
        /// <param name="pConnection">The connection to add.</param>
        /// <!-- WARNING From outside, this method must be called only for test purpose -->
        protected void AddConnection(ConnectionViewModel pConnection)
        {
            this.mConnections.Add(pConnection);
            this.mGraphItems.Add(pConnection);
        }

        /// <summary>
        /// Removes a connection from the view model.
        /// </summary>
        /// <param name="pNode">The node to remove.</param>
        /// <!-- WARNING From outside, this method must be called only for test purpose -->
        protected void RemoveNode(NodeViewModel pNode)
        {
            pNode.ParentGraph = null;
            this.mNodes.Remove(pNode);
            this.mGraphItems.Remove(pNode);
        }

        /// <summary>
        /// Removes a connection from the view model.
        /// </summary>
        /// <param name="pConnection">The connection to remove.</param>
        /// <!-- WARNING From outside, this method must be called only for test purpose -->
        protected void RemoveConnection(ConnectionViewModel pConnection)
        {
            this.mConnections.Remove(pConnection);
            this.mGraphItems.Remove(pConnection);
        }

        /// <summary>
        /// Adds a connection to view model.
        /// </summary>
        /// <param name="pNode">The node to add.</param>
        /// <!-- WARNING From outside, this method must be called only for test purpose -->
        protected void AddNode(NodeViewModel pNode)
        {
            pNode.ParentGraph = this;
            this.mNodes.Add(pNode);
            this.mGraphItems.Add(pNode);
        }

        /// <summary>
        /// Notifies a property has been modified.
        /// </summary>
        /// <param name="pPropertyName">The property name.</param>
        protected void NotifyPropertyChanged(string pPropertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(pPropertyName));
            }
        }

        /// <summary>
        /// Fires the request connection creation.
        /// </summary>
        /// <param name="pOutput">The output.</param>
        /// <param name="pInput">The input.</param>
        protected internal abstract void RequestConnectionCreation(PortViewModel pOutput, PortViewModel pInput);

        /// <summary>
        /// Fires the request connection creation.
        /// </summary>
        /// <param name="pConnectionToRemove">The connection to remove.</param>
        protected abstract void RequestConnectionRemoval(ConnectionViewModel pConnectionToRemove);

        #endregion // Methods.
    }
}
