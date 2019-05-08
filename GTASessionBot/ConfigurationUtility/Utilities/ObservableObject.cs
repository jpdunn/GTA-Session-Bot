using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ConfigurationUtility.Utilities
{
    /// <summary>
    /// Provides a base class for objects that notify of changes.
    /// </summary>
    public class ObservableObject : INotifyPropertyChanged
    {


        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Sets a property to the specified value.
        /// </summary>
        /// <typeparam name="T">The type of the property.</typeparam>
        /// <param name="value">The new value.</param>
        /// <param name="field">The backing field of the property.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns>True if the value changed; otherwise, False.</returns>
        protected bool SetProperty<T>(
            ref T field,
            T value,
            [CallerMemberName()] string propertyName = null
        )
        {

            if (!Equals(value, field))
            {
                field = value;
                OnPropertyChanged(propertyName);
                return true;
            }

            return false;
        }


        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected void OnPropertyChanged(string propertyName)
        {

            ValidatePropertyName(propertyName);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        /// <summary>
        /// Validates the property name.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        [Conditional("DEBUG")]
        private void ValidatePropertyName(string propertyName)
        {
            System.Reflection.BindingFlags flags = default(System.Reflection.BindingFlags);


            flags = System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.NonPublic;

            if (GetType().GetProperty(propertyName, flags) == null)
            {
                throw new ArgumentException($"Property '{propertyName}' not found.", nameof(propertyName));
            }
        }

    }
}
