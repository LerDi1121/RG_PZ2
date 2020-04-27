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
        public void SetScale(double width, double height)
        {
            xMin = Math.Min(Math.Min(substationEntities.Min((item) => item.X), nodeEntities.Min((item) => item.X)), switchEntities.Min((item) => item.X));
            yMin = Math.Min(Math.Min(substationEntities.Min((item) => item.Y), nodeEntities.Min((item) => item.Y)), switchEntities.Min((item) => item.Y));

            xScale =( width/2) / (Math.Max(Math.Min(substationEntities.Max((item) => item.X), nodeEntities.Max((item) => item.X)), switchEntities.Max((item) => item.X)) - xMin);
            yScale =( height /2)/ (Math.Max(Math.Min(substationEntities.Max((item) => item.Y), nodeEntities.Max((item) => item.Y)), switchEntities.Max((item) => item.Y)) - yMin);
         CreateEmptyMat();
        }
        private void CreateEmptyMat()
        {
            
            int counter = 0;
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
        }
        private void AddElementToMap(IEnumerable<PowerEntity> nodeEntities)
        {

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

                Canvas.SetLeft(element, item.X);
                Canvas.SetTop(element, item.Y);
                myCanvas.Children.Add(element);
            }
        }
        void DrawNode(Canvas myCanvas, MouseButtonEventHandler del)
        {
            foreach (var item in nodeEntities)
            {
                Ellipse element = new Ellipse() { Width = 6, Height = 6, Fill = Brushes.DeepSkyBlue };
                element.ToolTip = "ID:" + item.Id + "\nNode " + "\nName:" + item.Name;
                element.MouseLeftButtonDown += del;

                Canvas.SetLeft(element, item.X);
                Canvas.SetTop(element, item.Y);
                myCanvas.Children.Add(element);
            }
        }

        void DrawSwithc(Canvas myCanvas, MouseButtonEventHandler del)
        {
            foreach (var item in switchEntities)
            {
                Ellipse element = new Ellipse() { Width = 6, Height = 6, Fill = Brushes.ForestGreen };
                element.ToolTip = "ID: " + item.Id + "\nSwitch " + "\nName: " + item.Name + "\nStatus: " + item.Status;
                element.MouseLeftButtonDown += del;

                Canvas.SetLeft(element, item.X);
                Canvas.SetTop(element, item.Y);
                  myCanvas.Children.Add(element);
            }
        }
        public void DrawElements( Canvas myCanvas, MouseButtonEventHandler del)
        {

            DrawSubstation(myCanvas,del);
            DrawNode(myCanvas,del);
            DrawSwithc(myCanvas,del);
              foreach (var item in lineEntities)
              {
                  var element = new Line() { Stroke = Brushes.Black };
                  (element.X1, element.Y1) = FindElemt(item.FirstEnd);
                  (element.X2, element.Y2) = FindElemt(item.SecondEnd);
                  if (element.X1 == 0 || element.X2 == 0 || element.Y1 == 0 || element.Y2 == 0)//stavljeno zbog problema ili baga, javlja se odredjen broj linija koje krecu iz  gornjeg desnog ugla i spajaju se sa par tacaka
                    
                  {
                      continue;
                  }
                //  myCanvas.Children.Add(element);
               var lines= map.createLine(element.X1, element.Y1, element.X2, element.Y2);
                if(lines.Count>10 && lines.Count < 15)
                {
                    CreateLine(lines, myCanvas);
                   
                }
           
              }
        }
        void CreateLine(List<Cell> lines, Canvas myCanvas)
        {

            for(int i=0; i<lines.Count;i++)
            {
                Ellipse temp = new Ellipse() {  Width = 2, Height = 10, Fill = Brushes.Black };
                Canvas.SetLeft(temp, lines[i].X_Coord);
                Canvas.SetTop(temp, lines[i].Y_Coord);
                myCanvas.Children.Add(temp);
             
            }

            
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
    }
}
