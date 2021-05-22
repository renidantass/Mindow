using System.Drawing;
using System.Runtime.InteropServices;

namespace Mindow.Services
{
    class Cursor
    {
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);
        private static Point cursor = new Point();
        public static Point Position { 
            get {
                GetCursorPos(ref cursor);
                return cursor;
            }
        }
    }
}
