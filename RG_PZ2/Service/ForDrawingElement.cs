using RG_PZ2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml;

namespace RG_PZ2.Service
{
     public class ForDrawingElement
    {
        public  static List<PowerEntity> MyPowerEntity { get; set; }       
        private List<SubstationEntity> substationEntities = new List<SubstationEntity>();
        private List<NodeEntity> nodeEntities = new List<NodeEntity>();
        private List<SwitchEntity> switchEntities = new List<SwitchEntity>();
        private List<LineEntity> lineEntities = new List<LineEntity>();
        private Map map;
        private double xScale;
        private double yScale;
        private double size = 10;
        private double xMin;
        private double yMin;
        public ForDrawingElement()
        {
            MyPowerEntity = new List<PowerEntity>();

        }
        public void SetScale(double width, double height)
        {
            xMin = Math.Min(Math.Min(substationEntities.Min((item) => item.X), nodeEntities.Min((item) => item.X)), switchEntities.Min((item) => item.X));
            yMin = Math.Min(Math.Min(substationEntities.Min((item) => item.Y), nodeEntities.Min((item) => item.Y)), switchEntities.Min((item) => item.Y));

            xScale =( width/2) / (Math.Max(Math.Min(substationEntities.Max((item) => item.X), nodeEntities.Max((item) => item.X)), switchEntities.Max((item) => item.X)) - xMin);
            yScale =( height /2)/ (Math.Max(Math.Min(substationEntities.Max((item) => item.Y), nodeEntities.Max((item) => item.Y)), switchEntities.Max((item) => item.Y)) - yMin);
         CreateEmptyMat();
        }
        public  void SetDefault(object sender, EventArgs e)
        {
            foreach(var item in MyPowerEntity)
            {
                item.SetDefaultColor();
            }
        }
        private void CreateEmptyMat()
        {
            map = new Map(401, 401);
           
            for (int x=0; x<= 400; x++)
            {
                for (int y = 0; y <=400; y++)
                {
                        
                    map.Cells[x,y]= new Cell(x, y, x * size,  y * size,Space.FREE, null);
                    
                }
            }

        }
        public void LoadXml()
        {
            var doc = new XmlDocument();
            doc.Load("Geographic.xml");

            Common.AddEntities(substationEntities, doc.DocumentElement.SelectNodes("/NetworkModel/Substations/SubstationEntity"));
            Common.AddEntities(nodeEntities, doc.DocumentElement.SelectNodes("/NetworkModel/Nodes/NodeEntity"));
            Common.AddEntities(switchEntities, doc.DocumentElement.SelectNodes("/NetworkModel/Switches/SwitchEntity"));
            Common.AddEntities(lineEntities, doc.DocumentElement.SelectNodes("/NetworkModel/Lines/LineEntity"));
         //  sortLine();
        }
  
        public void SetCoords(double width, double height)
        {
            foreach (var item in substationEntities)
            {
                double x = Common.ConvertToCanvas(item.X, xScale, xMin, size, width/2);
                double y = Common.ConvertToCanvas(item.Y, yScale, yMin, size, height/2);
     
                (item.X, item.Y) = Common.FindClosestXY(x, y, size);
                map.AddElement(item.X, item.Y, Space.NODE);
              


            }
            foreach (var item in nodeEntities)
            {
                double x = Common.ConvertToCanvas(item.X, xScale, xMin, size, width / 2);
                double y = Common.ConvertToCanvas(item.Y, yScale, yMin, size, height / 2);
           
                (item.X, item.Y) = Common.FindClosestXY(x, y, size);
                map.AddElement(item.X, item.Y, Space.NODE);

            }
            foreach (var item in switchEntities)
            {
                double x = Common.ConvertToCanvas(item.X, xScale, xMin, size, width / 2);
                double y = Common.ConvertToCanvas(item.Y, yScale, yMin, size, height / 2) ;

                (item.X, item.Y) = Common.FindClosestXY(x, y, size);
                map.AddElement(item.X, item.Y, Space.NODE);

            }
           
        }
      void  DrawSubstation(Canvas myCanvas, MouseButtonEventHandler del)
        {
            foreach (var item in substationEntities)
            {
                Ellipse element = new Ellipse() { Width = 6, Height = 6, Fill = Brushes.HotPink };
                element.ToolTip = "ID:" + item.Id + "\nSubstation" + "\nName:" + item.Name;
                element.MouseLeftButtonDown += del;

                Canvas.SetLeft(element, item.X+2);
                Canvas.SetTop(element, item.Y + 2);

                item.shape = element;
                myCanvas.Children.Add(element);
                MyPowerEntity.Add(item);
            }
        }
        void DrawNode(Canvas myCanvas, MouseButtonEventHandler del)
        {
            foreach (var item in nodeEntities)
            {
                Ellipse element = new Ellipse() { Width = 6, Height = 6, Fill = Brushes.DeepSkyBlue };
                element.ToolTip = "ID:" + item.Id + "\nNode " + "\nName:" + item.Name;
                element.MouseLeftButtonDown += del;

                Canvas.SetLeft(element, item.X + 2);
                Canvas.SetTop(element, item.Y + 2);

                item.shape = element;
                myCanvas.Children.Add(element);
                MyPowerEntity.Add(item);
            }
        }

        void DrawSwithc(Canvas myCanvas, MouseButtonEventHandler del)
        {
            foreach (var item in switchEntities)
            {
                Ellipse element = new Ellipse() { Width = 6, Height = 6, Fill = Brushes.ForestGreen };
                element.ToolTip = "ID: " + item.Id + "\nSwitch " + "\nName: " + item.Name + "\nStatus: " + item.Status;
                element.MouseLeftButtonDown += del;

                Canvas.SetLeft(element, item.X +2);
                Canvas.SetTop(element, item.Y +  2);

                item.shape = element;
                myCanvas.Children.Add(element);
                MyPowerEntity.Add(item);
            }
        }
        void DrawLine(Canvas myCanvas)
        {
            foreach (var item in lineEntities)
            {
                var element = new Line() { Stroke = Brushes.Black };
                (element.X1, element.Y1) = FindElemt(item.FirstEnd);
                (element.X2, element.Y2) = FindElemt(item.SecondEnd);
                if (element.X1 == 0 || element.X2 == 0 || element.Y1 == 0 || element.Y2 == 0)//stavljeno zbog problema ili baga, javlja se odredjen broj linija koje krecu iz  gornjeg desnog ugla i spajaju se sa par tacaka

                {
                    continue;
                }
                if (Math.Abs((element.X1 - element.X2)) < 3800 || Math.Abs((element.Y1 - element.Y2)) < 3800)
                {
                    var lines = map.createLine(element.X1, element.Y1, element.X2, element.Y2, 0);//bes secenja drugih putanja

                    if (lines.Count < 2)
                    {
                        lines = map.createLine(element.X1, element.Y1, element.X2, element.Y2, 1);//sa secenjem drugih putanja
                    }
                    if (lines.Count > 2)
                    {
                        CreateLine(lines, myCanvas, FindConcreteElemt(item.FirstEnd), FindConcreteElemt(item.SecondEnd), item);

                    }
                }
            }
        }
        void DrawCrossing(Canvas myCanvas)
        {
            map.DrawCrossing(myCanvas);
        }
        public void DrawElements( Canvas myCanvas, MouseButtonEventHandler del)
        {

            DrawSubstation(myCanvas,del);
            DrawNode(myCanvas,del);
            DrawSwithc(myCanvas,del);
            DrawLine(myCanvas);
            DrawCrossing(myCanvas);
            


        }
       
        void CreateLine(List<Cell> lines, Canvas myCanvas, PowerEntity first, PowerEntity sec, LineEntity line)
        {
            Polyline tempPolyLine = new Polyline();
            tempPolyLine.Stroke = new SolidColorBrush(Colors.SaddleBrown);
            tempPolyLine.StrokeThickness = 1;
       
            for (int i=0; i<lines.Count;i++)
            {
                Space current= Space.FREE;
                if( i< lines.Count-1)
                {
                    if (lines[i].X_Coord != lines[i + 1].X_Coord && lines[i].Y_Coord != lines[i + 1].Y_Coord)
                    {
                        current = Space.LINE_CORNER;

                    }
                    else if (lines[i].X_Coord != lines[i + 1].X_Coord)
                    {
                        current = Space.LINE_HORIZONTAL;
                    }
                    else if (lines[i].Y_Coord != lines[i + 1].Y_Coord)
                    {
                        current = Space.LINE_VERICAL;
                    }
                    SetMarkOnMap(lines[i].X_Coord, lines[i].Y_Coord, current);
                }
                System.Windows.Point point = new System.Windows.Point(lines[i].X_Coord+5, lines[i].Y_Coord+5);
                tempPolyLine.Points.Add(point);
                tempPolyLine.MouseRightButtonDown += SetDefault;
                tempPolyLine.MouseRightButtonDown += first.ClickFunction;
                tempPolyLine.MouseRightButtonDown += sec.ClickFunction;
                tempPolyLine.ToolTip = "Power line\n" + "ID: " + line.Id + "\nName: " + line.Name + "\nTyle: " + line.LineType + "\nConductor material: " + line.ConductorMaterial + "\nUnderground: " + line.IsUnderground.ToString();
            }

            myCanvas.Children.Add(tempPolyLine);
        }
        void SetMarkOnMap(double xcoord, double ycoord, Space current)
        {
            if (current == Space.FREE)
                return;
            map.SetMarkOnMap(xcoord, ycoord, current);

        }
      private (double, double) FindElemt(long id)
         {
             return substationEntities.Find((item) => item.Id == id) != null
               ? (substationEntities.Find((item) => item.Id == id).X , substationEntities.Find((item) => item.Id == id).Y )
               : nodeEntities.Find((item) => item.Id == id) != null
               ? (nodeEntities.Find((item) => item.Id == id).X , nodeEntities.Find((item) => item.Id == id).Y )
               : switchEntities.Find((item) => item.Id == id) != null
               ? (switchEntities.Find((item) => item.Id == id).X , switchEntities.Find((item) => item.Id == id).Y ) : (0, 0);
        }
        private PowerEntity FindConcreteElemt(long id)
        {
            foreach(var item in substationEntities)
                {
                    if(item.Id==id)
                    {
                        return item;
                    }
                }
        
              foreach(var item in nodeEntities)
                {
                    if(item.Id==id)
                    {
                        return item;
                    }
             }
              foreach(var item in switchEntities)
                {
                    if(item.Id==id)
                    {
                        return item;
                    }
                }
            return null;
         }
        
    }
}
