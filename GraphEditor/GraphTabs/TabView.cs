using GraphEditor.GraphTab;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace GraphEditor.GraphTabs
{
    internal class TabView
    {
        private List<TabButton> _tabViewButtons;

        private List<List<UIElement>> _uiElementsCollections;

        private int _selectedTabId;

        private int _maxId = 1;

        private ControlTemplate _tabButtonTemplate;

        private Canvas _canvasTabViewUI;

        private Canvas _canvasMain;

        public TabView(TextBlock tabContent, Image addNewTabContent, ControlTemplate buttonAddTemplate, 
            ControlTemplate tabButtonTemplate, Canvas canvasTabViewUI, Canvas canvasMainCanvas)
        {
            InitializeTabView(canvasTabViewUI, tabButtonTemplate, canvasMainCanvas);
            SetUpAddNewTabButton(addNewTabContent, buttonAddTemplate);
            CreateTab(tabContent);
            AddTabViewToMainWindow();
        }

        private void SetUpAddNewTabButton(Image addNewTabContent, ControlTemplate buttonAddTemplate)
        {
            Tab addNewTab = new Tab(addNewTabContent, buttonAddTemplate, 0);
            TabButton addNewAsButton = addNewTab.GetTabAsTabButton();
            _tabViewButtons.Add(addNewAsButton);
            addNewAsButton.Click += OnAddNewTabClick;
        }

        private void CreateTab(TextBlock tabContent)
        {
            _uiElementsCollections.Add(new List<UIElement>());
            Tab tab = new Tab(tabContent, _tabButtonTemplate, _maxId);
            _maxId++;
            TabButton tabAsButton = tab.GetTabAsTabButton();
            _tabViewButtons.Insert(_uiElementsCollections.Count - 1, tabAsButton);
            tabAsButton.Click += OnExistingTabClick;
        }

        private void InitializeTabView(Canvas canvas, ControlTemplate tabButtonTemplate, Canvas mainCanvas)
        {
            _canvasTabViewUI = canvas;
            _canvasMain = mainCanvas;
            _tabButtonTemplate = tabButtonTemplate;
            _selectedTabId = 1;
            _uiElementsCollections = new List<List<UIElement>>();
            _tabViewButtons = new List<TabButton>();
        }

        private void OnAddNewTabClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_uiElementsCollections.Count >= 8) return;
            TextBlock textBlock = new TextBlock();
            textBlock.Text = "Graph " + (_uiElementsCollections.Count + 1);
            CreateTab(textBlock);
            AddTabViewToMainWindow();
            AnimateTabViewAddingNewTab();
        }

        private void AnimateTabViewAddingNewTab()
        {
            _tabViewButtons[_tabViewButtons.Count - 2].AnimateButtonExpansion();
            _tabViewButtons[_tabViewButtons.Count - 1].AnimateButtonMovementRight(CalculatePosition());
        }

        private double CalculatePosition()
        {
            double position = (_tabViewButtons.Count - 1) * (_tabViewButtons[1].Width + 10);
            return position;
        }

        private void OnExistingTabClick(object sender, System.Windows.RoutedEventArgs e)
        {
            TabButton tabButton = (sender as TabButton);
            int senderId = tabButton.TabButtonId;
            SwitchTab(senderId);

            foreach(TabButton button in _tabViewButtons)
            {
                if (button != tabButton && button.TabButtonId != 0)
                {
                    button.AnimationHeightShrinking();
                }
            }

            tabButton.AnimationHeightExpansion();
        }

        public void AddTabViewToMainWindow()
        {
            EmptyCanvasChildren(_canvasTabViewUI);

            double CanvasLeft = 0;
            double CanvasTop = 0;
            double Margin = 10;

            foreach(TabButton renamingButton in _tabViewButtons)
            {
                renamingButton.SetValue(Canvas.LeftProperty, CanvasLeft);
                renamingButton.SetValue(Canvas.TopProperty, CanvasTop);
                CanvasLeft += renamingButton.Width + Margin;
                _canvasTabViewUI.Children.Add(renamingButton);
            }
        }

        public void SwitchTab(int targetId)
        {

            SaveCanvasChildren();
            EmptyCanvasChildren(_canvasMain);
            AddChildrenToCanvas(targetId);
            _selectedTabId = targetId;

        }

        private void SaveCanvasChildren()
        {
            _uiElementsCollections[_selectedTabId - 1].Clear();

            foreach(UIElement element in _canvasMain.Children)
            {
                _uiElementsCollections[_selectedTabId - 1].Add(element);
            }
        }

        private void EmptyCanvasChildren(Canvas canvas)
        {
            canvas.Children.Clear();
        }

        private void AddChildrenToCanvas(int id)
        {
            foreach (UIElement uiElement in _uiElementsCollections[id-1])
            {
                _canvasMain.Children.Add(uiElement);
            }
        }

    }
}
