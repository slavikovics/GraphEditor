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
        private bool shouldBeDraged = false;

        List<Edge> edges = new List<Edge>();

        Ellipse ellipse;

        private Point pointerPosition;
        private Node _firstSelected;
        private Node _secondSelected;
        private EdgeAnimationController edgeAnimationController;
        private NodeAnimationController nodeAnimationController;

        public MainWindow()
        {
            InitializeComponent();
        }

        //public void EdgeDemoAnimation()
        //{

        //    DoubleAnimation edgeAnimationBackWidth = new DoubleAnimation();
        //    edgeAnimationBackWidth.To = 100;
        //    edgeAnimationBackWidth.Duration = new Duration(TimeSpan.FromSeconds(0.5));
        //    edgeAnimationBackWidth.AccelerationRatio = 1;

        //    DoubleAnimation edgeAnimationFrontLeft = new DoubleAnimation();
        //    edgeAnimationFrontLeft.To = 667;
        //    edgeAnimationFrontLeft.Duration = new Duration(TimeSpan.FromSeconds(0.5));
        //    edgeAnimationFrontLeft.AccelerationRatio = 1;

        //    DependencyProperty dependencyProperty = Canvas.LeftProperty;
        //    DemoRect.BeginAnimation(WidthProperty, edgeAnimationBackWidth);

        //    //dependencyProperty = Canvas.LeftProperty;
        //    //DemoTriang.BeginAnimation(dependencyProperty, edgeAnimationFrontLeft);
        //}

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
            shouldEdgeBeAdded = false;
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
            else if (_secondSelected == null)
            {
                if (_secondSelected == null)
                {
                    _secondSelected = node;
                    CreateEdge();
                }
                _firstSelected = null;
                _secondSelected = null;
            }
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point currentMousePosition = e.GetPosition(sender as Window);
            if (currentMousePosition.Y <= 10)
            {
                this.DragMove();
                return;
            }
            if (shouldNodeBeAdded)
            {
                Node node = new Node(currentMousePosition.X, currentMousePosition.Y, MainCanvas, this, nodeId);
                node.buttonSelected += OnNodeSelected;
                if (edgeAnimationController == null)
                {
                    edgeAnimationController = new EdgeAnimationController(node);
                }
                if (nodeAnimationController == null)
                {
                    nodeAnimationController = new NodeAnimationController();
                }
                nodeAnimationController.AddNode(node);
                nodeId++;
            }
            else
            {
                if (!shouldNodeBeMoved)
                {
                    shouldNodeBeAdded = false;
                    return;
                }
                
                shouldBeDraged = true;
                pointerPosition = currentMousePosition;
            }    
        }

        private void AutoGenerateNodes(int numberOfNodes)
        {
            Random random = new Random();
            List <Node> nodes = new List<Node>();
            List <Edge> edges = new List<Edge>();   

            for (int i = 0; i < numberOfNodes; i++)
            {
                double X = random.Next((int)this.ActualWidth - 100) + 80;
                double Y = random.Next((int)this.ActualWidth - 100) + 50;
                Node node = new Node(X, Y, MainCanvas, this, nodeId);
                node.buttonSelected += OnNodeSelected;
                if (edgeAnimationController == null)
                {
                    edgeAnimationController = new EdgeAnimationController(node);
                }
                if (nodeAnimationController == null)
                {
                    nodeAnimationController = new NodeAnimationController();
                }
                nodeAnimationController.AddNode(node);
                nodes.Add(node);
                nodeId++;
                Thread.Sleep(10);
            }
            for (int i = 0; i < nodes.Count - 1; i++)
            {
                for (int j = i + 1; j < nodes.Count; j++)
                {
                    _firstSelected = nodes[i];
                    _secondSelected = nodes[j];
                    edges.Add(CreateEdge());
                }
            }
            Console.WriteLine("Nodes: " + nodes.Count);
            Console.WriteLine("Edges: " + edges.Count);
        }

        private Edge CreateEdge()
        {
            Edge edge = new Edge(_firstSelected, _secondSelected, this, MainCanvas);
            edgeAnimationController.AddEdge(edge);
            return edge;
        }

        private void ButtonMagicWond_Click(object sender, RoutedEventArgs e)
        {
            MagicWondOrder?.Invoke();
            shouldEdgeBeAdded = false;
            shouldNodeBeAdded = false;
            shouldNodeBeMoved = false;
        }

        private void ButtonAddEdge_Click(object sender, RoutedEventArgs e)
        {
            shouldEdgeBeAdded = true;
            shouldNodeBeMoved = false;
            shouldNodeBeAdded = false;
            _firstSelected = null;
            _secondSelected = null;
        }

        private void ButtonSelect_Click(object sender, RoutedEventArgs e)
        {
            shouldNodeBeMoved = true;
            shouldNodeBeAdded = false;
            shouldEdgeBeAdded = false;
            if (nodeAnimationController != null)
            {
                nodeAnimationController.EndMovementAnimations();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //EdgeDemoAnimation();
            //AutoGenerateNodes(50);
        }

        private void CollapseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeWindowButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal) this.WindowState = WindowState.Maximized;
            else this.WindowState = WindowState.Normal;
        }

        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            shouldBeDraged = false;
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (!shouldBeDraged) return;
            Point newPosition = e.GetPosition(sender as Window);
            double dragDeltaX = newPosition.X - pointerPosition.X;
            double dragDeltaY = newPosition.Y - pointerPosition.Y;

            dragDeltaX /= 2;

            pointerPosition = newPosition;

            if (nodeAnimationController != null)
            {
                nodeAnimationController.SetDragParameters(dragDeltaX, dragDeltaY);
                nodeAnimationController.Drag();

                if (edgeAnimationController != null)
                {
                    edgeAnimationController.EdgesDraged(dragDeltaX, dragDeltaY);
                }
            }

        }
    }
}
