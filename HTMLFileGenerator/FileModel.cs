using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTMLFileGenerator
{
    public class FileModel
    {
        public string Content { get; set; }

        public FileModel(string content)
        {
            Content = content;
        }
    }
}
