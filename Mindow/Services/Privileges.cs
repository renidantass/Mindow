using System;
using System.Runtime.InteropServices;
using System.Security.Principal;

namespace Mindow.Services
{
    class Privileges
    {
        [DllImport("User32.dll", CharSet = CharSet.Unicode)]
        public static extern int MessageBox(IntPtr h, string m, string c, int type);

        private static bool IsAdmin
        {
            get
            {
                return WindowsIdentity.GetCurrent().Owner.IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid);
            }
        }

        public static void Check()
        {
            if (Privileges.IsAdmin == false)
            {
                MessageBox((IntPtr)0, "Você não tem privilégios de administrador, entre como um.", "Erro", 0);
                Environment.Exit(-1);
            }
        }
    }
}
