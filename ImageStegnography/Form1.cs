using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageStegnography
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.png, *.jpg) | *.png; *.jpg";
            openFileDialog.InitialDirectory = @"C:\Users\sarib\Desktop\";

            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog.FileName.ToString();
                pictureBox1.ImageLocation = textBox1.Text;
                textBox2.Clear();
                textBox3.Clear();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox3.Text.Length < 16 || textBox3.Text.Length > 16)
            {
                Exception ex = new Exception();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
            }
            else
            {
                try
                {
                    Bitmap img = new Bitmap(textBox1.Text);
                    AES enc = new AES();
                    string data = enc.matrixTostring(enc.Encrypt(enc.stringTomatrix(textBox2.Text), enc.stringTomatrix(textBox3.Text)));

                    for (int i = 0; i < img.Width; i++)
                    {
                        for (int j = 0; j < img.Height; j++)
                        {
                            Color Pixel = img.GetPixel(i, j);

                            if (i < 1 && j < textBox2.Text.Length)
                            {
                                char letter = Convert.ToChar(data.Substring(j, 1));
                                int val = Convert.ToInt32(letter);

                                img.SetPixel(i, j, Color.FromArgb(Pixel.R, Pixel.G, val));
                            }
                        }
                    }
                    textBox2.Text = data;

                    SaveFileDialog newFile = new SaveFileDialog();
                    newFile.Filter = "Image Files (*.png, *.jpg) | *.png; *.jpg";
                    newFile.InitialDirectory = @"C:\Users\sarib\Desktop\";

                    if (newFile.ShowDialog() == DialogResult.OK)
                    {
                        textBox1.Text = newFile.FileName.ToString();
                        pictureBox1.ImageLocation = textBox1.Text;

                        img.Save(textBox1.Text);
                        textBox2.Clear();
                        textBox3.Clear();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(textBox3.Text.Length < 16 || textBox3.Text.Length > 16)
            {
                Exception ex = new Exception();
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
            }
            else
            {
                try
                {
                    Bitmap img = new Bitmap(textBox1.Text);
                    string msg = "";
                    AES enc = new AES();

                    for (int i = 0; i < img.Width; i++)
                    {
                        for (int j = 0; j < img.Height; j++)
                        {
                            Color Pixel = img.GetPixel(i, j);
                            if (i < 1 && j < 16)
                            {
                                int val = Pixel.B;
                                char c = Convert.ToChar(val);
                                string letter = Convert.ToString(c);

                                msg = msg + letter;
                            }
                        }
                    }
                    string data = enc.matrixTostring(enc.Decrypt(enc.stringTomatrix(msg), enc.stringTomatrix(textBox3.Text)));
                    textBox2.Text = data;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK);
                }
            }
        }
    }
}
