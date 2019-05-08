using GTASessionBot.Windows_Libraries;
using System;
using System.Diagnostics;

namespace GTASessionBot
{
    public static class Extensions
    {
        public static void Suspend(this Process process)
        {
            foreach (ProcessThread thread in process.Threads)
            {
                IntPtr pOpenThread;


                pOpenThread = Kernel32.OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)thread.Id);

                if (pOpenThread == IntPtr.Zero)
                {
                    break;
                }

                Kernel32.SuspendThread(pOpenThread);
            }
        }


        public static void Resume(this Process process)
        {
            foreach (ProcessThread thread in process.Threads)
            {
                IntPtr pOpenThread;


                pOpenThread = Kernel32.OpenThread(ThreadAccess.SUSPEND_RESUME, false, (uint)thread.Id);

                if (pOpenThread == IntPtr.Zero)
                {
                    break;
                }

                Kernel32.ResumeThread(pOpenThread);
            }
        }


        public static string ToShortForm(this TimeSpan t)
        {
            string shortForm = "";


            if (t.Hours > 0)
            {
                shortForm += string.Format("{0}h ", t.Hours.ToString());
            }
            if (t.Minutes > 0)
            {
                shortForm += string.Format("{0}m ", t.Minutes.ToString());
            }
            if (t.Seconds > 0)
            {
                shortForm += string.Format("{0}s ", t.Seconds.ToString());
            }

            if (t.Milliseconds > 0)
            {
                shortForm += string.Format("{0}ms", t.Milliseconds.ToString());
            }
            return shortForm;
        }
    }
}
