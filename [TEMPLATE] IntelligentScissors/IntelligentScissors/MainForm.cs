using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace IntelligentScissors
{
    public partial class MainForm : Form
    {
        
        public MainForm()
        {
            InitializeComponent();
        }

        RGBPixel[,] ImageMatrix;
        int AnchorX, AnchorY, MousePosX, MousePosY;

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);
            }
            txtWidth.Text = ImageOperations.GetWidth(ImageMatrix).ToString();
            txtHeight.Text = ImageOperations.GetHeight(ImageMatrix).ToString();
            ImageOperations.makeImgGraph(ImageMatrix);
            MessageBox.Show("Done");
        }

        private void btnGaussSmooth_Click(object sender, EventArgs e)
        {
            double sigma = double.Parse(txtGaussSigma.Text);
            int maskSize = (int)nudMaskSize.Value ;
            ImageMatrix = ImageOperations.GaussianFilter1D(ImageMatrix, maskSize, sigma);
            ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            MousePosX = e.X;
            MousePosY = e.Y;

            txt_XPos.Text = MousePosX.ToString();
            txt_YPos.Text = MousePosY.ToString();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            AnchorX = e.X;
            AnchorY = e.Y;

            txt_AnchorX.Text = AnchorX.ToString();
            txt_AnchorY.Text = AnchorY.ToString();

            ImageOperations.getShortestBath(e.X, e.Y);
            MessageBox.Show("Done");
        }
    }
}
