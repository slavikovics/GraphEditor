using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            }
        }

        private void OpenFile(string fileName)
        {
            if (!CheckFileExtension(fileName)) return;
            Content = File.ReadAllText(fileName);
        }

        private void ParseFile()
        {
            int searchIndex = 0;
            int unitStartIndex = 0;
            int unitEndIndex = 0;

            string KeyNode = null;

            //Content.Replace(" ", "");
            //Content.Replace("\n", "");
            // should work only with one space between elements

            GraphUnits.Add(new GraphUnit());

            // finding first node to relation
            if (KeyNode == null)
            {
                unitEndIndex = Content.IndexOf(" ", searchIndex);
                GraphUnits.Last().FirstNodeName = Content.Substring(unitStartIndex, unitEndIndex - unitEndIndex);
                unitStartIndex = unitEndIndex + 1;
            }
            else
            {
                GraphUnits.Last().FirstNodeName = KeyNode;
            }

            // finding relation type
            unitEndIndex = Content.IndexOf(" ", unitStartIndex + 1);
            GraphUnits.Last().RelationType = Content.Substring(unitStartIndex, unitEndIndex - unitStartIndex);
            unitStartIndex = unitEndIndex + 1;

            // finding relation name
            if (CheckIfRelationHasName(unitStartIndex))
            {
                unitEndIndex = Content.IndexOf(" ", unitStartIndex + 1);
                GraphUnits.Last().RelationName = Content.Substring(unitStartIndex, unitEndIndex - unitStartIndex);
                unitStartIndex = unitEndIndex + 1;
            }
            else
            {
                GraphUnits.Last().RelationName = "";
            }

            if (Content[FindFirstIndexOfLineEnd(unitStartIndex)] == ';' && Content[FindFirstIndexOfLineEnd(unitStartIndex) + 1] == ';')
            {
                KeyNode = null;
            }
            else if (Content[FindFirstIndexOfLineEnd(unitStartIndex)] == ';')
            {
                KeyNode = GraphUnits.Last().FirstNodeName;
            }
            else
            {
                KeyNode = GraphUnits.Last().SecondNodeName;
            }


        }

        private bool CheckFileExtension(string fileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            if (fileInfo.Extension == ".scs" || fileInfo.Extension == ".txt") return true;
            else return false;
        }

        private int FindFirstIndexOfLineEnd(int startIndex)
        {
            int unitEndIndex1 = Content.IndexOf(";", startIndex);
            int unitEndIndex2 = Content.IndexOf("(", startIndex);
            return Math.Min(unitEndIndex1, unitEndIndex2);
        }

        private bool CheckIfRelationHasName(int unitStartIndex)
        {
            if (Content.Substring(unitStartIndex, Content.IndexOf(" ", unitStartIndex + 1)).Contains(":")) return true;
            else return false;      
        }
    }
}
