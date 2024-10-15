﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphEditor
{
    internal class EdgeAnimationController
    {
        List<Edge> edges;
        Node _controlNode;

        public EdgeAnimationController(Node controlNode)
        {
            edges = new List<Edge>();
            _controlNode = controlNode;
            _controlNode.OnNodesAnimated += OnNodesAnimated;
        }

        private void OnNodesAnimated()
        {
            foreach (Edge edge in edges)
            {
                edge.EdgePositioning(true);
            }
        }

        public void AddEdge(Edge edge)
        {
            edges.Add(edge);
        }
    }
}
