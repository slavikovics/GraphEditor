using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphEditor.GraphsSavingAndLoading
{
    internal class GraphUnit
    {
        public string FirstNodeName { get; set; }

        public string RelationName { get; set; }

        public string RelationType { get; set; }

        public string SecondNodeName { get; set; }    
        
        public GraphUnit() 
        { 
        }
    }
}
