﻿using GraphEditor.GraphsManager;
using GraphEditor.GraphTab;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace GraphEditor.GraphTabs
{
    internal class TabView
    {
        private List<TabButton> _tabViewButtons;

        private List<List<UIElement>> _uiElementsCollections;

        private List<List<UIElement>> _graphManagerCollections;

        private int _selectedTabId;

        private int _maxId = 1;

        private ControlTemplate _tabButtonTemplate;

        private Canvas _canvasTabViewUI;

        private Canvas _canvasMain;

        private StackPanel _stackPanelGraphManager;

        private GraphManager _graphManager;

        public event Action TabLoaded;

        public TabView(TextBlock tabContent, Image addNewTabContent, ControlTemplate buttonAddTemplate, 
            ControlTemplate tabButtonTemplate, Canvas canvasTabViewUI, Canvas canvasMainCanvas, 
            StackPanel stackPanelGraphManager, GraphManager graphManager)
        {
            InitializeTabView(canvasTabViewUI, tabButtonTemplate, canvasMainCanvas, stackPanelGraphManager, graphManager);
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
            _graphManagerCollections.Add(new List<UIElement>());
            Tab tab = new Tab(tabContent, _tabButtonTemplate, _maxId);
            _maxId++;
            TabButton tabAsButton = tab.GetTabAsTabButton();
            _tabViewButtons.Insert(_uiElementsCollections.Count - 1, tabAsButton);
            tabAsButton.Click += OnExistingTabClick;
        }

        private void InitializeTabView(Canvas canvas, ControlTemplate tabButtonTemplate, Canvas mainCanvas, 
            StackPanel stackPanelGraphManager, GraphManager graphManager)
        {
            _canvasTabViewUI = canvas;
            _canvasMain = mainCanvas;
            _stackPanelGraphManager = stackPanelGraphManager;
            _tabButtonTemplate = tabButtonTemplate;
            _selectedTabId = 1;
            _uiElementsCollections = new List<List<UIElement>>();
            _graphManagerCollections = new List<List<UIElement>>();
            _tabViewButtons = new List<TabButton>();
            _graphManager = graphManager;
        }

        private void OnAddNewTabClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_uiElementsCollections.Count >= 8) return;
            TextBlock textBlock = new TextBlock();
            textBlock.Text = "Graph " + (_uiElementsCollections.Count + 1);
            CreateTab(textBlock);
            AddTabViewToMainWindow();
            AddGraphNameToStackPanel(textBlock.Text);
            AnimateTabViewAddingNewTab();
        }

        private void AnimateTabViewAddingNewTab()
        {
            _tabViewButtons[_tabViewButtons.Count - 2].AnimateButtonExpansion();
            _tabViewButtons[_tabViewButtons.Count - 1].AnimateButtonMovementRight(CalculatePosition());
        }

        public void ResizeGraphsManagerGrid(Grid graphsManagerGrid)
        {
            GraphManager.AnimateGraphsManagerGridExpansion(_stackPanelGraphManager, graphsManagerGrid);
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
            TabLoaded?.Invoke();
        }

        private void AddGraphNameToStackPanel(string name)
        {
            Border graphNameBorder = _graphManager.AddGraph(name);
            _graphManagerCollections[_graphManagerCollections.Count - 1].Add(graphNameBorder);
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
            EmptyStackPanelChildren(_stackPanelGraphManager);
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

            _graphManagerCollections[_selectedTabId - 1].Clear();

            foreach (UIElement element in _stackPanelGraphManager.Children)
            {
                _graphManagerCollections[_selectedTabId - 1].Add(element);
            }
        }

        private void EmptyCanvasChildren(Canvas canvas)
        {
            canvas.Children.Clear();
        }

        private void EmptyStackPanelChildren(StackPanel stackPanel)
        {
            stackPanel.Children.Clear();
        }

        private void AddChildrenToCanvas(int id)
        {
            foreach (UIElement uiElement in _uiElementsCollections[id-1])
            {
                _canvasMain.Children.Add(uiElement);
            }

            foreach (UIElement uiElement in _graphManagerCollections[id-1])
            {
                _stackPanelGraphManager.Children.Add(uiElement);
            }
        }

    }
}
