using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using GraphEditor.EdgesAndNodes.Edges;
using GraphEditor.EdgesAndNodes.Edges.EdgesOriented;
using Color = System.Windows.Media.Color;
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
                case HighlightTargetColor.Blue: targetColor = Colors.CornflowerBlue; break;
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

        public static void AnimateEdgeHighlight(Edge edge, HighlightTargetColor highlightTarget)
        {
            if (edge is NonOrientedEdge nonOrientedEdge)
            {
                nonOrientedEdge.EdgeVisualRepresentation.Stroke.BeginAnimation(SolidColorBrush.ColorProperty, BuildPathfinderHighlightAnimation(highlightTarget));
                return;
            }

            if (edge is OrientedEdge orientedEdge)
            {
                if (orientedEdge._isPencil == true)
                {
                    orientedEdge.EdgeVisualRepresentation.Stroke.BeginAnimation(SolidColorBrush.ColorProperty, BuildPathfinderHighlightAnimation(highlightTarget));
                }
                else
                {
                    orientedEdge.EdgeVisualRepresentation.Stroke.BeginAnimation(SolidColorBrush.ColorProperty, BuildPathfinderHighlightAnimation(highlightTarget));
                }
            }
        }
        
        public static void AnimateEdgeHighlightRemoval(Edge edge)
        {
            if (edge is NonOrientedEdge nonOrientedEdge)
            {
                nonOrientedEdge.EdgeVisualRepresentation.Stroke.BeginAnimation(SolidColorBrush.ColorProperty, BuildPathfinderHighlightRemovalAnimation());
            }
            
            if (edge is OrientedEdge orientedEdge)
            {
                if (orientedEdge._isPencil == true)
                {
                    orientedEdge.EdgeVisualRepresentation.Stroke.BeginAnimation(SolidColorBrush.ColorProperty, BuildPathfinderHighlightRemovalAnimation());
                }
                else
                {
                    orientedEdge.EdgeVisualRepresentation.Stroke.BeginAnimation(SolidColorBrush.ColorProperty, BuildPathfinderHighlightRemovalAnimation());
                }
            }
        }
    }
}