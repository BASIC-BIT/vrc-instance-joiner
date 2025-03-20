using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace VRChatInstanceJoiner.ViewModels
{
    /// <summary>
    /// Base class for all ViewModels in the application, implementing INotifyPropertyChanged
    /// for data binding support.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Sets a property value and raises the PropertyChanged event if the value has changed.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="storage">Reference to the backing field.</param>
        /// <param name="value">The new value.</param>
        /// <param name="propertyName">Name of the property (automatically determined by the compiler).</param>
        /// <returns>True if the value was changed, false otherwise.</returns>
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return false;
            }

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Raises the PropertyChanged event for the specified property.
        /// </summary>
        /// <param name="propertyName">Name of the property that changed.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}