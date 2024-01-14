using System.Drawing;
using System.Threading;

namespace StealthGPT
{
    internal class overlay
    {
        private static Charm charm = new Charm();
        private static bool instanceRunning = false;
        private static bool drawingEnabled = false;
        private static string answer = "";
        private static int answerTimeout = 0;
        private static bool initMSG = false;
        private static void drawXdots(Charm.RPM rpm, Charm.Renderer renderer, int width, int height)
        {
            if (!initMSG)
            {
                renderer.DrawString(5, height - 30, "Initialized!", Color.Red, 11);
                initMSG = true;
                Thread.Sleep(4000);
            }

            if (drawingEnabled)
            {
                renderer.DrawString(5, height - 30, answer, Color.Red, 11);
            }
            Thread.Sleep(50);
        }

        private static void dotTimer()
        {
            Thread.Sleep(answerTimeout);
            answer = "";
            answerTimeout = 0;
            drawingEnabled = false;
        }

        public static void drawDots(string answerx, int answerTimeoutx)
        {
            answer = answerx;
            answerTimeout = answerTimeoutx;
            drawingEnabled = true;
            Thread myThread = new Thread(dotTimer);
            myThread.Start();
        }

        public static void initializeOverlay()
        {
            if (Program.config._attachedProcessName != null && !instanceRunning)
            {
                charm.CharmInit(drawXdots, Program.config._attachedProcessName);
                instanceRunning = true;
            }
        }
    }
}
