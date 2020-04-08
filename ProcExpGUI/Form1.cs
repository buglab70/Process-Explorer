using Microsoft.Win32;
using ProcExpCore.Infrastructure.Messages;
using ProcExpCore.Proc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProcExpGUI
{
    public partial class Form1 : Form
    {
        private FormView formView;
        public Form1()
        {
            InitializeComponent();
            tableLayoutPanel1.Dock = DockStyle.Fill;
            listView3.Dock = DockStyle.Fill;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadProcesses();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView1_Click(object sender, EventArgs e)
        {
            Response<ProcView> procView = null;
            ListViewItem lv = null;
            try
            {
                ListViewItem item = listView1.SelectedItems[0];
                Processes proc = new Processes();
                int id = Convert.ToInt32(item.SubItems[1].Text);
                procView = proc.ProcessById(id);
                if (procView.Success)
                {
                    listView2.Items.Clear();
                    listView2.Columns[0].Text = string.Format("Modules: {0}", procView.Value.ProcessModules.Count);
                    for (int i = 0; i < procView.Value.ProcessModules.Count; i++)
                    {
                        lv = new ListViewItem(procView.Value.ProcessModules[i].ModuleName);
                        lv.SubItems.Add(procView.Value.ProcessModules[i].FileVersionInfo.FileDescription);
                        lv.SubItems.Add(procView.Value.ProcessModules[i].FileVersionInfo.CompanyName);
                        lv.SubItems.Add(procView.Value.ProcessModules[i].FileName);
                        listView2.Items.Add(lv);
                    }
                }
                else
                {
                    listView2.Columns[0].Text = "Modules";
                    listView2.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void listView2_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                string modulePath = listView2.SelectedItems[0].SubItems[3].Text;
                Processes proc = new Processes();
                Response<ModuleView> response = proc.GetModuleInfo(modulePath);
                if (response.Success)
                {
                    Form2 form2 = new Form2(response.Value);
                    form2.Show();
                }else
                {
                    MessageBox.Show(response.Errors.FirstOrDefault().Message);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tableLayoutPanel1.Visible = false;
            listView3.Items.Clear();
            listView3.Location = new Point(0, 31);
            listView3.Height = 358;
            listView3.Width = 895;
            listView3.Columns[0].Width = 396;
            listView3.Columns[1].Width = 456;
            listView3.Visible = true;

            ListViewItem lv = null;
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");

            foreach (var item in key.GetValueNames())
            {
                lv = new ListViewItem(item);
                lv.SubItems.Add(key.GetValue(item).ToString());
                listView3.Items.Add(lv);
            }

        }

        private void LoadProcesses()
        {
            formView = FormView.Process;
            groupBox1.Text = "Processes";
            tableLayoutPanel1.Visible = true;
            listView3.Visible = false;
            listView1.Items.Clear();
            Processes proc = new Processes();
            List<ProcView> response = null;
            ListViewItem lv1 = null;
            try
            {
                response = proc.GetAllProcesses().Value;
                groupBox1.Text = string.Format("Processes {0}", response.Count);
                foreach (var item in response)
                {
                    lv1 = new ListViewItem(item.ProcName);
                    lv1.SubItems.Add(item.Pid.ToString());
                    lv1.SubItems.Add(item.FullProcPath);
                    lv1.ToolTipText = item.ToolTipName();
                    if (!item.IsDenied)
                    {
                        Color colour = ColorTranslator.FromHtml("#FF7F7F");
                        Color transparent = Color.FromArgb(255, colour);
                        lv1.BackColor = transparent;
                    }
                    listView1.Items.Add(lv1);
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }

        private void LoadAutoRun()
        {
            tableLayoutPanel1.Visible = false;
            listView3.Items.Clear();
            listView3.Location = new Point(0, 31);
            listView3.Height = 358;
            listView3.Width = 895;
            listView3.Columns[0].Width = 396;
            listView3.Columns[1].Width = 456;
            listView3.Visible = true;

            ListViewItem lv = null;
            RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
            groupBox1.Text = string.Format("Autorun ({0})", key.ValueCount);
            foreach (var item in key.GetValueNames())
            {
                lv = new ListViewItem(item);
                lv.SubItems.Add(key.GetValue(item).ToString());
                lv.ToolTipText = string.Format("Name: {0}", item);
                listView3.Items.Add(lv);
            }
        }

        private void closeProcessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(listView1.SelectedItems.Count != 0)
            {
                Processes pr = new Processes();
                ListViewItem item = listView1.SelectedItems[0];
                Response<int> response = pr.KillProcess(Convert.ToInt32(item.SubItems[1].Text));
                if (response.Success)
                {
                    MessageBox.Show("Process killed: "+ response.Value.ToString());
                    LoadProcesses();
                }
                else
                {
                    MessageBox.Show("Error occured: "+ response.Errors.FirstOrDefault().Message.ToString(), response.Errors.FirstOrDefault().Message.ToString());
                }
            }
        }

        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            //if (MouseButtons.Left == MouseButtons)
            //{
            //    MessageBox.Show("asdasd");
            //}
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void listToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadProcesses();
        }

        private void dvsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            formView = FormView.Autorun;
            LoadAutoRun();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(formView == FormView.Process)
            {
                LoadProcesses();
            }
            else
            {
                LoadAutoRun();
            }
        }
    }
}
