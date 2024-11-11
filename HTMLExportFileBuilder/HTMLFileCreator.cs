using System;
using System.IO;
using System.Reflection;
using Microsoft.CodeAnalysis.Diagnostics;
using RazorLight;

namespace HTMLExportFileBuilder
{    
    public class HTMLFileCreator
    {
        public HTMLFileCreator() 
        { 
            
        }       

        public static async void GenerateHTMLFile()
        {
            string content = "<p>Hello world</p>";
            FileModel fileModel = new FileModel(content);
            var engine = new RazorLightEngineBuilder().UseEmbeddedResourcesProject(typeof(HTMLFileCreator)).UseMemoryCachingProvider().Build();
            string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "FileModel.cshtml");
            string template = File.ReadAllText(templatePath);
            string result = await engine.CompileRenderStringAsync("templateKey", template, fileModel);
            string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "output.html");
            File.WriteAllText(outputPath, result);
        }
    }
}
