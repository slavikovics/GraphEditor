using GraphEditor.GraphsManagerControls;
using System;

namespace GraphEditor.EdgesAndNodes
{
    internal interface IEdgeable : IRenamable
    {
        void OnNodePositionChanged(object sender, EventArgs e);

        void EdgePositioning(bool isInGraph);

        void EdgeDragged(double dragDeltaX, double dragDeltaY);

        int GetFirstNodeId();

        int GetSecondNodeId();

        void Remove();

    }
}
