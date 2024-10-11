using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReportCreator_EquipmentModel
{
    public partial class Form2 : Form
    {
        bool[] AI_Column_Bo;
        string[] AI_Column;
        string allContent = "";
        string CalContent = "";
        string FilePath = "";
        DataTable table01 = new DataTable();
        DataTable table02 = new DataTable();

        public Form2(string ContenttoPrint, string CaltoPrint, string path, bool[] aI_Column_Bo, string[] al_col)
        {
            allContent = ContenttoPrint;
            CalContent = CaltoPrint;
            FilePath = path;
            AI_Column_Bo = aI_Column_Bo;
            AI_Column = al_col;
            InitializeComponent();

            TitleContent();

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.DataSource = DataTable_Startup();

            string[] LineContent = allContent.Split('\n');
            foreach (string thisLine in LineContent)
            {
                string[] LineItems = thisLine.Split(',');
                if (LineItems.Count() == AI_Column.Length + 2)
                {
                    if (LineItems[0] == "Bay Name") { continue; }
                    DataTable_AddRow(table01, LineItems);
                }
            }

            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView2.DataSource = DataTable2_Startup();

            string[] CalLineContent = CalContent.Split('\n');
            foreach (string CalLine in CalLineContent)
            {
                string[] thisCalLine = CalLine.Split(',');
                if (thisCalLine.Count() == AI_Column.Length + 2) { DataTable2_AddRow(table02, thisCalLine); }
            }

            for (int i = 0; i < AI_Column.Length; i++)
            {
                dataGridView1.Columns[AI_Column[i]].Visible = AI_Column_Bo[i];
                dataGridView2.Columns[AI_Column[i]].Visible = AI_Column_Bo[i];
            }
        }

        public void TitleContent()
        {
            string[] LineContent = allContent.Split('\n');

            label4.Text = LineContent[2].Split(':')[1].Trim();
            label5.Text = LineContent[0].Split(':')[1].Trim();
            label6.Text = LineContent[1].Split(':')[1].Trim();
        }

        // Setup columns for table 1
        public DataTable DataTable_Startup()
        {
            table01.Columns.Add("Bay Title");
            table01.Columns.Add("Timestamp");

            foreach (string AI_Name in AI_Column)
            {
                table01.Columns.Add(AI_Name);
            }

            return table01;
        }

        // Setup columns for table 2
        public DataTable DataTable2_Startup()
        {
            table02.Columns.Add(" ");
            table02.Columns.Add("  ");

            foreach (string AI_Name in AI_Column)
            {
                table02.Columns.Add(AI_Name);
            }

            return table02;
        }

        // Add row for table 1
        public void DataTable_AddRow(DataTable table, string[] thisItem)
        {
            DataRow row = table.NewRow();
            for (int i = 0; i < AI_Column.Length + 2; i++)
            {
                row[i] = thisItem[i];
            }

            table.Rows.Add(row);
        }

        // Add row for table 2
        public void DataTable2_AddRow(DataTable table, string[] thisItem)
        {
            DataRow row = table.NewRow();
            for (int i = 0; i < AI_Column.Length + 2; i++)
            {
                row[i] = thisItem[i];
            }

            table.Rows.Add(row);
        }

        // Export csv file
        private void Print_Close_Click(object sender, EventArgs e)
        {
            string FullPath = textBox1.Text + "//" + FilePath;
            string OverallContent = allContent + CalContent;

            File.WriteAllText(FullPath, OverallContent);
            MessageBox.Show(FullPath + " file created.");

            this.Close();
        }

        private void SearchFolder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }
    }
}
