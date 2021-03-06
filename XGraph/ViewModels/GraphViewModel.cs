﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using PropertyChanged;
using System.ComponentModel;
using System;

namespace XGraph.ViewModels
{
    /// <summary>
    /// This class represents a view model of the graph.
    /// </summary>
    [ImplementPropertyChanged]
    public class GraphViewModel : INotifyPropertyChanged
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
        /// Initializes a new instance of the <see cref="GraphViewModel"/> class.
        /// </summary>
        public GraphViewModel()
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

        /// <summary>
        /// Event raised when a node is added.
        /// </summary>
        public event Action<GraphViewModel, NodeViewModel> NodeAdded;

        /// <summary>
        /// Event raised when a node is removed.
        /// </summary>
        public event Action<GraphViewModel, NodeViewModel> NodeRemoved;

        /// <summary>
        /// Event raised when a connection is added.
        /// </summary>
        public event Action<GraphViewModel, ConnectionViewModel> ConnectionAdded;

        /// <summary>
        /// Event raised when a connection is removed.
        /// </summary>
        public event Action<GraphViewModel, ConnectionViewModel> ConnectionRemoved;
        
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
        public void AddConnection(ConnectionViewModel pConnection)
        {
            this.mConnections.Add(pConnection);
            this.mGraphItems.Add(pConnection);
            if (this.ConnectionAdded != null)
            {
                this.ConnectionAdded(this, pConnection);
            }
        }

        /// <summary>
        /// Removes a connection from the view model.
        /// </summary>
        /// <param name="pNode">The node to remove.</param>
        public void RemoveNode(NodeViewModel pNode)
        {
            this.mNodes.Remove(pNode);
            this.mGraphItems.Remove(pNode);
            if (this.NodeRemoved != null)
            {
                this.NodeRemoved(this, pNode);
            }
        }

        /// <summary>
        /// Removes a connection from the view model.
        /// </summary>
        /// <param name="pConnection">The connection to remove.</param>
        public void RemoveConnection(ConnectionViewModel pConnection)
        {
            this.mConnections.Remove(pConnection);
            this.mGraphItems.Remove(pConnection);
            if (this.ConnectionRemoved != null)
            {
                this.ConnectionRemoved(this, pConnection);
            }
        }

        /// <summary>
        /// Adds a connection to view model.
        /// </summary>
        /// <param name="pNode">The node to add.</param>
        public void AddNode(NodeViewModel pNode)
        {
            this.mNodes.Add(pNode);
            this.mGraphItems.Add(pNode);
            if (this.NodeAdded != null)
            {
                this.NodeAdded(this, pNode);
            }
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

        #endregion // Methods.
    }
}
