namespace GraphEditor.GraphsSavingAndLoading
{
    internal class RelationDataModel
    {
        public string FirstNodeName { get; set; }

        public string FirstNodeId { get; set; }

        public string RelationName { get; set; }

        public string RelationType { get; set; }
        
        public string RelationId { get; set; }

        public string SecondNodeName { get; set; }

        public string SecondNodeId { get; set; }

        public bool ShouldFirstNodeBeCreated { get; set; }

        public bool ShouldSecondNodeBeCreated { get; set; }

        public Node FirstNode { get; set; }

        public Node SecondNode { get; set; }

        public RelationDataModel()
        {
            ShouldFirstNodeBeCreated = true;
            ShouldSecondNodeBeCreated = true;
            FirstNode = null;
            SecondNode = null;
        }
    }
}
