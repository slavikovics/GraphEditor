using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace GraphEditor.Windows.MainWindow
{
    internal class MainWindowAnimator
    {
        public static DoubleAnimation BuildOrientedSimplePopUpTopAnimation(Point ButtonAddPosition)
        {
            DoubleAnimation OrientedSimplePopUpTopAnimation = new DoubleAnimation();
            OrientedSimplePopUpTopAnimation.From = ButtonAddPosition.Y;
            OrientedSimplePopUpTopAnimation.To = ButtonAddPosition.Y - 45;
            OrientedSimplePopUpTopAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(200));
            return OrientedSimplePopUpTopAnimation;
        }

        public static DoubleAnimation BuildNonOrientedPopUpTopAnimation(Point ButtonAddPosition)
        {
            DoubleAnimation NonOrientedPopUpTopAnimation = new DoubleAnimation();
            NonOrientedPopUpTopAnimation.From = ButtonAddPosition.Y;
            NonOrientedPopUpTopAnimation.To = ButtonAddPosition.Y + 5;
            NonOrientedPopUpTopAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(200));
            return NonOrientedPopUpTopAnimation;
        }

        public static DoubleAnimation BuildOrientedPencilePopUpTopAnimation(Point ButtonAddPosition)
        {
            DoubleAnimation OrientedPencilePopUpTopAnimation = new DoubleAnimation();
            OrientedPencilePopUpTopAnimation.From = ButtonAddPosition.Y;
            OrientedPencilePopUpTopAnimation.To = ButtonAddPosition.Y + 55;
            OrientedPencilePopUpTopAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(200));
            return OrientedPencilePopUpTopAnimation;
        }

        public static DoubleAnimation BuildEdgePopUpLeftAnimation()
        {
            DoubleAnimation EdgePopUpLeftAnimation = new DoubleAnimation();
            EdgePopUpLeftAnimation.From = -50;
            EdgePopUpLeftAnimation.To = 0;
            EdgePopUpLeftAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(200));
            return EdgePopUpLeftAnimation;
        }

        public static DoubleAnimation BuildArrowTypesWidthAnimation()
        {
            DoubleAnimation ArrowTypesWidthAnimation = new DoubleAnimation();
            ArrowTypesWidthAnimation.From = 3;
            ArrowTypesWidthAnimation.To = 40;
            ArrowTypesWidthAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(200));
            return ArrowTypesWidthAnimation;
        }

        public static DoubleAnimation BuildGraphsManagerGridExpansion(StackPanel GraphVisualTreeStackPanel)
        {
            DoubleAnimation gridAnimation = new DoubleAnimation();
            gridAnimation.To = 88 + GraphVisualTreeStackPanel.Children.Count * 38;
            gridAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(100));
            return gridAnimation;
        }
    }
}
