using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gateway_EditorTool
{
    public partial class Form3 : Form
    {
        public bool updateAll_Bool = false;

        public Form3()
        {
            InitializeComponent();
        }

        private void Update_All_Click(object sender, EventArgs e)
        {
            updateAll_Bool = true;
            this.Close();
        }

        private void Update_DB_Click(object sender, EventArgs e)
        {
            updateAll_Bool = false;
            this.Close();
        }
    }
}
