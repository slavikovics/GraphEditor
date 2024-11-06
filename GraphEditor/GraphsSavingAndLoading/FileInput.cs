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

            GraphUnits.Add(new GraphUnit());

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
            

            if (CheckIfRelationHasName(unitStartIndex))
            {
                unitEndIndex = Content.IndexOf(":", unitStartIndex);
                GraphUnits.Last().RelationName = Content.Substring(unitStartIndex, unitEndIndex - unitStartIndex);
                unitStartIndex = unitEndIndex + 1;
            }
            else
            {
                GraphUnits.Last().RelationName = "";
            }

            unitEndIndex = Content.IndexOf(" ", unitStartIndex + 1);
            GraphUnits.Last().RelationType = Content.Substring(unitStartIndex, unitEndIndex - unitStartIndex);

        }

        private bool CheckFileExtension(string fileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            if (fileInfo.Extension == ".scs" || fileInfo.Extension == ".txt") return true;
            else return false;
        }

        private bool CheckIfRelationHasName(int unitStartIndex)
        {
            int colonIndex = Content.IndexOf(":", unitStartIndex);
            int arrowStartIndex1 = Content.IndexOf("=", unitStartIndex);
            int arrowStartIndex2 = Content.IndexOf("<", unitStartIndex);
            int arrowStartIndex3 = Content.IndexOf(">", unitStartIndex);
            int arrowStartIndex4 = Content.IndexOf("(", unitStartIndex);
            if (colonIndex < arrowStartIndex1 && colonIndex < arrowStartIndex2 && colonIndex < arrowStartIndex3 && colonIndex < arrowStartIndex4)
            {
                return true;
            }
            else return false;
        }
    }
}
