using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfGlobalHooksLibrary
{
    // MOUSE ----------------------------------------------------------------------------------------


    ///// <summary>
    ///// <para> ///public MainWindow()
    /////{
    /////    InitializeComponent();
    /////    MouseHook.Start();
    /////    MouseHook.MouseAction += new EventHandler(Event);    
    /////}</para>
    ///// Using Example:





    /// <summary>   
    /// <para>==========:: Using - Example ::=====================</para>
    /// <para></para>
    /// <para> public MainWindow()</para>
    ///<para>{ </para>
    ///<para> InitializeComponent(); </para>
    ///<para> MouseHook.Start();  // Set Hook</para>
    ///<para>MouseHook.MouseAction += new EventHandler(Event);  // Register the Global Mouse Event</para>    
    ///<para>}</para>
    ///<para>.</para>
    ///<para>.</para>
    ///<para> ============:: Mouse Click Event - Method:: =========== </para> 
    /// <para> private void Event(object sender, EventArgs e)</para> 
    ///<para> {   </para>  
    ///<para> DoStuff();   </para>  
    ///<para> }  </para>  
    /// </summary>
    public static class MouseHook
    {
        // Mouse Messages
        private enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x020A,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205
        }






        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int x;
            public int y;
        }






        [StructLayout(LayoutKind.Sequential)]
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }





        // The Global Mouse EventHandler
        public static event EventHandler MouseAction = delegate { };
        public static event EventHandler MouseLeftButtonClick = delegate { };


        private static LowLevelMouseProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;


        /// <summary>
        /// <para> Create Hook: </para>
        /// <para> Later the Hook can be Unhooked with MouseHook.Stop();</para>
        /// </summary>
        public static void Start()
        {
            _hookID = SetHook(_proc);
        }





        /// <summary>
        /// <para> Unhook </para>   
        /// </summary>
        public static void Stop()
        {
            UnhookWindowsHookEx(_hookID);
        }




        private delegate IntPtr LowLevelMouseProc(int nCode, IntPtr wParam, IntPtr lParam);
        private const int WH_MOUSE_LL = 14;


        // Set - Hook
        private static IntPtr SetHook(LowLevelMouseProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_MOUSE_LL, proc,
                  GetModuleHandle(curModule.ModuleName), 0);
            }
        }









        // Hook - Callback
        private static IntPtr HookCallback(
          int nCode, IntPtr wParam, IntPtr lParam)
        {
            // Mouse Left Click 
            if (nCode >= 0 && MouseMessages.WM_LBUTTONDOWN == (MouseMessages)wParam)
            {
                MSLLHOOKSTRUCT hookStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                MouseLeftButtonClick(null, new EventArgs());
            }


            MouseAction(null, new EventArgs());

            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }







      




        // Global  Mouse Event Imports
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook,
          LowLevelMouseProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode,
          IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);
    }
}
