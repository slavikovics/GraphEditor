using GraphEditor.GraphTab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace GraphEditor.GraphTabs
{
    internal class TabView
    {
        private List<TabButton> TabViewButtons;

        private List<List<UIElement>> Collections;

        private int selectedTabId;

        private int maxId = 1;

        private ControlTemplate _tabButtonTemplate;

        private Canvas _canvasTabViewUI;

        private Canvas _canvasMainCanvas;

        public TabView(TextBlock TabContent, Image addNewTabContent, ControlTemplate buttonAddTemplate, 
            ControlTemplate tabButtonTemplate, Canvas canvasTabViewUI, Canvas canvasMainCanvas)
        {
            InitializeTabView(canvasTabViewUI, tabButtonTemplate, canvasMainCanvas);
            SetUpAddNewTabButton(addNewTabContent, buttonAddTemplate);
            CreateTab(TabContent);
            AddTabViewToMainWindow();
        }

        private void SetUpAddNewTabButton(Image addNewTabContent, ControlTemplate buttonAddTemplate)
        {
            Tab addNewTab = new Tab(addNewTabContent, buttonAddTemplate, 0);
            TabButton addNewAsButton = addNewTab.GetTabAsTabButton();
            TabViewButtons.Add(addNewAsButton);
            addNewAsButton.Click += AddNewTabClick;
        }

        private void CreateTab(TextBlock TabContent)
        {
            Collections.Add(new List<UIElement>());
            Tab tab = new Tab(TabContent, _tabButtonTemplate, maxId);
            maxId++;
            TabButton tabAsButton = tab.GetTabAsTabButton();
            TabViewButtons.Insert(Collections.Count - 1, tabAsButton);
            tabAsButton.Click += TabClick;
        }

        private void InitializeTabView(Canvas canvas, ControlTemplate tabButtonTemplate, Canvas mainCanvas)
        {
            _canvasTabViewUI = canvas;
            _canvasMainCanvas = mainCanvas;
            _tabButtonTemplate = tabButtonTemplate;
            selectedTabId = 1;
            Collections = new List<List<UIElement>>();
            TabViewButtons = new List<TabButton>();
        }

        private void AddNewTabClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (Collections.Count >= 8) return;
            TextBlock textBlock = new TextBlock();
            textBlock.Text = "Graph " + (Collections.Count + 1);
            CreateTab(textBlock);
            AddTabViewToMainWindow();
            AnimateTabViewAddingNewTab();
        }

        private void AnimateTabViewAddingNewTab()
        {
            TabViewButtons[TabViewButtons.Count - 2].AnimateButtonExpansion();
            TabViewButtons[TabViewButtons.Count - 1].AnimateButtonMovementRight(CalculatePosition());
        }

        private double CalculatePosition()
        {
            double position = (TabViewButtons.Count - 1) * (TabViewButtons[1].Width + 10);
            return position;
        }

        private void TabClick(object sender, System.Windows.RoutedEventArgs e)
        {
            TabButton tabButton = (sender as TabButton);
            int senderId = tabButton._tabButtonId;
            SwitchTab(senderId);

            foreach(TabButton button in TabViewButtons)
            {
                if (button != tabButton && button._tabButtonId != 0)
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

            foreach(TabButton renamingButton in TabViewButtons)
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
            EmptyCanvasChildren(_canvasMainCanvas);
            AddChildrenToCanvas(targetId);
            selectedTabId = targetId;

        }

        private void SaveCanvasChildren()
        {
            Collections[selectedTabId - 1].Clear();

            foreach(UIElement element in _canvasMainCanvas.Children)
            {
                Collections[selectedTabId - 1].Add(element);
            }
        }

        private void EmptyCanvasChildren(Canvas canvas)
        {
            canvas.Children.Clear();
        }

        private void AddChildrenToCanvas(int id)
        {
            foreach (UIElement uiElement in Collections[id-1])
            {
                _canvasMainCanvas.Children.Add(uiElement);
            }
        }

    }
}
