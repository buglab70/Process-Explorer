using ProcExpCore.Infrastructure.Messages;
using ProcExpCore.Infrastructure.Validation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProcExpCore.Proc
{
    public class Processes
    {
        public Response<List<ProcView>> GetAllProcesses()
        {
            Process[] allProcesses = null;
            List<ProcView> procView = new List<ProcView>();
            Response<List<ProcView>> response = null;
            ProcView temp = null;
            try
            {
                allProcesses = Process.GetProcesses();
                foreach (var item in allProcesses)
                {
                    temp = new ProcView();
                    temp.ProcName = item.ProcessName;
                    temp.Pid = item.Id;
                    temp.FullProcPath = GetMainModuleFilepath(item.Id);
                    temp.IsDenied = IsDenied(item);
                    procView.Add(temp);
                }
                response = new Response<List<ProcView>>(procView.OrderBy(x => x.Pid).ToList());
            }
            catch(Exception ex)
            {
                return new Response<List<ProcView>>(new Error("Error occured! " + ex.Message));
            }


            return response;
        }

        private bool IsDenied(Process proc)
        {
            try
            {
                    return !proc.HasExited;
            }
            catch
            {
                return false;
            }
        }

        public Response<ProcView> ProcessById(int id)
        {
            Process proc = new Process();
            ProcView procView = new ProcView();
            try
            {
                proc = Process.GetProcessById(id);
                procView.Pid = id;
                procView.ProcName = proc.ProcessName;
                procView.ProcessModules = proc.Modules;
                procView.FullProcPath = proc.MainModule.FileName;
                return new Response<ProcView>(procView);
            }
            catch(Exception ex)
            {
                return new Response<ProcView>(new Error("Error occured! "+ ex.Message));
            }
            
        }

        public Response<int> KillProcess(int id)
        {
            try
            {
                Process proc = Process.GetProcessById(id);
                proc.Kill();
                return new Response<int>(id);
            }
            catch (Exception ex)
            {
                return new Response<int>(new Error(ex.Message));
            }
        }
        //Get information about process path
        public string GetMainModuleFilepath(int procId)
        {
            IntPtr op = IntPtr.Zero;
            try
            {
                op = OpenProcess(ProcessAccessFlags.QueryLimitedInformation, false, procId);
                StringBuilder buffer = new StringBuilder(1024);
                int capacity = buffer.Capacity;
                if (QueryFullProcessImageName(op, 0, buffer, ref capacity))
                {
                    return buffer.ToString();
                }
            }
            catch(Exception ex)
            {
                return "n\\a";
            }
            finally
            {
                CloseHandle(op);
            }

            return "n\\a";
        }

        //Get dll hash information
        private void GetModuleHash(string path, out string md5, out string sha1)
        {
            byte[] hashMd5;
            byte[] hashSha1;
            md5 = "";
            sha1 = "";
            try
            {
                
                using (Stream fs = File.OpenRead(path))
                {
                    hashMd5 = MD5.Create().ComputeHash(fs);
                    md5 = BitConverter.ToString(hashMd5).Replace("-", "").ToLowerInvariant();
                }
                using (Stream fs = File.OpenRead(path))
                {
                    hashSha1 = SHA1.Create().ComputeHash(fs);
                    sha1 = BitConverter.ToString(hashSha1).Replace("-", "").ToLowerInvariant();
                }

            }
            catch (Exception ex)
            {
                md5 = ex.Message;
                sha1 = ex.Message;
            }

        }
        //Get full information about dll
        public Response<ModuleView> GetModuleInfo(string modulePath)
        {
            string md5 = "";
            string sha1 = "";
            ModuleView module = new ModuleView();
            try
            {
                GetModuleHash(modulePath, out md5, out sha1);
                FileVersionInfo info = FileVersionInfo.GetVersionInfo(modulePath);
                module.Name = info.FileName;
                module.Path = modulePath;
                module.Version = info.FileVersion;
                module.Description = info.FileDescription;
                module.CompanyName = info.CompanyName;
                module.Md5 = md5;
                module.Sha1 = sha1;
                module.IsTrust = WinTrust.VerifyEmbeddedSignature(modulePath);
            }
            catch(Exception ex)
            {
                return new Response<ModuleView>(new Error(ex.Message));
            }
            return new Response<ModuleView>(module);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(ProcessAccessFlags processAccess, bool bInheritHandle, int processId);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool QueryFullProcessImageName(IntPtr hProcess, int dwFlags, StringBuilder lpExeName, ref int lpdwSize);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool CloseHandle(IntPtr hObject);
    }
}
