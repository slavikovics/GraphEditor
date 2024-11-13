using GraphEditor.EdgesAndNodes.Edges;

namespace GraphEditor.GraphsSavingAndLoading
{
    internal class RelationDataModel
    {
        public string FirstNodeName { get; set; }

        public string FirstNodeId { get; set; }

        public string RelationName { get; set; }

        public  EdgeTypes RelationType  { get; set; }
        
        public string RelationId { get; set; }

        public string SecondNodeName { get; set; }

        public string SecondNodeId { get; set; }

        public bool ShouldFirstNodeBeCreated { get; set; } = true;

        public bool ShouldSecondNodeBeCreated { get; set; } = true;

        public Node FirstNode { get; set; }

        public Node SecondNode { get; set; }
    }
}
