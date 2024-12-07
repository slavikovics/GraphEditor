using System;
using System.Collections.Generic;

namespace PlanarGraph
{
    public class PlannarGraph
    {
        public List<Dot> Dots { get; set; }

        public List<Line> Lines { get; set; }

        public List<Line> RemovedLines { get; set; }

        public PlannarGraph(List<Dot> _dots, List<Line> _lines)
        {
            Dots = _dots;
            Lines = _lines;
            RemovedLines = new List<Line>();
        }

        public PlannarGraph()
        {
        }

        public bool FindAndBrakeFive(bool shouldBeBroken)
        {
            if (!shouldBeBroken)
            {
                foreach (Dot dot in Dots)
                {
                    dot.DotDegreeOfBadConnections = 0;
                }
            }

            if (Dots.Count < 5) return false;

            List<Dot> dotsToCheckFive = new List<Dot>();

            List<Line> linesToCheckFive = new List<Line>();


            foreach (Dot dot in Dots)
            {

                foreach (Dot dot1 in Dots)
                {
                    if (dot1.index == dot.index) continue;

                    foreach (Dot dot2 in Dots)
                    {
                        if (dot2.index == dot1.index || dot2.index == dot.index) continue;

                        foreach (Dot dot3 in Dots)
                        {

                            if (dot3.index == dot2.index || dot3.index == dot1.index || dot3.index == dot.index)
                                continue;

                            foreach (Dot dot4 in Dots)
                            {

                                if (dot4.index == dot3.index || dot4.index == dot2.index || dot4.index == dot1.index ||
                                    dot4.index == dot.index) continue;

                                dotsToCheckFive.Add(new Dot(dot.index));
                                dotsToCheckFive.Add(new Dot(dot1.index));
                                dotsToCheckFive.Add(new Dot(dot2.index));
                                dotsToCheckFive.Add(new Dot(dot3.index));
                                dotsToCheckFive.Add(new Dot(dot4.index));
                                linesToCheckFive = FindLines(dotsToCheckFive);
                                PlannarGraph check = new PlannarGraph(dotsToCheckFive, linesToCheckFive);
                                check.UpdateDotsInformation();

                                if (check.isFullFive())
                                {
                                    if (shouldBeBroken)
                                    {
                                        Console.WriteLine("five removed");

                                        RemoveLine(check.Dots, check.Lines, false);

                                        foreach (Dot _dot in dotsToCheckFive)
                                        {
                                            GetDotByIndex(_dot.index).DotDegreeOfBadConnections--;
                                        }

                                        return true;
                                    }

                                    if (!shouldBeBroken)
                                    {
                                        foreach (Dot _dot in dotsToCheckFive)
                                        {
                                            GetDotByIndex(_dot.index).DotDegreeOfBadConnections++;
                                        }
                                    }
                                }

                                dotsToCheckFive.Clear();
                                linesToCheckFive.Clear();
                            }
                        }
                    }
                }
            }

            return false;
        }


        private bool isFullFive()
        {
            foreach (Dot dot in Dots)
            {
                if (dot.NumberOfConnectedLines < 4)
                {
                    return false;
                }
            }

            return true;
        }

        public List<Line> FindLines(List<Dot> dotsToCheckFive)
        {
            List<Line> result = new List<Line>();
            foreach (Line line in Lines)
            {
                foreach (Dot dot in dotsToCheckFive)
                {
                    foreach (Dot dot1 in dotsToCheckFive)
                    {
                        if (dot1.index != dot.index &&
                            ((line.dot1.index == dot.index && line.dot2.index == dot1.index)))
                        {
                            result.Add(new Line(dot, dot1, dot1.index));
                        }
                    }
                }
            }

            return result;
        }

        public int FindLinesThreePIndexCalcl(int[] ps, int i)
        {
            if (i == -1) return 0;
            return ps[i] + 1;
        }

        public List<Line> FindLinesThree(List<Dot> dotsToCheckThree)
        {
            int[] indexies = new int[3];
            indexies[0] = -1;
            indexies[1] = -1;
            indexies[2] = -1;
            List<Line> linesToCheckThree = new List<Line>();

            for (int i = 0; i < 6; i++)
            {
                for (int j = i + 1; j < 6; j++)
                {
                    if (j == i) continue;
                    for (int k = j + 1; k < 6; k++)
                    {
                        if (k == j || k == i) continue;
                        for (int p = 0; p < 3; p++)
                        {
                            for (int x = FindLinesThreePIndexCalcl(indexies, p - 1); x < 6; x++)
                            {
                                if (x == i || x == j || x == k) continue;
                                indexies[p] = x;
                                break;
                            }

                            if (indexies[p] == -1) return linesToCheckThree;
                        }

                        if (AreConnected(dotsToCheckThree[i].index, dotsToCheckThree[indexies[0]].index, Lines) &&
                            AreConnected(dotsToCheckThree[i].index, dotsToCheckThree[indexies[1]].index, Lines) &&
                            AreConnected(dotsToCheckThree[i].index, dotsToCheckThree[indexies[2]].index, Lines) &&
                            AreConnected(dotsToCheckThree[j].index, dotsToCheckThree[indexies[0]].index, Lines) &&
                            AreConnected(dotsToCheckThree[j].index, dotsToCheckThree[indexies[1]].index, Lines) &&
                            AreConnected(dotsToCheckThree[j].index, dotsToCheckThree[indexies[2]].index, Lines) &&
                            AreConnected(dotsToCheckThree[k].index, dotsToCheckThree[indexies[0]].index, Lines) &&
                            AreConnected(dotsToCheckThree[k].index, dotsToCheckThree[indexies[1]].index, Lines) &&
                            AreConnected(dotsToCheckThree[k].index, dotsToCheckThree[indexies[2]].index, Lines))
                        {
                            linesToCheckThree.Add(new Line(dotsToCheckThree[i], dotsToCheckThree[indexies[0]], 1));
                            linesToCheckThree.Add(new Line(dotsToCheckThree[i], dotsToCheckThree[indexies[1]], 2));
                            linesToCheckThree.Add(new Line(dotsToCheckThree[i], dotsToCheckThree[indexies[2]], 3));
                            linesToCheckThree.Add(new Line(dotsToCheckThree[k], dotsToCheckThree[indexies[0]], 4));
                            linesToCheckThree.Add(new Line(dotsToCheckThree[k], dotsToCheckThree[indexies[1]], 5));
                            linesToCheckThree.Add(new Line(dotsToCheckThree[k], dotsToCheckThree[indexies[2]], 6));
                            linesToCheckThree.Add(new Line(dotsToCheckThree[j], dotsToCheckThree[indexies[0]], 7));
                            linesToCheckThree.Add(new Line(dotsToCheckThree[j], dotsToCheckThree[indexies[1]], 8));
                            linesToCheckThree.Add(new Line(dotsToCheckThree[j], dotsToCheckThree[indexies[2]], 9));

                            return linesToCheckThree;
                        }

                    }
                }
            }

            return linesToCheckThree;
        }

        public void MakePlannar()
        {
            FindAndBrakeFive(false);
            FindAndBrakeThreeThree(false);

            bool check = true;

            foreach (Dot dot in Dots)
            {
                if (dot.DotDegreeOfBadConnections != 0) check = false;
            }

            if (check == true) return;

            bool firstaTry5 = true, firstaTry6 = true;

            do
            {
                firstaTry5 = true;
                firstaTry6 = true;
                while (true)
                {
                    if (!FindAndBrakeFive(true)) break;
                    else firstaTry5 = false;
                    UpdateDotsInformation();
                }

                UpdateDotsInformation();
                this.SimplifyGraph();

                while (true)
                {
                    if (!FindAndBrakeThreeThree(true)) break;
                    else firstaTry6 = false;
                    UpdateDotsInformation();
                }

                UpdateDotsInformation();
                this.SimplifyGraph();
            } while (!firstaTry5 || !firstaTry6);

        }

        public bool FindAndBrakeThreeThree(bool shouldBeBraken)
        {

            if (Dots.Count < 6) return false;

            List<Dot> dotsToCheckThree = new List<Dot>();

            List<Line> linesToCheckThree = new List<Line>();

            foreach (Dot dot in Dots)
            {
                foreach (Dot dot1 in Dots)
                {
                    if (dot1.index == dot.index) continue;
                    foreach (Dot dot2 in Dots)
                    {
                        if (dot2.index == dot.index || dot2.index == dot1.index) continue;

                        foreach (Dot dot3 in Dots)
                        {
                            if (dot3.index == dot.index || dot3.index == dot1.index || dot3.index == dot2.index)
                                continue;

                            foreach (Dot dot4 in Dots)
                            {
                                if (dot4.index == dot.index || dot4.index == dot1.index || dot4.index == dot2.index ||
                                    dot4.index == dot3.index) continue;
                                foreach (Dot dot5 in Dots)
                                {
                                    if (dot5.index == dot.index || dot5.index == dot4.index ||
                                        dot5.index == dot3.index || dot5.index == dot2.index ||
                                        dot5.index == dot1.index) continue;
                                    dotsToCheckThree.Add(new Dot(dot.index));
                                    dotsToCheckThree.Add(new Dot(dot1.index));
                                    dotsToCheckThree.Add(new Dot(dot2.index));
                                    dotsToCheckThree.Add(new Dot(dot3.index));
                                    dotsToCheckThree.Add(new Dot(dot4.index));
                                    dotsToCheckThree.Add(new Dot(dot5.index));

                                    linesToCheckThree = FindLinesThree(dotsToCheckThree);

                                    PlannarGraph check = new PlannarGraph(dotsToCheckThree, linesToCheckThree);
                                    check.UpdateDotsInformation();

                                    if (linesToCheckThree.Count == 9)
                                    {
                                        if (shouldBeBraken)
                                        {
                                            Console.WriteLine("three-three removed");
                                            RemoveLine(check.Dots, check.Lines, true);

                                            foreach (Dot _dot in dotsToCheckThree)
                                            {
                                                GetDotByIndex(_dot.index).DotDegreeOfBadConnections--;
                                            }

                                            return true;
                                        }

                                        if (!shouldBeBraken)
                                        {
                                            foreach (Dot _dot in dotsToCheckThree)
                                            {
                                                GetDotByIndex(_dot.index).DotDegreeOfBadConnections++;
                                            }
                                        }
                                    }

                                    dotsToCheckThree.Clear();
                                    linesToCheckThree.Clear();
                                }
                            }
                        }
                    }
                }
            }

            return false;
        }

        public bool CheckLinesThree(List<Dot> dots, List<Line> lines)
        {
            PlannarGraph g = new PlannarGraph(dots, lines);
            g.UpdateDotsInformation();

            foreach (Dot dot in g.Dots)
            {
                if (dot.NumberOfConnectedLines < 3) return false;
            }

            return true;
        }

        public void UpdateDotsInformation()
        {
            foreach (Dot dot in Dots)
            {
                dot.CountConnectedLines(this);
            }
        }


        public void RemoveLine(List<Dot> dots, List<Line> lines, bool workMode)
        {

            UpdateDotsInformation();
            foreach (Dot dot in dots)
            {
                foreach (Dot _dot in Dots)
                {
                    if (dot.index == _dot.index)
                    {
                        dot.DotDegreeOfMentioning = _dot.DotDegreeOfMentioning;
                        dot.DotDegreeOfBadConnections = _dot.DotDegreeOfBadConnections;
                    }
                }
            }

            Dot max1, max2 = new Dot();

            max1 = dots[0];

            foreach (Dot dot in dots)
            {
                if (max1.DotDegreeOfMentioning > dot.DotDegreeOfMentioning) max1 = dot;
                else if (max1.DotDegreeOfMentioning == dot.DotDegreeOfMentioning &&
                         max1.DotDegreeOfBadConnections < dot.DotDegreeOfBadConnections) max1 = dot;
            }

            foreach (Dot dot in dots)
            {
                if ((dot.index != max1.index) && AreConnected(max1.index, dot.index, lines))
                {
                    max2 = dot;
                    break;
                }
            }


            foreach (Dot dot in dots)
            {
                if ((max2.DotDegreeOfMentioning > dot.DotDegreeOfMentioning) &&
                    AreConnected(max1.index, dot.index, lines)) max2 = dot;
                else if (max2.DotDegreeOfMentioning == dot.DotDegreeOfMentioning &&
                         max2.DotDegreeOfBadConnections < dot.DotDegreeOfBadConnections &&
                         AreConnected(max1.index, dot.index, lines)) max2 = dot;
            }

            foreach (Line line in Lines)
            {
                if ((line.dot1.index == max1.index && line.dot2.index == max2.index) ||
                    (line.dot1.index == max2.index && line.dot2.index == max1.index))
                {
                    RemoveLineByIds(line);
                    //Console.WriteLine($"{line.dot1.index}-{line.dot2.index}");    
                    RemovedLines.Add(line);
                    foreach (Dot dot in Dots)
                    {
                        if (dot.index == max1.index)
                        {
                            dot.DotDegreeOfMentioning++;
                        }

                        if (dot.index == max2.index)
                        {
                            dot.DotDegreeOfMentioning++;
                        }
                    }

                    return;
                }
            }

        }

        public void RemoveLineByIds(Line line)
        {
            int index = -1;
            foreach (Line _line in Lines)
            {
                if ((_line.dot1.index == line.dot1.index && _line.dot2.index == line.dot2.index) ||
                    (_line.dot2.index == line.dot1.index && _line.dot1.index == line.dot2.index))
                {
                    index = Lines.IndexOf(_line);
                    break;
                }
            }

            if (index != -1) Lines.RemoveAt(index);
            else Console.WriteLine("Unhandled exception");
        }

        public bool AreConnected(int i1, int i2, List<Line> lines)
        {
            foreach (Line l in lines)
            {
                if ((l.dot1.index == i1 && l.dot2.index == i2) || (l.dot1.index == i2 && l.dot2.index == i1))
                {
                    return true;
                }
            }

            return false;
        }

        public Dot GetDotByIndex(int i)
        {
            foreach (Dot dot in Dots)
            {
                if (dot.index == i) return dot;
            }

            return null;
        }

        public override string ToString()
        {

            string s = "_____________________________________________________________\n";
            s += "Graph contains\n";
            s += (Lines.Count.ToString() + " lines: \n");

            foreach (Line l in Lines)
            {
                s += (l.dot1.index.ToString() + " - " + l.dot2.index.ToString() + "\n");
            }

            s += (Dots.Count.ToString() + " dots with number of connected lines:\n");

            foreach (Dot dot in Dots)
            {
                s += (dot.index.ToString() + " - " + dot.NumberOfConnectedLines.ToString() + "\n");
            }

            s += "_____________________________________________________________\n";

            return s;
        }

        public void SimplifyGraph()
        {
            List<Dot> dots = Dots;
            List<Line> lines = Lines;
            List<Dot> dotsForRemoval = new List<Dot>();
            List<Line> linesForRemoval = new List<Line>();
            Dot dotStart, dotEnd;


            foreach (Dot dot in dots)
            {
                if (dot.NumberOfConnectedLines == 0)
                {
                    dotsForRemoval.Add(dot);
                    continue;
                }

                if (dot.NumberOfConnectedLines == 1)
                {
                    dotsForRemoval.Add(dot);

                    foreach (Line line in lines)
                    {
                        if (line.dot1.index == dot.index)
                        {
                            dotStart = line.dot2;
                            linesForRemoval.Add(line);
                            break;
                        }

                        if (line.dot2.index == dot.index)
                        {
                            dotEnd = line.dot1;
                            linesForRemoval.Add(line);
                            break;
                        }
                    }

                }

                if (dot.NumberOfConnectedLines == 2)
                {
                    dotsForRemoval.Add(dot);
                    foreach (Line line in lines)
                    {
                        if (line.dot1.index == dot.index)
                        {
                            dotStart = line.dot2;
                            linesForRemoval.Add(line);
                        }

                        if (line.dot2.index == dot.index)
                        {
                            dotEnd = line.dot1;
                            linesForRemoval.Add(line);
                        }
                    }
                }
            }

            foreach (Line line in linesForRemoval)
            {
                Lines.Remove(line);
            }

            foreach (Dot dot in dotsForRemoval)
            {
                Dots.Remove(dot);
            }
        }
    }
}