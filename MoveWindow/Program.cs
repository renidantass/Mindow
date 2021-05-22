using System;
using System.Diagnostics;
using System.Drawing;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Security.Principal;

namespace MoveWindow
{
    class Program
    {
        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        public static extern int MessageBox(IntPtr h, string m, string c, int type);

        static bool IsAdmin
        {
            get 
            {
                return WindowsIdentity.GetCurrent().Owner.IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid);
            }
        }

        static void Main(string[] args)
        {
            

            Programs.Detect();
        }
    }

    class Programs
    {
        public static void Detect()
        {
            ManagementEventWatcher w = null;
            WqlEventQuery q;
            try
            {
                q = new WqlEventQuery();
                q.EventClassName = "Win32_ProcessStartTrace";
                w = new ManagementEventWatcher(q);
                w.EventArrived += new EventArrivedEventHandler(Programs.ProcessStartEventArrived);
                w.Start();
                Console.ReadLine();
            }
            finally
            {
                w.Stop();
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        public struct WINDOWINFO
        {
            public uint cbSize;
            public Rectangle rcWindow;
            public Rectangle rcClient;
            public uint dwStyle;
            public uint dwExStyle;
            public uint dwWindowStatus;
            public uint cxWindowBorders;
            public uint cyWindowBorders;
            public ushort atomWindowType;
            public ushort wCreatorVersion;

            public WINDOWINFO(Boolean? filler)
             : this()   // Allows automatic initialization of "cbSize" with "new WINDOWINFO(null/true/false)".
            {
                cbSize = (UInt32)(Marshal.SizeOf(typeof(WINDOWINFO)));
            }

        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool GetWindowInfo(IntPtr hwnd, ref WINDOWINFO pwi);

        public static void ProcessStartEventArrived(object sender, EventArrivedEventArgs e)
        {
            Point defPnt = new Point();
            var processId = (UInt32)e.NewEvent.Properties["ProcessId"].Value;
            try
            {
                Rectangle rect = new Rectangle();
                GetCursorPos(ref defPnt);
                Process recentlyOpened = Process.GetProcessById((int)processId);
                if (recentlyOpened.MainWindowTitle != "")
                {
                    Console.WriteLine("Programa aberto {0}", recentlyOpened);
                    WINDOWINFO wINDOWINFO = new WINDOWINFO();
                    GetWindowInfo(recentlyOpened.MainWindowHandle, ref wINDOWINFO);
                    MoveWindow(recentlyOpened.MainWindowHandle, defPnt.X, wINDOWINFO.rcWindow.Y, 800, 600, true);
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
