using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace txtAutoRefresh
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string path = "";
        string line = "";
        Stream stream;
        StreamReader streamReader;
        bool cancel = false;

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Stop();

            openFileDialog1.FileName = "log.txt";
            openFileDialog1.Filter = "Text Files | *.txt*";
            openFileDialog1.Title = "Please locate the file you want to open";

            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                Application.Exit();
            }
            else
            {
                path = openFileDialog1.FileName;
                stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                streamReader = new StreamReader(stream);
            }

            this.Activate();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!cancel)
            {
                toolStripStatusLabel1.Text += ".";
                if (toolStripStatusLabel1.Text == "Refreshing....")
                {
                    toolStripStatusLabel1.Text = "Refreshing";
                }

                try
                {
                    stream.Position = 0;
                    textBox1.Clear();
                    textBox1.AppendText(streamReader.ReadToEnd());
                }
                catch (FileNotFoundException ex)
                {
                    MessageBox.Show(ex.Data + Environment.NewLine + ex.Message);
                }
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            cancel = false;
            toolStripStatusLabel1.Text = "Refreshing";

            timer1.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            cancel = true;

            toolStripStatusLabel1.Text = "Stopped";

            timer1.Stop();
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            var filestream = new FileStream(path,
                                          FileMode.Open,
                                          FileAccess.Read,
                                          FileShare.ReadWrite);
            var file = new StreamReader(filestream, Encoding.UTF8, true, 128);

            try
            {
                while ((line = file.ReadLine()) != null)
                {
                    if (line.StartsWith("RNG Start Seed:"))
                    {
                        txtScan.AppendText(Environment.NewLine + "--: New run: " + Regex.Match(line, @" (\w{4} \w{4}) ").Groups[1].Value + Environment.NewLine);
                    }
                    if (line.StartsWith("Adding collectible "))
                    {
                        txtScan.AppendText("+I: " + (Regex.Match(line, @"\(([^)]*)\)").Groups[1].Value) + Environment.NewLine);
                    }
                    if (line.StartsWith("Game Over"))
                    {
                        txtScan.AppendText("--: " + ("Game over :<") + Environment.NewLine);
                    }
                    if (line.StartsWith("playing cutscene"))
                    {
                        if (line.StartsWith("playing cutscene 1 (Intro).")) //don't match the intro cutscene that plays on every launch
                        {
                            ; ;
                        }
                        else
                        {
                            txtScan.AppendText("--: " + ("Victory!" + Environment.NewLine));
                        }
                    }
                    if (Regex.Match(line, (@"Room \d\.\d{4}\(")).Success)
                    {
                        txtScan.AppendText("+B: " + (Regex.Match(line, @"\(([^(]+)").Groups[1].Value.Trim(')')) + Environment.NewLine);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Data + ex.Message);
            }
            finally
            {
                filestream.Dispose();
                file.Dispose();
            }
        }

    }
}
