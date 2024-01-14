using StealthGPT.Properties;
using System;
using System.Windows.Forms;
using static StealthGPT.globalKeyboard;
using static StealthGPT.trayBar;

//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

namespace StealthGPT
{
    internal static class Program
    {
        public static config config = new config();
        public static stealthGPT stealthGPT_GUI;
        public static IntPtr _hookId = IntPtr.Zero;
        private static LowLevelKeyboardProc _proc = HookCallback;
        private static ProcessIcon processIcon = new ProcessIcon();

        [STAThread]
        static void Main()
        {
            config.Load();
            _hookId = SetHook(_proc);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            processIcon.Display();
            stealthGPT_GUI = new stealthGPT();
            stealthGPT_GUI.Icon = Resources.steam_ico;
            Application.Run(stealthGPT_GUI);
            disposeSafely();
        }

        public static void updateTitle(string txt)
        {
            stealthGPT_GUI.Text = "StealthGPT By Cornholio | V1.0 | " + txt;
        }

        public static void disposeSafely()
        {
            Application.Exit();
            config.Save();
            processIcon.Dispose();
            UnhookWindowsHookEx(_hookId);
        }
    }
}
