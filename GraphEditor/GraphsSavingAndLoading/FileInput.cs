using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Controls;

namespace GraphEditor.GraphsSavingAndLoading
{
    internal class FileInput
    {
        public string Content { get; private set; }

        public List<GraphUnit> GraphUnits { get; private set; }

        public FileInput(Canvas canvas)
        {
            GraphUnits = new List<GraphUnit>();


            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PNG Image|*.png";
            saveFileDialog.Title = "Save an Image File";
            DialogResult dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {

                RenderToPNGFile(canvas, saveFileDialog.FileName);
                //OpenFile(saveFileDialog.FileName);
            }
        }

        private const double defaultDpi = 300;

        public static ImageSource RenderToPNGImageSource(Visual targetControl)
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

        public static void RenderToPNGFile(Visual targetControl, string filename)
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
