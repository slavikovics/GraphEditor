using System.Collections.Generic;
using System.Diagnostics;
using GraphEditor.EdgesAndNodes;
using GraphEditor.GraphLogic;

namespace PlanarGraph
{
    public class PlanarityFinder
    {
        private Graph _usedGraph;
        
        private PlannarGraph _planarGraph;
            
        public PlanarityFinder(Graph graph)
        {
            _usedGraph = graph;
            SetPlanarGraphLinesAndDots();
            _planarGraph.SimplifyGraph();
        }

        private List<Dot> BuildDotsList()
        {
            List<Dot> dots = new List<Dot>();
            int startId = 1;

            foreach (var node in _usedGraph.Nodes)
            {
                Dot dot = new Dot(startId);
                dots.Add(dot);
                startId++;
            }

            return dots;
        }

        private void SetPlanarGraphLinesAndDots()
        {
            List<Dot> dots = BuildDotsList();
            List<Line> lines = BuildLinesList();
            
            _planarGraph = new PlannarGraph(dots, lines);
            _planarGraph.UpdateDotsInformation();
            string s = _planarGraph.ToString();
        }

        private List<Line> FindRemovedLines()
        {
            _planarGraph.MakePlannar();
            return _planarGraph.RemovedLines;
        }

        private List<Line> BuildLinesList()
        {
            List<Line> lines = new List<Line>();
            int innerNodeId;
            int outerNodeId = 1;
            
            foreach (var outerNode in _usedGraph.Nodes)
            {
                innerNodeId = 1;
                foreach (var innerNode in _usedGraph.Nodes)
                {
                    foreach (IEdge edge in _usedGraph.Edges)
                    {
                        if (edge.GetFirstNode() == outerNode && edge.GetSecondNode() == innerNode)
                        {
                            lines.Add(new Line(new Dot(outerNodeId), new Dot(innerNodeId), 1));
                            break;
                        }
                    }
                    
                    innerNodeId++;
                }

                outerNodeId++;
            }

            return lines;
        }

        public List<IEdge> FindIEdgesToRemoveInMainWindow()
        {
            List<IEdge> edges = new List<IEdge>();
            List<Line> linesToRemove = FindRemovedLines();
            
            if (linesToRemove.Count == 0) Debug.WriteLine("This graph is already planar, no edges to remove");
            
            foreach (Line line in linesToRemove)
            {
                edges.Add(_usedGraph.GetEdgeByTwoNodes(_usedGraph.Nodes[line.dot1.index - 1], _usedGraph.Nodes[line.dot2.index - 1])); 
            }
            
            return edges;
        }
    }
}