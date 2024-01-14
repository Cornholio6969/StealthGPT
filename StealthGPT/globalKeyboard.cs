using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace StealthGPT
{
    internal class globalKeyboard
    {
        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x0100;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        public static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }
        private static string result = "";
        public static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                if (Program.config.checkForKeyExistence(vkCode))
                {
                    if (stealthGPT.waitForTitleKey)
                    {
                        Program.config.TitleKey = vkCode;
                        stealthGPT.waitForTitleKey = false;
                    }
                    else if (stealthGPT.waitForQuizKey)
                    {
                        Program.config.QuizKey = vkCode;
                        stealthGPT.waitForQuizKey = false;
                    }
                    else if (stealthGPT.waitForQueryKey)
                    {
                        Program.config.QueryKey = vkCode;
                        stealthGPT.waitForQueryKey = false;
                    }
                    else if (stealthGPT.waitForPanicKey)
                    {
                        Program.config.PanicKey = vkCode;
                        stealthGPT.waitForPanicKey = false;
                    }
                }
                else
                {
                    if (Program.config.enabled)
                    {
                        if (Program.config.autoMode)
                        {
                            if (vkCode == Program.config.QueryKey)
                            {
                                result = gptAPI.queryImage(Image.imageCapture2Base64());
                            }
                        }
                        else
                        {
                            if (vkCode != Program.config.QueryKey || vkCode != Program.config.PanicKey)
                            {
                                if (Program.config.instaCopy)
                                {
                                    try
                                    {
                                        SendKeys.SendWait("^(c)");
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("Error: " + ex.Message);
                                    }
                                }

                            }

                            if (vkCode == Program.config.TitleKey) { gptAPI.currentTitle = Clipboard.GetText(); }
                            else
                            if (vkCode == Program.config.QuizKey) { gptAPI.currentContext = Clipboard.GetText(); }
                            else
                            if (vkCode == Program.config.PanicKey) { Program.disposeSafely(); }
                            else
                            if (vkCode == Program.config.QueryKey)
                            {
                                result = gptAPI.queryQuiz();
                            }
                        }
                    }
                    if (result != "")
                    {
                        //Temp airpod fix
                        Console.Beep();
                        //Temp airpod fix

                        if (Program.config.setClipboard)
                        {
                            Clipboard.SetText(result);
                        }

                        if (Program.config.overlay)
                        {
                            overlay.drawDots(result, 3500);
                        }

                        if (Program.config.speech)
                        {
                            Speech.text2speech(result);
                        }
                        result = "";
                    }
                }
            }

            return CallNextHookEx(Program._hookId, nCode, wParam, lParam);
        }

        public static string getKeyName(int vkCode)
        {
            Keys key = (Keys)vkCode;
            return key.ToString();
        }
    }
}
