using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcExpCore.Proc
{
    public class ModuleView
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string Version { get; set; }
        public string Description { get; set; }
        public string CompanyName { get; set; }
        public string Md5 { get; set; }
        public string Sha1 { get; set; }
        public bool IsTrust
        {
            get; set;
        }
    }
}
