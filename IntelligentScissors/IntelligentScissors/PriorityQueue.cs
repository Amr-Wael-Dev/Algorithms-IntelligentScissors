using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntelligentScissors
{
    public class Node
    {
        public Pixel Pixel { get; set; }
        public Pixel Parent { get; set; }
        public double Distance { get; set; }
        public int roadToParent { get; set; }
        public int HeapKey;

        public Node(int x, int y) // Analysis: O(1)
        {
            Pixel pixel = new Pixel();
            pixel.X = x;
            pixel.Y = y;
            this.Pixel = pixel;

            Pixel parent = new Pixel();
            parent.X = -1;
            parent.Y = -1;
            this.Parent = parent;

            Distance = double.PositiveInfinity;
            roadToParent = 0;
        }
    }

    public class PriorityQueue
    {
        public int Capacity { get; set; }
        public int CurrentSize { get; set; }
        public Node[] MinHeapArr { get; set; }

        public PriorityQueue(int Capacity) // Analysis: O(1)
        {
            this.Capacity = Capacity;
            MinHeapArr = new Node[Capacity];
            CurrentSize = 0;
        }

        public PriorityQueue(int ImgWidth, int ImgHeight, ref Node[,] ImgNodes) // Analysis: O(V log(V))
        {

            Capacity = ImgWidth * ImgHeight;
            MinHeapArr = new Node[Capacity];
            CurrentSize = 0;

            for (int i = 0; i < ImgWidth; i++)                           //
                for (int j = 0; j < ImgHeight; j++)                      // Analysis: O(V)
                    ImgNodes[i, j].HeapKey = Insert(ImgNodes[i, j]);     //

            for (int i = Parent(CurrentSize); i > 0; i--)             // Analysis: O(V log(V))
                MinHeapify(i);
        }

        public bool IsEmpty() // Analysis: O(1)
        {
            return CurrentSize == 0;
        }

        // Swaps two entities
        // Used in: 1- MinHeapify to swap nodes and their MinHeapArr indices
        //          2- ShortestPath.RelaxEdge to swap nodes and their MinHeapArr indices
        public void Swap<T>(ref T lhs, ref T rhs)  // Analysis: O(1)
        {
            T temp;
            temp = lhs;
            lhs = rhs;
            rhs = temp;
        }

        public int Parent(int index) // Analysis: O(1)
        {
            return index / 2;
        }
        public int Left(int index) // Analysis: O(1)
        {
            return 2 * index;
        }
        public int Right(int index) // Analysis: O(1)
        {
            return 2 * index + 1;
        }

        // Validates the tree by comparing each node with its right and left children
        // and swaps them if necessary
        public void MinHeapify(int index) // Analysis: O(log(V))
        {
            if (IsEmpty()) return;

            int left = Left(index);
            int right = Right(index);
            int smallest = index;

            if (left < CurrentSize && MinHeapArr[left].Distance < MinHeapArr[smallest].Distance)
            {
                smallest = left;
            }

            if (right < CurrentSize && MinHeapArr[right].Distance < MinHeapArr[smallest].Distance)
            {
                smallest = right;
            }

            if (smallest != index)
            {
                Swap<int>(ref MinHeapArr[smallest].HeapKey, ref MinHeapArr[index].HeapKey);
                Swap<Node>(ref MinHeapArr[smallest], ref MinHeapArr[index]);

                MinHeapify(smallest); // Analysis: O(log(V))
            }
        }

        // Inserts at the bottom right fo the tree
        // Then calls MinHeapify to validate the tree
        public int Insert(Node newNode) // Analysis: O(log(V))
        {
            if (CurrentSize == Capacity)
                return -1;

            int index = CurrentSize;
            MinHeapArr[index] = newNode;
            CurrentSize++;

            MinHeapify(index); // Analysis: O(log(V))

            return CurrentSize - 1;
        }

        // Returns the root node
        // Then calls MinHeapify to rebuild the tree
        public Node ExtractMin() // Analysis: O(log(V))
        {
            Node node = MinHeapArr[0];
            MinHeapArr[0] = MinHeapArr[CurrentSize - 1];
            MinHeapArr[0].HeapKey = 0;
            CurrentSize--;
            MinHeapify(0); // Analysis: O(log(V))
            return node;
        }
    }
}
