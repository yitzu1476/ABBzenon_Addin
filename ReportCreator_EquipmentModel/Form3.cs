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
using static ReportCreator_EquipmentModel.Form3;

namespace ReportCreator_EquipmentModel
{
    public partial class Form3 : Form
    {
        string AllContent = "";
        DataTable table01 = new DataTable();
        DataTable table02 = new DataTable();

        List<Calcus> Calculation4 = new List<Calcus>();
        List<AllBay> allBays = new List<AllBay>();
        List<Row_toShow> Rows_toShow = new List<Row_toShow>();
        List<Form1.AI_Content> AI_Items = new List<Form1.AI_Content>();

        public Form3(List<Form1.AI_Content> Main_AI, string thisContent)
        {
            AI_Items = Main_AI;
            AllContent = thisContent;
            InitializeComponent();

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.DataSource = DataTable_Startup();

            AddAllRow(table01);

            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView2.DataSource = DataTable2_Startup();

            Get4Calcus();

            string[] AllContent_Line = AllContent.Split('\n');
            label5.Text = AllContent_Line[0].Split(':')[1].Trim();
            label6.Text = AllContent_Line[1].Split(':')[1].Trim();
            label4.Text = AllContent_Line[2].Split(':')[1].Trim();
            label8.Text = AllContent_Line[3].Split(':')[1].Trim();

        }

        // Setup columns for table 1
        public DataTable DataTable_Startup()
        {
            table01.Columns.Add("Timestamp");

            // Write baytitle to column title
            foreach (var AI_Item in AI_Items)
            {
                string BayTitle = AI_Item.BayTitle;
                if (allBays.Exists(x => x.Show_BayTitle == BayTitle)) { continue; }         // Skip if already written
                else
                {
                    allBays.Add(new AllBay { Show_BayTitle = BayTitle });
                    table01.Columns.Add(BayTitle);
                }
            }

            return table01;
        }

        // Setup for columns for table 2
        public DataTable DataTable2_Startup()
        {
            table02.Columns.Add(" ");
            foreach (var BayT in allBays)
            {
                table02.Columns.Add(BayT.Show_BayTitle);
            }
            return table02;
        }

        // Draw all rows to table 1
        public void AddAllRow(DataTable table)
        {
            foreach (var AI_Item in AI_Items)
            {
                if (Rows_toShow.Exists(x => x.Timestamp == AI_Item.timeStamp)){ continue; }

                DataRow row = table.NewRow();
                string thisTimestamp = Int2date(AI_Item.timeStamp)[0] + " - " + Int2date(AI_Item.timeStamp)[1];
                row["Timestamp"] = thisTimestamp;

                // Write value of all bay to the same row if having the same timestamp
                foreach (var thisBay in allBays)
                {
                    if (AI_Items.Exists(x => (x.timeStamp == AI_Item.timeStamp) && (x.BayTitle == thisBay.Show_BayTitle)))
                    {
                        string thisValue = AI_Items.Find(x => (x.timeStamp == AI_Item.timeStamp) && (x.BayTitle == thisBay.Show_BayTitle)).VarValue;
                        row[thisBay.Show_BayTitle] = thisValue;
                    }
                }

                table.Rows.Add(row);

                Rows_toShow.Add(new Row_toShow { Timestamp = AI_Item.timeStamp });
            }
        }

        // Get calculated summary, average, minimum, maximum value
        public void Get4Calcus()
        {
            foreach (var AI_Item in AI_Items)
            {
                string Str_timestamp = Int2date(AI_Item.timeStamp)[0] + " - " + Int2date(AI_Item.timeStamp)[1];

                // Chekc if table 1 is drawn
                bool TimestampCheck = false;
                foreach (DataGridViewRow Data1_Row in dataGridView1.Rows)
                {
                    if (Data1_Row.Cells[0].Value.ToString() == Str_timestamp) { TimestampCheck = true; break; }
                }

                if (TimestampCheck)
                {
                    float thisVarValue = float.Parse(AI_Item.VarValue);
                    if (Calculation4.Exists(x => x.bayTitle == AI_Item.BayTitle))           // Update calculated value
                    {
                        Calcus thisCal = Calculation4.Find(x => x.bayTitle == AI_Item.BayTitle);

                        int thisCount = thisCal.BayItemCount + 1;

                        float thisSum = thisCal.BaySum + thisVarValue;
                        float thisAve = thisSum / thisCount;

                        thisCal.BaySum = thisSum;
                        thisCal.BayAve = thisAve;
                        if (thisVarValue < thisCal.BayMin) { thisCal.BayMin = thisVarValue; }
                        if (thisVarValue > thisCal.BayMax) { thisCal.BayMax = thisVarValue; }
                        thisCal.BayItemCount = thisCount;
                    }
                    else
                    {
                        // Set value of all datatype as the first value
                        Calculation4.Add(new Calcus
                        {
                            bayTitle = AI_Item.BayTitle,
                            BaySum = thisVarValue,
                            BayAve = thisVarValue,
                            BayMin = thisVarValue,
                            BayMax = thisVarValue,
                            BayItemCount = 1
                        });
                    }
                }
            }

            Draw_table2(table02);
        }

        // Draw to table 2
        public void Draw_table2(DataTable table)
        {
            DataRow row = table.NewRow();
            row[0] = "Minimum";
            foreach (var CalItem in Calculation4)
            {
                row[CalItem.bayTitle] = CalItem.BayMin;
            }
            table.Rows.Add(row);

            DataRow row2 = table.NewRow();
            row2[0] = "Maximum";
            foreach (var CalItem in Calculation4)
            {
                row2[CalItem.bayTitle] = CalItem.BayMax;
            }
            table.Rows.Add(row2);

            DataRow row3 = table.NewRow();
            row3[0] = "Average";
            foreach (var CalItem in Calculation4)
            {
                row3[CalItem.bayTitle] = CalItem.BayAve;
            }
            table.Rows.Add(row3);

            DataRow row4 = table.NewRow();
            row4[0] = "Summary";
            foreach (var CalItem in Calculation4)
            {
                row4[CalItem.bayTitle] = CalItem.BaySum;
            }
            table.Rows.Add(row4);
        }

        // Export csv file
        private void PrintClose_Click(object sender, EventArgs e)
        {
            string titleContent = "Timestamp,";
            foreach (var thisBay in allBays)
            {
                titleContent = titleContent + thisBay.Show_BayTitle + ",";
            }

            AllContent = AllContent + titleContent + "\n";

            foreach (DataGridViewRow thisRow in dataGridView1.Rows)
            {
                string tempContent = "";
                foreach (DataGridViewColumn thisColumn in dataGridView1.Columns)
                {
                    tempContent = tempContent + thisRow.Cells[thisColumn.Name].Value.ToString() + ",";
                }
                AllContent = AllContent + tempContent + "\n";
            }

            foreach (DataGridViewRow thisRow2 in dataGridView2.Rows)
            {
                string tempContent = "";
                foreach (DataGridViewColumn thisColumn2 in dataGridView2.Columns)
                {
                    tempContent = tempContent + thisRow2.Cells[thisColumn2.Name].Value.ToString() + ",";
                }
                AllContent = AllContent + tempContent + "\n";
            }

            string FilePath = DateTime.Now.ToString("yyyyMMddHHmmss") + "_Report.csv";
            string FullPath = textBox1.Text + "//" + FilePath;

            File.WriteAllText(FullPath, AllContent);
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


        public string[] Int2date(Int32 datetime_int)
        {
            string date_DT = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(datetime_int).AddHours(8).ToShortDateString();
            string time_DT = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(datetime_int).AddHours(8).ToLongTimeString();
            string[] datetime_result = { date_DT, time_DT };
            return datetime_result;
        }


        public class AllBay
        {
            public string Show_BayTitle { get; set; }
        }

        public class Row_toShow
        {
            public int Timestamp { get; set; }

        }

        public class Calcus
        {
            public string bayTitle { get; set; }
            public float BaySum { get; set; }
            public float BayAve { get; set; }
            public float BayMin { get; set; }
            public float BayMax { get; set; }
            public int BayItemCount { get; set; }
        }
    }
}
