namespace GTASessionBot.Utilities {
    public interface IGate {

        /// <summary>
        /// Opens the <see cref="Gate">Gate</see>.
        /// </summary>
        void Open();


        /// <summary>
        /// Closes the gate.
        /// </summary>
        void Close();



        /// <summary>
        /// Blocks the current thread until the <see cref="Gate">Gate</see> is opened.
        /// </summary>
        /// <param name="timeoutPeriod">
        /// An enumeration indicating the number of milliseconds to wait for the gate to open, or a positive 
        /// manually-specified value.</param>
        /// <returns>
        /// True if the gate was opened, or False if the current thread 
        /// waited for the specified time and the gate was not opened.
        /// </returns>
        bool WaitUntilOpen(GateTimeout timeoutPeriod);


        /// <summary>
        ///     ''' Blocks the current thread until the <see cref="Gate">Gate</see> is opened.
        ///     ''' </summary>
        ///     ''' <param name="timeoutPeriod">The number of milliseconds to wait for the gate to open.</param>
        ///     ''' <returns>
        ///     ''' True if the gate was opened, or False if the current thread 
        ///     ''' waited for the specified time and the gate was not opened.
        ///     ''' </returns>
        bool WaitUntilOpen(int timeoutPeriod);
    }
}
