using System;
using System.Runtime.InteropServices;

// see https://stackoverflow.com/questions/74594751/controlling-sdr-content-brightness-programmatically-in-windows-11
namespace SetHDRBrightness {
    
    internal static class Program {

        [DllImport("user32.dll")]
        static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);
        
        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        static extern IntPtr LoadLibrary(string lpFileName);
        
        [DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, int address);

        private delegate void DwmpSDRToHDRBoostPtr(IntPtr monitor, double brightness);

        static int Main(string[] args) {
            if (args.Length == 0) {
                System.Console.WriteLine("Missing brightness value (from 1.0 to 6.0)");
                return 1;
            }
            double brightness = Double.Parse(args[0]);
            var primaryMonitor = MonitorFromWindow(IntPtr.Zero, 1);
            var hmodule_dwmapi = LoadLibrary("dwmapi.dll");
            DwmpSDRToHDRBoostPtr changeBrightness = Marshal.GetDelegateForFunctionPointer<DwmpSDRToHDRBoostPtr>(GetProcAddress(hmodule_dwmapi, 171));
            changeBrightness(primaryMonitor, brightness);
            return 0;
        }
    }
}