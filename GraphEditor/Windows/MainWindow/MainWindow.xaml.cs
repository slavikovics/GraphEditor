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
using GraphEditor.GraphLogic;
using Button = System.Windows.Controls.Button;

namespace GraphEditor
{
    public partial class MainWindow : Window
    {
        public MainWindowStates MainWindowStates;

        private int _nodeId = 1;

        public event Action OnKillAllSelections;
        public event Action OnMagicWandOrder;

        private EdgeTypes _selectedEdgeType = EdgeTypes.OrientedSimple;
        
        public Graph CurrentGraph = new Graph();

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
            (ControlTemplate)FindResource("DialogButtonTemplate"), TabViewCanvas, MainCanvas, GraphVisualTreeStackPanel, _graphsManager, this);
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
            if (MainWindowStates.ShouldBeRemoved)
            {
                Node nodeToRemove = sender as Node;
                RemoveNode(nodeToRemove);
            }

            if (!MainWindowStates.ShouldEdgeBeAdded) return;

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
                    CreateEdge(_firstSelected, _secondSelected, _selectedEdgeType);
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
            if (MainWindowStates.ShouldNodeBeAdded)
            {
                CreateNode(currentMousePosition, false, "");
            }
            else
            {
                if (!MainWindowStates.ShouldNodeBeMoved)
                {
                    return;
                }
                else if (MainWindowStates.ShouldNodeBeMoved)
                {
                    MainWindowStates.MoveToDraggingState();
                }
                _pointerPosition = currentMousePosition;
            }    
        }

        public Node CreateNode(Point currentMousePosition, bool isImported, string nodeId)
        {
            Node node;
            if (isImported) node = new Node(currentMousePosition.X, currentMousePosition.Y, MainCanvas, this, nodeId);
            else node = new Node(currentMousePosition.X, currentMousePosition.Y, MainCanvas, this, _nodeId);
            
            node.OnButtonSelected += OnNodeSelected;
            BordersInserter.InsertNodeBorder(node, _graphsManager, GraphVisualTreeStackPanel);
            _graphsManager.AnimateGraphsManagerGridExpansion(GraphVisualTreeStackPanel, GraphsManagerGrid);
            CurrentGraph.AddNode(node);

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
            return node;
        }

        public IEdge CreateEdge(Node firstSelected, Node secondSelected, EdgeTypes edgeTypes)
        {
            IEdge edge;

            switch(edgeTypes)
            {
                case EdgeTypes.NonOriented: edge = new NonOrientedEdge(firstSelected, secondSelected, this, MainCanvas); break;
                case EdgeTypes.OrientedSimple: edge = new OrientedEdge(secondSelected, firstSelected, this, MainCanvas, EdgeOrientedArrow, false); break;
                default: edge = new OrientedEdge(secondSelected, firstSelected, this, MainCanvas, EdgeOrientedArrow, true); break;
            }
            RegisterEdge(edge);
            return edge;
        }

        private void RegisterEdge(IEdge edge)
        {           
            BordersInserter.InsertEdgeBorder(edge, _graphsManager, _selectedEdgeType, GraphVisualTreeStackPanel);
            _graphsManager.AnimateGraphsManagerGridExpansion(GraphVisualTreeStackPanel, GraphsManagerGrid);
            _edgeAnimationController.AddEdge(edge);
            CurrentGraph.AddEdge(edge);
        }

        public void OnButtonMagicWondClick(object sender, RoutedEventArgs e)
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
            _graphsManager = new GraphManager((ControlTemplate)FindResource("ButtonTemplate"), (Image)ButtonAddNode.Content, (Image)ButtonAddEdge.Content, (Image)ButtonGraph.Content, this);
            InsertGraphBorder();
            AddTabView();
        }

        private void InsertGraphBorder()
        {
            GraphVisualTreeStackPanel.Children.Add(_graphsManager.AddGraph("Graph"));
            _graphsManager.AnimateGraphsManagerGridExpansion(GraphVisualTreeStackPanel, GraphsManagerGrid);
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
            if (MainWindowStates.ShouldBeDragged) MainWindowStates.MoveToMovingState();
        }

        private void OnWindowMouseMove(object sender, MouseEventArgs e)
        {
            if (!MainWindowStates.ShouldBeDragged) return;
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
            if (GraphVisualTreeStackPanel == null || GraphsManagerGrid == null || _graphsManager == null) return;
            UpdateGraphsManagerHeight();
        }

        private void UpdateGraphsManagerHeight()
        {
            _graphsManager.AnimateGraphsManagerGridExpansion(GraphVisualTreeStackPanel, GraphsManagerGrid);
        }

        private void OnOrientedSimplePopUpClick(object sender, RoutedEventArgs e)
        {
            ((Image)ButtonAddEdge.Content).Source = (OrientedSimplePopUp.Content as Image)?.Source;
            _selectedEdgeType = EdgeTypes.OrientedSimple;
            HidePopUpMenus();
        }

        private void OnOrientedPencilPopUpClick(object sender, RoutedEventArgs e)
        {
            ((Image)ButtonAddEdge.Content).Source = (OrientedPencilPopUp.Content as Image)?.Source;
            _selectedEdgeType = EdgeTypes.OrientedPencil;
            HidePopUpMenus();
        }

        private void OnNonOrientedPopUpClick(object sender, RoutedEventArgs e)
        {
            ((Image)ButtonAddEdge.Content).Source = (NonOrientedPopUp.Content as Image)?.Source;
            _selectedEdgeType = EdgeTypes.NonOriented;
            HidePopUpMenus();
        }

        private void RemoveNode(Node nodeToRemove)
        {
            CurrentGraph.RemoveNode(nodeToRemove);
            nodeToRemove.Remove();
            _nodeAnimationController.RemoveNode(nodeToRemove);
            BordersRemover.RemoveAllBordersForNode(nodeToRemove.Id, GraphVisualTreeStackPanel);
            _graphsManager.AnimateGraphsManagerGridExpansion(GraphVisualTreeStackPanel, GraphsManagerGrid);

            for (int i = 0; i < CurrentGraph.GetEdgesCount(); i++)
            {
                if (CurrentGraph.Edges[i].GetFirstNodeId() == nodeToRemove.Id || CurrentGraph.Edges[i].GetSecondNodeId() == nodeToRemove.Id)
                {
                    RemoveEdge(CurrentGraph.Edges[i]);
                    i--;
                }
            }
            CurrentGraph.PrintAllNodes();
        }

        private void RemoveEdge(IEdge edgeToRemove)
        {
            CurrentGraph.RemoveEdge(edgeToRemove);
            RemoveNode((edgeToRemove as Edge).GetCenterNode());
            edgeToRemove.Remove();
            _edgeAnimationController.RemoveEdge(edgeToRemove);
            BordersRemover.RemoveAllBordersForEdge(edgeToRemove.GetNodesDependencies()[0], edgeToRemove.GetNodesDependencies()[1], GraphVisualTreeStackPanel);
            _graphsManager.AnimateGraphsManagerGridExpansion(GraphVisualTreeStackPanel, GraphsManagerGrid);
        }

        public void SetSelectedEdgeType(EdgeTypes edgeType)
        {
            _selectedEdgeType = edgeType;
            switch (edgeType)
            {
                case EdgeTypes.OrientedSimple: ButtonAddEdge.Content = new Image() { Source = (OrientedSimplePopUp.Content as Image).Source }; break;
                case EdgeTypes.NonOriented: ButtonAddEdge.Content = new Image() { Source = (NonOrientedPopUp.Content as Image).Source }; break;
                case EdgeTypes.OrientedPencil: ButtonAddEdge.Content = new Image() { Source = (OrientedPencilPopUp.Content as Image).Source }; break;
            }
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
            if (MainWindowStates.ShouldBeRemoved)
            {
                RemoveEdge(edge);
            }
            _selectedEdge = sender as IEdge;
            Node node = (((Edge)sender).GetEdgeCenterNode() as Node);
            node.OnButtonSelected += OnNodeSelected;
            OnNodeSelected(node, e);
        }

        private void OnSaveButtonClick(object sender, RoutedEventArgs e)
        {
            HtmlExport fileInput = new HtmlExport(MainCanvas, CurrentGraph.Nodes, CurrentGraph.Edges);
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            HtmlImport htmlImport = new HtmlImport(this);
            htmlImport.OnImportStarting += _graphsManager.OnImportStarting;
            htmlImport.ImportFile();
        }

        public void UpdateGraphVisualTreeStackPanelAfterImport()
        {
            GraphVisualTreeStackPanel.Children.Clear();
            GraphVisualTreeStackPanel.Children.Add(_graphsManager.GraphName);
        }
    }
}
