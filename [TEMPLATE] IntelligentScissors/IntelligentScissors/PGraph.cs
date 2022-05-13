using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntelligentScissors
{
    public struct Pixel
    {
        public int x, y;
    }
    public struct PixelAdj
    {
        public Dictionary<Pixel, double> adj;
        
    }

    public class PGraph
    {
        public Dictionary<Pixel, PixelAdj> G;
        public Dictionary<Pixel, double> distance;
        public Dictionary<Pixel, Pixel> parent;
        
        public PGraph()
        {
            G = new Dictionary<Pixel, PixelAdj>();
            distance = new Dictionary<Pixel, double>();
            parent = new Dictionary<Pixel, Pixel>();
        }

        public void addP(int x, int y)
        {
            Pixel p = new Pixel();
            p.x = x;
            p.y = y;
            PixelAdj pAdj = new PixelAdj();
            pAdj.adj = new Dictionary<Pixel, double>();
            
            G.Add(p, pAdj);
            
        }
    }
}


