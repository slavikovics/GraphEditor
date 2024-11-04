using GraphEditor.GraphTab;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace GraphEditor.GraphTabs
{
    internal class TabView
    {
        private List<TabButton> TabViewButtons;

        private List<List<UIElement>> Collections;

        private int selectedTabId;

        private ControlTemplate _tabButtonTemplate;

        private Canvas _canvas;

        public TabView(TextBlock firstTabContent, Image addNewTabContent, ControlTemplate buttonAddTemplate, 
            ControlTemplate tabButtonTemplate, Canvas canvas)
        {
            Tab firstTab = new Tab(firstTabContent, tabButtonTemplate, 1);
            Tab addNewTab = new Tab(addNewTabContent, buttonAddTemplate, 0);
            selectedTabId = 1;
            TabViewButtons = new List<TabButton>();
            Collections = new List<List<UIElement>>();
            Collections.Add(new List<UIElement>());
            _tabButtonTemplate = tabButtonTemplate;

            TabButton firstTabAsButton = firstTab.GetTabAsRenamingButton();
            TabButton addNewAsButton = addNewTab.GetTabAsRenamingButton();

            TabViewButtons.Add(firstTabAsButton);
            TabViewButtons.Add(addNewAsButton);

            firstTabAsButton.Click += TabClick;
            addNewAsButton.Click += AddNewTabClick;
            _canvas = canvas;
        }

        private void AddNewTabClick(object sender, System.Windows.RoutedEventArgs e)
        {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = "Graph " + (Collections.Count + 1);
            Collections.Add(new List<UIElement>());
            AddTabViewButton(textBlock, _tabButtonTemplate);
            AddTabViewToMainWindow();
            TabViewButtons[TabViewButtons.Count - 2].AnimateButtonExpansion();
            TabViewButtons[TabViewButtons.Count - 1].AnimateButtonMovementRight();
        }

        private int CalculatePosition()
        {
            int position = 0;
            return position;
        }

        private void TabClick(object sender, System.Windows.RoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public void AddTabViewButton(TextBlock firstTabContent, ControlTemplate tabButtonTemplate)
        {
            Tab tabView = new Tab(firstTabContent, tabButtonTemplate, TabViewButtons.Count);
            TabButton renamingButton = tabView.GetTabAsRenamingButton();
            TabViewButtons.Insert(Collections.Count - 1, renamingButton);
            //TabViewButtons.Add(renamingButton);
            renamingButton.Click += TabClick;
        }

        public void AddTabViewToMainWindow()
        {
            EmptyCanvasChildren();

            double CanvasLeft = 0;
            double CanvasTop = 0;
            double Margin = 10;

            foreach(TabButton renamingButton in TabViewButtons)
            {
                renamingButton.SetValue(Canvas.LeftProperty, CanvasLeft);
                renamingButton.SetValue(Canvas.TopProperty, CanvasTop);
                CanvasLeft += renamingButton.Width + Margin;
                _canvas.Children.Add(renamingButton);
            }
        }

        public void SwitchTab(int targetId)
        {
            SaveCanvasChildren();
            EmptyCanvasChildren();
            AddChildrenToCanvas(targetId);
            selectedTabId = targetId;
        }

        private void SaveCanvasChildren()
        {
            Collections[selectedTabId].Clear();

            foreach(UIElement element in _canvas.Children)
            {
                Collections[selectedTabId].Add(element);
            }
        }

        private void EmptyCanvasChildren()
        {
            _canvas.Children.RemoveRange(0, _canvas.Children.Count);
        }

        private void AddChildrenToCanvas(int id)
        {
            foreach (UIElement uiElement in Collections[id-1])
            {
                _canvas.Children.Add(uiElement);
            }
        }

    }
}
