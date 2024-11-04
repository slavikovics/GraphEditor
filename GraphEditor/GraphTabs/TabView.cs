using GraphEditor.GraphTab;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace GraphEditor.GraphTabs
{
    internal class TabView
    {
        private List<RenamingButton> TabViewButtons;

        private List<List<UIElement>> Collections;

        private int selectedTabId;

        public TabView(TextBlock firstTabContent, TextBlock addNewTabContent, ControlTemplate buttonAddTemplate, 
            ControlTemplate tabButtonTemplate)
        {
            Tab firstTab = new Tab(firstTabContent, tabButtonTemplate, 1);
            Tab addNewTab = new Tab(addNewTabContent, buttonAddTemplate, 0);
            selectedTabId = 1;
            TabViewButtons = new List<RenamingButton>();
            Collections = new List<List<UIElement>>();
            Collections.Add(new List<UIElement>());

            RenamingButton firstTabAsButton = firstTab.GetTabAsRenamingButton();
            RenamingButton addNewAsButton = addNewTab.GetTabAsRenamingButton();

            TabViewButtons.Add(firstTabAsButton);
            TabViewButtons.Add(addNewAsButton);

            firstTabAsButton.Click += TabClick;
            addNewAsButton.Click += AddNewTabClick;
        }

        private void AddNewTabClick(object sender, System.Windows.RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void TabClick(object sender, System.Windows.RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public void AddTabViewButton(TextBlock firstTabContent, ControlTemplate tabButtonTemplate)
        {
            Tab tabView = new Tab(firstTabContent, tabButtonTemplate, TabViewButtons.Count);
            RenamingButton renamingButton = tabView.GetTabAsRenamingButton();
            TabViewButtons.Insert(TabViewButtons.Count - 2, renamingButton);
            renamingButton.Click += TabClick;
        }

        public void AddTabViewToMainWindow(Canvas TabViewCanvas)
        {
            EmptyCanvasChildren(TabViewCanvas);

            double CanvasLeft = 0;
            double CanvasTop = 0;
            double Margin = 10;

            foreach(TabButton renamingButton in TabViewButtons)
            {
                renamingButton.SetValue(Canvas.LeftProperty, CanvasLeft);
                renamingButton.SetValue(Canvas.TopProperty, CanvasTop);
                CanvasLeft += renamingButton.Width + Margin;
                TabViewCanvas.Children.Add(renamingButton);
            }
        }

        public void SwitchTab(Canvas canvas, int targetId)
        {
            SaveCanvasChildren(canvas);
            EmptyCanvasChildren(canvas);
            AddChildrenToCanvas(targetId, canvas);
            selectedTabId = targetId;
        }

        private void SaveCanvasChildren(Canvas canvas)
        {
            Collections[selectedTabId].Clear();

            foreach(UIElement element in canvas.Children)
            {
                Collections[selectedTabId].Add(element);
            }
        }

        private void EmptyCanvasChildren(Canvas canvas)
        {
            canvas.Children.RemoveRange(0, canvas.Children.Count);
        }

        private void AddChildrenToCanvas(int id, Canvas canvas)
        {
            foreach (UIElement uiElement in Collections[id-1])
            {
                canvas.Children.Add(uiElement);
            }
        }
    }
}
