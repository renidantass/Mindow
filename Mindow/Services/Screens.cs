using System.Collections.Generic;
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
    }
}
