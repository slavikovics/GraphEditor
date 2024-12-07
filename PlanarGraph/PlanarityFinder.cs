using System.Collections.Generic;
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
            _planarGraph = new PlannarGraph();
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

        private void SetPlannarGraphLinesAndDots()
        {
            List<Dot> dots = BuildDotsList();
            List<Line> lines = BuildLinesList();
            
            _planarGraph.Lines = lines;
            _planarGraph.Dots = dots;
            _planarGraph.UpdateDotsInformation();
        }

        private List<Line> FindRemovedLines()
        {
            SetPlannarGraphLinesAndDots();
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
                            lines.Add(new Line(new Dot(outerNodeId), new Dot(innerNodeId), 1));
                    }
                }

                outerNodeId++;
            }

            return lines;
        }

        private List<IEdge> FindIEdgesToRemoveInMainWindow(List<Line> linesToRemove)
        {
            //
            return new List<IEdge>();
        }
    }
}