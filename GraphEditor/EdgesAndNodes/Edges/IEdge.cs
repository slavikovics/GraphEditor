using GraphEditor.GraphsManagerControls;
using System;
using System.Collections.Generic;
using System.Windows.Shapes;

namespace GraphEditor.EdgesAndNodes
{
    internal interface IEdge : IRenamable
    {
        void OnNodePositionChanged(object sender, EventArgs e);

        void EdgePositioning(bool isInGraph);

        void EdgeDragged(double dragDeltaX, double dragDeltaY);

        int GetFirstNodeId();

        int GetSecondNodeId();

        void Remove();

        List<int> GetNodesDependencies();

        Rectangle GetEdgeVisualRepresentation();

    }
}
