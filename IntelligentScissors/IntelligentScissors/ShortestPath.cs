using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntelligentScissors
{
    public static class ShortestPath
    {
        public static PriorityQueue Queue { get; private set; }

        public static void DijkstraSSSP(int anchorX, int anchorY) //Analysis: O(E log(V)) 
        {
            Node[,] extractedImage;
            int newWidth = 0;
            int newHight = 0;
            int xFrom, xTo;
            int yFrom, yTo;
            /*we make subImage to calculate the shortest path in it 
             this making the calulation more faster and more efficint*/
            xFrom = Math.Max(anchorX - 200, 0);
            xTo = Math.Min(Graph.ImgWidth, anchorX + 200);
            yFrom = Math.Max(anchorY - 200, 0);
            yTo = Math.Min(Graph.ImgHeight, anchorY + 200);

            newWidth = xTo - xFrom;
            newHight = yTo - yFrom;
            extractedImage = new Node[newWidth, newHight];

            int x = 0, y = 0;
            for (int i = xFrom; i < xTo; i++,x++) // Analysis: O(1)
            {
                y = 0;
                for (int j = yFrom; j < yTo; j++,y++) // Analysis: O(1)
                {
                    Graph.ImgNodes[i, j] = new Node(i, j);
                    extractedImage[x, y] = Graph.ImgNodes[i, j];
                }
            }

            Graph.ImgNodes[anchorX, anchorY].Distance = 0;

            Queue = new PriorityQueue(newWidth, newHight,ref extractedImage);
            
            while (!Queue.IsEmpty()) // Analysis: O(E log(V)) 
            {
                Node minNode = Queue.ExtractMin();
                RelaxNeighbors(ref minNode, Queue); // Analysis: O(log (V))
            }

            Queue = null;
        }

        public static void RelaxNeighbors(ref Node node, PriorityQueue queue) // Analysis: O(log(V))
        {
            if (node.Pixel.X > 0)
            {
                Pixel pixel1, pixel2;
                pixel1 = node.Pixel;
                pixel2.X = node.Pixel.X - 1;
                pixel2.Y = node.Pixel.Y;
                RelaxEdge(ref node, ref Graph.ImgNodes[node.Pixel.X - 1, node.Pixel.Y], Graph.CalculateWeight(pixel1, pixel2), queue); // Analysis: O(log(V))
            }
            if (node.Pixel.Y > 0)
            {
                Pixel pixel1, pixel2;
                pixel1 = node.Pixel;
                pixel2.X = node.Pixel.X;
                pixel2.Y = node.Pixel.Y - 1;
                RelaxEdge(ref node, ref Graph.ImgNodes[node.Pixel.X, node.Pixel.Y - 1], Graph.CalculateWeight(pixel1, pixel2), queue); // Analysis: O(log(V))
            }
            if (node.Pixel.Y < Graph.ImgHeight - 1)
            {
                Pixel pixel1, pixel2;
                pixel1 = node.Pixel;
                pixel2.X = node.Pixel.X;
                pixel2.Y = node.Pixel.Y + 1;
                RelaxEdge(ref node, ref Graph.ImgNodes[node.Pixel.X, node.Pixel.Y + 1], Graph.CalculateWeight(pixel1, pixel2), queue); // Analysis: O(log(V))
            }
            if (node.Pixel.X < Graph.ImgWidth - 1)
            {
                Pixel pixel1, pixel2;
                pixel1 = node.Pixel;
                pixel2.X = node.Pixel.X + 1;
                pixel2.Y = node.Pixel.Y;
                RelaxEdge(ref node, ref Graph.ImgNodes[node.Pixel.X + 1, node.Pixel.Y], Graph.CalculateWeight(pixel1, pixel2), queue); // Analysis: O(log(V))
            }
        }

        public static void RelaxEdge(ref Node node, ref Node neighbor, double weight, PriorityQueue queue) // Analysis: O(log(V))
        {
            if (node == null || neighbor == null || (!Graph.CheckAccess[neighbor.Pixel.X, neighbor.Pixel.Y])) return;

            if (neighbor.Distance > node.Distance + weight)
            {
                neighbor.Distance = node.Distance + weight;
                Pixel temp = new Pixel();
                temp.X = node.Pixel.X;
                temp.Y = node.Pixel.Y;
                neighbor.Parent = temp;
                neighbor.roadToParent = node.roadToParent + 1;

                while (queue.Parent(neighbor.HeapKey) > 0 && queue.MinHeapArr[neighbor.HeapKey].Distance < queue.MinHeapArr[queue.Parent(neighbor.HeapKey)].Distance) // Analysis: O(log(V))
                {
                    queue.Swap<Node>(ref queue.MinHeapArr[neighbor.HeapKey], ref queue.MinHeapArr[queue.Parent(neighbor.HeapKey)]);
                    queue.Swap<int>(ref queue.MinHeapArr[neighbor.HeapKey].HeapKey, ref queue.MinHeapArr[queue.Parent(neighbor.HeapKey)].HeapKey);
                }
            }
        }
    }
}
