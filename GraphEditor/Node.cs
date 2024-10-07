using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace GraphEditor
{
    internal class Node
    {
        public Ellipse ellipse { get; private set; }

        private Canvas _canvas;

        private MainWindow _window;

        public int _id { get; private set; }

        private const int EllipseDimensions = 20;

        private const int UILeftSize = 70;

        private bool isSelected = false;

        public Node(double CanvasLeft, double CanvasTop, Canvas parent, MainWindow window, int id)
        {
            ellipse = new Ellipse();
            ellipse.Width = EllipseDimensions;
            ellipse.Height = EllipseDimensions;
            ellipse.Fill = new SolidColorBrush(Colors.MediumPurple);
            ellipse.SetValue(Canvas.LeftProperty, CanvasLeft - UILeftSize);
            ellipse.SetValue(Canvas.TopProperty, CanvasTop);
            parent.Children.Add(ellipse);

            _id = id;
            _canvas = parent;
            _window = window;

            ellipse.MouseDown += OnMouseDown;
            window.MouseMove += OnMouseMove;
            window.KillAllSelections += Unselect;
            window.MagicWondOrder += OnMagicWondOrder;
        }

        private void Unselect()
        {
            isSelected = false;
        }

        private void OnMouseDown(object sender, EventArgs e)
        {
            if (isSelected)
            {
                isSelected = false;
            }
            else
            {
                isSelected = true;
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!isSelected) return;

            Point currentMousePosition = e.GetPosition(sender as Window);
            ellipse.SetValue(Canvas.TopProperty, currentMousePosition.Y - EllipseDimensions / 2);
            ellipse.SetValue(Canvas.LeftProperty, currentMousePosition.X - EllipseDimensions / 2 - UILeftSize);
        }

        private double GetCanvasLeft()
        {
            return (double)ellipse.GetValue(Canvas.LeftProperty);
        }

        private double GetCanvasTop()
        {
            return (double)ellipse.GetValue(Canvas.TopProperty);
        }

        private void OnMagicWondOrder()
        {
            Random random = new Random();
            double leftTarget = random.Next(100);
            leftTarget -= 50;
            double topTarget = random.Next(100);
            topTarget -= 50;

            DoubleAnimation ellipseAnimationLeft = new DoubleAnimation();
            ellipseAnimationLeft.To = GetCanvasLeft() + leftTarget;
            ellipseAnimationLeft.Duration = new Duration(TimeSpan.FromSeconds(1));

            DoubleAnimation ellipseAnimationTop = new DoubleAnimation();
            ellipseAnimationTop.To = GetCanvasTop() + topTarget;
            ellipseAnimationTop.Duration = new Duration(TimeSpan.FromSeconds(1));

            DependencyProperty dependencyProperty = Canvas.LeftProperty;
            ellipse.BeginAnimation(dependencyProperty, ellipseAnimationLeft);

            dependencyProperty = Canvas.TopProperty;
            ellipse.BeginAnimation(dependencyProperty, ellipseAnimationTop);
        }

    }
}
