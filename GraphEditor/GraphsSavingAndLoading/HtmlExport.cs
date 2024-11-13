using GraphEditor.EdgesAndNodes;
using GraphEditor.EdgesAndNodes.Edges;
using GraphEditor.EdgesAndNodes.Nodes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GraphEditor.Properties;

namespace GraphEditor.GraphsSavingAndLoading
{
    internal class HtmlExport
    {
        public string Content { get; private set; }

        private List<Node> Nodes { get; set; }

        private List<IEdge> Edges { get; set; }

        public HtmlExport(Canvas canvas, List<Node> nodes, List<IEdge> edges)
        {
            Nodes = nodes;
            Edges = edges;

            foreach (var child in canvas.Children)
            {
                Console.WriteLine(child.ToString());
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = Resources.SaveFileDialogPngFileFilter;
            saveFileDialog.Title = Resources.SaveFileDialogTitle;
            DialogResult dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                string fileDialogFolderName = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.IndexOf(".", StringComparison.Ordinal));
                string graphName = System.IO.Path.GetFileName(saveFileDialog.FileName).Replace(".png", "");
                string fileDialogImagePath = fileDialogFolderName + "\\graph.png";
                Directory.CreateDirectory(fileDialogFolderName);
                RenderToPngFile(canvas, fileDialogImagePath);
                GenerateHtml(fileDialogImagePath.Replace(".png", ".html"), graphName);
            }            
        }

        private void GenerateHtml(string htmlPath, string graphName)
        {
            string content = Resources.HtmlBeginWithCssStyles + graphName + Resources.HtmlMiddleWithImagePath + graphName + Resources.HtmlImageDivEnd + BuildBody() + Resources.HtmlEnd;
            File.WriteAllText(htmlPath, content);         
        }

        private string BuildNodeContent(Node node, bool isFirstNode)
        {
            string nodeClass = Resources.firstNode;
            if (!isFirstNode)
            {
                nodeClass = Resources.secondNode;
            }

            string name = node.Name;
            string id = node.Id;

            if (node is HiddenNode)
            {
                name = (node as HiddenNode).Id;
                id = (node as HiddenNode).Id;
            }

            string result = Resources.NodeItemGroupStart + nodeClass + Resources.NodeItemGroupIdTag + id + Resources.EndOfTag + name + Resources.SpanAndDivEnd;

            return result;
        }

        private string BuildEdgeContent(IEdge edge)
        {
            string edgeType = Resources.orientedSimple;
            string edgeContent = Resources.OrientedSimpleSvg;
            if (edge is OrientedEdge && (edge as OrientedEdge)._isPencil)
            {
                edgeContent = Resources.OrientedPencilSvg;
                edgeType = Resources.orientedPencil;
            }
            else if (edge is NonOrientedEdge)
            {
                edgeContent = Resources.NonOrientedSvg;
                edgeType = Resources.nonOriented;
            }
            edgeContent += Resources.SpanRelationStart + edgeType + Resources.NodeItemGroupIdTag + (edge as Edge).ToString() + Resources.TagClosing + (edge as Edge).Name + Resources.SpanRelationEnd;
            return edgeContent;
        }

        private string BuildBody()
        {
            string body = "";
            foreach (IEdge edge in Edges)
            {
                body += Resources.DivRelationStart;
                body += BuildNodeContent((edge as Edge).SecondNode, true);
                body += BuildEdgeContent((edge as Edge));
                body += BuildNodeContent((edge as Edge).FirstNode, false);
                body += Resources.DivRelationEnd;
            }

            body += BuildLonelyNodesContent();
            return body;
        }

        private List<Node> FindNodes()
        {
            List<Node> nodes = new List<Node>();
            bool shouldBeSkipped = false;
            foreach (Node node in Nodes)
            {
                foreach (IEdge edge in Edges)
                {
                    if (node == (edge as Edge).FirstNode)
                    {
                        shouldBeSkipped = true;
                        break;
                    }

                    if (node == (edge as Edge).SecondNode)
                    {
                        shouldBeSkipped = true;
                        break;
                    }
                }

                if (!shouldBeSkipped)
                {
                    nodes.Add(node);
                }
                shouldBeSkipped = false;
            }
            
            return nodes;
        }

        private string BuildLonelyNodesContent()
        {
            List<Node> nodes = FindNodes();
            string body = "";

            foreach (Node node in nodes)
            {
                body += BuildNodeContent(node, true);
            }

            return body;
        }

        private const double DefaultDpi = 300;

        public static ImageSource RenderToPngImageSource(Visual targetControl)
        {
            var renderTargetBitmap = GetRenderTargetBitmapFromControl(targetControl);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

            var result = new BitmapImage();

            using (var memoryStream = new MemoryStream())
            {
                encoder.Save(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);

                result.BeginInit();
                result.CacheOption = BitmapCacheOption.OnLoad;
                result.StreamSource = memoryStream;
                result.EndInit();
            }

            return result;
        }

        private static void RenderToPngFile(Visual targetControl, string filename)
        {
            var renderTargetBitmap = GetRenderTargetBitmapFromControl(targetControl);

            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));

            var result = new BitmapImage();

            try
            {
                using (var fileStream = new FileStream(filename, FileMode.Create))
                {
                    encoder.Save(fileStream);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"There was an error saving the file: {ex.Message}");
            }
        }

        private static BitmapSource GetRenderTargetBitmapFromControl(Visual targetControl, double dpi = DefaultDpi)
        {
            if (targetControl == null) return null;

            var bounds = VisualTreeHelper.GetDescendantBounds(targetControl);
            var renderTargetBitmap = new RenderTargetBitmap((int)(bounds.Width * dpi / 96.0),
                                                            (int)(bounds.Height * dpi / 96.0),
                                                            dpi,
                                                            dpi,
                                                            PixelFormats.Pbgra32);

            var drawingVisual = new DrawingVisual();

            using (var drawingContext = drawingVisual.RenderOpen())
            {
                var visualBrush = new VisualBrush(targetControl);
                drawingContext.DrawRectangle(visualBrush, null, new Rect(new Point(), bounds.Size));
            }

            renderTargetBitmap.Render(drawingVisual);
            return renderTargetBitmap;
        }
    }
}
