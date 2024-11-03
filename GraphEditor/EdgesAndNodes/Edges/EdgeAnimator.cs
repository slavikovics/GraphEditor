using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace GraphEditor.EdgesAndNodes.Edges
{
    internal class EdgeAnimator
    {
        public static ColorAnimation BuildEdgeHoverAnimationLeavePhase(EdgeConfiguration edgeConfiguration)
        {
            ColorAnimation edgeHoverAnimation = new ColorAnimation();
            edgeHoverAnimation.To = edgeConfiguration.StrokeColor;
            edgeHoverAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(200));
            return edgeHoverAnimation;
        }

        public static ColorAnimation BuildEdgeHoverAnimationEnterPhase()
        {
            ColorAnimation edgeHoverAnimation = new ColorAnimation();
            edgeHoverAnimation.To = Color.FromArgb(255, 153, 143, 199);
            edgeHoverAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(200));
            return edgeHoverAnimation;
        }

        public static DoubleAnimation BuildEdgeCreationAnimation(EdgeConfiguration edgeConfiguration)
        {
            DoubleAnimation edgeWidthAnimation = new DoubleAnimation();
            edgeWidthAnimation.From = edgeConfiguration.Height;
            edgeWidthAnimation.To = edgeConfiguration.Width;
            edgeWidthAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(300));
            edgeWidthAnimation.AccelerationRatio = 0.5;
            edgeWidthAnimation.DecelerationRatio = 0.5;
            return edgeWidthAnimation;
        }
    }
}
