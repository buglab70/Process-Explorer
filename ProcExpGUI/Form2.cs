using ProcExpCore.Proc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProcExpGUI
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public Form2(ModuleView module)
        {
            InitializeComponent();
            label1.Text = module.Name;
            textBox1.Text = module.Path;
            textBox2.Text = module.Version;
            textBox3.Text = module.Description;
            textBox4.Text = module.CompanyName;
            textBox5.Text = module.Md5;
            textBox6.Text = module.Sha1;
            textBox7.Text = (module.IsTrust) ? "Trusted" : "Untrusted" ;
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            
        }
    }
}
