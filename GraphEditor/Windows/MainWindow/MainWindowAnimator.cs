using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace GraphEditor.Windows.MainWindow
{
    internal static class MainWindowAnimator
    {
        public static DoubleAnimation BuildOrientedSimplePopUpTopAnimation(Point buttonAddPosition)
        {
            DoubleAnimation orientedSimplePopUpTopAnimation = new DoubleAnimation();
            orientedSimplePopUpTopAnimation.From = buttonAddPosition.Y;
            orientedSimplePopUpTopAnimation.To = buttonAddPosition.Y - 45;
            orientedSimplePopUpTopAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(200));
            return orientedSimplePopUpTopAnimation;
        }

        public static DoubleAnimation BuildNonOrientedPopUpTopAnimation(Point buttonAddPosition)
        {
            DoubleAnimation nonOrientedPopUpTopAnimation = new DoubleAnimation();
            nonOrientedPopUpTopAnimation.From = buttonAddPosition.Y;
            nonOrientedPopUpTopAnimation.To = buttonAddPosition.Y + 5;
            nonOrientedPopUpTopAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(200));
            return nonOrientedPopUpTopAnimation;
        }

        public static DoubleAnimation BuildOrientedPencilePopUpTopAnimation(Point buttonAddPosition)
        {
            DoubleAnimation orientedPencilPopUpTopAnimation = new DoubleAnimation();
            orientedPencilPopUpTopAnimation.From = buttonAddPosition.Y;
            orientedPencilPopUpTopAnimation.To = buttonAddPosition.Y + 55;
            orientedPencilPopUpTopAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(200));
            return orientedPencilPopUpTopAnimation;
        }

        public static DoubleAnimation BuildEdgePopUpLeftAnimation()
        {
            DoubleAnimation edgePopUpLeftAnimation = new DoubleAnimation();
            edgePopUpLeftAnimation.From = -50;
            edgePopUpLeftAnimation.To = 0;
            edgePopUpLeftAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(200));
            return edgePopUpLeftAnimation;
        }

        public static DoubleAnimation BuildArrowTypesWidthAnimation()
        {
            DoubleAnimation arrowTypesWidthAnimation = new DoubleAnimation();
            arrowTypesWidthAnimation.From = 3;
            arrowTypesWidthAnimation.To = 40;
            arrowTypesWidthAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(200));
            return arrowTypesWidthAnimation;
        }

        public static DoubleAnimation BuildGraphsManagerGridExpansion(StackPanel graphVisualTreeStackPanel, double windowHeight)
        {
            Console.WriteLine();
            int count = (int)Math.Round((windowHeight - 150) / 38);
            DoubleAnimation gridAnimation = new DoubleAnimation();
            double targetCount = graphVisualTreeStackPanel.Children.Count;
            if (targetCount > count) targetCount = count;
            gridAnimation.To = 50 + targetCount * 38;
            gridAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(100));
            return gridAnimation;
        }
    }
}
