using GraphEditor.EdgesAndNodes.Edges;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using GraphEditor.Properties;

namespace GraphEditor.GraphsSavingAndLoading
{
    public class HtmlImport
    {
        private readonly MainWindow _mainWindow;

        public HtmlImport(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            ImportFile();
        }

        private async void ImportFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = Resources.OpenFileDialogFilter;
            DialogResult dialogResult = openFileDialog.ShowDialog();
            
            if (dialogResult != DialogResult.OK) return;
            _mainWindow.MainCanvas.Children.Clear();
            _mainWindow._edges.Clear();
            _mainWindow._nodes.Clear();
            await ParseHtmlString(File.ReadAllText(openFileDialog.FileName));
        }

        private static void FindNextFirstNode(string content, ref int i1)
        {
            i1 = content.IndexOf("\"firstNode\"", i1, StringComparison.Ordinal);
        }

        private static void FindNextSecondNode(string content, ref int i1)
        {
            i1 = content.IndexOf("\"secondNode\"", i1, StringComparison.Ordinal);
        }

        private static string FindNextFirstNodeName(string content, ref int i1)
        {
            i1 = content.IndexOf(">", i1, StringComparison.Ordinal) + 1;
            int i2 = content.IndexOf("<", i1, StringComparison.Ordinal);
            return content.Substring(i1, i2 - i1);
        }

        private static string FindNextSecondNodeName(string content, ref int i1)
        {
            i1 = content.IndexOf(">", i1, StringComparison.Ordinal) + 1;
            int i2 = content.IndexOf("<", i1, StringComparison.Ordinal);
            return content.Substring(i1, i2 - i1);
        }

        private static string FindNextRelationType(string content, ref int i1)
        {
            i1 = content.IndexOf("\"relationName\"", i1, StringComparison.Ordinal);
            i1 = content.IndexOf("class", i1, StringComparison.Ordinal);
            i1 = content.IndexOf("\"", i1, StringComparison.Ordinal) + 1;
            int i2 = content.IndexOf("\"", i1, StringComparison.Ordinal);
            return content.Substring(i1, i2 - i1);
        }

        private static string FindNextRelationName(string content, ref int i1)
        {
            i1 = content.IndexOf(">", i1, StringComparison.Ordinal) + 1;
            int i2 = content.IndexOf("<", i1, StringComparison.Ordinal);
            return content.Substring(i1, i2 - i1);
        }

        private static string FindNextId(string content, ref int i1)
        {
            i1 = content.IndexOf("id", i1, StringComparison.Ordinal);
            i1 = content.IndexOf("\"", i1, StringComparison.Ordinal) + 1;
            int i2 = content.IndexOf("\"", i1, StringComparison.Ordinal);
            return content.Substring(i1, i2 - i1);
        }

        private void CheckNodesExistence(RelationDataModel relationDataModel)
        {
            foreach (Node node in _mainWindow._nodes)
            {
                if (relationDataModel.FirstNode == null && node.Id == relationDataModel.FirstNodeId)
                {
                    relationDataModel.ShouldFirstNodeBeCreated = false;
                    relationDataModel.FirstNode = node;
                }
                if (relationDataModel.SecondNode == null && node.Id == relationDataModel.SecondNodeId)
                { 
                    relationDataModel.ShouldSecondNodeBeCreated = false;
                    relationDataModel.SecondNode = node;
                }
            }
        }

        private async Task AddRelation(RelationDataModel relationDataModel)
        {
            Random random = new Random();
            if (relationDataModel.ShouldFirstNodeBeCreated)
            {
                relationDataModel.FirstNode = _mainWindow.CreateNode(new System.Windows.Point(random.Next(800), random.Next(800)));
                relationDataModel.FirstNode.Rename(relationDataModel.FirstNodeName);
                relationDataModel.FirstNode.Id = relationDataModel.FirstNodeId;
                await Task.Delay(300);
            }
            if (relationDataModel.ShouldSecondNodeBeCreated)
            {
                relationDataModel.SecondNode = _mainWindow.CreateNode(new System.Windows.Point(random.Next(600), random.Next(600)));
                relationDataModel.SecondNode.Rename(relationDataModel.SecondNodeName);
                relationDataModel.SecondNode.Id = relationDataModel.SecondNodeId;
                await Task.Delay(300);
            }

            switch (relationDataModel.RelationType)
            {
                case "nonOriented": _mainWindow.CreateEdge(relationDataModel.FirstNode, relationDataModel.SecondNode, Edge.EdgeTypes.NonOriented); break;
                case "orientedSimple": _mainWindow.CreateEdge(relationDataModel.FirstNode, relationDataModel.SecondNode, Edge.EdgeTypes.OrientedSimple); break;
                case "orientedPencil": _mainWindow.CreateEdge(relationDataModel.FirstNode, relationDataModel.SecondNode, Edge.EdgeTypes.OrientedPencil); break;
            }

            await Task.Delay(300);
        }

        private static void FillRelationDataModel(RelationDataModel relationDataModel, string content, ref int i1)
        {
            FindNextFirstNode(content, ref i1);
            relationDataModel.FirstNodeId = FindNextId(content, ref i1);
            relationDataModel.FirstNodeName = FindNextFirstNodeName(content, ref i1);
            relationDataModel.RelationType = FindNextRelationType(content, ref i1);
            relationDataModel.RelationId = FindNextId(content, ref i1);
            relationDataModel.RelationName = FindNextRelationName(content, ref i1);
            FindNextSecondNode(content, ref i1);
            relationDataModel.SecondNodeId = FindNextId(content, ref i1);
            relationDataModel.SecondNodeName = FindNextSecondNodeName(content, ref i1);
        }

        private void FindExistingNodes(RelationDataModel relationDataModel)
        {
            foreach (Edge edge in _mainWindow._edges)
            {
                if (relationDataModel.FirstNodeId.Contains(edge.GetFirstNodeId()) && relationDataModel.FirstNodeId.Contains(edge.GetSecondNodeId()))
                {
                    relationDataModel.ShouldFirstNodeBeCreated = false;
                    relationDataModel.FirstNode = edge.GetEdgeCenterNode();
                }

                if (relationDataModel.SecondNodeId.Contains(edge.GetFirstNodeId()) && relationDataModel.SecondNodeId.Contains(edge.GetSecondNodeId()))
                {
                    relationDataModel.ShouldSecondNodeBeCreated = false;
                    relationDataModel.SecondNode = edge.GetEdgeCenterNode();
                }
            }
        }

        private async Task ParseHtmlString(string content)
        {
            int i1 = 0;
            List<RelationDataModel> plannedRelations = new List<RelationDataModel>();
            int animationIndex = 0;

            while (true)
            {
                RelationDataModel relationDataModel = new RelationDataModel();

                FillRelationDataModel(relationDataModel, content, ref i1);
                CheckNodesExistence(relationDataModel);

                if (relationDataModel.FirstNodeId.Contains("hn:") || relationDataModel.SecondNodeId.Contains("hn:"))
                {
                    plannedRelations.Add(relationDataModel);
                    if (content.IndexOf("relationName", i1, StringComparison.Ordinal) == -1) break;
                    continue;
                }

                await AddRelation(relationDataModel);
                
                if (animationIndex == 3)
                {
                    animationIndex = 0;
                }       
                if (animationIndex == 0)
                {
                    _mainWindow.OnButtonMagicWondClick(null, null);
                }
                await Task.Delay(200);                
                if (content.IndexOf("relationName", i1, StringComparison.Ordinal) == -1) break;
                animationIndex++;
            }

            Random random = new Random();
            
            while (true)
            {
                if (content.IndexOf("firstNode", i1, StringComparison.Ordinal) == -1) break;
                
                RelationDataModel relationDataModel = new RelationDataModel();
                FindNextFirstNode(content, ref i1);
                relationDataModel.FirstNodeId = FindNextId(content, ref i1);
                relationDataModel.FirstNodeName = FindNextFirstNodeName(content, ref i1);
                relationDataModel.FirstNode = _mainWindow.CreateNode(new System.Windows.Point(random.Next(800), random.Next(800)));
                relationDataModel.FirstNode.Rename(relationDataModel.FirstNodeName);
                relationDataModel.FirstNode.Id = relationDataModel.FirstNodeId;
            }

            foreach (RelationDataModel relationDataModel in plannedRelations)
            {
                FindExistingNodes(relationDataModel);
                CheckNodesExistence(relationDataModel);
                await AddRelation(relationDataModel);
                _mainWindow.OnButtonMagicWondClick(null, null);
                await Task.Delay(200); 
            }
        }
    }
}
