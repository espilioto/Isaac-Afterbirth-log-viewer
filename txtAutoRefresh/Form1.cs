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

        string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games\\Binding of Isaac Afterbirth+\\log.txt");
        string line = "";
        Stream stream;
        StreamReader streamReader;
        FileStream fileStream;

        bool cancel = false;

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Stop();

            if (File.Exists(path))
            {
                stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                streamReader = new StreamReader(stream);

                fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            }
            else
                if (MessageBox.Show("Game log not found in the default location") == DialogResult.OK)
                {
                    openFileDialog1.FileName = "log.txt";
                    openFileDialog1.Filter = "Isaac log file | *.txt";
                    openFileDialog1.Title = "Please locate the log file";

                    if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                    {
                        Application.Exit();
                    }
                    else
                    {
                        path = openFileDialog1.FileName;
                        stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        streamReader = new StreamReader(stream);

                        fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    }

                    this.Activate();
                }

            ColumnHeader header = new ColumnHeader();
            txtScan.HeaderStyle = ColumnHeaderStyle.None;
            txtScan.Columns.Add(header);
            txtScan.Columns[0].Width = 327;
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
                    //stream.Position = 0;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        textBox1.AppendText(line + Environment.NewLine);

                        if (line.StartsWith("[INFO] - RNG Start Seed:")) //regex returns the seed in this form: ((a-Z 0-9)x4)space((a-Z 0-9)x4)
                        {
                            if (txtScan.Items.Count > 0)
                            {
                                txtScan.Items.Add(Environment.NewLine);
                            }
                            ListViewItem seed = new ListViewItem("- - - - - - - - - - - - - - - New run: " + Regex.Match(line, @" (\w{4} \w{4}) ").Groups[1].Value + " - - - - - - - - - - - - - - -");
                            seed.BackColor = Color.Black;
                            seed.ForeColor = Color.White;
                            txtScan.Items.Add(seed);
                        }
                        if (line.StartsWith("[INFO] - Initialized player with"))
                        {
                            var charId = Regex.Match(line, @"Subtype (\d+)").Groups[1].Value;

                            switch (charId)
                            {
                                case "0":
                                    txtScan.Items.Add("Char: Isaac");
                                    break;
                                case "1":
                                    txtScan.Items.Add("Char: Magdalene");
                                    break;
                                case "2":
                                    txtScan.Items.Add("Char: Cain");
                                    break;
                                case "3":
                                    txtScan.Items.Add("Char: Judas");
                                    break;
                                case "4":
                                    txtScan.Items.Add("Char: ???");
                                    break;
                                case "5":
                                    txtScan.Items.Add("Char: Eve");
                                    break;
                                case "6":
                                    txtScan.Items.Add("Char: Samson");
                                    break;
                                case "7":
                                    txtScan.Items.Add("Char: Azazel");
                                    break;
                                case "8":
                                    txtScan.Items.Add("Char: Lazarus");
                                    break;
                                case "9":
                                    txtScan.Items.Add("Char: Eden");
                                    break;
                                case "10":
                                    txtScan.Items.Add("Char: The Lost");
                                    break;
                                case "11":
                                    txtScan.Items.Add("Char: Lilith");
                                    break;
                                case "12":
                                    txtScan.Items.Add("Char: Keeper");
                                    break;
                                case "15":
                                    txtScan.Items.Add("Char: Apollyon");
                                    break;
                                default:
                                    break;

                            }
                            txtScan.BackColor = Color.White;
                            txtScan.ForeColor = Color.Black;
                        }
                        else if (line.StartsWith("[INFO] - Level::Init"))
                        {
                            var stage = Regex.Match(line, @"m_Stage (\d+)").Groups[1].Value;            //regex stage id
                            var altStage = Regex.Match(line, @"m_StageType (\d+)").Groups[1].Value;      //regex alt stage id

                            if (altStage == null)
                            {
                                altStage = Regex.Match(line, @"m_AltStage (\d+)").Groups[1].Value;      //floor detection fix for Rebirth
                            }

                            if (stage == "1" && altStage == "0")                                                //chapter 1
                            {
                                txtScan.Items.Add("Stage: Basement I");
                            }
                            else if (stage == "1" && altStage == "1")
                            {
                                txtScan.Items.Add("Stage: Cellar I");
                            }
                            else if (stage == "1" && altStage == "2")
                            {
                                txtScan.Items.Add("Stage: Burning Basement II");
                            }
                            else if (stage == "2" && altStage == "0")
                            {
                                txtScan.Items.Add("Stage: Basement II");
                            }
                            else if (stage == "2" && altStage == "1")
                            {
                                txtScan.Items.Add("Stage: Cellar II");
                            }
                            else if (stage == "2" && altStage == "2")
                            {
                                txtScan.Items.Add("Stage: Burning Basement II");
                            }

                            else if (stage == "3" && altStage == "0")                                            //chapter 2
                            {
                                txtScan.Items.Add("Stage: Caves I");
                            }
                            else if (stage == "3" && altStage == "1")
                            {
                                txtScan.Items.Add("Stage: Catacombs I");
                            }
                            else if (stage == "3" && altStage == "2")
                            {
                                txtScan.Items.Add("Stage: Flooded Caves I");
                            }
                            else if (stage == "4" && altStage == "0")
                            {
                                txtScan.Items.Add("Stage: Caves II");
                            }
                            else if (stage == "4" && altStage == "1")
                            {
                                txtScan.Items.Add("Stage: Catacombs II");
                            }
                            else if (stage == "4" && altStage == "2")
                            {
                                txtScan.Items.Add("Stage: Flooded Caves II");
                            }

                            else if (stage == "5" && altStage == "0")                                            //chapter 3
                            {
                                txtScan.Items.Add("Stage: Depths I");
                            }
                            else if (stage == "5" && altStage == "1")
                            {
                                txtScan.Items.Add("Stage: Necropolis I");
                            }
                            else if (stage == "5" && altStage == "2")
                            {
                                txtScan.Items.Add("Stage: Dank Depths I");
                            }
                            else if (stage == "6" && altStage == "0")
                            {
                                txtScan.Items.Add("Stage: Depths II");
                            }
                            else if (stage == "6" && altStage == "1")
                            {
                                txtScan.Items.Add("Stage: Necropolis II");
                            }
                            else if (stage == "6" && altStage == "2")
                            {
                                txtScan.Items.Add("Stage: Dank Depths II");
                            }

                            else if (stage == "7" && altStage == "0")                                            //chapter 4
                            {
                                txtScan.Items.Add("Stage: Womb I");
                            }
                            else if (stage == "7" && altStage == "1")
                            {
                                txtScan.Items.Add("Stage: Utero I");
                            }
                            else if (stage == "7" && altStage == "2")
                            {
                                txtScan.Items.Add("Stage: Scarred Womb  I");
                            }
                            else if (stage == "8" && altStage == "0")
                            {
                                txtScan.Items.Add("Stage: Womb II");
                            }
                            else if (stage == "8" && altStage == "1")
                            {
                                txtScan.Items.Add("Stage: Utero II");
                            }
                            else if (stage == "8" && altStage == "2")
                            {
                                txtScan.Items.Add("Stage: Scarred Womb  II");
                            }

                            else if (stage == "9" && altStage == "0")
                            {
                                txtScan.Items.Add("Stage: ???");
                            }

                            else if (stage == "10" && altStage == "0")                                            //chapter 5
                            {
                                txtScan.Items.Add("Stage: Sheol");
                            }
                            else if (stage == "10" && altStage == "1")
                            {
                                txtScan.Items.Add("Stage: Cathedral");
                            }

                            else if (stage == "11" && altStage == "0")                                           //chapter 6
                            {
                                txtScan.Items.Add("Stage: Dark Room");
                            }
                            else if (stage == "11" && altStage == "1")
                            {
                                txtScan.Items.Add("Stage: Chest");
                            }
                        }
                        if (line.StartsWith("[INFO] - Curse of"))
                        {
                            string curse = string.Empty;

                            if (line.Contains("Maze"))
                            {
                                txtScan.Items.Add("Curse of the Maze");
                            }
                            else if (line.Contains("Blind"))
                            {
                                txtScan.Items.Add("Curse of the Blind");
                            }
                            else if (line.Contains("Lost"))
                            {
                                txtScan.Items.Add("Curse of the Lost!");
                            }
                            else if (line.Contains("Unknown"))
                            {
                                txtScan.Items.Add("Curse of the Unknown");
                            }
                            else if (line.Contains("Darkness"))
                            {
                                txtScan.Items.Add("Curse of Darkness");
                            }
                            else if (line.Contains("Labyrinth"))                                        //if it's an XL floor
                            {
                                txtScan.Items.Add("Curse of the Labyrinth");
                            }
                        }
                        if (line.StartsWith("[INFO] - Adding collectible ")) //regex returns the text inside the parentheses (item name)
                        {
                            ListViewItem item = new ListViewItem("+Item: " + (Regex.Match(line, @"\(([^)]*)\)").Groups[1].Value) + " (ID: " + (Regex.Match(line, @"(\d+)").Groups[1].Value) + ")");
                            item.ForeColor = Color.DarkGoldenrod;
                            txtScan.Items.Add(item);
                        }
                        if (line.StartsWith("[INFO] - Game Over"))
                        {
                            ListViewItem rip = new ListViewItem("Game over :<");
                            rip.BackColor = Color.Black;
                            rip.ForeColor = Color.Red;
                            txtScan.Items.Add(rip);
                        }
                        if (line.StartsWith("[INFO] - playing cutscene"))
                        {
                            if (line.StartsWith("[INFO] - playing cutscene 1 (Intro).")) //don't match the intro cutscene that plays on every launch
                            {
                                ; ;
                            }
                            else
                            {
                                ListViewItem win = new ListViewItem("Victory!");
                                win.BackColor = Color.Black;
                                win.ForeColor = Color.Green;
                                txtScan.Items.Add(win);
                            }
                        }
                        if (Regex.Match(line, (@"Room \d\.\d{4}\(")).Success) //regex returns the boss name
                        {
                            ListViewItem boss = new ListViewItem("Boss: " + (Regex.Match(line, @"\(([^(]+)").Groups[1].Value.Trim(')')));
                            boss.ForeColor = Color.Red;
                            txtScan.Items.Add(boss);
                        }
                    }
                }

                catch (FileNotFoundException ex)
                {
                    MessageBox.Show(ex.Data + ex.Message);
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
            var file = new StreamReader(fileStream, Encoding.UTF8, true, 128);

            timer1_Tick(this, null);

            file.Dispose();
        }
    }
}
