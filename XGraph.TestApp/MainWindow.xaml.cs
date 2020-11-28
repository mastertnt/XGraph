using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using XGraph.ViewModels;
using XGraphTestApp;
using XGraphTestApp.ViewModels;

namespace XGraph.TestApp
{
    /// <summary>
    /// Main window of the sample.
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();

            this.GraphView.DataContext = new GraphViewModel();
            this.GraphView.SelectionChanged += this.OnGraphViewSelectionChanged;
        }

        #endregion // Constructors.

        #region Methods

        /// <summary>
        /// Creates the type graph.
        /// </summary>
        /// <returns>The graph view model.</returns>
        public AGraphViewModel CreateTypeGraph()
        {
            GraphViewModel lGraph = new GraphViewModel();
            NodeViewModel lNode0 = new TypeNodeViewModel(typeof(SampleClass));
            lGraph.AddNode(lNode0);

            NodeViewModel lNode1 = new TypeNodeViewModel(typeof(SampleClass1VeryTooMuchLong));
            lGraph.AddNode(lNode1);

            NodeViewModel lNode2 = new NodeViewModel();
            lNode2.DisplayString = "Empty node";
            lGraph.AddNode(lNode2);

            int i = 0;
            foreach (NodeViewModel lNode in lGraph.Nodes)
            {
                lNode.X = 300 * i;
                lNode.Y = 100 * i;
                i++;
            }

            ConnectionViewModel lConnectionViewModel = new ConnectionViewModel();
            lConnectionViewModel.Output = lGraph.Nodes.ElementAt(0).Ports.FirstOrDefault(pPort => pPort.Direction == PortDirection.Output);
            lConnectionViewModel.Input = lGraph.Nodes.ElementAt(1).Ports.FirstOrDefault(pPort => pPort.Direction == PortDirection.Input);
            lGraph.AddConnection(lConnectionViewModel);
            
            return lGraph;
        }

        /// <summary>
        /// Creates the graph.
        /// </summary>
        /// <param name="pNodeCount">The node count.</param>
        /// <returns>The graph view model.</returns>
        public AGraphViewModel CreateRandomGraph(int pNodeCount)
        {
            GraphViewModel lGraph = new GraphViewModel();
            Random lRandom = new Random();

            for (int i = 0; i < pNodeCount; i++)
            {
                NodeViewModel lNode = this.CreateNode(string.Format("{0}", i), string.Format("NODE_{0}", i), lRandom.Next(1, 5), lRandom.Next(1, 3));
                lNode.X = 50 * i;
                lNode.Y = 100 * i;
                lGraph.AddNode(lNode);
            }

            return lGraph;
        }

        /// <summary>
        /// This method creates the node.
        /// </summary>
        /// <param name="pId">The identifier.</param>
        /// <param name="pTitle">The title.</param>
        /// <param name="pInputPort">The input port.</param>
        /// <param name="pOutputPort">The output port.</param>
        /// <returns>The initialized node</returns>
        public NodeViewModel CreateNode(string pId, string pTitle, int pInputPort, int pOutputPort)
        {
            NodeViewModel lNode = new NodeViewModel();
            lNode.HasInputBreakpoint = true;
            lNode.HasInputBreakpoint = false;
            lNode.DisplayString = pTitle;
            for (int i = 0; i < pInputPort; i++)
            {
                PortViewModel lPort = new PortViewModel();
                lPort.DisplayString = string.Format("IPort {0}", i);
                lPort.Direction = PortDirection.Input;

                lNode.Ports.Add(lPort);
            }

            for (int i = 0; i < pOutputPort; i++)
            {
                PortViewModel lPort = new PortViewModel();
                lPort.DisplayString = string.Format("OPort {0}", i);
                lPort.Direction = PortDirection.Output;

                lNode.Ports.Add(lPort);
            }
            return lNode;

        }

        /// <summary>
        /// Delegate called when the "Add" button is clicked.
        /// </summary>
        /// <param name="pSender">The button sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnNewButtonClicked(object pSender, RoutedEventArgs pEventArgs)
        {
            this.GraphView.DataContext = new GraphViewModel();
        }

        /// <summary>
        /// Delegate called when the "Add" button is clicked.
        /// </summary>
        /// <param name="pSender">The button sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnAddButtonClicked(object pSender, RoutedEventArgs pEventArgs)
        {
            GraphViewModel lRootViewModel = this.GraphView.DataContext as GraphViewModel;
            NodeViewModel lNode1 = new TypeNodeViewModel(typeof(SampleClass1VeryTooMuchLong));
            lNode1.HasInputBreakpoint = true;
            lRootViewModel.AddNode(lNode1);
        }

        /// <summary>
        /// Delegate called when the delete button is clicked.
        /// </summary>
        /// <param name="pSender">The button sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnDeleteButtonClicked(object pSender, RoutedEventArgs pEventArgs)
        {
            GraphViewModel lRootViewModel = this.GraphView.DataContext as GraphViewModel;
            foreach (IGraphItemViewModel lItem in this.GraphView.SelectedViewModels)
            {
                ConnectionViewModel lConnection = lItem as ConnectionViewModel;
                if (lConnection != null)
                {
                    lRootViewModel.RemoveConnection(lConnection);
                }

                NodeViewModel lNode = lItem as NodeViewModel;
                if (lNode != null)
                {
                    lRootViewModel.RemoveNode(lNode);
                }
            }
        }

        /// <summary>
        /// Delegate called when the is active button is clicked.
        /// </summary>
        /// <param name="pSender">The button sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnIsActiveButtonClicked(object pSender, RoutedEventArgs pEventArgs)
        {
            AGraphViewModel lRootViewModel = this.GraphView.DataContext as AGraphViewModel;
            NodeViewModel lSelectedItem = this.GraphView.SelectedViewModels.OfType<NodeViewModel>().FirstOrDefault();
            if (lSelectedItem != null)
            {
                lSelectedItem.IsActive = !lSelectedItem.IsActive;
            }            
        }

        /// <summary>
        /// Delegate called when the errors button is clicked.
        /// </summary>
        /// <param name="pSender">The button sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnErrorsButtonClicked(object pSender, RoutedEventArgs pEventArgs)
        {
            AGraphViewModel lRootViewModel = this.GraphView.DataContext as AGraphViewModel;
            NodeViewModel lSelectedItem = this.GraphView.SelectedViewModels.OfType<NodeViewModel>().FirstOrDefault();
            if (lSelectedItem != null)
            {
                if (lSelectedItem.Errors == null)
                {
                    string[] lErrors = new string[] { "My first little error.", "My second really really really really long error." };
                    lSelectedItem.Errors = lErrors;
                }
                else
                {
                    lSelectedItem.Errors = null;
                }
            }
        }

        /// <summary>
        /// Delegate called when the warning button is clicked.
        /// </summary>
        /// <param name="pSender">The button sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnWarningsButtonClicked(object pSender, RoutedEventArgs pEventArgs)
        {
            AGraphViewModel lRootViewModel = this.GraphView.DataContext as AGraphViewModel;
            NodeViewModel lSelectedItem = this.GraphView.SelectedViewModels.OfType<NodeViewModel>().FirstOrDefault();
            if (lSelectedItem != null)
            {
                if (lSelectedItem.Warnings == null)
                {
                    string[] lWarnings = new string[] { "My first little warning.", "My second really really really really long warning." };
                    lSelectedItem.Warnings = lWarnings;
                }
                else
                {
                    lSelectedItem.Warnings = null;
                }
            }
        }

        /// <summary>
        /// Delegate called when the warning button is clicked.
        /// </summary>
        /// <param name="pSender">The button sender.</param>
        /// <param name="pEventArgs">The event arguments.</param>
        private void OnConnectionBrushButtonClicked(object pSender, RoutedEventArgs pEventArgs)
        {
            AGraphViewModel lRootViewModel = this.GraphView.DataContext as AGraphViewModel;
            ConnectionViewModel lSelectedItem = this.GraphView.SelectedViewModels.OfType<ConnectionViewModel>().FirstOrDefault();
            if (lSelectedItem != null)
            {
                SolidColorBrush lDefaultBrush = new SolidColorBrush(Color.FromRgb(157, 157, 157));
                SolidColorBrush lSelectedItemBrush = lSelectedItem.Brush as SolidColorBrush;
                if (lSelectedItemBrush == null || lSelectedItemBrush.Color == lDefaultBrush.Color)
                {
                    lSelectedItem.Brush = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    lSelectedItem.Brush = lDefaultBrush;
                }
            }
        }

        /// <summary>
        /// Delegate called when the selection changed.
        /// </summary>
        /// <param name="pSender"></param>
        /// <param name="pEventArgs"></param>
        private void OnGraphViewSelectionChanged(object pSender, SelectionChangedEventArgs pEventArgs)
        {
            INotifyPropertyChanged lPreviousSelectedItem = this.mPropertyEditor.SelectedObject as INotifyPropertyChanged;
            if (lPreviousSelectedItem != null)
            {
                lPreviousSelectedItem.PropertyChanged -= this.OnPropertyChanged;
            }
            NodeViewModel lNodeViewModel = this.GraphView.SelectedViewModels.OfType<NodeViewModel>().FirstOrDefault();
            if (lNodeViewModel != null)
            {
                this.mPropertyEditor.SelectedObject = lNodeViewModel;
                lNodeViewModel.PropertyChanged += this.OnPropertyChanged;
            }

            ConnectionViewModel lConnectionViewModel = this.GraphView.SelectedViewModels.OfType<ConnectionViewModel>().FirstOrDefault();
            if (lConnectionViewModel != null)
            {
                this.mPropertyEditor.SelectedObject = lConnectionViewModel;
                lConnectionViewModel.PropertyChanged += this.OnPropertyChanged;
            } }

        private void OnPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Console.WriteLine(e.PropertyName);
        }

        #endregion // Methods.
    }
}
