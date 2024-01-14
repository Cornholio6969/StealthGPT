using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace StealthGPT
{
    internal class Image
    {
        public static string imageCapture2Base64()
        {
            Program.updateTitle("Capturing screen..");
            Bitmap screenshot = CaptureScreen();
            string filePath = GenerateRandomString(15) + ".png";
            SaveScreenshot(screenshot, filePath);
            string base64Image = ImageToBase64(filePath);
            File.Delete(filePath);
            return base64Image;
        }

        private static Bitmap CaptureScreen()
        {
            Bitmap screenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            using (Graphics g = Graphics.FromImage(screenshot))
            {
                g.CopyFromScreen(0, 0, 0, 0, screenshot.Size);
            }

            return screenshot;
        }

        private static void SaveScreenshot(Bitmap image, string filePath)
        {
            image.Save(filePath, ImageFormat.Png);
        }

        private static string ImageToBase64(string imagePath)
        {
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            return Convert.ToBase64String(imageBytes);
        }

        private static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();

            char[] randomArray = new char[length];
            for (int i = 0; i < length; i++)
            {
                randomArray[i] = chars[random.Next(chars.Length)];
            }

            return new string(randomArray);
        }
    }
}
