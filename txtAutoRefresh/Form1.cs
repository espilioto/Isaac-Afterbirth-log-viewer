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
        FileStream fileStream;

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

                fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
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
                    //stream.Position = 0;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        textBox1.AppendText(line + Environment.NewLine);

                        if (line.StartsWith("[INFO] - RNG Start Seed:")) //regex returns the seed in this form: ((a-Z 0-9)x4)space((a-Z 0-9)x4)
                        {
                            if (txtScan.Lines.Count() > 0)
                            {
                                txtScan.AppendText(Environment.NewLine);
                            }
                            txtScan.AppendText(Environment.NewLine + "- - - - - - - - - - - New run: " + Regex.Match(line, @" (\w{4} \w{4}) ").Groups[1].Value + Environment.NewLine);
                        }
                        if (line.StartsWith("[INFO] - Initialized player with"))
                        {
                            var character = Regex.Match(line, @"Subtype (\d+)").Groups[1].Value;

                            switch (character)
                            {
                                case "0":
                                    txtScan.AppendText("--: Char: Isaac" + Environment.NewLine);
                                    break;
                                case "1":
                                    txtScan.AppendText("--: Char: Magdalene" + Environment.NewLine);
                                    break;
                                case "2":
                                    txtScan.AppendText("--: Char: Cain" + Environment.NewLine);
                                    break;
                                case "3":
                                    txtScan.AppendText("--: Char: Judas" + Environment.NewLine);
                                    break;
                                case "4":
                                    txtScan.AppendText("--: Char: ???" + Environment.NewLine);
                                    break;
                                case "5":
                                    txtScan.AppendText("--: Char: Eve" + Environment.NewLine);
                                    break;
                                case "6":
                                    txtScan.AppendText("--: Char: Samson" + Environment.NewLine);
                                    break;
                                case "7":
                                    txtScan.AppendText("--: Char: Azazel" + Environment.NewLine);
                                    break;
                                case "8":
                                    txtScan.AppendText("--: Char: Lazarus" + Environment.NewLine);
                                    break;
                                case "9":
                                    txtScan.AppendText("--: Char: Eden" + Environment.NewLine);
                                    break;
                                case "10":
                                    txtScan.AppendText("--: Char: The Lost" + Environment.NewLine);
                                    break;
                                case "11":
                                    txtScan.AppendText("--: Char: Lilith" + Environment.NewLine);
                                    break;
                                case "12":
                                    txtScan.AppendText("--: Char: Keeper" + Environment.NewLine);
                                    break;
                                case "15":
                                    txtScan.AppendText("--: Char: Apollyon" + Environment.NewLine);
                                    break;
                                default:
                                    break;
                            }
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
                                txtScan.AppendText("--: Stage: Basement I" + Environment.NewLine);
                            }
                            else if (stage == "1" && altStage == "1")
                            {
                                txtScan.AppendText("--: Stage: Cellar I" + Environment.NewLine);
                            }
                            else if (stage == "1" && altStage == "2")
                            {
                                txtScan.AppendText("--: Stage: Burning Basement II" + Environment.NewLine);
                            }
                            else if (stage == "2" && altStage == "0")
                            {
                                txtScan.AppendText("--: Stage: Basement II" + Environment.NewLine);
                            }
                            else if (stage == "2" && altStage == "1")
                            {
                                txtScan.AppendText("--: Stage: Cellar II" + Environment.NewLine);
                            }
                            else if (stage == "2" && altStage == "2")
                            {
                                txtScan.AppendText("--: Stage: Burning Basement II" + Environment.NewLine);
                            }

                            else if (stage == "3" && altStage == "0")                                            //chapter 2
                            {
                                txtScan.AppendText("--: Stage: Caves I" + Environment.NewLine);
                            }
                            else if (stage == "3" && altStage == "1")
                            {
                                txtScan.AppendText("--: Stage: Catacombs I" + Environment.NewLine);
                            }
                            else if (stage == "3" && altStage == "2")
                            {
                                txtScan.AppendText("--: Stage: Flooded Caves I" + Environment.NewLine);
                            }
                            else if (stage == "4" && altStage == "0")
                            {
                                txtScan.AppendText("--: Stage: Caves II" + Environment.NewLine);
                            }
                            else if (stage == "4" && altStage == "1")
                            {
                                txtScan.AppendText("--: Stage: Catacombs II" + Environment.NewLine);
                            }
                            else if (stage == "4" && altStage == "2")
                            {
                                txtScan.AppendText("--: Stage: Flooded Caves II" + Environment.NewLine);
                            }

                            else if (stage == "5" && altStage == "0")                                            //chapter 3
                            {
                                txtScan.AppendText("--: Stage: Depths I" + Environment.NewLine);
                            }
                            else if (stage == "5" && altStage == "1")
                            {
                                txtScan.AppendText("--: Stage: Necropolis I" + Environment.NewLine);
                            }
                            else if (stage == "5" && altStage == "2")
                            {
                                txtScan.AppendText("--: Stage: Dank Depths I" + Environment.NewLine);
                            }
                            else if (stage == "6" && altStage == "0")
                            {
                                txtScan.AppendText("--: Stage: Depths II" + Environment.NewLine);
                            }
                            else if (stage == "6" && altStage == "1")
                            {
                                txtScan.AppendText("--: Stage: Necropolis II" + Environment.NewLine);
                            }
                            else if (stage == "6" && altStage == "2")
                            {
                                txtScan.AppendText("--: Stage: Dank Depths II" + Environment.NewLine);
                            }

                            else if (stage == "7" && altStage == "0")                                            //chapter 4
                            {
                                txtScan.AppendText("--: Stage: Womb I" + Environment.NewLine);
                            }
                            else if (stage == "7" && altStage == "1")
                            {
                                txtScan.AppendText("--: Stage: Utero I" + Environment.NewLine);
                            }
                            else if (stage == "7" && altStage == "2")
                            {
                                txtScan.AppendText("--: Stage: Scarred Womb  I" + Environment.NewLine);
                            }
                            else if (stage == "8" && altStage == "0")
                            {
                                txtScan.AppendText("--: Stage: Womb II" + Environment.NewLine);
                            }
                            else if (stage == "8" && altStage == "1")
                            {
                                txtScan.AppendText("--: Stage: Utero II" + Environment.NewLine);
                            }
                            else if (stage == "8" && altStage == "2")
                            {
                                txtScan.AppendText("--: Stage: Scarred Womb  II" + Environment.NewLine);
                            }

                            else if (stage == "9" && altStage == "0")
                            {
                                txtScan.AppendText("--: Stage: ???" + Environment.NewLine);
                            }

                            else if (stage == "10" && altStage == "0")                                            //chapter 5
                            {
                                txtScan.AppendText("--: Stage: Sheol" + Environment.NewLine);
                            }
                            else if (stage == "10" && altStage == "1")
                            {
                                txtScan.AppendText("--: Stage: Cathedral" + Environment.NewLine);
                            }

                            else if (stage == "11" && altStage == "0")                                           //chapter 6
                            {
                                txtScan.AppendText("--: Stage: Dark Room" + Environment.NewLine);
                            }
                            else if (stage == "11" && altStage == "1")
                            {
                                txtScan.AppendText("--: Stage: Chest" + Environment.NewLine);
                            }
                        }
                        if (line.StartsWith("[INFO] - Adding collectible ")) //regex returns the text inside the parentheses (item name)
                        {
                            txtScan.AppendText("+I: " + (Regex.Match(line, @"\(([^)]*)\)").Groups[1].Value) + Environment.NewLine); //item name
                            //txtScan.AppendText("+I: " + (Regex.Match(line, @"(\d+)").Groups[1].Value) + Environment.NewLine);     //item id
                        }
                        if (line.StartsWith("[INFO] - Game Over"))
                        {
                            txtScan.AppendText("--: " + ("Game over :<") + Environment.NewLine);
                        }
                        if (line.StartsWith("[INFO] - playing cutscene"))
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
                        if (Regex.Match(line, (@"Room \d\.\d{4}\(")).Success) //regex returns the boss name
                        {
                            txtScan.AppendText("+B: " + (Regex.Match(line, @"\(([^(]+)").Groups[1].Value.Trim(')')) + Environment.NewLine);
                        }
                    }
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
            var file = new StreamReader(fileStream, Encoding.UTF8, true, 128);

            try
            {
                while ((line = file.ReadLine()) != null)
                {
                    if (line.StartsWith("[INFO] - RNG Start Seed:")) //regex returns the seed in this form: ((a-Z 0-9)x4)space((a-Z 0-9)x4)
                    {
                        if (txtScan.Lines.Count() > 0)
                        {
                            txtScan.AppendText(Environment.NewLine);
                        }
                        txtScan.AppendText(Environment.NewLine + "- - - - - - - - - - - New run: " + Regex.Match(line, @" (\w{4} \w{4}) ").Groups[1].Value + Environment.NewLine);
                    }
                    if (line.StartsWith("[INFO] - Initialized player with"))
                    {
                        var character = Regex.Match(line, @"Subtype (\d+)").Groups[1].Value;

                        switch (character)
                        {
                            case "0":
                                txtScan.AppendText("--: Char: Isaac");
                                break;
                            case "1":
                                txtScan.AppendText("--: Char: Magdalene");
                                break;
                            case "2":
                                txtScan.AppendText("--: Char: Cain");
                                break;
                            case "3":
                                txtScan.AppendText("--: Char: Judas");
                                break;
                            case "4":
                                txtScan.AppendText("--: Char: ???");
                                break;
                            case "5":
                                txtScan.AppendText("--: Char: Eve");
                                break;
                            case "6":
                                txtScan.AppendText("--: Char: Samson");
                                break;
                            case "7":
                                txtScan.AppendText("--: Char: Azazel");
                                break;
                            case "8":
                                txtScan.AppendText("--: Char: Lazarus");
                                break;
                            case "9":
                                txtScan.AppendText("--: Char: Eden");
                                break;
                            case "10":
                                txtScan.AppendText("--: Char: The Lost");
                                break;
                            case "11":
                                txtScan.AppendText("--: Char: Lilith");
                                break;
                            case "12":
                                txtScan.AppendText("--: Char: Keeper");
                                break;
                            case "15":
                                txtScan.AppendText("--: Char: Apollyon");
                                break;
                            default:
                                break;
                        }
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
                            txtScan.AppendText("--: Stage: Basement I" + Environment.NewLine);
                        }
                        else if (stage == "1" && altStage == "1")
                        {
                            txtScan.AppendText("--: Stage: Cellar I" + Environment.NewLine);
                        }
                        else if (stage == "1" && altStage == "2")
                        {
                            txtScan.AppendText("--: Stage: Burning Basement II" + Environment.NewLine);
                        }
                        else if (stage == "2" && altStage == "0")
                        {
                            txtScan.AppendText("--: Stage: Basement II" + Environment.NewLine);
                        }
                        else if (stage == "2" && altStage == "1")
                        {
                            txtScan.AppendText("--: Stage: Cellar II" + Environment.NewLine);
                        }
                        else if (stage == "2" && altStage == "2")
                        {
                            txtScan.AppendText("--: Stage: Burning Basement II" + Environment.NewLine);
                        }

                        else if (stage == "3" && altStage == "0")                                            //chapter 2
                        {
                            txtScan.AppendText("--: Stage: Caves I" + Environment.NewLine);
                        }
                        else if (stage == "3" && altStage == "1")
                        {
                            txtScan.AppendText("--: Stage: Catacombs I" + Environment.NewLine);
                        }
                        else if (stage == "3" && altStage == "2")
                        {
                            txtScan.AppendText("--: Stage: Flooded Caves I" + Environment.NewLine);
                        }
                        else if (stage == "4" && altStage == "0")
                        {
                            txtScan.AppendText("--: Stage: Caves II" + Environment.NewLine);
                        }
                        else if (stage == "4" && altStage == "1")
                        {
                            txtScan.AppendText("--: Stage: Catacombs II" + Environment.NewLine);
                        }
                        else if (stage == "4" && altStage == "2")
                        {
                            txtScan.AppendText("--: Stage: Flooded Caves II" + Environment.NewLine);
                        }

                        else if (stage == "5" && altStage == "0")                                            //chapter 3
                        {
                            txtScan.AppendText("--: Stage: Depths I" + Environment.NewLine);
                        }
                        else if (stage == "5" && altStage == "1")
                        {
                            txtScan.AppendText("--: Stage: Necropolis I" + Environment.NewLine);
                        }
                        else if (stage == "5" && altStage == "2")
                        {
                            txtScan.AppendText("--: Stage: Dank Depths I" + Environment.NewLine);
                        }
                        else if (stage == "6" && altStage == "0")
                        {
                            txtScan.AppendText("--: Stage: Depths II" + Environment.NewLine);
                        }
                        else if (stage == "6" && altStage == "1")
                        {
                            txtScan.AppendText("--: Stage: Necropolis II" + Environment.NewLine);
                        }
                        else if (stage == "6" && altStage == "2")
                        {
                            txtScan.AppendText("--: Stage: Dank Depths II" + Environment.NewLine);
                        }

                        else if (stage == "7" && altStage == "0")                                            //chapter 4
                        {
                            txtScan.AppendText("--: Stage: Womb I" + Environment.NewLine);
                        }
                        else if (stage == "7" && altStage == "1")
                        {
                            txtScan.AppendText("--: Stage: Utero I" + Environment.NewLine);
                        }
                        else if (stage == "7" && altStage == "2")
                        {
                            txtScan.AppendText("--: Stage: Scarred Womb  I" + Environment.NewLine);
                        }
                        else if (stage == "8" && altStage == "0")
                        {
                            txtScan.AppendText("--: Stage: Womb II" + Environment.NewLine);
                        }
                        else if (stage == "8" && altStage == "1")
                        {
                            txtScan.AppendText("--: Stage: Utero II" + Environment.NewLine);
                        }
                        else if (stage == "8" && altStage == "2")
                        {
                            txtScan.AppendText("--: Stage: Scarred Womb  II" + Environment.NewLine);
                        }

                        else if (stage == "9" && altStage == "0")
                        {
                            txtScan.AppendText("--: Stage: ???" + Environment.NewLine);
                        }

                        else if (stage == "10" && altStage == "0")                                            //chapter 5
                        {
                            txtScan.AppendText("--: Stage: Sheol" + Environment.NewLine);
                        }
                        else if (stage == "10" && altStage == "1")
                        {
                            txtScan.AppendText("--: Stage: Cathedral" + Environment.NewLine);
                        }

                        else if (stage == "11" && altStage == "0")                                           //chapter 6
                        {
                            txtScan.AppendText("--: Stage: Dark Room" + Environment.NewLine);
                        }
                        else if (stage == "11" && altStage == "1")
                        {
                            txtScan.AppendText("--: Stage: Chest" + Environment.NewLine);
                        }
                    }
                    if (line.StartsWith("[INFO] - Adding collectible ")) //regex returns the text inside the parentheses (item name)
                    {
                        txtScan.AppendText("+I: " + (Regex.Match(line, @"\(([^)]*)\)").Groups[1].Value) + Environment.NewLine); //item name
                                                                                                                                //txtScan.AppendText("+I: " + (Regex.Match(line, @"(\d+)").Groups[1].Value) + Environment.NewLine);     //item id
                    }
                    if (line.StartsWith("[INFO] - Game Over"))
                    {
                        txtScan.AppendText("--: " + ("Game over :<") + Environment.NewLine);
                    }
                    if (line.StartsWith("[INFO] - playing cutscene"))
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
                    if (Regex.Match(line, (@"Room \d\.\d{4}\(")).Success) //regex returns the boss name
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
                file.Dispose();
            }
        }

        private void txtScan_TextChanged(object sender, EventArgs e)
        {
            txtScan.ScrollToCaret();
        }
    }
}
