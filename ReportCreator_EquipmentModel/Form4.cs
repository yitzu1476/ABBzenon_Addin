using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReportCreator_EquipmentModel
{
    public partial class Form4 : Form
    {
        string[] All_Column;
        public bool[] All_Column_Bool;

        public Form4(string[] main_column, bool[] main_column_Bool)
        {
            All_Column = main_column;
            All_Column_Bool = main_column_Bool;
            InitializeComponent();

            int Itemcount = 0;
            foreach (string column in All_Column)
            {
                checkedListBox1.Items.Add(column);
                checkedListBox1.SetItemChecked(Itemcount, true);

                Itemcount = Itemcount + 1;
            }
        }

        // Set false to unchecked item
        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                All_Column_Bool[i] = checkedListBox1.GetItemChecked(i);
            }
            this.Close();
        }
    }
}
