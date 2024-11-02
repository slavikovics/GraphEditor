using GraphEditor.EdgesAndNodes;
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

        private const string OrientedSimple = "OrientedSimple";
        private const string OrientedPencile = "OrientedPencile";
        private const string NonOriented = "NonOriented";


        int nodeId = 1;

        public event Action KillAllSelections;
        public event Action MagicWandOrder;

        public bool shouldNodeBeAdded = false;
        public bool shouldEdgeBeAdded = false;
        public bool shouldNodeBeMoved = true;
        private bool shouldBeDragged = false;
        private bool shouldBeRemoved = false;

        private string selectedEdgeType = OrientedSimple;

        List<Node> nodes = new List<Node>();
        List<IEdgeable> edges = new List<IEdgeable>();

        Ellipse ellipse;

        private Point pointerPosition;
        private Node _firstSelected;
        private Node _secondSelected;
        private IEdgeable _selectedEdge;
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
            shouldBeRemoved = false;
            KillAllSelections?.Invoke();
            HidePopUpMenus();
        }

        public void OnNodeSelected(object sender, EventArgs e)
        {
            HidePopUpMenus();
            if (shouldBeRemoved)
            {
                Node nodeToRemove = sender as Node;
                shouldBeRemoved = false;
                RemoveNode(nodeToRemove);
            }

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
            HidePopUpMenus();
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
                nodes.Add(node);

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

        private IEdgeable CreateEdge()
        {
            IEdgeable edge;

            switch(selectedEdgeType)
            {
                case NonOriented: edge = new Edge(_firstSelected, _secondSelected, this, MainCanvas); break;
                case OrientedSimple: edge = new EdgeOriented(_secondSelected, _firstSelected, this, MainCanvas, EdgeOrientedArrow, false); break;
                default: edge = new EdgeOriented(_secondSelected, _firstSelected, this, MainCanvas, EdgeOrientedArrow, true); break;
            }
            AnimateGraphsManagerGridExpansion();
            InsertEdgeBorder(edge);
            //GraphVisualTreeStackPanel.Children.Add(GenerateGraphManagerGraphBorder("", edge.ToString(), "edge"));
            edgeAnimationController.AddEdge(edge);
            edges.Add(edge);
            return edge;
        }

        private void ButtonMagicWond_Click(object sender, RoutedEventArgs e)
        {
            HidePopUpMenus();
            MagicWandOrder?.Invoke();
            shouldEdgeBeAdded = false;
            shouldNodeBeAdded = false;
            shouldNodeBeMoved = false;
            shouldBeRemoved = false;
        }

        private void ButtonAddEdge_Click(object sender, RoutedEventArgs e)
        {
            shouldEdgeBeAdded = true;
            shouldNodeBeMoved = false;
            shouldNodeBeAdded = false;
            shouldBeRemoved = false;
            _firstSelected = null;
            _secondSelected = null;
            ShowAddEdgePopUpMenu();
        }

        private void ShowAddEdgePopUpMenu()
        {

            Point ButtonAddPosition = ButtonAddEdge.TranslatePoint(new Point(0, 0), mainGrid);

            OrientedSimplePopUp.Visibility = Visibility.Visible;
            NonOrientedPopUp.Visibility = Visibility.Visible;
            OrientedPencilePopUp.Visibility = Visibility.Visible;

            DoubleAnimation OrientedSimplePopUpTopAnimation = new DoubleAnimation();
            OrientedSimplePopUpTopAnimation.From = ButtonAddPosition.Y;
            OrientedSimplePopUpTopAnimation.To = ButtonAddPosition.Y - 45;
            OrientedSimplePopUpTopAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(200));

            DoubleAnimation NonOrientedPopUpTopAnimation = new DoubleAnimation();
            NonOrientedPopUpTopAnimation.From = ButtonAddPosition.Y;
            NonOrientedPopUpTopAnimation.To = ButtonAddPosition.Y + 5;
            NonOrientedPopUpTopAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(200));


            DoubleAnimation OrientedPencilePopUpTopAnimation = new DoubleAnimation();
            OrientedPencilePopUpTopAnimation.From = ButtonAddPosition.Y;
            OrientedPencilePopUpTopAnimation.To = ButtonAddPosition.Y + 55;
            OrientedPencilePopUpTopAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(200));

            DoubleAnimation EdgePopUpLeftAnimation = new DoubleAnimation();
            EdgePopUpLeftAnimation.From = -50;
            EdgePopUpLeftAnimation.To = 0;
            EdgePopUpLeftAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(200));

            DoubleAnimation arrowTypesWidthAnimation = new DoubleAnimation();
            arrowTypesWidthAnimation.From = 3;
            arrowTypesWidthAnimation.To = 40;
            arrowTypesWidthAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(200));

            OrientedSimplePopUp.BeginAnimation(Button.WidthProperty, arrowTypesWidthAnimation);
            OrientedSimplePopUp.BeginAnimation(Button.HeightProperty, arrowTypesWidthAnimation);

            NonOrientedPopUp.BeginAnimation(Button.WidthProperty, arrowTypesWidthAnimation);
            NonOrientedPopUp.BeginAnimation(Button.HeightProperty, arrowTypesWidthAnimation);

            OrientedPencilePopUp.BeginAnimation(Button.WidthProperty, arrowTypesWidthAnimation);
            OrientedPencilePopUp.BeginAnimation(Button.HeightProperty, arrowTypesWidthAnimation);

            OrientedSimplePopUp.BeginAnimation(Canvas.TopProperty, OrientedSimplePopUpTopAnimation);
            NonOrientedPopUp.BeginAnimation(Canvas.TopProperty, NonOrientedPopUpTopAnimation);
            OrientedPencilePopUp.BeginAnimation(Canvas.TopProperty, OrientedPencilePopUpTopAnimation);

            OrientedSimplePopUp.BeginAnimation(Canvas.LeftProperty, EdgePopUpLeftAnimation);
            NonOrientedPopUp.BeginAnimation(Canvas.LeftProperty, EdgePopUpLeftAnimation);
            OrientedPencilePopUp.BeginAnimation(Canvas.LeftProperty, EdgePopUpLeftAnimation);
        }

        private void ButtonSelect_Click(object sender, RoutedEventArgs e)
        {
            HidePopUpMenus();
            shouldNodeBeMoved = true;
            shouldNodeBeAdded = false;
            shouldEdgeBeAdded = false;
            shouldBeRemoved = false;
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

        private void InsertEdgeBorder(IEdgeable edge)
        {
            Border edgeBorder = graphsManager.AddEdge(edge, edge.GetNodesDependencies());
            edgeBorder.Margin = new Thickness(40, 4, 4, 4);
            int firstNodeId;
            if (selectedEdgeType == NonOriented)
            {
                 firstNodeId = edge.GetFirstNodeId();
            }
            else
            {
                firstNodeId = edge.GetSecondNodeId();
            }
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
            GraphVisualTreeStackPanel.Children.Add(graphsManager.AddNode(node, node.GetIdAsList()));
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
            HidePopUpMenus();
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeWindowButton_Click(object sender, RoutedEventArgs e)
        {
            HidePopUpMenus();
            if (this.WindowState == WindowState.Normal) this.WindowState = WindowState.Maximized;
            else this.WindowState = WindowState.Normal;
        }

        private void CloseWindowButton_Click(object sender, RoutedEventArgs e)
        {
            HidePopUpMenus();
            this.Close();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            HidePopUpMenus();
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
            shouldBeRemoved = false;
            HidePopUpMenus();
            InformationWindow informationWindow = new InformationWindow();
            informationWindow.Show();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            HidePopUpMenus();
        }

        private void HidePopUpMenus()
        {
            OrientedSimplePopUp.Visibility = Visibility.Hidden;
            NonOrientedPopUp.Visibility = Visibility.Hidden;
            OrientedPencilePopUp.Visibility = Visibility.Hidden;
        }

        private void GraphsManagerGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            HidePopUpMenus();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            HidePopUpMenus();
        }

        private void OrientedSimplePopUp_Click(object sender, RoutedEventArgs e)
        {
            (ButtonAddEdge.Content as Image).Source = (OrientedSimplePopUp.Content as Image).Source;
            selectedEdgeType = OrientedSimple;
            HidePopUpMenus();
        }

        private void OrientedPencilePopUp_Click(object sender, RoutedEventArgs e)
        {
            (ButtonAddEdge.Content as Image).Source = (OrientedPencilePopUp.Content as Image).Source;
            selectedEdgeType = OrientedPencile;
            HidePopUpMenus();
        }

        private void NonOrientedPopUp_Click(object sender, RoutedEventArgs e)
        {
            (ButtonAddEdge.Content as Image).Source = (NonOrientedPopUp.Content as Image).Source;
            selectedEdgeType = NonOriented;
            HidePopUpMenus();
        }

        private void RemoveNode(Node nodeToRemove)
        {
            nodeToRemove.Remove();
            nodeAnimationController.RemoveNode(nodeToRemove);
            RemoveAllBordersForNode(nodeToRemove._id);

            foreach(IEdgeable edge in edges)
            {
                if (edge.GetFirstNodeId() == nodeToRemove._id || edge.GetSecondNodeId() == nodeToRemove._id)
                {
                    RemoveEdge(edge);
                }
            }
        }

        private void RemoveEdge(IEdgeable edgeToRemove)
        {
            edgeToRemove.Remove();
            edgeAnimationController.RemoveEdge(edgeToRemove);
            RemoveAllBordersForEdge(edgeToRemove.GetNodesDependencies()[0], edgeToRemove.GetNodesDependencies()[1]);
            shouldBeRemoved = false;
        }

        private void RemoveAllBordersForNode(int nodeId)
        {
            List<GraphItemBorder> bordersToRemove = new List<GraphItemBorder>();
            foreach(GraphItemBorder border in GraphVisualTreeStackPanel.Children)
            {
                foreach(int node in border._nodesDependencies)
                {
                    if (node == nodeId)
                    {
                        bordersToRemove.Add(border);
                    }
                }
            }

            foreach(GraphItemBorder border in bordersToRemove)
            {
                GraphVisualTreeStackPanel.Children.Remove(border);
            }
        }

        private void RemoveAllBordersForEdge(int firstNodeId, int secondNodeId)
        {
            GraphItemBorder borderToRemove = null;
            foreach (GraphItemBorder border in GraphVisualTreeStackPanel.Children)
            {
                if (border._nodesDependencies.Count == 2)
                {
                    if ((firstNodeId == border._nodesDependencies[0] && secondNodeId == border._nodesDependencies[1]) || (secondNodeId == border._nodesDependencies[0] && firstNodeId == border._nodesDependencies[1]))
                    {
                        borderToRemove = border;
                    }
                }
            }


            GraphVisualTreeStackPanel.Children.Remove(borderToRemove);

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            shouldBeRemoved = true;
            shouldEdgeBeAdded = false;
            shouldNodeBeMoved = false;
            shouldNodeBeAdded = false;
            _firstSelected = null;
            _secondSelected = null;
        }

        public void OnEdgeSelected(object sender, EventArgs e)
        {
            IEdgeable edgeable = sender as IEdgeable;
            if (shouldBeRemoved)
            {
                RemoveEdge(edgeable);
            }
            _selectedEdge = sender as IEdgeable;
        }
    }
}
