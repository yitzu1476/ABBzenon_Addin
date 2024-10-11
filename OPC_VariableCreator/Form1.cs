using Scada.AddIn.Contracts;
using Scada.AddIn.Contracts.Variable;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace OPC_VariableCreator
{
    public partial class Form1 : Form
    {
        IProject thisProject;

        List<StationClass> Station_List = new List<StationClass>();
        List<PointClass> Point_List = new List<PointClass>();
        List<SheetClass> Sheet_List = new List<SheetClass>();
        List<VarClass> Var_List = new List<VarClass>();
        List<SheetHeaderClass> SheetHeader_List = new List<SheetHeaderClass>();

        public Form1(IProject fromProject)
        {
            InitializeComponent();
            textBox1.Text = "C:\\Users\\TWJOHUA1\\Joyce - Work\\016 PSMC P3\\PSMC P3 SYSCON_zenon_0627.xlsx";

            thisProject = fromProject;

            listBox1.SelectedIndexChanged += ListBox1_Changed;
        }

        private void ListBox1_Changed(object sender, EventArgs e)
        {
            richTextBox1.AppendText("Collecting sheet header...\n");
            richTextBox1.ScrollToCaret();

            string path = textBox1.Text;
            SheetHeader_List.Clear();

            Excel.Application XLapp = new Excel.Application();
            Excel.Workbook XLworkBook = XLapp.Workbooks.Open(path);

            bool HeaderFit = true;
            foreach (string thisSheetName in listBox1.SelectedItems)
            {
                Excel._Worksheet workSheet_page = XLworkBook.Sheets[thisSheetName];
                Excel.Range this_XLrange = workSheet_page.UsedRange;
                int columneCNT = this_XLrange.Columns.Count;

                if (SheetHeader_List.Count == 0)
                {
                    for (int k = 1; k < columneCNT + 1; k++)
                    {
                        SheetHeader_List.Add(new SheetHeaderClass
                        {
                            ColumnIndex = k,
                            ColumnName = workSheet_page.Cells[1, k].Text
                        });
                    }
                }
                else
                {
                    for (int k = 1; k < columneCNT + 1; k++)
                    {
                        string HeaderContent = workSheet_page.Cells[1, k].Text;

                        var TempHeader = SheetHeader_List.Find(x => x.ColumnName == HeaderContent);
                        if (TempHeader == null)
                        {
                            HeaderFit = false;
                            break;
                        }
                        if (TempHeader.ColumnIndex != k)
                        {
                            HeaderFit = false;
                            break;
                        }
                    }
                }


                Marshal.ReleaseComObject(workSheet_page);
            }


            XLworkBook.Close();
            Marshal.ReleaseComObject(XLworkBook);
            XLapp.Quit();
            Marshal.ReleaseComObject(XLapp);

            listBox2.Items.Clear();
            comboBox_DeviceName.Items.Clear();
            comboBox_DeviceDescription.Items.Clear();
            comboBox_DeviceStationNum.Items.Clear();
            comboBox_VariableName.Items.Clear();
            comboBox_VariableDescription.Items.Clear();
            comboBox_VariableDataType.Items.Clear();
            comboBox_SymbolicAdd.Items.Clear();
            comboBox_VariableDataLen.Items.Clear();

            comboBox_DeviceName.Text = "";
            comboBox_DeviceDescription.Text = "";
            comboBox_DeviceStationNum.Text = "";
            comboBox_VariableName.Text = "";
            comboBox_VariableDescription.Text = "";
            comboBox_VariableDataType.Text = "";
            comboBox_SymbolicAdd.Text = "";
            comboBox_VariableDataLen.Text = "";


            if (HeaderFit == true)
            {
                
                foreach (var Header_item in SheetHeader_List)
                {
                    listBox2.Items.Add(Header_item.ColumnName);
                    comboBox_DeviceName.Items.Add(Header_item.ColumnName);
                    comboBox_DeviceDescription.Items.Add(Header_item.ColumnName);
                    comboBox_DeviceStationNum.Items.Add(Header_item.ColumnName);
                    comboBox_VariableName.Items.Add(Header_item.ColumnName);
                    comboBox_VariableDescription.Items.Add(Header_item.ColumnName);
                    comboBox_VariableDataType.Items.Add(Header_item.ColumnName);
                    comboBox_SymbolicAdd.Items.Add(Header_item.ColumnName);
                    comboBox_VariableDataLen.Items.Add(Header_item.ColumnName);
                }

                richTextBox1.AppendText("Please select column for SPA content.\n");
                richTextBox1.ScrollToCaret();
            }
            else
            {
                listBox2.Items.Add("Sheet header error.");

                richTextBox1.AppendText("Sheet header error, please re-select new sheet name.\n");
                richTextBox1.ScrollToCaret();
            }
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

            if (File.Exists(textBox1.Text) != true)
            {
                richTextBox1.AppendText("Excel file error...\n");
                richTextBox1.ScrollToCaret();
                return;
            }

            richTextBox1.AppendText("Start getting sheets...\n");
            richTextBox1.ScrollToCaret();

            string path = textBox1.Text;

            Excel.Application XLapp = new Excel.Application();
            Excel.Workbook XLworkBook = XLapp.Workbooks.Open(path);

            int ExcelSheet_cnt = XLworkBook.Worksheets.Count;
            for (int i = 1; i < ExcelSheet_cnt + 1; i++)
            {
                Excel._Worksheet workSheet_page = XLworkBook.Sheets[i];

                string page_name = workSheet_page.Name;
                Sheet_List.Add(new SheetClass
                {
                    SheetName = page_name,
                });

                Marshal.ReleaseComObject(workSheet_page);
            }


            XLworkBook.Close();
            Marshal.ReleaseComObject(XLworkBook);
            XLapp.Quit();
            Marshal.ReleaseComObject(XLapp);

            listBox1.Items.Clear();

            foreach (var item in Sheet_List)
            {
                listBox1.Items.Add(item.SheetName);
            }

            richTextBox1.AppendText("-------- End of Operation. --------\n");
            richTextBox1.AppendText("\n");
            richTextBox1.ScrollToCaret();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int DeviceName_Ind = SheetHeader_List.Find(x => x.ColumnName == comboBox_DeviceName.Text).ColumnIndex;
            int DeviceDescription_Ind = SheetHeader_List.Find(x => x.ColumnName == comboBox_DeviceDescription.Text).ColumnIndex;
            int DeviceStationNum_Ind = SheetHeader_List.Find(x => x.ColumnName == comboBox_DeviceStationNum.Text).ColumnIndex;
            int VariableName_Ind = SheetHeader_List.Find(x => x.ColumnName == comboBox_VariableName.Text).ColumnIndex;
            int VariableDescription_Ind = SheetHeader_List.Find(x => x.ColumnName == comboBox_VariableDescription.Text).ColumnIndex;
            int VariableDatatype_Ind = SheetHeader_List.Find(x => x.ColumnName == comboBox_VariableDataType.Text).ColumnIndex;
            int SymbolicAddress_Ind = SheetHeader_List.Find(x => x.ColumnName == comboBox_SymbolicAdd.Text).ColumnIndex;
            int VariableLen_Ind = SheetHeader_List.Find(x => x.ColumnName == comboBox_VariableDataLen.Text).ColumnIndex;

            Sheet_List.Clear();

            richTextBox1.AppendText("Start collecting points...\n");
            richTextBox1.ScrollToCaret();

            string StationText = "";
            string PointText = "";

            //int Station_ind = 0;

            string path = textBox1.Text;


            foreach (string item_sheet in listBox1.SelectedItems)
            {
                Sheet_List.Add(new SheetClass { SheetName = item_sheet, });
            }


            Excel.Application XLapp = new Excel.Application();
            Excel.Workbook XLworkBook = XLapp.Workbooks.Open(path);

            foreach (var sheet_ListItem in Sheet_List)
            {
                string thisSheetName = sheet_ListItem.SheetName;

                Excel._Worksheet workSheet_page = XLworkBook.Sheets[thisSheetName];
                Excel.Range this_XLrange = workSheet_page.UsedRange;
                int rowCNT = this_XLrange.Rows.Count;

                // Collect stations
                for (int i = 2; i < rowCNT + 1; i++)
                {
                    string thisStationName = workSheet_page.Cells[i, DeviceName_Ind].Text;
                    if (thisStationName.Length < 3) { continue; }

                    string thisStationDescription = workSheet_page.Cells[i, DeviceDescription_Ind].Text;
                    int thisStationIndex = int.Parse(workSheet_page.Cells[i, DeviceStationNum_Ind].Text);

                    if (thisStationName.Length < 3) { continue; }

                    bool Station_exist = false;
                    foreach (var station in Station_List)
                    {
                        if (station.StationName == thisStationName) { Station_exist = true; }
                    }

                    if (Station_exist == false && thisStationDescription.Length > 0)
                    {
                        Station_List.Add(new StationClass
                        {
                            StationName = thisStationName,
                            StationDescription = thisStationDescription,
                            StationIndex = thisStationIndex
                        });
                    }

                }

                // Collect points
                for (int j = 2; j < rowCNT + 1; j++)
                {
                    string thisStationName = workSheet_page.Cells[j, DeviceName_Ind].Text;
                    if (thisStationName.Length < 3) { continue; }

                    string thisVariableName = workSheet_page.Cells[j, VariableName_Ind].Text;
                    if (thisVariableName.Length < 5) { continue; }

                    int lengthInt = int.Parse(workSheet_page.Cells[j, VariableLen_Ind].Text);
                    //if (workSheet_page.Cells[j, 12].Text != workSheet_page.Cells[j, 13].Text) { lengthInt = 2; }

                    string AllAddress = workSheet_page.Cells[j, SymbolicAddress_Ind].Text;
                    string[] SplitAddress = AllAddress.Split(':');
                    string smartAddress = SplitAddress[1] + ":" + SplitAddress[2] + ":" + SplitAddress[3];

                    Point_List.Add(new PointClass
                    {
                        DeviceName = workSheet_page.Cells[j, DeviceName_Ind].Text,
                        StartAddress = smartAddress,
                        LengthInt = lengthInt
                    });

                    string thisDataType = "BOOL";

                    if (lengthInt == 2) { thisDataType = "UDINT"; }
                    if (workSheet_page.Cells[j, VariableDatatype_Ind].Text == "AI") { thisDataType = "REAL"; }

                    Var_List.Add(new VarClass
                    {
                        VariableName = thisVariableName,
                        VariableDescription = workSheet_page.Cells[j, DeviceDescription_Ind].Text,
                        VariableSymbolicAdd = AllAddress,
                        VariableDataType = thisDataType
                    });
                }

                Marshal.ReleaseComObject(workSheet_page);
            }

            XLworkBook.Close();
            Marshal.ReleaseComObject(XLworkBook);
            XLapp.Quit();
            Marshal.ReleaseComObject(XLapp);


            foreach (var item in Station_List)
            {
                string StationItem = item.StationName + ",1," + item.StationDescription + ",BASIC," + item.StationIndex.ToString() + ",65535,,,,,,,,,,,,,,";

                StationText = StationText + StationItem + "\n";
            }

            File.WriteAllText("C:\\Temp\\StationContent.txt", StationText);
            richTextBox1.AppendText("C:\\Temp\\StationContent.txt file created.\n");
            richTextBox1.ScrollToCaret();


            int point_cnt = 0;
            foreach (var point in Point_List)
            {
                point_cnt = point_cnt + 1;

                string PointItem = point_cnt + "," + point.DeviceName + ",Unsigned,3,300,Disabled,0," + point.StartAddress + "," + point.LengthInt + ",,,,,,,,,,,";
                PointText = PointText + PointItem + "\n";
            }

            File.WriteAllText("C:\\Temp\\PointContent.txt", PointText);
            richTextBox1.AppendText("C:\\Temp\\PointContent.txt file created.\n");
            richTextBox1.ScrollToCaret();


            // zenon variable
            IDriverCollection driverCollection = thisProject.DriverCollection;
            IDriver thisDriver = driverCollection[0];
            foreach (IDriver driver in driverCollection)
            {
                if (driver.Name == "OPC2CLI32") { thisDriver = driver; }
            }

            IDataTypeCollection datatypeCollection = thisProject.DataTypeCollection;
            IVariableCollection variableCollection = thisProject.VariableCollection;

            foreach (var var_item in Var_List)
            {
                if (variableCollection[var_item.VariableName] == null)
                {
                    variableCollection.Create(var_item.VariableName, thisDriver, ChannelType.PlcMarker, datatypeCollection[var_item.VariableDataType]);
                    richTextBox1.AppendText("Variable " + var_item.VariableName + " created.\n");
                    richTextBox1.ScrollToCaret();
                }

                IVariable thisVar = variableCollection[var_item.VariableName];

                if (thisVar.Driver.Name != "OPC2CLI32")
                {
                    richTextBox1.AppendText("Variable " + var_item.VariableName + " driver error.\n");
                    richTextBox1.ScrollToCaret();
                    continue;
                }

                thisVar.Identification = var_item.VariableDescription;
                thisVar.SetDynamicProperty("SymbAddr", var_item.VariableSymbolicAdd);

                richTextBox1.AppendText("Variable " + var_item.VariableName + " modified.\n");
                richTextBox1.ScrollToCaret();
            }

            richTextBox1.AppendText("-------- End of Operation. --------\n");
            richTextBox1.ScrollToCaret();
        }


        public class SheetClass
        {
            public string SheetName { get; set; }
        }

        public class StationClass
        {
            public string StationName { get; set; }
            public string StationDescription { get; set; }
            public int StationIndex { get; set; }
        }

        public class PointClass
        {
            public int PointIndex { get; set; }
            public string DeviceName { get; set; }
            public string StartAddress { get; set; }
            public int LengthInt { get; set; }
            public string VariableName { get; set; }
        }

        public class VarClass
        {
            public string VariableName { get; set;}
            public string VariableDescription { get; set; }
            public string VariableSymbolicAdd { get; set; }
            public string VariableDataType { get; set; }

        }

        public class SheetHeaderClass
        {
            public int ColumnIndex { get; set; }
            public string ColumnName { get; set; }

        }
        
    }
}
