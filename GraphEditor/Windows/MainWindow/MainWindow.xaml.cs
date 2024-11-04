using GraphEditor.EdgesAndNodes;
using GraphEditor.GraphsManager;
using GraphEditor.GraphsManagerControls;
using GraphEditor.GraphTabs;
using GraphEditor.Windows.MainWindow;
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
        private const string OrientedSimple = "OrientedSimple";
        private const string OrientedPencile = "OrientedPencile";
        private const string NonOriented = "NonOriented";

        public MainWindowStates states;

        int nodeId = 1;

        public event Action KillAllSelections;
        public event Action MagicWandOrder;

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
            AddTabView();
            states = new MainWindowStates();
        }

        private void AddTabView()
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = "Graph 1";
            TabView tabView = new TabView(textBlock, (Image)ButtonAddNode.Content, (ControlTemplate)FindResource("ButtonTemplate"), (ControlTemplate)FindResource("DialogButtonTemplate"), TabViewCanvas);
            tabView.AddTabViewToMainWindow();
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
            states.MoveToNodeAddingState();
            KillAllSelections?.Invoke();
            HidePopUpMenus();
        }

        public void OnNodeSelected(object sender, EventArgs e)
        {
            HidePopUpMenus();
            if (states.shouldBeRemoved)
            {
                Node nodeToRemove = sender as Node;
                //states.MoveToMovingState();
                RemoveNode(nodeToRemove);
            }

            if (!states.shouldEdgeBeAdded) return;

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
                    } // edge can be only between two different nodes
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
            if (states.shouldNodeBeAdded)
            {
                Node node = new Node(currentMousePosition.X, currentMousePosition.Y, MainCanvas, this, nodeId);
                node.buttonSelected += OnNodeSelected;
                GraphManager.AnimateGraphsManagerGridExpansion(GraphVisualTreeStackPanel, GraphsManagerGrid);   
                BordersInserter.InsertNodeBorder(node, graphsManager, GraphVisualTreeStackPanel);
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
                if (!states.shouldNodeBeMoved)
                {
                    //states.MoveToMovingState();
                    return;
                }
                else if (states.shouldNodeBeMoved)
                {
                    states.MoveToDraggingState();
                }
                pointerPosition = currentMousePosition;
            }    
        }

        private IEdgeable CreateEdge()
        {
            IEdgeable edge;

            switch(selectedEdgeType)
            {
                case NonOriented: edge = new EdgeNonOriented(_firstSelected, _secondSelected, this, MainCanvas); break;
                case OrientedSimple: edge = new EdgeOriented(_secondSelected, _firstSelected, this, MainCanvas, EdgeOrientedArrow, false); break;
                default: edge = new EdgeOriented(_secondSelected, _firstSelected, this, MainCanvas, EdgeOrientedArrow, true); break;
            }
            RegisterEdge(edge);
            return edge;
        }

        private void RegisterEdge(IEdgeable edge)
        {
            GraphManager.AnimateGraphsManagerGridExpansion(GraphVisualTreeStackPanel, GraphsManagerGrid);
            BordersInserter.InsertEdgeBorder(edge, graphsManager, selectedEdgeType, GraphVisualTreeStackPanel, NonOriented);
            edgeAnimationController.AddEdge(edge);
            edges.Add(edge);
        }

        private void ButtonMagicWond_Click(object sender, RoutedEventArgs e)
        {
            HidePopUpMenus();
            MagicWandOrder?.Invoke();
            states.MoveToMagicState();
        }

        private void ButtonAddEdge_Click(object sender, RoutedEventArgs e)
        {
            states.MoveToEdgeAddingState();
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

            DoubleAnimation OrientedSimplePopUpTopAnimation = MainWindowAnimator.BuildOrientedSimplePopUpTopAnimation(ButtonAddPosition);
            DoubleAnimation NonOrientedPopUpTopAnimation = MainWindowAnimator.BuildNonOrientedPopUpTopAnimation(ButtonAddPosition);
            DoubleAnimation OrientedPencilePopUpTopAnimation = MainWindowAnimator.BuildOrientedPencilePopUpTopAnimation(ButtonAddPosition);
            DoubleAnimation EdgePopUpLeftAnimation = MainWindowAnimator.BuildEdgePopUpLeftAnimation();
            DoubleAnimation arrowTypesWidthAnimation = MainWindowAnimator.BuildArrowTypesWidthAnimation();

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
            states.MoveToMovingState();
            if (nodeAnimationController != null)
            {
                nodeAnimationController.EndMovementAnimations();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            graphsManager = new GraphManager((ControlTemplate)FindResource("ButtonTemplate"), (Image)ButtonAddNode.Content, (Image)ButtonAddEdge.Content, (Image)ButtonGraph.Content);
            InsertGraphBorder();
        }

        private void InsertGraphBorder()
        {
            GraphVisualTreeStackPanel.Children.Add(graphsManager.AddGraph("Graph"));
            GraphManager.AnimateGraphsManagerGridExpansion(GraphVisualTreeStackPanel, GraphsManagerGrid);
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
            if (states.shouldBeDragged) states.MoveToMovingState();
        }

        private void WindowMouseMove(object sender, MouseEventArgs e)
        {
            if (!states.shouldBeDragged) return;
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
            BordersRemover.RemoveAllBordersForNode(nodeToRemove._id, GraphVisualTreeStackPanel);
            GraphManager.AnimateGraphsManagerGridExpansion(GraphVisualTreeStackPanel, GraphsManagerGrid);

            foreach (IEdgeable edge in edges)
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
            BordersRemover.RemoveAllBordersForEdge(edgeToRemove.GetNodesDependencies()[0], edgeToRemove.GetNodesDependencies()[1], GraphVisualTreeStackPanel);
            GraphManager.AnimateGraphsManagerGridExpansion(GraphVisualTreeStackPanel, GraphsManagerGrid);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            states.MoveToRemovingState();
            _firstSelected = null;
            _secondSelected = null;
        }

        public void OnEdgeSelected(object sender, EventArgs e)
        {
            IEdgeable edge = sender as IEdgeable;
            if (states.shouldBeRemoved)
            {
                RemoveEdge(edge);
            }
            _selectedEdge = sender as IEdgeable;
        }
    }
}
