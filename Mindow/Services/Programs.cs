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
             : this()
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
                AutomationElement element = (AutomationElement)s;
                string name = string.Empty;

                try
                {
                    name = element.Current.Name;
                } catch(Exception)
                {
                    Console.WriteLine("Null element"); ;
                }

                Console.WriteLine("open: " + name + " hwnd:" + element.Current.NativeWindowHandle);
                WINDOWINFO wINDOWINFO = new WINDOWINFO();

                IntPtr windowHandle = (IntPtr)element.Current.NativeWindowHandle;

                GetWindowInfo(windowHandle, ref wINDOWINFO);

                var screenProgram = Screens.GetScreenFromLocation(wINDOWINFO.rcWindow.X, wINDOWINFO.rcWindow.Y);

                int left = wINDOWINFO.rcClient.X > screenProgram.Bounds.X ? wINDOWINFO.rcWindow.X  - screenProgram.Bounds.X : screenProgram.Bounds.X - wINDOWINFO.rcWindow.X;
                int top = wINDOWINFO.rcClient.Y > screenProgram.Bounds.Y ? wINDOWINFO.rcWindow.Y - screenProgram.Bounds.Y : screenProgram.Bounds.Y - wINDOWINFO.rcWindow.Y;

                MoveWindow(windowHandle, 
                    Screens.Current.Bounds.X + left,
                    Screens.Current.Bounds.Y + top, 
                    wINDOWINFO.rcWindow.Width - wINDOWINFO.rcWindow.X, 
                    wINDOWINFO.rcWindow.Height - wINDOWINFO.rcWindow.Y, true);
            });
            Console.ReadLine();
        }
    }
}
