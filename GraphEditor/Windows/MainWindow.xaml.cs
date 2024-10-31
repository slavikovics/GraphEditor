using GraphEditor.GraphsManager;
using GraphEditor.GraphsManagerControls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Button = System.Windows.Controls.Button;

namespace GraphEditor 
{
    public partial class MainWindow : Window
    {
        bool isElementSelected = false;
        bool areActionsDesired = false;

        int nodeId = 1;

        public event Action KillAllSelections;
        public event Action MagicWandOrder;

        public bool shouldNodeBeAdded = false;
        public bool shouldEdgeBeAdded = false;
        public bool shouldNodeBeMoved = true;
        private bool shouldBeDragged = false;

        List<Edge> edges = new List<Edge>();

        Ellipse ellipse;

        private Point pointerPosition;
        private Node _firstSelected;
        private Node _secondSelected;
        private EdgeAnimationController edgeAnimationController;
        private NodeAnimationController nodeAnimationController;
        private GraphManager graphsManager;

        public MainWindow()
        {
            InitializeComponent();
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
                    if (_firstSelected == _secondSelected)
                    {
                        _secondSelected = null;
                        return;
                    } // edge can e only between two different nodes
                    CreateEdge();
                }
                _firstSelected = null;
                _secondSelected = null;
            }
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point currentMousePosition = e.GetPosition(sender as Window);
            if (currentMousePosition.Y <= 20)
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    this.DragMove();
                }
                return;
            }
            if (shouldNodeBeAdded)
            {
                Node node = new Node(currentMousePosition.X, currentMousePosition.Y, MainCanvas, this, nodeId);
                node.buttonSelected += OnNodeSelected;
                AnimateGraphsManagerGridExpansion();   
                InsertNodeBorder(node);

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
                
                shouldBeDragged = true;
                pointerPosition = currentMousePosition;
            }    
        }

        //private void AutoGenerateNodes(int numberOfNodes)
        //{
        //    Random random = new Random();
        //    List <Node> nodes = new List<Node>();
        //    List <Edge> edges = new List<Edge>();   

        //    for (int i = 0; i < numberOfNodes; i++)
        //    {
        //        double X = random.Next((int)this.ActualWidth - 100) + 80;
        //        double Y = random.Next((int)this.ActualWidth - 100) + 50;
        //        Node node = new Node(X, Y, MainCanvas, this, nodeId);
        //        node.buttonSelected += OnNodeSelected;
        //        if (edgeAnimationController == null)
        //        {
        //            edgeAnimationController = new EdgeAnimationController(node);
        //        }
        //        if (nodeAnimationController == null)
        //        {
        //            nodeAnimationController = new NodeAnimationController();
        //        }
        //        nodeAnimationController.AddNode(node);
        //        nodes.Add(node);
        //        nodeId++;
        //        //Thread.Sleep(10);
        //    }
        //    for (int i = 0; i < nodes.Count - 1; i++)
        //    {
        //        for (int j = i + 1; j < nodes.Count; j++)
        //        {
        //            _firstSelected = nodes[i];
        //            _secondSelected = nodes[j];
        //            edges.Add(CreateEdge());
        //        }
        //    }
        //    Console.WriteLine("Nodes: " + nodes.Count);
        //    Console.WriteLine("Edges: " + edges.Count);
        //}

        private Edge CreateEdge()
        {
            Edge edge = new Edge(_firstSelected, _secondSelected, this, MainCanvas);
            AnimateGraphsManagerGridExpansion();
            InsertEdgeBorder(edge);
            //GraphVisualTreeStackPanel.Children.Add(GenerateGraphManagerGraphBorder("", edge.ToString(), "edge"));
            edgeAnimationController.AddEdge(edge);
            return edge;
        }

        private void ButtonMagicWond_Click(object sender, RoutedEventArgs e)
        {
            MagicWandOrder?.Invoke();
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
            //AutoGenerateNodes(5);
            graphsManager = new GraphManager((ControlTemplate)FindResource("ButtonTemplate"), (Image)ButtonAddNode.Content, (Image)ButtonAddEdge.Content, (Image)ButtonGraph.Content);
            InsertGraphBorder();
            
        }

        private void InsertGraphBorder()
        {
            GraphVisualTreeStackPanel.Children.Add(graphsManager.AddGraph("Graph"));
        }

        private void InsertEdgeBorder(Edge edge)
        {
            Border edgeBorder = graphsManager.AddEdge(edge);
            edgeBorder.Margin = new Thickness(40, 4, 4, 4);
            int firstNodeId = edge.GetFirstNodeId();
            int i = 0;
            foreach (UIElement uIElement in GraphVisualTreeStackPanel.Children)
            {
                i++;
                if ((uIElement as Border)?.Name == "node" + firstNodeId.ToString())
                {
                    break;
                }
            }

            GraphVisualTreeStackPanel.Children.Insert(i, edgeBorder);
        }

        private void InsertNodeBorder(Node node)
        {
            GraphVisualTreeStackPanel.Children.Add(graphsManager.AddNode(node));
        }

        private void AnimateGraphsManagerGridExpansion()
        {
            if (GraphsManagerGrid.ActualHeight >= 600) return;
            DoubleAnimation gridAnimation = new DoubleAnimation();
            gridAnimation.To = GraphsManagerGrid.ActualHeight + 38;
            gridAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(100));
            GraphsManagerGrid.BeginAnimation(HeightProperty, gridAnimation);
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
            shouldBeDragged = false;
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (!shouldBeDragged) return;
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
                    edgeAnimationController.EdgesDragged(dragDeltaX, dragDeltaY);
                }
            }

        }

        private void ButtonGraph_Click(object sender, RoutedEventArgs e)
        {
            InformationWindow informationWindow = new InformationWindow();
            informationWindow.Show();
        }
    }
}
