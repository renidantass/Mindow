using Mindow.Services;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Management;
using System.Runtime.InteropServices;

namespace Mindow
{
    class Program
    {
        static void Main(string[] args)
        {
            Privileges.Check();
            Programs.Detect();
        }
    }
}
