using GraphEditor.EdgesAndNodes.Edges;
using System;
using System.IO;
using System.Threading;
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
               await ParseHtmlString(File.ReadAllText(openFileDialog.FileName));
            }
        }

        private async Task ParseHtmlString(string content)
        {
            int i1 = 0;
            int i2 = 0;
            string firstNode = "";
            string relationType = "";
            string relationName = "";
            string secondNode = "";
            bool shouldFirstBeCreated = true;
            bool shouldSecondBeCreated = true;
            Node existingFirstNode = null;
            Node existingSecondNode = null;
            Random random = new Random();
            int animationIndex = 0;

            while (true)
            {
                shouldFirstBeCreated = true;
                shouldSecondBeCreated = true;

                i1 = content.IndexOf("\"firstNode\"", i1);
                i1 = content.IndexOf(">", i1) + 1;
                i2 = content.IndexOf("<", i1);
                firstNode = content.Substring(i1, i2 - i1);

                i1 = content.IndexOf("\"relationName\"", i1);
                i1 = content.IndexOf("class", i1);
                i1 = content.IndexOf("\"", i1) + 1;
                i2 = content.IndexOf("\"", i1);
                relationType = content.Substring(i1, i2 - i1);

                i1 = content.IndexOf(">", i1) + 1;
                i2 = content.IndexOf("<", i1);
                relationName = content.Substring(i1, i2 - i1);

                i1 = content.IndexOf("\"secondNode\"", i1);
                i1 = content.IndexOf(">", i1) + 1;
                i2 = content.IndexOf("<", i1);
                secondNode = content.Substring(i1, i2 - i1);

                foreach(var node in _mainWindow._nodes)
                {
                    if (node.Name == firstNode)
                    {
                        shouldFirstBeCreated = false;
                        existingFirstNode = node;
                    }
                    if (node.Name == secondNode)
                    {
                        shouldSecondBeCreated = false;
                        existingSecondNode = node;
                    }
                }

                if (shouldFirstBeCreated)
                {
                    existingFirstNode = _mainWindow.CreateNode(new System.Windows.Point(random.Next(1000), random.Next(1000)));
                    existingFirstNode.Rename(firstNode);
                    await Task.Delay(200);
                }
                if (shouldSecondBeCreated)
                {
                    existingSecondNode = _mainWindow.CreateNode(new System.Windows.Point(random.Next(1000), random.Next(1000)));
                    existingSecondNode.Rename(secondNode);
                    await Task.Delay(200);
                }

                switch (relationType)
                {
                    case "nonOriented": _mainWindow.CreateEdge(existingFirstNode, existingSecondNode, Edge.EdgeTypes.NonOriented); break;
                    case "orientedSimple": _mainWindow.CreateEdge(existingFirstNode, existingSecondNode, Edge.EdgeTypes.OrientedSimple); break;
                    case "orientedPencil": _mainWindow.CreateEdge(existingFirstNode, existingSecondNode, Edge.EdgeTypes.OrientedPencil); break;
                }
                Console.WriteLine(firstNode + " " + relationName + " " + secondNode);
                
                if (animationIndex == 3)
                {
                    animationIndex = 0;
                }       
                if (animationIndex == 0)
                {
                    _mainWindow.OnButtonMagicWondClick(null, null);
                }
                await Task.Delay(200);                
                if (content.IndexOf("firstNode", i1) == -1) return;
                animationIndex++;
            }
        }
    }
}
