using StealthGPT.Properties;
using System;
using System.Windows.Forms;

namespace StealthGPT
{
    internal class trayBar
    {
        public class ProcessIcon : IDisposable
        {
            public NotifyIcon trayIcon { get; set; }

            public ProcessIcon()
            {
                trayIcon = new NotifyIcon();
            }

            public void Display()
            {
                trayIcon.Text = "Steam";
                trayIcon.Icon = Resources.steam_ico;
                trayIcon.Visible = true;

                var contextMenu = new ContextMenu();
                contextMenu.MenuItems.Add(0, new MenuItem("Exit", (object sender, EventArgs e) => { Application.Exit(); }));
                contextMenu.MenuItems.Add(0, new MenuItem("Toggle GUI", new EventHandler(toggle_GUI)));

                trayIcon.ContextMenu = contextMenu;
            }

            public void Dispose()
            {
                trayIcon.Dispose();
            }

            private void toggle_GUI(object sender, EventArgs e)
            {
                Program.stealthGPT_GUI.Visible = !Program.stealthGPT_GUI.Visible;
            }
        }
    }
}
