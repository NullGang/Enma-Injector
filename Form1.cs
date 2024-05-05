using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Media;
using System.Windows.Forms;
using System.Windows.Media;

namespace Enma_Injector
{
    public partial class Form1 : Form
    {
        private MediaPlayer m_mediaPlayer;
        string origen = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "injectfolder");

        string settingsvolume = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "assets", "config");
        string destino = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "apktool", "ywapk");
        List<string> list = new List<string>();
        public Form1()
        {
            InitializeComponent();

        }
        List<string> argcomandos = new List<string>();

        private void gunaGradientPanel1_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        static void ReplaceFolder(string origen, string destino)
        {
            Directory.Move(origen, destino);

            foreach (string subFolder in Directory.GetDirectories(destino))
            {
                string subDestino = Path.Combine(destino, Path.GetFileName(subFolder));
                string subOrigen = Path.Combine(origen, Path.GetFileName(subFolder));
                ReplaceFolder(subOrigen, subDestino);
            }

            foreach (string file in Directory.GetFiles(destino))
            {
                string fileName = Path.GetFileName(file);
                string sourceFile = Path.Combine(origen, fileName);
                string destFile = Path.Combine(destino, fileName);
                ReplaceFile(sourceFile, destFile);
            }
        }

        static void ReplaceFile(string origen, string destino)
        {
            File.Replace(origen, destino, null);
        }

        private void gunaAdvenceButton1_Click(object sender, EventArgs e)
        {
            SoundPlayer player = new SoundPlayer(Application.StartupPath + "/assets/music/btn_sound.wav");
            player.Play();
           if(gunaCheckBox1.Checked == true)
            {
                openFileDialog1.Multiselect = true;
               
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    foreach (string filename in openFileDialog1.FileNames)
                    {
                        string normalname = Path.GetFileName(filename).Replace(".apk", "");
                        string randomName = Path.GetFileNameWithoutExtension(normalname).Replace(" ", "_");
                        string fileExtension = Path.GetExtension(filename);
                        string newFileName = randomName + ".apk";

                        string targetFolderPath = Path.Combine(Application.StartupPath, "apktool");

                       
                        string targetFilePath = Path.Combine(targetFolderPath, newFileName);

                        try
                        {
                            File.Copy(filename, targetFilePath);
                            ProcessStartInfo processStart = new ProcessStartInfo();
                            processStart.FileName = "cmd.exe";
                            processStart.WindowStyle = ProcessWindowStyle.Normal;
                            processStart.Arguments = @"/k cd apktool && apktool.bat d -f "+ newFileName + " && exit";
                            Process.Start(processStart);
                            list.Add(Path.GetFileNameWithoutExtension(randomName.Replace(".apk", "")));
                            gunaAdvenceButton3.Enabled = true;
                            gunaAdvenceButton1.Enabled = false;
                        }
                        catch (IOException ex)
                        {
                            MessageBox.Show("Error coping the files: " + ex.Message); 
                            gunaAdvenceButton3.Enabled = false;
                            gunaAdvenceButton1.Enabled = true;
                        }
                        
                    }
                    //Process.Start("explorer.exe", Application.StartupPath + "/apktool/");

                }
            } else
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string archivoSeleccionado = openFileDialog1.FileName;
                    string nombreArchivo = Path.GetFileNameWithoutExtension(archivoSeleccionado);
                    string destinoCarpeta = Path.Combine(Application.StartupPath, "apktool");
                    string destino = Path.Combine(destinoCarpeta, "ywapk.apk");

                    if (!File.Exists(destino))
                    {
                        File.Copy(archivoSeleccionado, destino, true);
                    }


                    ProcessStartInfo processStart = new ProcessStartInfo();
                    processStart.FileName = "cmd.exe";
                    processStart.WindowStyle = ProcessWindowStyle.Normal;
                    processStart.Arguments = @"/k cd apktool && apktool.bat d -f ywapk.apk && exit";
                    Process.Start(processStart);
                    gunaAdvenceButton3.Enabled = true;
                    gunaAdvenceButton1.Enabled = false;
                    //Process.Start("explorer.exe", Application.StartupPath + "/apktool/");

                }
            }
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            m_mediaPlayer = new MediaPlayer();
            m_mediaPlayer.Open(new Uri(Application.StartupPath + "/assets/music/enmavilla.wav"));
            m_mediaPlayer.MediaEnded += (senderr, ee) =>
            {
                m_mediaPlayer.Position = TimeSpan.Zero;
                m_mediaPlayer.Play();
            };
            m_mediaPlayer.Play();
            string dest = Application.StartupPath + "/assets/config/startmusic";
            using (StreamReader sr = File.OpenText(dest))
            {
                int output = int.Parse(sr.ReadLine());
                m_mediaPlayer.Volume = output / 100.0f;
                gunaMetroVTrackBar1.Value = output;
            }
        }

        private void gunaAdvenceButton4_Click(object sender, EventArgs e)
        {
            SoundPlayer player = new SoundPlayer(Application.StartupPath + "/assets/music/btn_sound.wav");
            player.Play();
            if(gunaCheckBox1.Checked == true)
            {

                    foreach(string filename in list)
                    {
                        ProcessStartInfo processStart = new ProcessStartInfo();
                        processStart.FileName = "cmd.exe";
                        processStart.WindowStyle = ProcessWindowStyle.Normal;
                        processStart.Arguments = @"/k cd ./apktool && apktool b " + filename + " && exit";
                        Process.Start(processStart);
                    }
                
            } else
            {
                ProcessStartInfo processStart = new ProcessStartInfo();
                processStart.FileName = "cmd.exe";
                processStart.WindowStyle = ProcessWindowStyle.Normal;
                processStart.Arguments = @"/k cd ./apktool && apktool b ywapk && exit";
                Process.Start(processStart);
            }
                gunaAdvenceButton5.Enabled = true;
                gunaAdvenceButton4.Enabled = false;
              

        }

        private void gunaAdvenceButton5_Click(object sender, EventArgs e)
        {
            SoundPlayer player = new SoundPlayer(Application.StartupPath + "/assets/music/btn_sound.wav");
            player.Play();
            if (gunaCheckBox1.Checked == true)
            {
                
                foreach (string filename in list)
                {
                    string origen = Application.StartupPath + @"\apktool\" + filename+ @"\dist\";
                    string destinox = Application.StartupPath + @"\apktool\sign";
                    if (!Directory.Exists(destinox))
                    {
                        Directory.CreateDirectory(destinox);
                    }
                    string[] archivosApk = Directory.GetFiles(origen, "*.apk");
                    foreach (string archivoApk in archivosApk)
                    {
                        string nombreArchivo = Path.GetFileName(archivoApk);
                        string destino = Path.Combine(destinox, nombreArchivo);
                        File.Move(archivoApk, destino);
                        ProcessStartInfo processStart = new ProcessStartInfo();
                        processStart.FileName = "cmd.exe";
                        processStart.WindowStyle = ProcessWindowStyle.Normal;
                        processStart.Arguments = @"/k cd ./apktool && java -jar uber-apk-signer.jar --apks ./sign/";
                        Process.Start(processStart);
                    }
                }
            }else
            {
                ProcessStartInfo processStart = new ProcessStartInfo();
                processStart.FileName = "cmd.exe";
                processStart.WindowStyle = ProcessWindowStyle.Normal;
                processStart.Arguments = @"/k cd ./apktool && java -jar uber-apk-signer.jar --apks ./ywapk/dist/";
                Process.Start(processStart);
            }
            gunaAdvenceButton1.Enabled = false;
        }

        private void gunaAdvenceButton2_Click(object sender, EventArgs e)
        {
            SoundPlayer player = new SoundPlayer(Application.StartupPath + "/assets/music/btn_sound.wav");
            player.Play();
            Application.Exit();
        }

        private void gunaAdvenceButton3_Click(object sender, EventArgs e)
        {
            gunaAdvenceButton4.Enabled = true;
            gunaAdvenceButton3.Enabled = false;
            SoundPlayer player = new SoundPlayer(Application.StartupPath + "/assets/music/btn_sound.wav");
            player.Play();
            try
            {
               
                if (Directory.Exists(origen))
                {

                    ReplaceFolder(origen, destino);
                    MessageBox.Show("Folders replaced successfully.");
                    
                }

                else if (File.Exists(origen))
                {

                    ReplaceFile(origen, destino);
                   
                }
                else
                {
                    MessageBox.Show("Error, route don't exist.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
    }

        private void gunaMetroVTrackBar1_Scroll(object sender, ScrollEventArgs e)
        {
            m_mediaPlayer.Volume = e.NewValue / 100.0f;
            string filePath = settingsvolume + @"\startmusic";
            if (!File.Exists(filePath))
            {
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    sw.WriteLine(e.NewValue);
                }
            } else
            {
                File.WriteAllText(filePath, e.NewValue.ToString());
            }
        }

        private void gunaControlBox2_Click(object sender, EventArgs e)
        {
            SoundPlayer player = new SoundPlayer(Application.StartupPath + "/assets/music/btn_sound.wav");
            player.Play();
        }

        private void gunaControlBox1_Click(object sender, EventArgs e)
        {
            SoundPlayer player = new SoundPlayer(Application.StartupPath + "/assets/music/btn_sound.wav");
            player.Play();
        }

        private void gunaPictureBox2_Click(object sender, EventArgs e)
        {
            Process.Start("https://gamebanana.com/wips/84407");
        }

        private void gunaPictureBox3_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/NullGang/Enma-Injector/releases/");
        }
    }
}
