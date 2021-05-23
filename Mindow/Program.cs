using Mindow.Services;
using System.Threading;

namespace Mindow
{
    class Program
    {
        static void Main(string[] args)
        {
            Privileges.Check();

            Thread t = new Thread(new ThreadStart(Programs.Detect));
            t.Start();
            t.Join();
        }
    }
}
