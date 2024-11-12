using GraphEditor.GraphsManagerControls;
using System;
using System.Collections.Generic;
using System.Windows.Shapes;

namespace GraphEditor.EdgesAndNodes
{
    public interface IEdge : IRenamable
    {
        void OnNodePositionChanged(object sender, EventArgs e);

        void EdgePositioning(bool isInGraph);

        void EdgeDragged(double dragDeltaX, double dragDeltaY);

        string GetFirstNodeId();

        string GetSecondNodeId();

        void Remove();

        List<string> GetNodesDependencies();

        Rectangle GetEdgeVisualRepresentation();

    }
}
