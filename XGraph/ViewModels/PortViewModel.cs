using System.ComponentModel;
using System.Windows;
using PropertyChanged;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Diagnostics;

namespace XGraph.ViewModels
{
    /// <summary>
    /// This enumeration give the type of the direction.
    /// </summary>
    public enum PortDirection
    {
        /// <summary>
        /// Defines an input port.
        /// </summary>
        Input,

        /// <summary>
        /// Defines an ouput port.
        /// </summary>
        Output,
    }

    /// <summary>
    /// This class represents a port view model.
    /// A port can be connected to another port.
    /// </summary>
    /// <!-- NBY -->
    [DebuggerDisplay("[{Direction}] {DisplayString}")]
    public class PortViewModel : INotifyPropertyChanged
    {
        #region Properties

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        public virtual string Id
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets or sets the parent node of the port.
        /// </summary>
        public NodeViewModel ParentNode
        {
            get;
            internal set;
        }

        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        public Point Position
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of the port.
        /// </summary>
        public string PortType
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        public PortDirection Direction
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is selected.
        /// </summary>
        public virtual bool IsSelected
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is active.
        /// </summary>
        public virtual bool IsActive
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the display string.
        /// </summary>
        public virtual string DisplayString
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the flag indicating if the port is connected or not.
        /// </summary>
        /// <remarks>
        /// Must not be set by user. Only the GUI is able to update this flag properly.
        /// </remarks>
        [AlsoNotifyFor("Icon")]
        public bool IsConnected
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the icon.
        /// </summary>
        public virtual ImageSource Icon
        {
            get
            {
                if (this.IsConnected)
                {
                    return this.ConnectedIcon;
                }
                else
                {
                    return this.DisconnectedIcon;
                }
            }
            set
            {
                // Nothing to do.
            }
        }

        /// <summary>
        /// Gets the icon to display when the port is disconnected.
        /// </summary>
        protected virtual ImageSource DisconnectedIcon
        {
            get
            {
                if (this.Direction == PortDirection.Input)
                {
                    return Themes.ThemeManager.Instance.FindResource("InputPort_Disconnected_Icon") as BitmapImage;
                }
                else
                {
                    return Themes.ThemeManager.Instance.FindResource("OutputPort_Disconnected_Icon") as BitmapImage;
                }
            }
        }

        /// <summary>
        /// Gets the icon to display when the port is connected.
        /// </summary>
        protected virtual ImageSource ConnectedIcon
        {
            get
            {
                if (this.Direction == PortDirection.Input)
                {
                    return Themes.ThemeManager.Instance.FindResource("InputPort_Connected_Icon") as BitmapImage;
                }
                else
                {
                    return Themes.ThemeManager.Instance.FindResource("OutputPort_Connected_Icon") as BitmapImage;
                }
            }
        }

        /// <summary>
        /// Gets the data template.
        /// </summary>
        public DataTemplate DataTemplate
        {
            get
            {
                if (this.Direction == PortDirection.Input)
                {
                    return Themes.ThemeManager.Instance.FindResource("InputPortViewDefaultDataTemplate") as DataTemplate;
                }
                else
                {
                    return Themes.ThemeManager.Instance.FindResource("OutputPortViewDefaultDataTemplate") as DataTemplate;
                }
            }
        }

        /// <summary>
        /// Gets or sets the ToolTip of the port.
        /// </summary>
        public virtual string ToolTip
        {
            get;
            set;
        }

        #endregion // Properties.

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PortViewModel"/> class.
        /// </summary>
        public PortViewModel()
        {
            this.Id = string.Empty;
            this.IsActive = true;
            this.IsConnected = false;
        }

        #endregion // Constructors.

        #region Events

        /// <summary>
        /// Event raised when a property is modified.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion // Events.

        #region Methods

        /// <summary>
        /// Determines whether this source port can be connected to the specified p port view model.
        /// </summary>
        /// <param name="pTargetPortViewModel">The target port view model.</param>
        /// <returns>True if the connection can be done, false otherwise.</returns>
        public virtual bool CanBeConnectedTo(PortViewModel pTargetPortViewModel)
        {
            return true;
        }

        /// <summary>
        /// Notifies when a property is changed.
        /// </summary>
        /// <param name="pPropertyName"></param>
        public void OnPropertyChanged(string pPropertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(pPropertyName));
            }
        }

        #endregion // Methods.
    }
}
