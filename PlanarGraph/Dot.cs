namespace PlanarGraph
{
    public class Dot
    {
        public int index { get; set; }
        public int NumberOfConnectedLines { get; set; }
        public int DotDegreeOfBadConnections { get; set; }
        public int DotDegreeOfMentioning { get; set; }

        public Dot(int ind)
        {
            index = ind;
            DotDegreeOfBadConnections = 0;
            DotDegreeOfMentioning = 0;
        }

        public Dot()
        {

        }

        public void CountConnectedLines(PlannarGraph plannarGraph)
        {
            double number = 0;
            foreach (Line line in plannarGraph.Lines)
            {
                if (line.dot1.index == this.index || line.dot2.index == this.index)
                {
                    number++;
                }
            }

            NumberOfConnectedLines = (int)number;
        }
    }
}