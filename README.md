# Project Documentaion
---
## How to Run
1. Run the launcher, and you will be able to use the app, it's based on C# Windows Forms
2. Click on the "Open Image" button and select an image
3. Simply click on any edge to place the anchor point (refer to further reading to read more about what an anchor point is)
4. Move the mouse along the edge to start applying the magnetic lasso
5. You can click on another point on the edge to place another anchor point
6. You can also clear the applied changes (remove the lasso to start over by clicking the "Clear" button

## Algorithm
The algorithm is based on:
1. Intelligent Scissors for Image Composition
2. Interactive Segmentation with Intelligent Scissors. Simply I'm using Dijkstra to get the shortest path between two points on the boundaries of the shape based on the color intensity, and the magnetic lasso feature operates on a specified window size

## Results
![Image 1 Before Applying The Lasso](https://github.com/Amr-Wael-Dev/Algorithms-IntelligentScissors/blob/main/Resources/res1.jpg)
![Image 1 After Applying The Lasso](https://github.com/Amr-Wael-Dev/Algorithms-IntelligentScissors/blob/main/Resources/res2.jpg)

![Image 2 Before Applying The Lasso](https://github.com/Amr-Wael-Dev/Algorithms-IntelligentScissors/blob/main/Resources/res3.jpg)
![Image 2 After Applying The Lasso](https://github.com/Amr-Wael-Dev/Algorithms-IntelligentScissors/blob/main/Resources/res4.jpg)
---
---
# Further Reading
## Problem Definition
Selection tools

![MS Paint Select Tool](https://github.com/Amr-Wael-Dev/Algorithms-IntelligentScissors/blob/main/Resources/2.png)

can be used to select objects in an image to resize/delete/copy/move the objects. There are many types of selection tools such as rectangles or free-form selection tool, sometimes free-form selection tools are called Lasso’s. You can imagine a lasso as a rope surrounding your selection. Unfortunately, selection using ordinary lasso’s can be tedious and boring. In Photoshop, there is a more advanced version of ordinary lasso’s called Magnetic Lasso Tool ![Lasso Tool](https://github.com/Amr-Wael-Dev/Algorithms-IntelligentScissors/blob/main/Resources/1.gif).

The Magnetic Lasso Tool is a lasso that automatically snaps to the objects’ boundaries

![Car Selected Using Magnetic Lasso Tool](https://github.com/Amr-Wael-Dev/Algorithms-IntelligentScissors/blob/main/Resources/3.jpg)

The technical term for the Magnetic Lasso Tool is Livewire or Intelligent Scissors.

In this project we want to implement a simple magnetic lasso.

### Terminologies
- Livewire: A livewire is defined by two points and a wire (path) between them where an anchor point is a fixed point on the image the user selects at the beginning, and the free point is a moving point following the mouse cursor.
- Edge Detection: There are many simple image filtering techniques that can detect the object boundaries (edges) and tell us the position and strength of an edge at a certain pixel. An image-edge can be simply defined as a sudden change in image intensity at a certain position, and since these edges represent the object’s boundary, we can use these edges to snap or pull the lasso towards them.

![Rubik's Cube](https://github.com/Amr-Wael-Dev/Algorithms-IntelligentScissors/blob/main/Resources/4.png)
![Greyscale Rubik's Cube](https://github.com/Amr-Wael-Dev/Algorithms-IntelligentScissors/blob/main/Resources/5.png)
![Rubik's Cube With Edge Detection Applied](https://github.com/Amr-Wael-Dev/Algorithms-IntelligentScissors/blob/main/Resources/6.png)

- Representing Images By Graphs: Images can be represented as graphs, which can be helpful in many image analysis problems, because it reduces the problem from an image domain problem to a graph domain problem. On such graphs you can apply typical graphs algorithms (Dijkstra, BFS, DFS, …etc) to solve the problem at hand.

![Representing Images By Graphs](https://github.com/Amr-Wael-Dev/Algorithms-IntelligentScissors/blob/main/Resources/7.gif)

- Graph Construction:	To construct an undirected weighted-graph we need to define vertices (nodes), and	connectivity (edges). An image-graph can be structured as vertices where each image pixel is considered as a vertex in the graph. So if we have an NxM image then we have an NxM vertices in our graph, and connectivity. There are many ways to connect the vertices grid, the simplest way is to establish a 4-connectivity. So we need to connect each pixel with the pixel on the above, below, left, and right

![Graph Construction: 4-Connectivity, 8-Connectivity](https://github.com/Amr-Wael-Dev/Algorithms-IntelligentScissors/blob/main/Resources/8.png)

- Mapping a "livewire in an image" Problem to a "shortest path in a graph" Problem: Assuming that we have an undirected weighted-graph for the image, with small weights on the objects' boundaries (image-edges) and large weights at the homogenous parts of the image.	If we need to generate a livewire between two pixels P1(i,j) and P2(x,y), it is the same as getting the shortest path between the two corresponding vertices V1(i,j) and V2(x,y), because the low edge-weights are at the image-edges on which we want our livewire to snap on.	Now remains one issue towards constructing our graph, which is determining the edges’ weights between pixels.
- Edge Weights Generation: Assuming that you have a value G that measures the image-edge strength and direction between two pixels P1 and P2, then we can set the edge-weight between P1 and P2 as Wp1,p2 = 1/G. so regions with Low G have high weight, and regions with high G have low weight.
