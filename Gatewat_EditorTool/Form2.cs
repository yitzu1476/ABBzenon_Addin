using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GW_EditorTool
{
    public partial class Form2 : Form
    {
        public bool YN_form2 = false;
        public Form2()
        {
            InitializeComponent();
        }

        private void yes_Click(object sender, EventArgs e)
        {
            YN_form2 = true;
            this.Close();
        }

        private void no_Click(object sender, EventArgs e)
        {
            YN_form2 = false;
            this.Close();
        }
    }
}
