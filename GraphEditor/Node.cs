using GraphEditor.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup.Localizer;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace GraphEditor
{
    internal class Node
    {
        public Button ellipse { get; private set; }

        private Canvas _canvas;

        private MainWindow _window;

        private Random random;

        public int _id { get; private set; }

        private const int EllipseDimensions = 40;

        private const int UILeftSize = 70;

        private bool isSelected = false;

        private double movementDiffLeft;

        private double movementDiffTop;

        private bool testWasMagicWondClicked = false;

        public event EventHandler buttonSelected;

        public Node(double CanvasLeft, double CanvasTop, Canvas parent, MainWindow window, int id)
        {
            random = new Random();
            ellipse = new Button();
            ellipse.Width = EllipseDimensions;
            ellipse.Height = EllipseDimensions;
            ellipse.Template = window.ButtonMagicWond.Template;
            ellipse.Content = new Image();
            (ellipse.Content as Image).Source = ((Image)window.EllipseExample.Content).Source;


            ellipse.SetValue(Canvas.LeftProperty, CanvasLeft - UILeftSize);
            ellipse.SetValue(Canvas.TopProperty, CanvasTop);
            parent.Children.Add(ellipse);

            _id = id;
            _canvas = parent;
            _window = window;

            (ellipse.Content as Image).MouseDown += OnMouseDown;
            window.MouseMove += OnMouseMove;
            window.KillAllSelections += Unselect;
            window.MagicWondOrder += OnMagicWondOrder;
        }

        private void Unselect()
        {
            isSelected = false;
        }

        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (isSelected)
            {
                isSelected = false;
            }
            else
            {
                isSelected = true;

                buttonSelected?.Invoke(this, new EventArgs());

                _window.shouldNodeBeAdded = false;
                Point currentMousePosition = e.GetPosition(sender as Window);

                movementDiffLeft = currentMousePosition.X - EllipseDimensions / 2 - UILeftSize - (double)ellipse.GetValue(Canvas.LeftProperty);
                movementDiffTop = currentMousePosition.Y - EllipseDimensions / 2 - (double)ellipse.GetValue(Canvas.TopProperty);
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!isSelected) return;

            DependencyProperty dependencyPropertyL = Canvas.LeftProperty;
            DependencyProperty dependencyPropertyT = Canvas.TopProperty;
            ellipse.BeginAnimation(dependencyPropertyL, null);
            ellipse.BeginAnimation(dependencyPropertyT, null);

            Point currentMousePosition = e.GetPosition(sender as Window);
            ellipse.SetValue(Canvas.TopProperty, currentMousePosition.Y - EllipseDimensions / 2 - movementDiffTop);
            ellipse.SetValue(Canvas.LeftProperty, currentMousePosition.X - EllipseDimensions / 2 - UILeftSize - movementDiffLeft);
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
            double leftTarget = random.Next(100);
            leftTarget -= 50;
            double topTarget = random.Next(100);
            topTarget -= 50;

            double toLeft = GetCanvasLeft() + leftTarget;

            DoubleAnimation ellipseAnimationLeft = new DoubleAnimation();
            ellipseAnimationLeft.To = toLeft;
            ellipseAnimationLeft.Duration = new Duration(TimeSpan.FromSeconds(1));
            ellipseAnimationLeft.AccelerationRatio = 0.3;
            ellipseAnimationLeft.DecelerationRatio = 0.7;

            double toTop = GetCanvasTop() + topTarget;

            DoubleAnimation ellipseAnimationTop = new DoubleAnimation();
            ellipseAnimationTop.To = toTop;
            ellipseAnimationTop.Duration = new Duration(TimeSpan.FromSeconds(1));
            ellipseAnimationTop.AccelerationRatio = 0.3;
            ellipseAnimationTop.DecelerationRatio = 0.7;

            DependencyProperty dependencyProperty = Canvas.LeftProperty;
            ellipse.BeginAnimation(dependencyProperty, ellipseAnimationLeft);

            dependencyProperty = Canvas.TopProperty;
            ellipse.BeginAnimation(dependencyProperty, ellipseAnimationTop);

            testWasMagicWondClicked = true;
        }

    }
}
