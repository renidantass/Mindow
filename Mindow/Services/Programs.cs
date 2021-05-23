using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Automation;

namespace Mindow.Services
{
    class Programs
    {
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
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        public static void Detect()
        {
            Automation.AddAutomationEventHandler(WindowPattern.WindowOpenedEvent, AutomationElement.RootElement, TreeScope.Children, (s, ev) =>
            {
                var element = (AutomationElement)s;
                var name = element.Current.Name;
                Console.WriteLine("open: " + name + " hwnd:" + element.Current.NativeWindowHandle);
                WINDOWINFO wINDOWINFO = new WINDOWINFO();
                GetWindowInfo((IntPtr)element.Current.NativeWindowHandle, ref wINDOWINFO);
                MoveWindow((IntPtr)element.Current.NativeWindowHandle, Screens.Current.Bounds.X, wINDOWINFO.rcWindow.Y, wINDOWINFO.rcClient.Width - wINDOWINFO.rcClient.X, wINDOWINFO.rcWindow.Height - wINDOWINFO.rcWindow.Y, true);
            });
            Console.ReadLine();
        }
    }
}
