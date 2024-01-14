using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace StealthGPT
{
    public partial class stealthGPT : Form
    {
        public static bool loaded = false;
        public static bool waitForTitleKey = false;
        public static bool waitForQuizKey = false;
        public static bool waitForQueryKey = false;
        public static bool waitForPanicKey = false;
        public stealthGPT()
        {
            InitializeComponent();
            setOverlayAttachProcesses();
            textBox1.Text = globalKeyboard.getKeyName(Program.config.TitleKey);
            textBox2.Text = globalKeyboard.getKeyName(Program.config.QuizKey);
            textBox3.Text = globalKeyboard.getKeyName(Program.config.QueryKey);
            textBox5.Text = globalKeyboard.getKeyName(Program.config.PanicKey);
            textBox4.Text = Program.config._gptApiKey;
            comboBox1.SelectedItem = Program.config._gptModel;
            comboBox2.SelectedItem = Program.config._attachedProcessName;
            richTextBox1.Text = Program.config._gptStartPrompt;
            checkBox1.Checked = Program.config.setClipboard;
            checkBox2.Checked = Program.config.speech;
            checkBox3.Checked = Program.config.instaCopy;
            checkBox4.Checked = Program.config.autoMode;
            checkBox5.Checked = Program.config.enabled;
            loaded = true;
        }

        public static void setConfig()
        {
            if (loaded)
            {
                Program.stealthGPT_GUI.textBox1.Text = globalKeyboard.getKeyName(Program.config.TitleKey);
                Program.stealthGPT_GUI.textBox2.Text = globalKeyboard.getKeyName(Program.config.QuizKey);
                Program.stealthGPT_GUI.textBox3.Text = globalKeyboard.getKeyName(Program.config.QueryKey);
                Program.stealthGPT_GUI.textBox5.Text = globalKeyboard.getKeyName(Program.config.PanicKey);
                Program.stealthGPT_GUI.textBox4.Text = Program.config._gptApiKey;
                Program.stealthGPT_GUI.comboBox1.SelectedItem = Program.config._gptModel;
                Program.stealthGPT_GUI.comboBox2.SelectedItem = Program.config._attachedProcessName;
                Program.stealthGPT_GUI.richTextBox1.Text = Program.config._gptStartPrompt;
                Program.stealthGPT_GUI.checkBox1.Checked = Program.config.setClipboard;
                Program.stealthGPT_GUI.checkBox2.Checked = Program.config.speech;
                Program.stealthGPT_GUI.checkBox3.Checked = Program.config.instaCopy;
                Program.stealthGPT_GUI.checkBox4.Checked = Program.config.autoMode;
                Program.stealthGPT_GUI.checkBox5.Checked = Program.config.enabled;
            }
        }

        private void setOverlayAttachProcesses()
        {
            Process[] processes = Process.GetProcesses();
            Dictionary<string, Process> uniqueProcesses = new Dictionary<string, Process>();

            foreach (Process process in processes)
            {
                if (!uniqueProcesses.ContainsKey(process.ProcessName))
                {
                    uniqueProcesses.Add(process.ProcessName, process);
                }
            }

            Process[] uniqueProcessesArray = uniqueProcesses.Values.ToArray();
            foreach (Process process in uniqueProcessesArray)
            {
                comboBox2.Items.Add(process.ProcessName);
            }
        }

        private void Form1_Load(object sender, EventArgs e) { this.Hide(); }

        private void textBox1_DoubleClick(object sender, EventArgs e)
        {
            if (waitForQuizKey || waitForQueryKey || waitForPanicKey)
            {
                textBox2.Text = globalKeyboard.getKeyName(Program.config.QuizKey);
                textBox3.Text = globalKeyboard.getKeyName(Program.config.QueryKey);
                textBox5.Text = globalKeyboard.getKeyName(Program.config.PanicKey);
                waitForQuizKey = false;
                waitForQueryKey = false;
                waitForPanicKey = false;
            }
            waitForTitleKey = true;
            textBox1.Text = "< Enter key >";
        }

        private void textBox2_DoubleClick(object sender, EventArgs e)
        {
            if (waitForTitleKey || waitForQueryKey || waitForPanicKey)
            {
                textBox1.Text = globalKeyboard.getKeyName(Program.config.TitleKey);
                textBox3.Text = globalKeyboard.getKeyName(Program.config.QueryKey);
                textBox5.Text = globalKeyboard.getKeyName(Program.config.PanicKey);
                waitForTitleKey = false;
                waitForQueryKey = false;
                waitForPanicKey = false;
            }
            waitForQuizKey = true;
            textBox2.Text = "< Enter key >";
        }

        private void textBox3_DoubleClick(object sender, EventArgs e)
        {
            if (waitForTitleKey || waitForQuizKey || waitForPanicKey)
            {
                textBox1.Text = globalKeyboard.getKeyName(Program.config.TitleKey);
                textBox2.Text = globalKeyboard.getKeyName(Program.config.QuizKey);
                textBox5.Text = globalKeyboard.getKeyName(Program.config.PanicKey);
                waitForTitleKey = false;
                waitForQuizKey = false;
                waitForPanicKey = false;
            }
            waitForQueryKey = true;
            textBox3.Text = "< Enter key >";
        }

        private void textBox4_TextChanged(object sender, EventArgs e) { Program.config._gptApiKey = textBox4.Text; }

        private void textBox5_DoubleClick(object sender, EventArgs e)
        {
            if (waitForTitleKey || waitForQuizKey || waitForPanicKey)
            {
                textBox1.Text = globalKeyboard.getKeyName(Program.config.TitleKey);
                textBox2.Text = globalKeyboard.getKeyName(Program.config.QuizKey);
                textBox5.Text = globalKeyboard.getKeyName(Program.config.QueryKey);
                waitForTitleKey = false;
                waitForQuizKey = false;
                waitForQueryKey = false;
            }
            waitForPanicKey = true;
            textBox5.Text = "< Enter key >";
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e) { Program.config._gptStartPrompt = richTextBox1.Text; }

        private void checkBox1_CheckStateChanged(object sender, EventArgs e) { Program.config.setClipboard = checkBox1.Checked; }

        private void checkBox2_CheckStateChanged(object sender, EventArgs e) { Program.config.speech = checkBox2.Checked; }

        private void checkBox3_CheckStateChanged(object sender, EventArgs e) { Program.config.instaCopy = checkBox3.Checked; }

        private void checkBox4_CheckStateChanged(object sender, EventArgs e)
        {
            Program.config.autoMode = checkBox4.Checked;
        }

        private void checkBox5_CheckStateChanged(object sender, EventArgs e) { Program.config.enabled = checkBox5.Checked; }

        private void checkBox6_CheckStateChanged(object sender, EventArgs e)
        {
            Program.config.overlay = checkBox6.Checked;
            if (checkBox6.Checked)
            {
                overlay.initializeOverlay();
            }
        }

        private void stealthGPT_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
                this.Hide();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) { Program.config._gptModel = comboBox1.SelectedItem.ToString(); }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e) { Program.config._attachedProcessName = comboBox2.SelectedItem.ToString(); }

        private void textBox1_Leave(object sender, EventArgs e) { }
    }
}
