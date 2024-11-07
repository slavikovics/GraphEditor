using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace GraphEditor.GraphsSavingAndLoading
{
    internal class FileInput
    {
        public string Content { get; private set; }

        public List<GraphUnit> GraphUnits { get; private set; }

        public FileInput()
        {
            GraphUnits = new List<GraphUnit>();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            DialogResult dialogResult = openFileDialog.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                OpenFile(openFileDialog.FileName);
                try
                {
                    ParseFile();
                }
                catch
                { 
                }             
                PrintGraph();
            }     
        }

        private void OpenFile(string fileName)
        {
            if (!CheckFileExtension(fileName)) return;
            Content = File.ReadAllText(fileName);
        }

        private void ParseFile()
        {
            int unitStartIndex = 0;
            int unitEndIndex = 0;

            string keyNode = null;

            //Content.Replace(" ", "");
            
            Content.Replace("\r", "");
            Content.Replace("\n", "");
            Content = Regex.Replace(Content, @"\s+", " ").Trim();

            Console.WriteLine(Content);
            // should work only with one space between elements

            while (unitStartIndex <= Content.Length)
            {
                GraphUnits.Add(new GraphUnit());

                // finding first node to relation
                if (keyNode == null)
                {
                    unitEndIndex = Content.IndexOf(" ", unitStartIndex);
                    if (unitEndIndex == -1) return;
                    GraphUnits.Last().FirstNodeName = Content.Substring(unitStartIndex, unitEndIndex - unitStartIndex);
                    unitStartIndex = unitEndIndex + 1;
                    if (unitStartIndex > Content.Length) return;
                }
                else
                {
                    GraphUnits.Last().FirstNodeName = keyNode;
                }

                // finding relation type
                unitEndIndex = Content.IndexOf(" ", unitStartIndex);
                if (unitEndIndex == -1) return;
                GraphUnits.Last().RelationType = Content.Substring(unitStartIndex, unitEndIndex - unitStartIndex);
                unitStartIndex = unitEndIndex + 1;
                if (unitStartIndex > Content.Length) return;

                // finding relation name
                if (CheckIfRelationHasName(unitStartIndex))
                {
                    unitEndIndex = Content.IndexOf(" ", unitStartIndex);
                    if (unitEndIndex == -1) return;
                    GraphUnits.Last().RelationName = Content.Substring(unitStartIndex, unitEndIndex - unitStartIndex);
                    unitStartIndex = unitEndIndex + 1;
                    if (unitStartIndex > Content.Length) return;
                }
                else
                {
                    GraphUnits.Last().RelationName = "";
                }

                unitEndIndex = FindFirstIndexOfLineEnd(unitStartIndex);
                if (unitEndIndex == -1) return;
                GraphUnits.Last().SecondNodeName = Content.Substring(unitStartIndex, unitEndIndex - unitStartIndex).Trim();
                unitStartIndex = unitEndIndex;
                if (unitStartIndex + 1 > Content.Length) return;

                if (Content[FindFirstIndexOfLineEnd(unitStartIndex)] == ';' && Content[FindFirstIndexOfLineEnd(unitStartIndex) + 1] == ';')
                {
                    keyNode = null;
                    unitStartIndex += 2;
                    continue;
                }
                else if (Content[FindFirstIndexOfLineEnd(unitStartIndex)] == ';')
                {
                    keyNode = GraphUnits.Last().FirstNodeName;
                    unitStartIndex++;
                    continue;
                }

                keyNode = GraphUnits.Last().SecondNodeName;
                unitStartIndex += 2;
            }

            
        }

        private bool CheckFileExtension(string fileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            if (fileInfo.Extension == ".scs" || fileInfo.Extension == ".txt") return true;
            else return false;
        }

        private void PrintGraph()
        {
            foreach (GraphUnit graphUnit in GraphUnits)
            {
                Console.WriteLine($"{GraphUnits.IndexOf(graphUnit) + 1} {graphUnit.FirstNodeName} {graphUnit.RelationType} {graphUnit.RelationName} {graphUnit.SecondNodeName}");
            }
        }

        private int FindFirstIndexOfLineEnd(int startIndex)
        {
            int unitEndIndex1 = Content.IndexOf(";", startIndex);
            int unitEndIndex2 = Content.IndexOf("(", startIndex);
            return Math.Min(unitEndIndex1, unitEndIndex2);
        }

        private bool CheckIfRelationHasName(int unitStartIndex)
        {
            if (unitStartIndex >= Content.Length) return false;
            int index = Content.IndexOf(" ", unitStartIndex);
            if (index == -1) return false;
            if (Content.Substring(unitStartIndex, index - unitStartIndex).Contains(":")) return true;
            else return false;      
        }
    }
}
