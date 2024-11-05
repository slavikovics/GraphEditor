using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace GraphEditor.EdgesAndNodes.Nodes
{
    internal class NodeAnimator
    {
        public static DoubleAnimation BuildEllipseMagicAnimationLeft(double toLeft)
        {
            DoubleAnimation ellipseAnimationLeft = new DoubleAnimation();
            ellipseAnimationLeft.To = toLeft;
            ellipseAnimationLeft.Duration = new Duration(TimeSpan.FromSeconds(1));
            ellipseAnimationLeft.AccelerationRatio = 0.3;
            ellipseAnimationLeft.DecelerationRatio = 0.7;
            return ellipseAnimationLeft;
        }

        public static DoubleAnimation BuildEllipseMagicAnimationTop(double toTop)
        {
            DoubleAnimation ellipseAnimationTop = new DoubleAnimation();
            ellipseAnimationTop.To = toTop;
            ellipseAnimationTop.Duration = new Duration(TimeSpan.FromSeconds(1));
            ellipseAnimationTop.AccelerationRatio = 0.3;
            ellipseAnimationTop.DecelerationRatio = 0.7;
            return ellipseAnimationTop;
        }

        public static DoubleAnimation BuildEllipseArrivalAnimationWidth(int ellipseDimensions)
        {
            DoubleAnimation nodeWidthAnimation = new DoubleAnimation();
            nodeWidthAnimation.From = 5;
            nodeWidthAnimation.To = ellipseDimensions;
            nodeWidthAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(100));
            return nodeWidthAnimation;
        }

        public static DoubleAnimation BuildEllipseArrivalAnimationHeight(int ellipseDimensions)
        {
            DoubleAnimation nodeHeightAnimation = new DoubleAnimation();
            nodeHeightAnimation.From = 5;
            nodeHeightAnimation.To = ellipseDimensions;
            nodeHeightAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(100));
            return nodeHeightAnimation;
        }

        public static DoubleAnimation BuildEllipseArrivalAnimationLeft(Node node, int ellipseDimensions)
        {
            DoubleAnimation nodeMovingLeftAnimation = new DoubleAnimation();
            nodeMovingLeftAnimation.From = node.GetPosLeft() + ellipseDimensions / 2;
            nodeMovingLeftAnimation.To = node.GetPosLeft();
            nodeMovingLeftAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(100));
            return nodeMovingLeftAnimation;
        }

        public static DoubleAnimation BuildEllipseArrivalAnimationTop(Node node, int ellipseDimensions)
        {
            DoubleAnimation nodeMovingTopAnimation = new DoubleAnimation();
            nodeMovingTopAnimation.From = node.GetPosTop() + ellipseDimensions / 2;
            nodeMovingTopAnimation.To = node.GetPosTop();
            nodeMovingTopAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(100));
            return nodeMovingTopAnimation;
        }

        public static void StopEllipseAnimationOnProperty(Button ellipse, DependencyProperty dependencyProperty)
        {
            ellipse.BeginAnimation(dependencyProperty, null);
        }
    }
}
