using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading;
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

        int nodeId = 1;

        public event Action KillAllSelections;
        public event Action MagicWondOrder;

        public bool shouldNodeBeAdded = false; 
        public bool shouldEdgeBeAdded = false;
        public bool shouldNodeBeMoved = true;

        Ellipse ellipse;

        private Node _firstSelected;
        private Node _secondSelected;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void EdgeDemoAnimation()
        {

            DoubleAnimation edgeAnimationBackWidth = new DoubleAnimation();
            edgeAnimationBackWidth.To = 100;
            edgeAnimationBackWidth.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            edgeAnimationBackWidth.AccelerationRatio = 1;

            DoubleAnimation edgeAnimationFrontLeft = new DoubleAnimation();
            edgeAnimationFrontLeft.To = 667;
            edgeAnimationFrontLeft.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            edgeAnimationFrontLeft.AccelerationRatio = 1;

            DependencyProperty dependencyProperty = Canvas.LeftProperty;
            DemoRect.BeginAnimation(WidthProperty, edgeAnimationBackWidth);

            //dependencyProperty = Canvas.LeftProperty;
            //DemoTriang.BeginAnimation(dependencyProperty, edgeAnimationFrontLeft);
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

        private void ButtonAddNode_Click(object sender, RoutedEventArgs e)
        {
            shouldNodeBeAdded = true;
            KillAllSelections?.Invoke();
        }

        private void OnNodeSelected(object sender, EventArgs e)
        {
            if (!shouldEdgeBeAdded) return;

            Node node = sender as Node;

            if (_firstSelected == null)
            {
                _firstSelected = node;
            }
            else
            {
                _secondSelected = node;
                CreateEdge();
            }
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!shouldNodeBeAdded) return;
            Point currentMousePosition = e.GetPosition(sender as Window);
            Node node = new Node(currentMousePosition.X, currentMousePosition.Y, MainCanvas, this, nodeId);
            node.buttonSelected += OnNodeSelected;
            nodeId++;
        }

        private void AutoGenerateNodes(int numberOfNodes)
        {
            Random random = new Random();

            for (int i = 0; i < numberOfNodes; i++)
            {
                double X = random.Next((int)this.ActualWidth - 100) + 80;
                double Y = random.Next((int)this.ActualWidth - 100) + 50;
                Node node = new Node(X, Y, MainCanvas, this, nodeId);
                node.buttonSelected += OnNodeSelected;
                nodeId++;
                Thread.Sleep(10);
            }
        }

        private void CreateEdge()
        {
            Edge edge = new Edge(_firstSelected, _secondSelected, this, MainCanvas);
        }

        private void ButtonMagicWond_Click(object sender, RoutedEventArgs e)
        {
            MagicWondOrder?.Invoke();
        }

        private void ButtonAddEdge_Click(object sender, RoutedEventArgs e)
        {
            shouldEdgeBeAdded = true;
            shouldNodeBeMoved = false;
            _firstSelected = null;
            _secondSelected = null;
        }

        private void ButtonSelect_Click(object sender, RoutedEventArgs e)
        {
            shouldNodeBeMoved = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            EdgeDemoAnimation();
            //AutoGenerateNodes(300);
        }
    }
}
