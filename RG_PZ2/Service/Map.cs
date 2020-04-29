using RG_PZ2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace RG_PZ2.Service
{
    
    class Map
    {
        private readonly static List<int> dr = new List<int> { -1, +1, 0, 0 };
        private readonly static List<int> dc = new List<int> { 0, 0, +1, -1 };
        public int NumX => Cells.GetLength(0);
        public int NumY => Cells.GetLength(1);
        internal Cell[,] Cells { get ; set ; }
        public Map(int X, int Y)
        {
            Cells = new Cell[X, Y];
        }
        public void AddElement(  double XCoord, double YCoord, Space spc)
        {         
            for(int x =0; x<NumY; x++)
            {
                if(XCoord == Cells[x,0].X_Coord)
                {
                    for (int y = 0; y < NumY; y++)
                    {
                        if (YCoord == Cells[x, y].Y_Coord)
                        {
                            Cells[x, y].Space_ = spc;    
                            break;                         
                         }
                    }
                }
            }
        }
      private  int  findRow(double xCoord)
        {
            for (int x = 0; x < NumY; x++)
            {
                if (xCoord == Cells[x, 0].X_Coord)
                {
                    return x;
                }
            }
            return -1;
        }
        private int findColumn(double yCoord)
        {
            for (int y = 0; y < NumY; y++)
            {
                if (yCoord == Cells[0, y].Y_Coord)
                {
                    return y;
                }
            }
            return -1;
        }
        bool isEnd(int x, int y, Cell End)
        {
            return (x == End.X && y == End.Y);
        }
        public List<Cell> createLine(double StartX, double StartY, double StopX, double StopY, int flag)
        {
            List<Cell> shortest= new List<Cell>();
            Queue<Cell> queue = new Queue<Cell>();
            Cell[,] previous = new Cell[NumX, NumY];

            int StartRow = findRow(StartX);
            int StopRow = findColumn(StopX);
            int StartColumn = findRow(StartY);
            int StopColumn = findColumn(StopY);

            Cell startCell = new Cell(StartRow,StartColumn, StartX, StartY);
            Cell stopCell = new Cell(StopRow,StopColumn, StopX,StopY);

            previous[StartRow, StartColumn] = startCell;
            queue.Enqueue(startCell);

            bool complitePath = false;

            while(queue.Count>0)
            {
                Cell tempCell = queue.Dequeue();
                if(isEnd(tempCell.X, tempCell.Y,stopCell))//da li je doslo do kraja
                {
                    complitePath = true;
                    break;
                }
                for( int i= 0; i<4;i++)
                {
                    int nextRow= tempCell.X+dr[i];
                    int nextColumn = tempCell.Y + dc[i];
                    if(nextRow<0 || nextColumn<0 || nextRow>=NumX|| nextColumn>= NumY)
                    {
                        continue;//preskoci ako izadjes van matrice
                    }
                    if(previous[nextRow,nextColumn]!=null )
                    {
                        continue;//preskoci polja koja si posetio;
                    }
                    if(!isEnd(nextRow,nextColumn,stopCell) &&( Cells[nextRow, nextColumn].Space_ != Space.FREE) && flag==0)
                    {
                        continue; 
                    }
                    if (!isEnd(nextRow, nextColumn, stopCell) && (Cells[nextRow, nextColumn].Space_ == Space.NODE) && flag == 1)
                    {
                        continue; 
                    }

                    queue.Enqueue(new Cell(nextRow, nextColumn, Cells[nextRow, nextColumn].X_Coord, Cells[nextRow, nextColumn].Y_Coord));

                    previous[nextRow, nextColumn] = tempCell;
                }
            }
            if(complitePath)
            {
                shortest.Add(stopCell);
                Cell prev = previous[stopCell.X, stopCell.Y];
                while (prev.X > 0 && !compareCell(prev, startCell)) {
                    MarkSpace(prev.X, prev.Y);
                    shortest.Add(prev);
                    prev = previous[prev.X, prev.Y];
                }
                shortest.Add(prev);       
            }

            return shortest;
        }
        void MarkSpace(int x, int y)
        {
            if(Cells[x,y].Space_==Space.FREE)
            {
                Cells[x, y].Space_ = Space.LINE;
            }
        
        }
        private bool compareCell(Cell first, Cell sec)
        {
            return (first.X == sec.X && first.Y == sec.Y && first.X_Coord == sec.X_Coord && first.Y_Coord ==sec.Y_Coord);
        }
        public void SetMarkOnMap(double xcoord, double ycoord, Space current)
        {
            for (int x = 0; x < NumY; x++)
            {
                if (xcoord == Cells[x, 0].X_Coord)
                {
                    for (int y = 0; y < NumY; y++)
                    {
                        if (ycoord == Cells[x, y].Y_Coord)
                        {
                            if (Cells[x, y].Space_ == Space.NODE)
                                return;
                            if (Cells[x, y].Space_ == Space.LINE || Cells[x, y].Space_ == Space.FREE)
                                Cells[x, y].Space_ = current;
                            else if (Cells[x, y].Space_ != current)
                                Cells[x, y].Space_ = Space.LINE_X;
                            return;
                        }
                    }
                }
            }
         
           
        }
        public void DrawCrossing(Canvas myCanvas)
        {
           
            foreach (var item in Cells)
            {
                if (item.Space_ == Space.LINE_X)
                {
                    myCanvas.Children.Add(CreateCrosingEllipse(item.X_Coord, item.Y_Coord));
                }
            }
        }
        Ellipse CreateCrosingEllipse(double xcoord, double ycoord)
        {
            var temp = new Ellipse() { Width = 4, Height = 4, Fill = Brushes.Black };
            Canvas.SetLeft(temp,xcoord+3);
            Canvas.SetTop(temp, ycoord+3);
            return temp;
        }
    }
}
