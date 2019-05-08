namespace GTASessionBot.Utilities {
    public enum GateTimeout {
        /// <summary>
        ///     ''' Wait up to 1 second at most.
        ///     ''' </summary>
        Short = -2,

        /// <summary>
        ///     ''' Wait up to 10 seconds at most.
        ///     ''' </summary>
        Medium = -3,

        /// <summary>
        ///     ''' Wait up to 1 minute at most.
        ///     ''' </summary>
        Long = -4,

        /// <summary>
        ///     ''' Wait until the gate is released, with no timeout.
        ///     ''' </summary>
        Infinite = -5
    }
}
