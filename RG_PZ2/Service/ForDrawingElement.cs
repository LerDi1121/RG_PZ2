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
        private double xScale;
        private double yScale;
        private double size = 10;
        private double xMin;
        private double yMin;
        public void SetScale(double width, double height)
        {
            xMin = Math.Min(Math.Min(substationEntities.Min((item) => item.X), nodeEntities.Min((item) => item.X)), switchEntities.Min((item) => item.X));
            yMin = Math.Min(Math.Min(substationEntities.Min((item) => item.Y), nodeEntities.Min((item) => item.Y)), switchEntities.Min((item) => item.Y));

            xScale = width / (Math.Max(Math.Min(substationEntities.Max((item) => item.X), nodeEntities.Max((item) => item.X)), switchEntities.Max((item) => item.X)) - xMin);
            yScale = height / (Math.Max(Math.Min(substationEntities.Max((item) => item.Y), nodeEntities.Max((item) => item.Y)), switchEntities.Max((item) => item.Y)) - yMin);
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
        public void SetCoords(double width, double height)
        {
            foreach (var item in substationEntities)
            {
                double x = Common.ConvertToCanvas(item.X, xScale, xMin, size, width);
                double y = Common.ConvertToCanvas(item.Y, yScale, yMin, size, height);
                (item.X, item.Y) = Common.FindClosestXY(x, y, size);
            }
            foreach (var item in nodeEntities)
            {
                double x = Common.ConvertToCanvas(item.X, xScale, xMin, size, width);
                double y = Common.ConvertToCanvas(item.Y, yScale, yMin, size, height);
                (item.X, item.Y) = Common.FindClosestXY(x, y, size);
            }
            foreach (var item in switchEntities)
            {
                double x = Common.ConvertToCanvas(item.X, xScale, xMin, size, width);
                double y = Common.ConvertToCanvas(item.Y, yScale, yMin, size, height);
                (item.X, item.Y) = Common.FindClosestXY(x, y, size);
            }
        }
        public void DrawElements(MouseButtonEventHandler del)
        {
            foreach (var item in substationEntities)
            {
                Ellipse element = new Ellipse() { Width = 5, Height = 5, Fill = Brushes.Red };
                element.ToolTip = "ID:" + item.Id + "\nSubstation" + "\nName:" + item.Name;
                element.MouseLeftButtonDown += del;

                Canvas.SetLeft(element, item.X);
                Canvas.SetTop(element, item.Y);
                MainWindow.CanvasAddDel(element);
            }

            foreach (var item in nodeEntities)
            {
                Ellipse element = new Ellipse() { Width = 5, Height = 5, Fill = Brushes.Blue };
                element.ToolTip = "ID:" + item.Id + "\nNode " + "\nName:" + item.Name;
                element.MouseLeftButtonDown += del;
                Canvas.SetLeft(element, item.X);
                Canvas.SetTop(element, item.Y);
                MainWindow.CanvasAddDel(element);
            }

            foreach (var item in switchEntities)
            {
                Ellipse element = new Ellipse() { Width = 5, Height = 5, Fill = Brushes.Green };
                element.ToolTip = "ID: " + item.Id + "\nSwitch " + "\nName: " + item.Name + "\nStatus: " + item.Status;
                element.MouseLeftButtonDown += del;
                Canvas.SetLeft(element, item.X);
                Canvas.SetTop(element, item.Y);
                MainWindow.CanvasAddDel(element);
            }

            /*  foreach (var item in lineEntities)
              {
                  var element = new Line() { Stroke = Brushes.Black };
                  (element.X1, element.Y1) = FindElemt(item.FirstEnd);
                  (element.X2, element.Y2) = FindElemt(item.SecondEnd);
                  if (element.X1 == 0 || element.X2 == 0 || element.Y1 == 0 || element.Y2 == 0)
                  {
                      continue;
                  }
                  canvas.Children.Add(element);
              }*/
        }
    }
}
