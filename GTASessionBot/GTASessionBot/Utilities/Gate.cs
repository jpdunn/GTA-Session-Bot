using System;

namespace GTASessionBot.Utilities {
    public class Gate : IGate {


        /// <summary>
        ///     ''' Corresponds to <see cref="GateTimeout.Short"/>.
        ///     ''' </summary>
        private const int GateTimeoutShort = 1 * 1000;


        /// <summary>
        ///     ''' Corresponds to <see cref="GateTimeout.Medium"/>.
        ///     ''' </summary>
        private const int GateTimeoutMedium = 10 * 1000;


        /// <summary>
        ///     ''' Corresponds to <see cref="GateTimeout.Long"/>.
        ///     ''' </summary>
        private const int GateTimeoutLong = 60 * 1000;


        /// <summary>
        ///     ''' Corresponds to <see cref="GateTimeout.Infinite" />
        ///     ''' </summary>
        private const int GateTimeoutInfinite = -1;


        /// <summary>
        ///     ''' Wait no shorter than the short timeout.
        ///     ''' </summary>
        private const int GateTimeoutMinimum = GateTimeoutShort;


        /// <summary>
        ///     ''' Wait at most 1 day.
        ///     ''' </summary>
        private const int GateTimeoutMaximium = 24 * 60 * 60 * 1000;


        private readonly System.Threading.ManualResetEvent cgEvent;


        /// <summary>
        ///     ''' Initializes a new instance of the <see cref="Gate">Gate</see> class, with the gate initially closed.
        ///     ''' </summary>
        public Gate() {
            cgEvent = new System.Threading.ManualResetEvent(false);
        }


        /// <summary>
        ///     ''' Opens the <see cref="Gate">Gate</see>.
        ///     ''' </summary>
        public void Open() {
            cgEvent.Set();
        }


        /// <summary>
        ///     ''' Closes the <see cref="Gate">Gate</see>.
        ///     ''' </summary>
        public void Close() {
            cgEvent.Reset();
        }


        /// <summary>
        ///     ''' Blocks the current thread until the <see cref="Gate">Gate</see> is opened.
        ///     ''' </summary>
        ///     ''' <param name="timeoutPeriod">
        ///     ''' An enumeration indicating the number of milliseconds to wait for the gate to open, or a positive 
        ///     ''' manually-specified value.</param>
        ///     ''' <returns>
        ///     ''' True if the gate was opened, or False if the current thread 
        ///     ''' waited for the specified time and the gate was not opened.
        ///     ''' </returns>
        public bool WaitUntilOpen(GateTimeout timeoutPeriod) {

            // When a non-enum value is passed in it is used, but is constrained to be in the min and max values. 
            // Whether we allow infinite timeouts is an interesting problem. In some cases it is acceptable. For 
            // example, in a controlled system you may need to wait at a gate until something is added to a queue.  
            // When something is added to a queue the gate is opened. An infinite timeout is OK here, as long as 
            // the gate is guaranteed to be opened when the system is stopped There may be a better solution to 
            // this that doesn't require an infinite timeout at a gate. I don't think we want to allow just anyone
            // to use an infinite timeout in the same way that we don't want locks to have infinite timeouts.

            int timeout;


            timeout = (int)timeoutPeriod;

            switch (timeoutPeriod) {
                case GateTimeout.Infinite:
                    timeout = GateTimeoutInfinite;
                    break;


                case GateTimeout.Long:
                    timeout = GateTimeoutLong;
                    break;


                case GateTimeout.Medium:
                    timeout = GateTimeoutMedium;
                    break;


                case GateTimeout.Short:
                    timeout = GateTimeoutShort;
                    break;


                default:
                    timeout = Math.Max(GateTimeoutMinimum, timeout);
                    timeout = Math.Min(GateTimeoutMaximium, timeout);
                    break;

            }

            return cgEvent.WaitOne(timeout, false);
        }


        /// <summary>
        ///     ''' Blocks the current thread until the <see cref="Gate">Gate</see> is opened.
        ///     ''' </summary>
        ///     ''' <param name="timeoutPeriod">The number of milliseconds to wait for the gate to open.</param>
        ///     ''' <returns>
        ///     ''' True if the gate was opened, or False if the current thread 
        ///     ''' waited for the specified time and the gate was not opened.
        ///     ''' </returns>
        public bool WaitUntilOpen(int timeoutPeriod) {
            return cgEvent.WaitOne(timeoutPeriod, false);
        }
    }

}
