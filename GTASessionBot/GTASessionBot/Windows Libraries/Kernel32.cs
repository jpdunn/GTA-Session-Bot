using System;
using System.Runtime.InteropServices;


namespace GTASessionBot.Windows_Libraries {

    [Flags]
    public enum ThreadAccess : int {
        TERMINATE = (0x0001),
        SUSPEND_RESUME = (0x0002),
        GET_CONTEXT = (0x0008),
        SET_CONTEXT = (0x0010),
        SET_INFORMATION = (0x0020),
        QUERY_INFORMATION = (0x0040),
        SET_THREAD_TOKEN = (0x0080),
        IMPERSONATE = (0x0100),
        DIRECT_IMPERSONATION = (0x0200)
    }


    public class Kernel32 {

        /// <summary>
        /// Opens an existing thread object.
        /// </summary>
        /// <param name="dwDesiredAccess">
        /// The access to the thread object. This access right is checked against the security 
        /// descriptor for the thread. This parameter can be one or more of the thread access rights.
        /// </param>
        /// <param name="bInheritHandle">
        /// If this value is TRUE, processes created by this process will inherit the handle. 
        /// Otherwise, the processes do not inherit this handle.
        /// </param>
        /// <param name="dwThreadId">The identifier of the thread to be opened.</param>
        /// <returns>If successful, an open handle to the specified thread is returned, otherwise null.</returns>
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);


        /// <summary>
        /// Suspends the specified thread.
        /// </summary>
        /// <param name="hThread">
        /// A handle to the thread that is to be suspended.
        /// The handle must have the THREAD_SUSPEND_RESUME access right.
        /// </param>
        /// <returns>The thread's previous suspend count if successful; otherwise, -1</returns>
        [DllImport("kernel32.dll")]
        public static extern uint SuspendThread(IntPtr hThread);


        /// <summary>
        /// Decrements a thread's suspend count. When the suspend count is decremented to zero, 
        /// the execution of the thread is resumed.
        /// </summary>
        /// <param name="hThread">A handle to the thread to be restarted.</param>
        /// <returns>The thread's previous suspend count if successful; otherwise, -1.</returns>
        [DllImport("kernel32.dll")]
        public static extern int ResumeThread(IntPtr hThread);


        [DllImport("kernel32")]
        public static extern UInt64 GetTickCount64();
    }
}
