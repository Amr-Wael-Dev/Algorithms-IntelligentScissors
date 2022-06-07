using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntelligentScissors
{
    // Holds the pixel position (X, Y) in the image
    public struct Pixel
    {
        public int X;
        public int Y;
    }
    
    // Graph class is static
    // because there exists only one graph
    // so there is no need to instatiate it
    public static class Graph
    {
        // Since there exists only one graph
        // Therefore there exists only one instance of each attribute
        public static int ImgWidth { get; set; }          // Image width
        public static int ImgHeight { get; set; }         // Image height
        public static RGBPixel[,] ImgMatrix { get; set; } // Original image matrix obtained from ImageOperations
        public static Node[,] ImgNodes { get; set; }      // 2D Array of image pixels as Nodes (Pixel, Parent, Distance, roadToParent, HeapKey)
                                                          // to be used in the PriorityQueue, and DijkstraSSSP
        public static bool[,] CheckAccess { get; set; }   // 2D Array, true for pixels near the anchor point (100 pixels in each of the 4 directions)
                                                          // and false otherwise
                                                          // because the DijkstraSSSP function only operates on the pixels with true value in this 2D array

        // Initializes the Graph
        public static void ConstructGraph(RGBPixel[,] ImgMatrix) // Analysis: O(V^2)
        {
            ImgWidth = ImageOperations.GetWidth(ImgMatrix);
            ImgHeight = ImageOperations.GetHeight(ImgMatrix);
            Graph.ImgMatrix = ImgMatrix;            
            ImgNodes = new Node[ImgWidth, ImgHeight];

            // Analysis: O(V^2)
            CheckAccess = new bool[ImgWidth, ImgHeight];
            for (int i = 0; i < ImgWidth; i++)
                for (int j = 0; j < ImgHeight; j++)
                    CheckAccess[i, j] = true;
        }

        // Calculates the edge weight between two Pixels
        // Used for relaxing edges in the Dijkstra's algorithm
        public static double CalculateWeight(Pixel pixel1, Pixel pixel2) // Analysis: O(1)
        {
            double weight;

            if (pixel1.X < pixel2.X) // Second Pixel is to the right of the first Pixel
                weight = ImageOperations.CalculatePixelEnergies(pixel1.X, pixel1.Y, ImgMatrix).X;
            else if (pixel1.X > pixel2.X) // First Pixel is to the right of the second Pixel
                weight = ImageOperations.CalculatePixelEnergies(pixel2.X, pixel2.Y, ImgMatrix).X;
            else if (pixel1.Y < pixel2.Y) // Second Pixel is below the first Pixel
                weight = ImageOperations.CalculatePixelEnergies(pixel1.X, pixel1.Y, ImgMatrix).Y;
            else // Second Pixel is above the first Pixel
                weight = ImageOperations.CalculatePixelEnergies(pixel2.X, pixel2.Y, ImgMatrix).Y;

            // Terminology 8: Edge Weights Generation
            // We can set the edge-weight between P1 and P2 as Wp1,Wp2 = 1/G
            // So regions with Low G have high weight, and regions with high G have low weight
            return 1.0 / weight;
        }
    }
}
