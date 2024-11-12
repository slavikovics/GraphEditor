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

namespace GraphEditor.GraphsSavingAndLoading
{
    internal class HtmlExport
    {
        public string Content { get; private set; }

        public List<Node> Nodes { get; private set; }

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
            saveFileDialog.Filter = "PNG Image|*.png";
            saveFileDialog.Title = "Save the graph folder";
            DialogResult dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                string fileDialogFolderName = saveFileDialog.FileName.Substring(0, saveFileDialog.FileName.IndexOf("."));
                string graphName = System.IO.Path.GetFileName(saveFileDialog.FileName).Replace(".png", "");
                string fileDialogImagePath = fileDialogFolderName + "\\graph.png";
                Directory.CreateDirectory(fileDialogFolderName);
                RenderToPngFile(canvas, fileDialogImagePath);
                GenerateHtml(fileDialogImagePath.Replace(".png", ".html"), graphName);
            }            
        }

        private void GenerateHtml(string htmlPath, string graphName)
        {
            string content = "<!DOCTYPE html>\r\n<html lang=\"en\">\r\n    <style>\r\nbody, html {\r\n    margin: 0;\r\n    padding: 0;\r\n    width: 100%;\r\n    height: 100%;\r\n    font-family: 'Montserrat', sans-serif;\r\n    background-color: rgb(13, 1, 19); /* Medium purple background */\r\n}\r\n\r\n.container {\r\n    display: flex;\r\n    flex-direction: column;\r\n    align-items: center;\r\n    justify-content: flex-start;\r\n    margin: 2%;\r\n}\r\n\r\n.relation {\r\n    display: flex;\r\n    flex-direction: row;\r\n    align-items: left;\r\n    justify-content: flex-start;\r\n      margin-top: 5%;}\r\n\r\n.main-image {\r\n    width: 40%;\r\n    height: 40%; /* Adjust as needed */\r\n    object-fit: contain;\r\n}\r\n\r\n.header {\r\n    color: #AEA3D8;\r\n    font-size: 8vw;\r\n}\r\n\r\ntable \r\n{ \r\n    width: 100%; \r\n    border-collapse: collapse; \r\n    text-align: center; \r\n} \r\ntd \r\n{ \r\n    height: fit-content;\r\n    padding: 10px; \r\n    text-align: center;\r\n    margin: 10px;\r\n} \r\n.circle \r\n{ \r\n    width: 50px; \r\n    height: 50px; \r\n    background-color: #AEA3D8; \r\n    border-radius: 50%; \r\n    display: inline-block; \r\n    margin: auto;\r\n} \r\n.innerCircle \r\n{ \r\n    width: 40px; \r\n    height: 40px; \r\n    background-color: #AEA3D8; \r\n    border-radius: 50%; \r\n    display: inline-block; \r\n    margin: auto;\r\n} \r\n\r\n.arrow, .firstNode, .relationName, .secondNode\r\n{ \r\n  width: 20vw; \r\n  color: #AEA3D8; margin-right: 10px;\r\n    margin-left: 20px;\r\n    font-size: 2vw;\r\n    font-weight: bold;\r\n    transition: all 1s;\r\n}\r\n\r\n.arrow:hover, .firstNode:hover, .relationName:hover, .secondNode:hover\r\n{\r\n    color: #9370db;\r\n}\r\n\r\n\r\n.nodeItemGroup\r\n{\r\n    width: 40%;\r\n    text-align: center;\r\n}\r\n\r\n.node\r\n{\r\n      height: 3vw;\r\n    width: 3vw;}\r\n\r\n.edge\r\n{\r\n    height: 40px;\r\n        width: 10vw;\r\n    object-fit: contain;\r\n}\r\n\r\nfooter\r\n{\r\n margin-top: 8%;  \r\n   align-items: center;\r\n}\r\n\r\n.button \r\n{ \r\n    display: inline-block; \r\n    padding: 10px 20px; \r\n    margin: 20px; \r\n    background-color: #AEA3D8; \r\n    color: white; \r\n    text-align: center; \r\n    text-decoration: none; \r\n    border-radius: 15px; \r\n        font-size: 0.5vw; \r\n    cursor: pointer; \r\n    transition: all 1s;\r\n}\r\n\r\n.button:hover\r\n{\r\n    background-color: #9370db;; \r\n}\r\n\r\n    </style>\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>" + graphName + "</title>\r\n    <link href=\"https://fonts.googleapis.com/css2?family=Montserrat:wght@400;700&display=swap\" rel=\"stylesheet\">\r\n    <link rel=\"stylesheet\" href=\"styles.css\">\r\n</head>\r\n<body>\r\n    <div class=\"container\">\r\n        <img src=\"graph.png\" alt=\"Graph Image\" class=\"main-image\">\r\n        <h1 class=\"header\">" + graphName + "</h1>\r\n    </div>\r\n  " + BuildBody() + "  <footer>\r\n        <span class=\"arrow\">Generated in GraphEditor</span>\r\n        <a class=\"button\" href=\"https://github.com/slavikovics/GraphEditor\">GitHub</a> \r\n    </footer>\r\n</body>\r\n</html>\r\n";
            File.WriteAllText(htmlPath, content);         
        }

        private string BuildNodeContent(Node node, bool isFirstNode)
        {
            string nodeClass = "firstNode";
            if (!isFirstNode)
            {
                nodeClass = "secondNode";
            }

            string name = node.Name;
            string id = node.Id;

            if (node is HiddenNode)
            {
                name = (node as HiddenNode).Id;
                id = (node as HiddenNode).Id;
            }

            string result = "<div class=\"nodeItemGroup\">\r\n            <svg class=\"node\" id=\"_Слой_1\" data-name=\"Слой 1\" xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 56 56\">\r\n                <defs>\r\n                  <style>\r\n                    .cls-1 {\r\n                      fill: none;\r\n                      stroke: #aea3d8;\r\n                      stroke-miterlimit: 10;\r\n                      stroke-width: 10px;\r\n                    }\r\n                  </style>\r\n                </defs>\r\n                <circle class=\"cls-1\" cx=\"28\" cy=\"28\" r=\"23\"/>\r\n              </svg>\r\n            <span class=\"" + nodeClass + "\" id=\"" + id + "\">"+ name +"</span>\r\n        </div>";

            return result;
        }

        private string BuildEdgeContent(IEdge edge)
        {
            string edgeType = "orientedSimple";
            string edgeContent = "<svg class=\"edge\" id=\"_Слой_1\" data-name=\"Слой 1\" xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 500 44.25\">\r\n            <defs>\r\n              <style>\r\n                .cls-1 {\r\n                  fill: #9370db;\r\n                  stroke-width: 0px;\r\n                }\r\n              </style>\r\n            </defs>\r\n            <path class=\"cls-1\" d=\"m496.52,16.51l-31.81-15.82c-5.36-2.67-11.05,2.97-8.44,8.36l3.9,8.03H5c-2.76,0-5,2.24-5,5s2.24,5,5,5h455.23l-3.95,8.14c-2.62,5.39,3.08,11.03,8.44,8.36l31.81-15.82c4.64-2.31,4.64-8.93,0-11.24Z\"/>\r\n          </svg>";
            if (edge is OrientedEdge && (edge as OrientedEdge)._isPencil)
            {
                edgeContent = "<svg class=\"edge\" id=\"_Слой_1\" data-name=\"Слой 1\" xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 497.97 40.61\">\r\n            <defs>\r\n              <style>\r\n                .cls-1 {\r\n                  fill: #aea3d8;\r\n                  stroke-width: 0px;\r\n                }\r\n          \r\n                .cls-2 {\r\n                  fill: none;\r\n                  stroke: #aea3d8;\r\n                  stroke-miterlimit: 10;\r\n                  stroke-width: 11px;\r\n                }\r\n              </style>\r\n            </defs>\r\n            <path class=\"cls-1\" d=\"m465.59.63l29.19,14.52c4.26,2.12,4.26,8.19,0,10.31l-29.19,14.52c-4.92,2.45-10.15-2.73-7.74-7.67l4.61-9.49c.77-1.59.77-3.44,0-5.03l-4.61-9.49c-2.4-4.94,2.83-10.12,7.74-7.67Z\"/>\r\n            <rect class=\"cls-2\" x=\"5.5\" y=\"9.43\" width=\"461.78\" height=\"22.37\" rx=\"11.19\" ry=\"11.19\"/>\r\n          </svg>";
                edgeType = "orientedPencil";
            }
            else if (edge is NonOrientedEdge)
            {
                edgeContent = "<svg class=\"edge\" id=\"_Слой_1\" data-name=\"Слой 1\" xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 482 85\">\r\n            <defs>\r\n              <style>\r\n                .cls-1 {\r\n                  fill: none;\r\n                  stroke: #aea3d8;\r\n                  stroke-miterlimit: 10;\r\n                  stroke-width: 20px;\r\n                }\r\n              </style>\r\n            </defs>\r\n            <rect class=\"cls-1\" x=\"10\" y=\"10\" width=\"462\" height=\"65\" rx=\"32.5\" ry=\"32.5\"/>\r\n          </svg>";
                edgeType = "nonOriented";
            }
            edgeContent += "<span class=\"relationName\" class=\"" + edgeType + "\" id=\"" + (edge as Edge).ToString() + "\">" + (edge as Edge).Name + "</span>";
            return edgeContent;
        }

        private string BuildBody()
        {
            string body = "";
            foreach (IEdge edge in Edges)
            {
                body += "<div class=\"relation\">";
                body += BuildNodeContent((edge as Edge).SecondNode, true);
                body += BuildEdgeContent((edge as Edge));
                body += BuildNodeContent((edge as Edge).FirstNode, false);
                body += "</div>\r\n";
            }
            return body;
        }

        private const double defaultDpi = 300;

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

        private static BitmapSource GetRenderTargetBitmapFromControl(Visual targetControl, double dpi = defaultDpi)
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
