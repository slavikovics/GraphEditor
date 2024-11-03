using GraphEditor.EdgesAndNodes.Nodes;
using GraphEditor.GraphsManagerControls;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace GraphEditor.EdgesAndNodes.Nodes
{
    internal class HiddenNode : Node
    {
        public HiddenNode(double CanvasLeft, double CanvasTop, Canvas parent, MainWindow window, int id, IEdgeable edge) : base(CanvasLeft, CanvasTop, parent, window, id)
        {
            HideNode();
            window.MagicWandOrder -= OnMagicWondOrder;

        }

        private void HideNode()
        {
            ellipse.Visibility = Visibility.Hidden;
            _textBlock.Visibility = Visibility.Hidden;
        }
    }
}
