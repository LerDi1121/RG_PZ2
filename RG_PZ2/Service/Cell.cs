using RG_PZ2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RG_PZ2.Service
{
    public enum Space { FREE, NODE, LINE_HORIZONTAL, LINE_VERICAL, LINE_X }
    class Cell
    {

        public PowerEntity Value { get; set; }
        public double  X_Coord{get;set;}
        public double Y_Coord { get; set; }

        public Space Space_ { get; set; }
        public Brush Color { get; set; } = Brushes.Black;
        public int X { get; set; }
        public Shape UIElement { get; set; }
        public int Y { get; set; }

        public Cell(int X, int Y, double Xcoord, double Ycoord,Space space, Shape uiElement)
        {
            this.X = X;
            this.Y = Y;
            X_Coord = Xcoord;
            Y_Coord = Ycoord;
           UIElement = uiElement;
            Space_ = space;
  
        }
        public Cell(int X, int Y, double Xcoord, double Ycoord)
        {
            this.X = X;
            this.Y = Y;
            X_Coord = Xcoord;
            Y_Coord = Ycoord;
            UIElement = null;
            Space_ = Space.FREE;

        }
        public Cell()
        {
            this.X = -1;
            this.Y = -1;
            X_Coord = -1;
            Y_Coord = -1;
            UIElement = null;
            Space_ = Space.FREE;
        }
    }
}
