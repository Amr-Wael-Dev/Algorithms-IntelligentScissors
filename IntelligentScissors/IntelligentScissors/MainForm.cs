using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows;
using System.Windows.Forms;

namespace IntelligentScissors
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        RGBPixel[,] ImgMatrix; // 2D Array of pixels of opened image
        Bitmap ImageCopy;      // Will hold a copy of each stage of the image
                               // For example: 1- when resetting the image using the clear button
                               //              2- when placing multiple anchor points
        Pixel currentClick;    // Holds the coordinates of the current anchor point
        Pixel previousClick;   // Holds the coordinates of the previous anchor point
        Pixel firstPosition;   // Holds the coordinates of the first anchor point
        bool isAnchorPixel;    // True if the user has clicked on the image, flase otherwise
        bool checkAccess;      // True if there exists at least one anchor point
                               // Used when drawing the line between the most recent anchor point and the free point

        private void Open_Click(object sender, EventArgs e) // Analysis: O(1)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                ImgMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImgMatrix, PictureBox);

                // Initialize the graph attributes using the determined image matrix
                Graph.ConstructGraph(ImgMatrix);
            }
            txtWidth.Text = ImageOperations.GetWidth(ImgMatrix).ToString();
            txtHeight.Text = ImageOperations.GetHeight(ImgMatrix).ToString();

            // First image copy is the input image
            ImageCopy = new Bitmap(PictureBox.Image);

            // No anchor is placed yet (isAnchorPixel = false)
            // No line should be drawn yet (checkAccess = false)
            isAnchorPixel = checkAccess = false;
        }

        // Called for each pixel the mouse moves over
        private void PictureBox_MouseMove(object sender, MouseEventArgs e) // Analysis: O(1)
        {
            MousePositionX.Text = e.X.ToString();
            MousePositionY.Text = e.Y.ToString();

            // Only draw the line on the image if there exists an anchor point
            if (isAnchorPixel == true)
            {
                // Set the current anchor point to the click position
                currentClick.X = e.X;
                currentClick.Y = e.Y;

                // Set the image in the picture box to a copy of the current image and whatever is drawn on it
                // and refresh the picture box
                PictureBox.Image = (Bitmap)ImageCopy.Clone();
                PictureBox.Refresh();

                // If the click position is invalid: Return (Do nothing)
                // Else: Draw()
                if (Graph.ImgNodes[currentClick.X, currentClick.Y] == null) return;

                Draw();
            }
        }

        // Called when the user clicks on the picture box (places an anchor)
        private void PictureBox_MouseClick(object sender, MouseEventArgs e) // Analysis: O(E log(V))
        {
            AnchorX.Text = e.X.ToString();
            AnchorY.Text = e.Y.ToString();

            // If it's a right click
            // Closes the lasso
            if (e.Button == MouseButtons.Right)
            {
                // Calculates the shortest path to the first click (anchor point)
                ClickHandler(firstPosition);
                isAnchorPixel = false;

                // Analysis: O(V^2)
                for (int i = 0; i < Graph.ImgWidth; i++)
                    for (int j = 0; j < Graph.ImgHeight; j++)
                        Graph.CheckAccess[i, j] = true;
            }

            Pixel anchorPosition;
            anchorPosition.X = e.X;
            anchorPosition.Y = e.Y;

            ClickHandler(anchorPosition); // Analysis: O(E log(V))
        }

        // Called when a user clicks the image (places an anchor)
        // Sets necessary variables and calculates the shortest path to the previous click (anchor point)
        private void ClickHandler(Pixel pixel) // Analysis: O(E log(V))
        {
            if (isAnchorPixel == false) // If no previous anchor is set
            {
                isAnchorPixel = true;
                previousClick.X = pixel.X;
                previousClick.Y = pixel.Y;
                firstPosition = previousClick;
            }
            else // If there exists a previous anchor point
            {
                PictureBox.Refresh();
                currentClick.X = pixel.X;
                currentClick.Y = pixel.Y;
                checkAccess = true;
                Draw();
                checkAccess = false;
                ImageCopy = (Bitmap)PictureBox.Image;
                previousClick = currentClick;
            }

            ShortestPath.DijkstraSSSP(previousClick.X, previousClick.Y); // Analysis: O(E log(V))
        }

        // Called inside PictureBox_MouseMove
        // and PictureBox_MouseClick (if there exists a previous anchor point)
        private void Draw() // Analysis: O(1)
        {
            Node node = new Node(0, 0);
            Node parent = new Node(0, 0);

            // Get the node of the graph where the click (anchor point) is, return (do nothing) if null
            node = Graph.ImgNodes[currentClick.X, currentClick.Y];
            if (node == null) return;

            // Creates a Graphics object from the current image
            // Graphics class creates a drawing surface
            Graphics G = Graphics.FromImage(PictureBox.Image);

            // While there exists a parent to the current iteration node
            // && the parent to the current iteration node is not the previous anchor point
            while (node.Parent.X != -1 && node.Parent.Y != -1 && (node.Parent.X != previousClick.X || node.Parent.Y != previousClick.Y))
            {
                // Get the parent of the current iteration node
                parent = Graph.ImgNodes[node.Parent.X, node.Parent.Y];

                // Set its parent's value in the Graph.CheckAccess array to false
                if (checkAccess == true)
                    Graph.CheckAccess[parent.Pixel.X, parent.Pixel.Y] = false;

                // Draws a 1 pixel red line between two points
                // Point 1: the current iteration node
                // Point 2: the current iteration node's parent
                G.DrawLine(new Pen(Color.Red, 1), new Point(node.Pixel.X, node.Pixel.Y), new Point(parent.Pixel.X, parent.Pixel.Y));

                // Set the next iteration node to its parent
                node = parent;
            }

            PictureBox.Refresh();
        }

        // Called when the user clicks on the Clear button
        // Resets the image in the picture box
        // i.e. Removes all drwan lines (lasso)
        private void Clear_Click(object sender, EventArgs e) // Analysis: O(1)
        {
            // 1- Displays the original image
            // 2- Re-constructs the graph
            // 3- Takes a copy from the input image
            // 4- Sets isAnchorPixel to false to reset all anchor points
            ImageOperations.DisplayImage(ImgMatrix, PictureBox);
            Graph.ConstructGraph(ImgMatrix);
            ImageCopy = new Bitmap(PictureBox.Image);
            isAnchorPixel = false;
        }
    }
}
