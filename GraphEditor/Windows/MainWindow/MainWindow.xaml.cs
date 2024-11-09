using GraphEditor.EdgesAndNodes;
using GraphEditor.EdgesAndNodes.Edges;
using GraphEditor.GraphsManager;
using GraphEditor.GraphsSavingAndLoading;
using GraphEditor.GraphTabs;
using GraphEditor.Windows.MainWindow;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Xml.Linq;
using Button = System.Windows.Controls.Button;

namespace GraphEditor
{
    public partial class MainWindow : Window
    {
        public MainWindowStates MainWindowStates;

        private int _nodeId = 1;

        public event Action OnKillAllSelections;
        public event Action OnMagicWandOrder;

        private Edge.EdgeTypes _selectedEdgeType = Edge.EdgeTypes.OrientedSimple;

        private List<Node> _nodes = new List<Node>();
        private List<IEdge> _edges = new List<IEdge>();

        private Point _pointerPosition;
        private Node _firstSelected;
        private Node _secondSelected;
        private IEdge _selectedEdge;
        private EdgeAnimationController _edgeAnimationController;
        private NodeAnimationController _nodeAnimationController;
        private GraphManager _graphsManager;
        private TabView _tabView;

        public MainWindow()
        {
            InitializeComponent();
            MainWindowStates = new MainWindowStates();
        }

        private void AddTabView()
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = "Graph 1";
            _tabView = new TabView(textBlock, (Image)ButtonAddNode.Content, (ControlTemplate)FindResource("ButtonTemplate"), 
            (ControlTemplate)FindResource("DialogButtonTemplate"), TabViewCanvas, MainCanvas, GraphVisualTreeStackPanel, _graphsManager);
            _tabView.TabLoaded += OnTabViewTabLoaded;
            _tabView.AddTabViewToMainWindow();
        }

        private void OnTabViewTabLoaded()
        {
            _tabView.ResizeGraphsManagerGrid(GraphsManagerGrid);
        }

        private void OnButtonMouseEnter(object sender, MouseEventArgs e)
        {
            VisualStateManager.GoToState(sender as Button, "MouseOver", true);
        }

        private void OnButtonMouseLeave(object sender, MouseEventArgs e)
        {
            VisualStateManager.GoToState(sender as Button, "Normal", true);
        }

        private void OnButtonMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            VisualStateManager.GoToState(sender as Button, "Pressed", true);
        }

        private void OnButtonMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            VisualStateManager.GoToState(sender as Button, "Normal", true);
        }

        private void OnButtonAddNodeClick(object sender, RoutedEventArgs e)
        {
            MainWindowStates.MoveToNodeAddingState();
            OnKillAllSelections?.Invoke();
            HidePopUpMenus();
        }

        public void OnNodeSelected(object sender, EventArgs e)
        {
            HidePopUpMenus();
            if (MainWindowStates.shouldBeRemoved)
            {
                Node nodeToRemove = sender as Node;
                RemoveNode(nodeToRemove);
            }

            if (!MainWindowStates.shouldEdgeBeAdded) return;

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
                    }
                    CreateEdge();
                }
                _firstSelected = null;
                _secondSelected = null;
            }
        }

        private void OnCanvasMouseDown(object sender, MouseButtonEventArgs e)
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
            if (MainWindowStates.shouldNodeBeAdded)
            {
                Node node = new Node(currentMousePosition.X, currentMousePosition.Y, MainCanvas, this, _nodeId);
                node.OnButtonSelected += OnNodeSelected;  
                BordersInserter.InsertNodeBorder(node, _graphsManager, GraphVisualTreeStackPanel);
                GraphManager.AnimateGraphsManagerGridExpansion(GraphVisualTreeStackPanel, GraphsManagerGrid);
                _nodes.Add(node);

                if (_edgeAnimationController == null)
                {
                    _edgeAnimationController = new EdgeAnimationController(node, MainCanvas);
                }
                if (_nodeAnimationController == null)
                {
                    _nodeAnimationController = new NodeAnimationController(MainCanvas);
                }
                _nodeAnimationController.AddNode(node);
                _nodeId++;
            }
            else
            {
                if (!MainWindowStates.shouldNodeBeMoved)
                {
                    return;
                }
                else if (MainWindowStates.shouldNodeBeMoved)
                {
                    MainWindowStates.MoveToDraggingState();
                }
                _pointerPosition = currentMousePosition;
            }    
        }

        private IEdge CreateEdge()
        {
            IEdge edge;

            switch(_selectedEdgeType)
            {
                case Edge.EdgeTypes.NonOriented: edge = new NonOrientedEdge(_firstSelected, _secondSelected, this, MainCanvas); break;
                case Edge.EdgeTypes.OrientedSimple: edge = new OrientedEdge(_secondSelected, _firstSelected, this, MainCanvas, EdgeOrientedArrow, false); break;
                default: edge = new OrientedEdge(_secondSelected, _firstSelected, this, MainCanvas, EdgeOrientedArrow, true); break;
            }
            RegisterEdge(edge);
            return edge;
        }

        private void RegisterEdge(IEdge edge)
        {           
            BordersInserter.InsertEdgeBorder(edge, _graphsManager, _selectedEdgeType, GraphVisualTreeStackPanel);
            GraphManager.AnimateGraphsManagerGridExpansion(GraphVisualTreeStackPanel, GraphsManagerGrid);
            _edgeAnimationController.AddEdge(edge);
            _edges.Add(edge);
        }

        private void OnButtonMagicWondClick(object sender, RoutedEventArgs e)
        {
            HidePopUpMenus();
            OnMagicWandOrder?.Invoke();
            MainWindowStates.MoveToMagicState();
        }

        private void OnButtonAddEdgeClick(object sender, RoutedEventArgs e)
        {
            MainWindowStates.MoveToEdgeAddingState();
            _firstSelected = null;
            _secondSelected = null;
            ShowAddEdgePopUpMenu();
        }

        private void ShowAddEdgePopUpMenu()
        {
            Point ButtonAddPosition = ButtonAddEdge.TranslatePoint(new Point(0, 0), mainGrid);

            OrientedSimplePopUp.Visibility = Visibility.Visible;
            NonOrientedPopUp.Visibility = Visibility.Visible;
            OrientedPencilPopUp.Visibility = Visibility.Visible;

            DoubleAnimation OrientedSimplePopUpTopAnimation = MainWindowAnimator.BuildOrientedSimplePopUpTopAnimation(ButtonAddPosition);
            DoubleAnimation NonOrientedPopUpTopAnimation = MainWindowAnimator.BuildNonOrientedPopUpTopAnimation(ButtonAddPosition);
            DoubleAnimation OrientedPencilePopUpTopAnimation = MainWindowAnimator.BuildOrientedPencilePopUpTopAnimation(ButtonAddPosition);
            DoubleAnimation EdgePopUpLeftAnimation = MainWindowAnimator.BuildEdgePopUpLeftAnimation();
            DoubleAnimation arrowTypesWidthAnimation = MainWindowAnimator.BuildArrowTypesWidthAnimation();

            OrientedSimplePopUp.BeginAnimation(Button.WidthProperty, arrowTypesWidthAnimation);
            OrientedSimplePopUp.BeginAnimation(Button.HeightProperty, arrowTypesWidthAnimation);

            NonOrientedPopUp.BeginAnimation(Button.WidthProperty, arrowTypesWidthAnimation);
            NonOrientedPopUp.BeginAnimation(Button.HeightProperty, arrowTypesWidthAnimation);

            OrientedPencilPopUp.BeginAnimation(Button.WidthProperty, arrowTypesWidthAnimation);
            OrientedPencilPopUp.BeginAnimation(Button.HeightProperty, arrowTypesWidthAnimation);

            OrientedSimplePopUp.BeginAnimation(Canvas.TopProperty, OrientedSimplePopUpTopAnimation);
            NonOrientedPopUp.BeginAnimation(Canvas.TopProperty, NonOrientedPopUpTopAnimation);
            OrientedPencilPopUp.BeginAnimation(Canvas.TopProperty, OrientedPencilePopUpTopAnimation);

            OrientedSimplePopUp.BeginAnimation(Canvas.LeftProperty, EdgePopUpLeftAnimation);
            NonOrientedPopUp.BeginAnimation(Canvas.LeftProperty, EdgePopUpLeftAnimation);
            OrientedPencilPopUp.BeginAnimation(Canvas.LeftProperty, EdgePopUpLeftAnimation);
        }

        private void OnButtonSelectClick(object sender, RoutedEventArgs e)
        {
            HidePopUpMenus();
            MainWindowStates.MoveToMovingState();
            if (_nodeAnimationController != null)
            {
                _nodeAnimationController.EndMovementAnimations();
            }
        }

        private void OnWindowLoaded(object sender, RoutedEventArgs e)
        {
            _graphsManager = new GraphManager((ControlTemplate)FindResource("ButtonTemplate"), (Image)ButtonAddNode.Content, (Image)ButtonAddEdge.Content, (Image)ButtonGraph.Content);
            InsertGraphBorder();
            AddTabView();
        }

        private void InsertGraphBorder()
        {
            GraphVisualTreeStackPanel.Children.Add(_graphsManager.AddGraph("Graph"));
            GraphManager.AnimateGraphsManagerGridExpansion(GraphVisualTreeStackPanel, GraphsManagerGrid);
        }

        private void OnCollapseWindowButtonClick(object sender, RoutedEventArgs e)
        {
            HidePopUpMenus();
            this.WindowState = WindowState.Minimized;
        }

        private void OnMaximizeWindowButtonClick(object sender, RoutedEventArgs e)
        {
            HidePopUpMenus();
            if (this.WindowState == WindowState.Normal) this.WindowState = WindowState.Maximized;
            else this.WindowState = WindowState.Normal;
        }

        private void OnCloseWindowButtonClick(object sender, RoutedEventArgs e)
        {
            HidePopUpMenus();
            this.Close();
        }

        private void OnGridMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            HidePopUpMenus();
            this.DragMove();
        }

        private void OnWindowMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (MainWindowStates.shouldBeDragged) MainWindowStates.MoveToMovingState();
        }

        private void OnWindowMouseMove(object sender, MouseEventArgs e)
        {
            if (!MainWindowStates.shouldBeDragged) return;
            Point newPosition = e.GetPosition(sender as Window);
            double dragDeltaX = newPosition.X - _pointerPosition.X;
            double dragDeltaY = newPosition.Y - _pointerPosition.Y;

            dragDeltaX /= 2;

            _pointerPosition = newPosition;

            if (_nodeAnimationController != null)
            {
                _nodeAnimationController.SetDragParameters(dragDeltaX, dragDeltaY);
                _nodeAnimationController.Drag();

                if (_edgeAnimationController != null)
                {
                    _edgeAnimationController.EdgesDragged(dragDeltaX, dragDeltaY);
                }
            }
        }

        private void OnButtonGraphClick(object sender, RoutedEventArgs e)
        {
            HidePopUpMenus();
            InformationWindow informationWindow = new InformationWindow();
            informationWindow.Show();
        }

        private void OmWindowMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            HidePopUpMenus();
        }

        private void HidePopUpMenus()
        {
            OrientedSimplePopUp.Visibility = Visibility.Hidden;
            NonOrientedPopUp.Visibility = Visibility.Hidden;
            OrientedPencilPopUp.Visibility = Visibility.Hidden;
        }

        private void OnGraphsManagerGridMouseDown(object sender, MouseButtonEventArgs e)
        {
            HidePopUpMenus();
        }

        private void OnWindowSizeChanged(object sender, SizeChangedEventArgs e)
        {
            HidePopUpMenus();
        }

        private void OnOrientedSimplePopUpClick(object sender, RoutedEventArgs e)
        {
            (ButtonAddEdge.Content as Image).Source = (OrientedSimplePopUp.Content as Image).Source;
            _selectedEdgeType = Edge.EdgeTypes.OrientedSimple;
            HidePopUpMenus();
        }

        private void OnOrientedPencilPopUpClick(object sender, RoutedEventArgs e)
        {
            (ButtonAddEdge.Content as Image).Source = (OrientedPencilPopUp.Content as Image).Source;
            _selectedEdgeType = Edge.EdgeTypes.OrientedPencil;
            HidePopUpMenus();
        }

        private void OnNonOrientedPopUpClick(object sender, RoutedEventArgs e)
        {
            (ButtonAddEdge.Content as Image).Source = (NonOrientedPopUp.Content as Image).Source;
            _selectedEdgeType = Edge.EdgeTypes.NonOriented;
            HidePopUpMenus();
        }

        private void RemoveNode(Node nodeToRemove)
        {
            nodeToRemove.Remove();
            _nodeAnimationController.RemoveNode(nodeToRemove);
            BordersRemover.RemoveAllBordersForNode(nodeToRemove.Id, GraphVisualTreeStackPanel);
            GraphManager.AnimateGraphsManagerGridExpansion(GraphVisualTreeStackPanel, GraphsManagerGrid);

            foreach (IEdge edge in _edges)
            {
                if (edge.GetFirstNodeId() == nodeToRemove.Id || edge.GetSecondNodeId() == nodeToRemove.Id)
                {
                    RemoveEdge(edge);
                }
            }
        }

        private void RemoveEdge(IEdge edgeToRemove)
        {
            edgeToRemove.Remove();
            _edgeAnimationController.RemoveEdge(edgeToRemove);
            BordersRemover.RemoveAllBordersForEdge(edgeToRemove.GetNodesDependencies()[0], edgeToRemove.GetNodesDependencies()[1], GraphVisualTreeStackPanel);
            GraphManager.AnimateGraphsManagerGridExpansion(GraphVisualTreeStackPanel, GraphsManagerGrid);
        }

        private void OnDeleteButtonClick(object sender, RoutedEventArgs e)
        {
            HidePopUpMenus();
            MainWindowStates.MoveToRemovingState();
            _firstSelected = null;
            _secondSelected = null;
        }

        public void OnEdgeSelected(object sender, EventArgs e)
        {
            IEdge edge = sender as IEdge;
            if (MainWindowStates.shouldBeRemoved)
            {
                RemoveEdge(edge);
            }
            _selectedEdge = sender as IEdge;
            Node node = ((sender as Edge).GetEdgeCenterNode() as Node);
            node.OnButtonSelected += OnNodeSelected;
            OnNodeSelected(node, e);
        }

        private void OnSaveButtonClick(object sender, RoutedEventArgs e)
        {
            FileInput fileInput = new FileInput(MainCanvas);
        }
    }
}
