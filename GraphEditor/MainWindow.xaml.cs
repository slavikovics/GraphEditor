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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static MaterialDesignThemes.Wpf.Theme;

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
            if (!isElementSelected) isElementSelected = true; 
            else isElementSelected = false;
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

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (!isElementSelected) return;

            Point currentMousePosition = e.GetPosition(sender as Canvas);
            double ellipseCanvasTop = (double)ellipse.GetValue(Canvas.TopProperty);
            double ellipseCanvasLeft = (double)ellipse.GetValue(Canvas.LeftProperty);
            Console.WriteLine($"MousePosition: {currentMousePosition.X}, {currentMousePosition.Y}; Canvas.TopProperty: {ellipse.GetValue(Canvas.TopProperty)}");
            ellipse.SetValue(Canvas.TopProperty, currentMousePosition.Y - PointDimensions / 2);
            ellipse.SetValue(Canvas.LeftProperty, currentMousePosition.X - PointDimensions / 2 - 50);
        }

        private void ButtonAnimation(object sender, MouseEventArgs e)
        {
            //Button button = sender as Button;
            //ColorAnimation buttonAnimation = new ColorAnimation();
            //buttonAnimation.From = Colors.MediumPurple;
            //buttonAnimation.To = Colors.Purple;
            //buttonAnimation.Duration = new Duration(TimeSpan.FromSeconds(1));
            //button.BeginAnimation(new PropertyPath(Button.BorderBrushProperty), buttonAnimation);
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            VisualStateManager.GoToState(sender as System.Windows.Controls.Button, "MouseOver", true);
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            VisualStateManager.GoToState(sender as System.Windows.Controls.Button, "Normal", true);
        }

        private void Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            VisualStateManager.GoToState(sender as System.Windows.Controls.Button, "Pressed", true);
        }

        private void Button_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            VisualStateManager.GoToState(sender as System.Windows.Controls.Button, "Normal", true);
        }
    }
}
