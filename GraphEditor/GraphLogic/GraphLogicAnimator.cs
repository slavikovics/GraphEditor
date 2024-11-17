using System;
using System.Drawing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using GraphEditor.EdgesAndNodes.Edges;
using GraphEditor.EdgesAndNodes.Edges.EdgesOriented;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace GraphEditor.GraphLogic
{
    public static class GraphLogicAnimator
    {
        private static ColorAnimation BuildPathfinderHighlightAnimation()
        {
            ColorAnimation highlightAnimation = new ColorAnimation();
            highlightAnimation.To = Colors.SeaGreen;
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

        public static void AnimateEdgeHighlight(Edge edge)
        {
            if (edge is NonOrientedEdge nonOrientedEdge)
            {
                nonOrientedEdge.EdgeVisualRepresentation.Stroke.BeginAnimation(SolidColorBrush.ColorProperty, BuildPathfinderHighlightAnimation());
                return;
            }

            if (edge is OrientedEdge orientedEdge)
            {
                if (orientedEdge._isPencil == true)
                {
                    orientedEdge.EdgeVisualRepresentation.Stroke.BeginAnimation(SolidColorBrush.ColorProperty, BuildPathfinderHighlightAnimation());
                }
                else
                {
                    orientedEdge.EdgeVisualRepresentation.Stroke.BeginAnimation(SolidColorBrush.ColorProperty, BuildPathfinderHighlightAnimation());
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