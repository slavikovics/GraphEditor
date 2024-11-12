using GraphEditor.EdgesAndNodes.Edges;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphEditor.GraphsSavingAndLoading
{
    public class HtmlImport
    {
        private MainWindow _mainWindow;

        public HtmlImport(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            ImportFile();
        }

        private async void ImportFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "HTML Files|*.html;*.htm|All Files|*.*";
            DialogResult dialogResult = openFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                _mainWindow.MainCanvas.Children.Clear();
                _mainWindow._edges.Clear();
                _mainWindow._nodes.Clear();
               await ParseHtmlString(File.ReadAllText(openFileDialog.FileName));
            }
        }

        private void FindNextFirstNode(string content, ref int i1)
        {
            i1 = content.IndexOf("\"firstNode\"", i1);
        }

        private void FindNextSecondNode(string content, ref int i1)
        {
            i1 = content.IndexOf("\"secondNode\"", i1);
        }

        private string FindNextFirstNodeName(string content, ref int i1)
        {
            int i2 = 0;
            i1 = content.IndexOf(">", i1) + 1;
            i2 = content.IndexOf("<", i1);
            return content.Substring(i1, i2 - i1);
        }

        private string FindNextSecondNodeName(string content, ref int i1)
        {
            int i2 = 0;
            i1 = content.IndexOf(">", i1) + 1;
            i2 = content.IndexOf("<", i1);
            return content.Substring(i1, i2 - i1);
        }

        private string FindNextRelationType(string content, ref int i1)
        {
            int i2 = 0;
            i1 = content.IndexOf("\"relationName\"", i1);
            i1 = content.IndexOf("class", i1);
            i1 = content.IndexOf("\"", i1) + 1;
            i2 = content.IndexOf("\"", i1);
            return content.Substring(i1, i2 - i1);
        }

        private string FindNextRelationName(string content, ref int i1)
        {
            int i2 = 0;
            i1 = content.IndexOf(">", i1) + 1;
            i2 = content.IndexOf("<", i1);
            return content.Substring(i1, i2 - i1);
        }

        private string FindNextId(string content, ref int i1)
        {
            int i2 = 0;
            i1 = content.IndexOf("id", i1);
            i1 = content.IndexOf("\"", i1) + 1;
            i2 = content.IndexOf("\"", i1);
            i1--;
            return content.Substring(i1, i2 - i1);
        }

        private async Task CheckNodesExistance(RelationDataModel relationDataModel)
        {
            foreach (var node in _mainWindow._nodes)
            {
                if (node.Name == relationDataModel.FirstNodeName)
                {
                    relationDataModel.ShouldFirstNodeBeCreated = false;
                    relationDataModel.FirstNode = node;
                }
                else if (node.Name == relationDataModel.SecondNodeName)
                {
                    relationDataModel.ShouldSecondNodeBeCreated = false;
                    relationDataModel.SecondNode = node;
                }
            }
        }

        private async Task ParseHtmlString(string content)
        {
            int i1 = 0;
            int i2 = 0;
            Random random = new Random();
            List<RelationDataModel> plannedRelations = new List<RelationDataModel>();
            int animationIndex = 0;

            while (true)
            {
                RelationDataModel relationDataModel = new RelationDataModel();

                FindNextFirstNode(content, ref i1);
                relationDataModel.FirstNodeId = FindNextId(content, ref i1);
                relationDataModel.FirstNodeName = FindNextFirstNodeName(content, ref i1);
                relationDataModel.RelationType = FindNextRelationType(content, ref i1);
                relationDataModel.RelationId = FindNextId(content, ref i1);
                relationDataModel.RelationName = FindNextRelationName(content, ref i1);
                FindNextSecondNode(content, ref i1);
                relationDataModel.SecondNodeId = FindNextId(content, ref i1);
                relationDataModel.SecondNodeName = FindNextSecondNodeName(content, ref i1);

                await CheckNodesExistance(relationDataModel);

                if (relationDataModel.FirstNodeId.Contains("hn:") || relationDataModel.SecondNodeId.Contains("hn:"))
                {
                    plannedRelations.Add(relationDataModel);
                }

                if (relationDataModel.ShouldFirstNodeBeCreated)
                {
                    relationDataModel.FirstNode = _mainWindow.CreateNode(new System.Windows.Point(random.Next(1000), random.Next(1000)));
                    relationDataModel.FirstNode.Rename(relationDataModel.FirstNodeName);
                    await Task.Delay(100);
                }
                if (relationDataModel.ShouldSecondNodeBeCreated)
                {
                    relationDataModel.SecondNode = _mainWindow.CreateNode(new System.Windows.Point(random.Next(1000), random.Next(1000)));
                    relationDataModel.SecondNode.Rename(relationDataModel.SecondNodeName);
                    await Task.Delay(100);
                }

                switch (relationDataModel.RelationType)
                {
                    case "nonOriented": _mainWindow.CreateEdge(relationDataModel.FirstNode, relationDataModel.SecondNode, Edge.EdgeTypes.NonOriented); break;
                    case "orientedSimple": _mainWindow.CreateEdge(relationDataModel.FirstNode, relationDataModel.SecondNode, Edge.EdgeTypes.OrientedSimple); break;
                    case "orientedPencil": _mainWindow.CreateEdge(relationDataModel.FirstNode, relationDataModel.SecondNode, Edge.EdgeTypes.OrientedPencil); break;
                }
                Console.WriteLine(relationDataModel.FirstNode + " " + relationDataModel.RelationName + " " + relationDataModel.SecondNode);
                
                if (animationIndex == 3)
                {
                    animationIndex = 0;
                }       
                if (animationIndex == 0)
                {
                    _mainWindow.OnButtonMagicWondClick(null, null);
                }
                await Task.Delay(100);                
                if (content.IndexOf("firstNode", i1) == -1) return;
                animationIndex++;
            }
        }
    }
}
