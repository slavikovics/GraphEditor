using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraphEditor
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isElementSelected = false;
        bool areActionsDesired = false;
        Ellipse ellipse;

        public MainWindow()
        {
            InitializeComponent();
            ellipse = Ellipse;
        }

        const int PointDimensions = 10;

        private void Ellipse_DragLeave(object sender, DragEventArgs e)
        {

        }

        private void Ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isElementSelected = true;           
        }

        private void MouseMoveHandler(object sender, MouseEventArgs e)
        {
            if (!isElementSelected) return;

            Point currentMousePosition = e.GetPosition(sender as Canvas);
            double ellipseCanvasTop = (double)ellipse.GetValue(Canvas.TopProperty);
            double ellipseCanvasLeft = (double)ellipse.GetValue(Canvas.LeftProperty);
            Console.WriteLine($"MousePosition: {currentMousePosition.X}, {currentMousePosition.Y}; Canvas.TopProperty: {ellipse.GetValue(Canvas.TopProperty)}" );
            ellipse.SetValue(Canvas.TopProperty, currentMousePosition.Y - PointDimensions / 2);
            ellipse.SetValue(Canvas.LeftProperty, currentMousePosition.X - PointDimensions / 2);
        }

        private void Ellipse_MouseUp(object sender, MouseButtonEventArgs e)
        {

                isElementSelected = false;
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            areActionsDesired = true;
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            areActionsDesired = false;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            Canvas canvas = (Canvas)sender;
            UIElementCollection children = canvas.Children;
            foreach (UIElement child in children)
            {
                
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
