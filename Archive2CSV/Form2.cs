using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Archive2CSV
{
    public partial class Form2 : Form
    {
        DataTable table01 = new DataTable();

        public Form2(string csvContent, string Dura, string Cate)
        {
            InitializeComponent();

            label2.Text = Dura;
            label4.Text = Cate;

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.DataSource = DataTable_Startup();

            string AllContent = csvContent;
            string[] csvEachRow = AllContent.Split('\n');
            foreach (string eachRow in csvEachRow)
            {
                string[] thisRow = eachRow.Split(',');
                if (thisRow.Length != 4) { continue; }
                if (thisRow[0] == "Time") { continue; }

                DataTable_AddRow(table01, thisRow[0], thisRow[1], thisRow[2], thisRow[3]);
            }
        }

        public DataTable DataTable_Startup()
        {
            table01.Columns.Add("Timestamp");
            table01.Columns.Add("Archive Name");
            table01.Columns.Add("Variable Name");
            table01.Columns.Add("Value");

            return table01;
        }

        public void DataTable_AddRow(DataTable table, string TimeS, string ArchiveN, string VariableN, string VarValue)
        {
            DataRow row = table.NewRow();
            row["Timestamp"] = TimeS;
            row["Archive Name"] = ArchiveN;
            row["Variable Name"] = VariableN;
            row["Value"] = VarValue;

            table.Rows.Add(row);
        }
    }
}
