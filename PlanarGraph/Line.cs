namespace PlanarGraph
{
    public class Line
    {
        public Dot dot1 { get; set; }
        public Dot dot2 { get; set; }
        public int index { get; set; }

        public Line(Dot _dot1, Dot _dot2, int ind)
        {
            dot1 = _dot1;
            dot2 = _dot2;
            index = ind;
        }

        public Line()
        {

        }
    }
}