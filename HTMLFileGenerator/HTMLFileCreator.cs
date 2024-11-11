using RazorLight;

namespace HTMLFileGenerator
{
    internal class HTMLFileCreator
    {

        public static async Task<string> GenerateHtmlFromModel(FileModel model)
        {
            var engine = new RazorLightEngineBuilder()
                .UseFileSystemProject(Directory.GetCurrentDirectory())
                .UseMemoryCachingProvider()
                .Build();

            string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "FileComponent.razor");
            string template = File.ReadAllText(templatePath);
            string result = await engine.CompileRenderStringAsync("templateKey", template, model);

            return result;
        }
    }
}


