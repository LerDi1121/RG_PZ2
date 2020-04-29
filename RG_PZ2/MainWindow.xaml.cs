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
       private ForDrawingElement forDrawingElement = new ForDrawingElement();

        public MainWindow()
        {
            InitializeComponent();
            forDrawingElement.LoadXml();
            forDrawingElement.SetScale(canvas.Width,canvas.Height);
           forDrawingElement.SetCoords(canvas.Width, canvas.Height);
            forDrawingElement.DrawElements( canvas, EllipseMousedown);
        }
    

        public void EllipseMousedown(object sender, MouseButtonEventArgs e)
        {
            Ellipse temp = (Ellipse)e.Source;
            ScaleTransform scale = new ScaleTransform();

            scale.CenterX =3;
            scale.CenterY = 3;

            temp.RenderTransform = scale;

            double startNum = 1;
            double endNum = 10;

            DoubleAnimation growAnimation = new DoubleAnimation();
            growAnimation.Duration = TimeSpan.FromSeconds(5);
            growAnimation.From = startNum;
            growAnimation.To = endNum;
            growAnimation.AutoReverse = true;
            temp.RenderTransform = scale;

            scale.BeginAnimation(ScaleTransform.ScaleXProperty, growAnimation);
            scale.BeginAnimation(ScaleTransform.ScaleYProperty, growAnimation);

        }
  

    }
}
