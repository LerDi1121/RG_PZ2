using RG_PZ2.Models;
using RG_PZ2.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;


namespace RG_PZ2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public  delegate void CanvasAddDelegate(UIElement element);
        public static CanvasAddDelegate CanvasAddDel;
        //public delegate MouseButtonEventHandler CanvasElipseEventDlegate(object sender, MouseButtonEventArgs e);
        //public static CanvasElipseEventDlegate CanvasEllipseEventDelegate;
        private ForDrawingElement forDrawingElement = new ForDrawingElement();

       /* private List<SubstationEntity> substationEntities = new List<SubstationEntity>();
        private List<NodeEntity> nodeEntities = new List<NodeEntity>();
        private List<SwitchEntity> switchEntities = new List<SwitchEntity>();*/
  
        public MainWindow()
        {
            CanvasAddDel += CanvasAdd;
          //  CanvasEllipseEventDelegate += EllipseMousedown;
            InitializeComponent();
            forDrawingElement.LoadXml();
            forDrawingElement.SetScale(canvas.Width,canvas.Height);
           forDrawingElement.SetCoords(canvas.Width, canvas.Height);
            forDrawingElement.DrawElements(EllipseMousedown);
        }
    

  
       public  void CanvasAdd(UIElement element)
        {
            canvas.Children.Add(element);
        }
      
    
        
        public void EllipseMousedown(object sender, MouseButtonEventArgs e)
        {


            slider.Value = 1;
            Storyboard storyboardh = new Storyboard();
            Storyboard storyboardv = new Storyboard();

            ScaleTransform scale = new ScaleTransform(1, 1);

            scale.CenterX = Canvas.GetLeft((Ellipse)sender);
            scale.CenterY = Canvas.GetTop((Ellipse)sender);

            canvas.RenderTransform = scale;

            double startNum = 1;
            double endNum = 10;
            DoubleAnimation growAnimation = new DoubleAnimation();
            growAnimation.Duration = TimeSpan.FromSeconds(2);
            growAnimation.From = startNum;
            growAnimation.To = endNum;
            growAnimation.AutoReverse = true;
            storyboardh.Children.Add(growAnimation);
            storyboardv.Children.Add(growAnimation);

            Storyboard.SetTargetProperty(growAnimation, new PropertyPath("RenderTransform.ScaleX"));
            Storyboard.SetTarget(growAnimation, canvas);
            storyboardh.Begin();

            Storyboard.SetTargetProperty(growAnimation, new PropertyPath("RenderTransform.ScaleY"));
            Storyboard.SetTarget(growAnimation, canvas);
            storyboardv.Begin();



        }
       /* private (double, double) FindElemt(long id)
        {
            return substationEntities.Find((item) => item.Id == id) != null
                ? (substationEntities.Find((item) => item.Id == id).X + (5 / 2), substationEntities.Find((item) => item.Id == id).Y + (5 / 2))
                : nodeEntities.Find((item) => item.Id == id) != null
                ? (nodeEntities.Find((item) => item.Id == id).X + (5 / 2), nodeEntities.Find((item) => item.Id == id).Y + (5 / 2))
                : switchEntities.Find((item) => item.Id == id) != null
                ? (switchEntities.Find((item) => item.Id == id).X + (5 / 2), switchEntities.Find((item) => item.Id == id).Y + (5 / 2)) : (0, 0);
        }*/

    }
}
