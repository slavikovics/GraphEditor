using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using GraphEditor.EdgesAndNodes.Edges;
using GraphEditor.EdgesAndNodes.Edges.EdgesOriented;
using Color = System.Windows.Media.Color;
using Image = System.Windows.Controls.Image;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace GraphEditor.GraphLogic
{
    public static class GraphLogicAnimator
    {
        private static ColorAnimation BuildPathfinderHighlightAnimation(HighlightTargetColor highlightTarget)
        {
            Color targetColor = new Color();
            
            switch (highlightTarget)
            {
                case HighlightTargetColor.Green: targetColor = Colors.SeaGreen; break;
                case HighlightTargetColor.Red: targetColor = Colors.Crimson; break;
                case HighlightTargetColor.Yellow: targetColor = Colors.Orange; break;
                case HighlightTargetColor.Blue: targetColor = Colors.RoyalBlue; break;
            }
            
            ColorAnimation highlightAnimation = new ColorAnimation();
            highlightAnimation.To = targetColor;
            highlightAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(300));
            return highlightAnimation;
        }

        private static ColorAnimation BuildPathfinderHighlightRemovalAnimation()
        {
            ColorAnimation removeHighlightAnimation = new ColorAnimation();
            removeHighlightAnimation.To = (new OrientedEdgeConfiguration()).StrokeColor;
            removeHighlightAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(300));
            return removeHighlightAnimation;
        }

        public static void AnimateEdgeHighlight(Edge edge, HighlightTargetColor highlightTarget, Image highlightImage)
        {
            if (edge is NonOrientedEdge nonOrientedEdge)
            {
                nonOrientedEdge.EdgeVisualRepresentation.Stroke.BeginAnimation(SolidColorBrush.ColorProperty, BuildPathfinderHighlightAnimation(highlightTarget));
                return;
            }

            if (edge is OrientedEdge orientedEdge)
            {
                orientedEdge.EdgeVisualRepresentation.Stroke.BeginAnimation(SolidColorBrush.ColorProperty, BuildPathfinderHighlightAnimation(highlightTarget));
                orientedEdge.Arrow.Source = highlightImage.Source;
            }
        }
        
        public static void AnimateEdgeHighlightRemoval(Edge edge, Image normalImage)
        {
            if (edge is NonOrientedEdge nonOrientedEdge)
            {
                nonOrientedEdge.EdgeVisualRepresentation.Stroke.BeginAnimation(SolidColorBrush.ColorProperty, BuildPathfinderHighlightRemovalAnimation());
            }
            
            if (edge is OrientedEdge orientedEdge)
            {
                orientedEdge.EdgeVisualRepresentation.Stroke.BeginAnimation(SolidColorBrush.ColorProperty, BuildPathfinderHighlightRemovalAnimation());
                orientedEdge.Arrow.Source = normalImage.Source;
            }
        }
    }
}