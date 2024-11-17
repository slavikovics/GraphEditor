using GraphEditor.EdgesAndNodes.Edges;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using GraphEditor.Properties;

namespace GraphEditor.GraphsSavingAndLoading
{
    public class HtmlImport
    {
        private readonly MainWindow _mainWindow;
        
        private List<RelationDataModel> _relationDataModels = new List<RelationDataModel>();
        
        public event EventHandler<ImportEventArgs> OnImportStarting;

        public HtmlImport(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public async void ImportFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = Resources.OpenFileDialogFilter;
            DialogResult dialogResult = openFileDialog.ShowDialog();
            
            if (dialogResult != DialogResult.OK) return;
            string content = File.ReadAllText(openFileDialog.FileName);
            OnImportStarting?.Invoke(this, new ImportEventArgs(FindGraphName(content)));
            _mainWindow.MainCanvas.Children.Clear();
            _mainWindow.CurrentGraph.ClearGraph();
            await ParseHtmlString(content);
        }

        private static void FindNextFirstNode(string content, ref int i1)
        {
            i1 = content.IndexOf("\"firstNode\"", i1, StringComparison.Ordinal);
        }

        private static string FindGraphName(string content)
        {
            int i1 = content.IndexOf("<title>", 0, StringComparison.Ordinal);
            i1 = content.IndexOf(">", i1, StringComparison.Ordinal) + 1;
            int i2 = content.IndexOf("<", i1, StringComparison.Ordinal);
            return content.Substring(i1, i2 - i1);
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

        private static EdgeTypes FindNextRelationType(string content, ref int i1)
        {
            i1 = content.IndexOf("\"relationName\"", i1, StringComparison.Ordinal);
            i1 = content.IndexOf("class", i1, StringComparison.Ordinal);
            i1 = content.IndexOf("\"", i1, StringComparison.Ordinal) + 1;
            int i2 = content.IndexOf("\"", i1, StringComparison.Ordinal);

            switch (content.Substring(i1, i2 - i1))
            {
                case "nonOriented": return EdgeTypes.NonOriented;
                case "orientedSimple": return EdgeTypes.OrientedSimple;
                case "orientedPencil": return EdgeTypes.OrientedPencil;
                default: return EdgeTypes.NonOriented;
            }
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
            foreach (Node node in _mainWindow.CurrentGraph.Nodes)
            {
                if (relationDataModel.FirstNode == null && node.Id == relationDataModel.FirstNodeName)
                {
                    relationDataModel.ShouldFirstNodeBeCreated = false;
                    relationDataModel.FirstNode = node;
                }
                if (relationDataModel.SecondNode == null && node.Id == relationDataModel.SecondNodeName)
                { 
                    relationDataModel.ShouldSecondNodeBeCreated = false;
                    relationDataModel.SecondNode = node;
                }
            }
        }

        private void CheckPlannedNodesExistence(RelationDataModel relationDataModel)
        {
            
        }

        private static Point GetRandomPoint()
        {
            Random random = new Random();
            return new Point(random.Next(600) + 200, random.Next(400) + 150);
        }

        private async Task AddRelation(RelationDataModel relationDataModel)
        {
            Random random = new Random();
            if (relationDataModel.ShouldFirstNodeBeCreated)
            {
                relationDataModel.FirstNode = _mainWindow.CreateNode(GetRandomPoint(), true, relationDataModel.FirstNodeName);
                relationDataModel.FirstNode.Rename(relationDataModel.FirstNodeName);
                //relationDataModel.FirstNode.Id = relationDataModel.FirstNodeId;
                await Task.Delay(150);
            }
            if (relationDataModel.ShouldSecondNodeBeCreated)
            {
                relationDataModel.SecondNode = _mainWindow.CreateNode(GetRandomPoint(), true, relationDataModel.SecondNodeName);
                relationDataModel.SecondNode.Rename(relationDataModel.SecondNodeName);
                //relationDataModel.SecondNode.Id = relationDataModel.SecondNodeId;
                await Task.Delay(150);
            }
            
            _mainWindow.SetSelectedEdgeType(relationDataModel.RelationType);
            relationDataModel.CenterNode = (_mainWindow.CreateEdge(relationDataModel.FirstNode, relationDataModel.SecondNode, relationDataModel.RelationType) as Edge).GetCenterNode();

            await Task.Delay(150);
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
            /*foreach (Edge edge in _mainWindow.CurrentGraph.Edges)
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
            }*/

            foreach (RelationDataModel createdRelation in _relationDataModels)
            {
                int len = relationDataModel.FirstNodeId.IndexOf('-') - 3;
                string centerNodeFirstNodeId;
                string centerNodeSecondNodeId;

                if (len >= 0)
                {
                    centerNodeFirstNodeId = relationDataModel.FirstNodeId.Substring(3, len);
                    len++;
                    centerNodeSecondNodeId = relationDataModel.FirstNodeId.Substring(len + 3);
                }
                else
                {
                    centerNodeFirstNodeId = null;
                    centerNodeSecondNodeId = null;
                }
                
                if (centerNodeFirstNodeId != null)
                {
                    if ((centerNodeFirstNodeId == createdRelation.FirstNodeId &&
                         centerNodeSecondNodeId == createdRelation.SecondNodeId) ||
                        (centerNodeFirstNodeId == createdRelation.SecondNodeId &&
                         centerNodeSecondNodeId == createdRelation.FirstNodeId))
                    {
                        relationDataModel.ShouldFirstNodeBeCreated = false;
                        relationDataModel.FirstNode = createdRelation.CenterNode;
                    }
                }

                len = relationDataModel.SecondNodeId.IndexOf('-') - 3;
                
                if (len >= 0)
                {
                    centerNodeFirstNodeId = relationDataModel.SecondNodeId.Substring(3, len);
                    len++;
                    centerNodeSecondNodeId = relationDataModel.SecondNodeId.Substring(len + 3);
                }
                else
                {
                    centerNodeFirstNodeId = null;
                    centerNodeSecondNodeId = null;
                }

                if (centerNodeFirstNodeId != null)
                {
                    if ((centerNodeFirstNodeId == createdRelation.FirstNodeId &&
                         centerNodeSecondNodeId == createdRelation.SecondNodeId) ||
                        (centerNodeFirstNodeId == createdRelation.SecondNodeId &&
                         centerNodeSecondNodeId == createdRelation.FirstNodeId))
                    {
                        relationDataModel.ShouldSecondNodeBeCreated = false;
                        relationDataModel.SecondNode = createdRelation.CenterNode;
                    }
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
                _relationDataModels.Add(relationDataModel);

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
                await Task.Delay(150);                
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
                await CreateLonelyNode(relationDataModel, random);
                //relationDataModel.FirstNode.Id = relationDataModel.FirstNodeId;
            }

            foreach (RelationDataModel relationDataModel in plannedRelations)
            {
                _relationDataModels.Add(relationDataModel);
                FindExistingNodes(relationDataModel);
                CheckNodesExistence(relationDataModel);
                await AddRelation(relationDataModel);
                _mainWindow.OnButtonMagicWondClick(null, null);
                await Task.Delay(200); 
            }
        }

        private async Task CreateLonelyNode(RelationDataModel relationDataModel, Random random)
        {
            relationDataModel.FirstNode = _mainWindow.CreateNode(new System.Windows.Point(random.Next(600) + 200, random.Next(400) + 150), true, relationDataModel.FirstNodeName);
            relationDataModel.FirstNode.Rename(relationDataModel.FirstNodeName);
            await Task.Delay(200);
        }
    }
}
