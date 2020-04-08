using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcExpCore.Proc
{
    public class ProcView
    {
        public int Pid { get; set; }
        public string ProcName { get; set; }
        public string FullProcPath { get; set; }
        public bool IsDenied { get; set; }
        public ProcessModuleCollection ProcessModules { get; set; }

        public string ToolTipName()
        {
            string response = "";
            if (this.IsDenied)
            {
                response = string.Format("Process Name: {0} \n Process Path: {1}", this.ProcName, this.FullProcPath);
            }
            else
            {
                response = string.Format("Access is denied. \n Process Name: {0} \n Process Path: {1}", this.ProcName, this.FullProcPath);
            }
            return response;
        }

    }
}
