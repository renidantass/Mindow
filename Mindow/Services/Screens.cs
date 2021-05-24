using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Mindow.Services
{
    class Screens
    {
        private static List<System.Windows.Forms.Screen> AllScreens = System.Windows.Forms.Screen.AllScreens.ToList();
        public static System.Windows.Forms.Screen Current 
        {
            get
            {
                return AllScreens.Where(s => Cursor.Position.X > s.Bounds.X).FirstOrDefault();
            }
        }


        public static System.Windows.Forms.Screen GetScreenFromLocation(int x, int y)
        {
            return AllScreens.Where(s => {
                return x >= s.Bounds.X && y >= s.Bounds.Y;
            }).FirstOrDefault();
        }
    }
}
