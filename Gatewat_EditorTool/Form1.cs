using Gateway_EditorTool;
using Microsoft.Office.Interop.Excel;
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
using System.Xml;
using static GW_EditorTool.EngineeringStudioWizardExtension;
using Excel = Microsoft.Office.Interop.Excel;

namespace GW_EditorTool
{
    public partial class Form1 : Form
    {
        IProject thisProject;
        List<IED_Driver_Key> AllIED = new List<IED_Driver_Key>();
        List<Station_List> Allstations = new List<Station_List>();
        List<PointList> DI_Points = new List<PointList>();
        List<PointList> CO_Points = new List<PointList>();
        List<PointList> AI_Points = new List<PointList>();
        List<PointList> ACC_Points = new List<PointList>();


        string path = "";
        string SavingPath = "";

        Creator_Class ItemCreator;
        StationCreator StationCreator;
        ReadBackXML XMLReader;
        Panel02_Driver Panel02_DriverPage;
        Panel03_ToDelete Panel03_DeleteItem;
        Panel04_GWfunctions Panel04_GWFun_Creations;

        public Form1()
        {
            InitializeComponent();
            thisProject = EngineeringStudioWizardExtension.thisProject;

            textBox1.Text = "C:\\Temp\\GW_PointList_Template.xlsx";
            textBox2.Text = "C:\\Temp\\GW_PointList_AddPoints.xlsx";

            ItemCreator = new Creator_Class(thisProject, richTextBox1);
            StationCreator = new StationCreator(DI_Points, CO_Points, AI_Points, ACC_Points, thisProject, ItemCreator, richTextBox1);
            XMLReader = new ReadBackXML(thisProject, richTextBox1, listBox1, Allstations);

            path = textBox1.Text;
            SavingPath = path.Substring(0, path.Length - 27) + "\\GW_PointList";

            Panel01_active();   
        }

        private void CollectGW_PointList_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

            path = textBox1.Text;
            if (File.Exists(path) == false)
            {
                richTextBox1.AppendText("Template File Path Error.");
                richTextBox1.ScrollToCaret();
                return;
            }

            SavingPath = path.Substring(0, path.Length - 27) + "\\GW_PointList";
            Directory.CreateDirectory(SavingPath);

            // If excel files are existed in target folder, show Form2 for conformation
            bool XLSXF = false;
            foreach (string file in Directory.GetFiles(SavingPath))
            {
                if (file.Substring(file.Length - 4, 4) == "xlsx") { XLSXF = true; }
            }

            if (XLSXF)
            {
                Form2 YNdia = new Form2();
                YNdia.Activate();
                YNdia.ShowDialog();

                if (YNdia.YN_form2 == false)
                {
                    richTextBox1.AppendText("Operation cancelled.\n");
                    richTextBox1.ScrollToCaret();
                    return;
                }
            }


            CollectDriver();

            richTextBox1.AppendText("Creating excel files...\n");
            richTextBox1.ScrollToCaret();

            Excel.Application XLapp = new Excel.Application();
            Excel.Workbook XLworkBook = XLapp.Workbooks.Open(path);

            // Collect all points in the template excel, and create variable in editor
            StationCreator.TemplateVariableCM(XLworkBook, AllIED);


            // Station setting
            Allstations.Clear();

            Excel._Worksheet workSheet_station = XLworkBook.Worksheets["Station_Setting"];

            for (int i = 12; i < 32; i++)
            {
                string thisStationName = workSheet_station.Cells[i, 1].Text;
                string thisChannel = workSheet_station.Cells[i, 2].Text;

                string[] tempStation = new string[12];
                for (int j = 1; j < 12; j++) { tempStation[j] = workSheet_station.Cells[i, j].Text; }

                if (thisStationName != "")
                {
                    if (thisStationName != "TCP_template" && thisStationName != "Serial_template")
                    {
                        if (thisChannel == "TCP/IP")
                        {
                            if (tempStation[7] == "" || tempStation[8] == "" || tempStation[9] == "" || tempStation[10] == "" || tempStation[11] == "")
                            {
                                richTextBox1.AppendText(thisStationName + " setting error.\n");
                                richTextBox1.ScrollToCaret();
                                continue;
                            }
                            
                            Allstations.Add(new Station_List
                            {
                                StationName = workSheet_station.Cells[i, 1].Text,
                                Channel = workSheet_station.Cells[i, 2].Text,
                                TPCListeningPort = int.Parse(workSheet_station.Cells[i, 7].Text),
                                NetworkCard = workSheet_station.Cells[i, 8].Text,
                                OutstationAdd = int.Parse(workSheet_station.Cells[i, 9].Text),
                                MasterAdd = int.Parse(workSheet_station.Cells[i, 10].Text),
                                MasterIP = workSheet_station.Cells[i, 11].Text
                            });

                            createStationExcel(thisStationName, i);
                            listBox1.Items.Add(workSheet_station.Cells[i, 1].Text);
                        }

                        if (thisChannel == "Serial")
                        {
                            if (tempStation[3] == "" || tempStation[4] == "" || tempStation[5] == "" || tempStation[6] == "" || tempStation[9] == "" || tempStation[10] == "")
                            {
                                richTextBox1.AppendText(thisStationName + " setting error.\n");
                                richTextBox1.ScrollToCaret();
                                continue;
                            }

                            Allstations.Add(new Station_List
                            {
                                StationName = workSheet_station.Cells[i, 1].Text,
                                Channel = workSheet_station.Cells[i, 2].Text,
                                SerialPort = workSheet_station.Cells[i, 3].Text,
                                MonitorPort = workSheet_station.Cells[i, 4].Text,
                                BaudRate = int.Parse(workSheet_station.Cells[i, 5].Text),
                                PortSetting = workSheet_station.Cells[i, 6].Text,
                                OutstationAdd = int.Parse(workSheet_station.Cells[i, 9].Text),
                                MasterAdd = int.Parse(workSheet_station.Cells[i, 10].Text)
                            });

                            createStationExcel(thisStationName, i);
                            listBox1.Items.Add(workSheet_station.Cells[i, 1].Text);
                        }
                    }
                }
            }

            Marshal.ReleaseComObject(workSheet_station);

            XLworkBook.Close();
            Marshal.ReleaseComObject(XLworkBook);
            XLapp.Quit();
            Marshal.ReleaseComObject(XLapp);

            richTextBox1.AppendText("-------- End of Operation. --------\n");
            richTextBox1.AppendText("\n");
            richTextBox1.ScrollToCaret();
        }

        private void GW_XML_INI_Creator_Click(object sender, EventArgs e)
        {
            path = textBox1.Text;
            if (File.Exists(path) == false)
            {
                richTextBox1.AppendText("Template File Path Error.");
                richTextBox1.ScrollToCaret();
                return;
            }

            SavingPath = path.Substring(0, path.Length - 27) + "\\GW_PointList";
            Directory.CreateDirectory(SavingPath);

            if (listBox1.Items.Count == 0) { Allstations = XMLReader.GetStations(textBox1.Text); }

            Form3 XMlINI_Update = new Form3();
            XMlINI_Update.ShowDialog();
            XMlINI_Update.Activate();

            if (XMlINI_Update.updateAll_Bool == true)
            {
                string[] allFiles = Directory.GetFiles(SavingPath);
                foreach (string file in allFiles)
                {
                    string GW_fileName = file.Substring(SavingPath.Length + 1);
                    if (GW_fileName.Length > 17)
                    {
                        if (GW_fileName.Substring(0, 13) == "GW_PointList_" && GW_fileName.Substring(GW_fileName.Length - 4, 4) == "xlsx")
                        {
                            string station_title = GW_fileName.Substring(13, GW_fileName.Length - 13 - 5);
                            richTextBox1.AppendText("<< " + station_title + " station found >>\n");
                            richTextBox1.ScrollToCaret();

                            richTextBox1.AppendText("Collecting items...\n");
                            richTextBox1.ScrollToCaret();

                            StationCreator.StationCollector(file, station_title, SavingPath, AllIED, Allstations);
                        }
                    }
                }

                richTextBox1.AppendText("-------- End of Operation. --------\n");
                richTextBox1.AppendText("\n");
                richTextBox1.ScrollToCaret();
            }

            if (XMlINI_Update.updateAll_Bool == false)
            {
                if (listBox1.SelectedItems.Count == 0)
                {
                    richTextBox1.AppendText("Please select station from List box.\n");
                    richTextBox1.AppendText("-------- End of Operation. --------\n");
                    richTextBox1.AppendText("\n");
                    richTextBox1.ScrollToCaret();
                }
                else
                {
                    SavingPath = path.Substring(0, textBox1.Text.Length - 27) + "\\GW_PointList";
                    foreach (var Station_Info in listBox1.SelectedItems)
                    {
                        string StationN = Station_Info.ToString();
                        string Excel_file = SavingPath + "\\GW_PointList_" + StationN + ".xlsx";
                        string Xml_file = SavingPath + "\\AccessDNP3_SG-" + StationN + ".xml";

                        StationCreator.XML_DB_Modifier(Excel_file, Xml_file);
                    }

                    richTextBox1.AppendText("-------- End of Operation. --------\n");
                    richTextBox1.AppendText("\n");
                    richTextBox1.ScrollToCaret();
                }
            }

            listBox1.SelectedItems.Clear();
        }

        public void CollectDriver()
        {
            string ProjectID = thisProject.ProjectId;

            string DriverPath = "C:\\ProgramData\\ABB\\SQL2017\\" + ProjectID + "\\FILES\\zenon\\custom\\drivers";
            int DPath_len = DriverPath.Length + 1;

            string[] AllDriverF = Directory.GetFiles(DriverPath);
            foreach (string Dfile in AllDriverF)
            {
                string fileName = Dfile.Substring(DPath_len);
                if (fileName.Length > 8)
                {
                    if (fileName.Substring(0, 7) == "IEC850_")
                    {
                        string txtName = fileName.Substring(7);
                        string driverName = txtName.Substring(0, txtName.Length - 4);

                        if (thisProject.DriverCollection[driverName] == null) { continue; }

                        string fileContent = File.ReadAllText(Dfile);
                        string[] fileLines = fileContent.Split('\n');
                        for (int i = 0; i < fileLines.Length; i++)
                        {
                            if (fileLines[i].Contains("*** SERVER ***"))
                            {
                                int netAddress = int.Parse(fileLines[i + 2]);
                                string technicalKey = fileLines[i + 3];

                                AllIED.Add(new IED_Driver_Key
                                {
                                    DriverFilePath = Dfile,
                                    DriverName = driverName,
                                    NetAddress = netAddress,
                                    TechnicalKey = technicalKey.Trim()
                                });

                            }
                        }
                    }
                }
            }
        }

        public void createStationExcel(string stationName, int stationCell)
        {
            string newFilePath = SavingPath + "\\GW_PointList_" + stationName + ".xlsx";
            File.Copy(path, newFilePath, true);

            richTextBox1.AppendText(newFilePath + " excel file created.\n");
            richTextBox1.ScrollToCaret();
        }

        private void ReadBackXML_Click(object sender, EventArgs e)
        {
            path = textBox1.Text;
            if (File.Exists(path) == false)
            {
                richTextBox1.AppendText("Template File Path Error.");
                richTextBox1.ScrollToCaret();
                return;
            }

            SavingPath = path.Substring(0, textBox1.Text.Length - 27) + "\\GW_PointList";
            CollectDriver();
            XMLReader.AllIED_void(AllIED);
            XMLReader.ReadXMLPoints(path, SavingPath);

        }




        public class IED_Driver_Key
        {
            public string DriverFilePath { get; set; }
            public string DriverName { get; set; }
            public int NetAddress { get; set; }
            public string TechnicalKey { get; set; }
        }

        public class Station_List
        {
            public string StationName { get; set; }
            public string Channel { get; set; }
            public string SerialPort { get; set; }
            public string MonitorPort { get; set; }
            public int BaudRate { get; set; }
            public string PortSetting { get; set; }
            public int TPCListeningPort { get; set; }
            public string NetworkCard { get; set; }
            public int OutstationAdd { get; set; }
            public int MasterAdd { get; set; }
            public string MasterIP { get; set; }
        }

        public class PointList
        {
            public int DNP_index { get; set; }
            public string Device { get; set; }
            public string Description { get; set; }
            public string Variable_name { get; set; }
            public string Symbolic_address { get; set; }
            public string Unit { get; set; }
            public string Static_variation { get; set; }
            public string Event_variation { get; set; }
            public bool Invert { get; set; }
            public bool Shift { get; set; }
        }

        



        private void Page1_button_Click(object sender, EventArgs e)
        {
            Panel01_active();
        }

        private void Page2_button_Click(object sender, EventArgs e)
        {
            Panel02_active();
            Panel02_DriverPage = new Panel02_Driver(thisProject, richTextBox1 );
        }

        private void Page3_button_Click(object sender, EventArgs e)
        {
            Panel03_active();
            Panel03_DeleteItem = new Panel03_ToDelete(thisProject, richTextBox1 );
        }

        private void Page4_button_Click(object sender, EventArgs e)
        {
            Panel04_active();
            Panel04_GWFun_Creations = new Panel04_GWfunctions(thisProject, richTextBox1 );
        }

        private void Page5_button_Click(object sender, EventArgs e)
        {
            Panel05_active();
        }

        private void P2_Button_StartSelected_Click(object sender, EventArgs e)
        {
            if (checkBox2_1.Checked == true)
            {
                Panel02_DriverPage.DriverRCBModify();
                label2_1.Visible = true;
            }

            if (checkBox2_2.Checked == true)
            {
                Panel02_DriverPage.ControlDriverCreator();
                label2_2.Visible = true;
            }

            if (checkBox2_3.Checked == true)
            {
                Panel02_DriverPage.ChangeCO_driver();
                label2_3.Visible = true;
            }

            if (checkBox2_4.Checked == true)
            {
                CollectDriver();
                Panel02_DriverPage.DriverModeVF(AllIED);
                label2_4.Visible = true;
            }
        }

        private void P3_Button_StartSelected_Click(object sender, EventArgs e)
        {
            if (checkBox3_1.Checked == true)
            {
                Panel03_DeleteItem.Delete_CommandProcessing();
                label3_1.Visible = true;
            }

            if (checkBox3_2.Checked == true)
            {
                Panel03_DeleteItem.Delete_BayPopup();
                label3_2.Visible = true;
            }

            if (checkBox3_3.Checked == true)
            {
                Panel03_DeleteItem.Delete_Navi1to4();
                label3_3.Visible = true;
            }
        }

        private void P4_Button_StartSelected_Click(object sender, EventArgs e)
        {
            if (Allstations.Count == 0) { Allstations = XMLReader.GetStations(textBox1.Text); }

            if (checkBox4_1.Checked == true)
            {
                Panel04_GWFun_Creations.GW_functions(Allstations);
                label4_1.Visible = true;
            }

            if (checkBox4_2.Checked == true)
            {
                Panel04_GWFun_Creations.GW_File2System(Allstations, textBox1.Text);
                label4_2.Visible = true;
            }

            if (checkBox4_3.Checked == true)
            {
                Panel04_GWFun_Creations.GW_Serial_Vars(Allstations);
                Panel04_GWFun_Creations.GW_LogicContent(Allstations, textBox1.Text);
                Panel04_GWFun_Creations.GW_ProgramStatus(Allstations);
                label4_3.Visible = true;
            }
        }

        private void Clear_Stations_Click(object sender, EventArgs e)
        {
            Allstations.Clear();
            listBox1.Items.Clear();

            richTextBox1.AppendText("Stations cleared.\n");
            richTextBox1.AppendText("-------- End of Operation. --------\n");
            richTextBox1.AppendText("\n");
            richTextBox1.ScrollToCaret();
        }


        public void Panel01_active()
        {
            // Panel 01
            textBox1.Visible = true;
            pictureBox1.Visible = true;
            pictureBox2.Visible = true;
            pictureBox3.Visible = true;
            label8.Visible = true;

            CollectGW_PointList.Visible = true;
            GW_XML_INI_Creator.Visible = true;
            ReadBackXML.Visible = true;

            Page1_button.BackColor = SystemColors.GradientActiveCaption;

            // Panel 02
            P2_Button_StartSelected.Visible = false;

            checkBox2_1.Visible = false;
            checkBox2_2.Visible = false;
            checkBox2_3.Visible = false;
            checkBox2_4.Visible = false;
            label2_1.Visible = false;
            label2_2.Visible = false;
            label2_3.Visible = false;
            label2_4.Visible = false;

            tableLayoutPanel1.Visible = false;

            Page2_button.BackColor = SystemColors.ControlLightLight;

            // Panel 03
            P3_Button_StartSelected.Visible = false;
            tableLayoutPanel2.Visible = false;

            Page3_button.BackColor = SystemColors.ControlLightLight;

            // Panel 04
            label4_1.Visible = false;
            label4_2.Visible = false;
            label4_3.Visible = false;

            checkBox4_1.Checked = false;
            checkBox4_2.Checked = false;
            checkBox4_3.Checked = false;

            P4_Button_StartSelected.Visible = false;
            tableLayoutPanel3.Visible = false;

            Page4_button.BackColor = SystemColors.ControlLightLight;

            // Panel 05
            label1.Visible = false;
            textBox2.Visible = false;
            P5_Button_AddPoint.Visible = false;

            Page5_button.BackColor = SystemColors.ControlLightLight;
        }

        public void Panel02_active()
        {
            // Panel 01
            textBox1.Visible = false;
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            pictureBox3.Visible = false;
            label8.Visible = false;

            CollectGW_PointList.Visible = false;
            GW_XML_INI_Creator.Visible = false;
            ReadBackXML.Visible = false;

            Page1_button.BackColor = SystemColors.ControlLightLight;

            // Panel 02
            P2_Button_StartSelected.Visible = true;

            checkBox2_1.Visible = true;
            checkBox2_2.Visible = true;
            checkBox2_3.Visible = true;
            checkBox2_4.Visible = true;
            label2_1.Visible = false;
            label2_2.Visible = false;
            label2_3.Visible = false;
            label2_4.Visible = false;

            checkBox2_1.Checked = true;
            checkBox2_2.Checked = true;
            checkBox2_3.Checked = true;
            checkBox2_4.Checked = true;

            tableLayoutPanel1.Visible = true;

            Page2_button.BackColor = SystemColors.GradientActiveCaption;

            // Panel 03
            P3_Button_StartSelected.Visible = false;
            tableLayoutPanel2.Visible = false;

            Page3_button.BackColor = SystemColors.ControlLightLight;

            // Panel 04
            label4_1.Visible = false;
            label4_2.Visible = false;
            label4_3.Visible = false;

            checkBox4_1.Checked = false;
            checkBox4_2.Checked = false;
            checkBox4_3.Checked = false;

            P4_Button_StartSelected.Visible = false;
            tableLayoutPanel3.Visible = false;

            Page4_button.BackColor = SystemColors.ControlLightLight;

            // Panel 05
            label1.Visible = false;
            textBox2.Visible = false;
            P5_Button_AddPoint.Visible = false;

            Page5_button.BackColor = SystemColors.ControlLightLight;

        }

        public void Panel03_active()
        {
            // Panel 01
            textBox1.Visible = false;
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            pictureBox3.Visible = false;
            label8.Visible = false;

            CollectGW_PointList.Visible = false;
            GW_XML_INI_Creator.Visible = false;
            ReadBackXML.Visible = false;

            Page1_button.BackColor = SystemColors.ControlLightLight;

            // Panel 02
            P2_Button_StartSelected.Visible = false;

            checkBox2_1.Visible = false;
            checkBox2_2.Visible = false;
            checkBox2_3.Visible = false;
            checkBox2_4.Visible = false;
            label2_1.Visible = false;
            label2_2.Visible = false;
            label2_3.Visible = false;
            label2_4.Visible = false;

            checkBox2_1.Checked = false;
            checkBox2_2.Checked = false;
            checkBox2_3.Checked = false;
            checkBox2_4.Checked = false;

            tableLayoutPanel1.Visible = false;

            Page2_button.BackColor = SystemColors.ControlLightLight;

            // Panel 03
            label3_1.Visible = false;
            label3_2.Visible = false;
            label3_3.Visible = false;

            checkBox3_1.Checked = true;
            checkBox3_2.Checked = true;
            checkBox3_3.Checked = true;

            P3_Button_StartSelected.Visible = true;
            tableLayoutPanel2.Visible = true;

            Page3_button.BackColor = SystemColors.GradientActiveCaption;

            // Panel 04
            label4_1.Visible = false;
            label4_2.Visible = false;
            label4_3.Visible = false;

            checkBox4_1.Checked = false;
            checkBox4_2.Checked = false;
            checkBox4_3.Checked = false;

            P4_Button_StartSelected.Visible = false;
            tableLayoutPanel3.Visible = false;

            Page4_button.BackColor = SystemColors.ControlLightLight;

            // Panel 05
            label1.Visible = false;
            textBox2.Visible = false;
            P5_Button_AddPoint.Visible = false;

            Page5_button.BackColor = SystemColors.ControlLightLight;

        }

        public void Panel04_active()
        {
            // Panel 01
            textBox1.Visible = true;
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            pictureBox3.Visible = false;
            label8.Visible = true;

            CollectGW_PointList.Visible = false;
            GW_XML_INI_Creator.Visible = false;
            ReadBackXML.Visible = false;

            Page1_button.BackColor = SystemColors.ControlLightLight;

            // Panel 02
            P2_Button_StartSelected.Visible = false;

            checkBox2_1.Visible = false;
            checkBox2_2.Visible = false;
            checkBox2_3.Visible = false;
            checkBox2_4.Visible = false;
            label2_1.Visible = false;
            label2_2.Visible = false;
            label2_3.Visible = false;
            label2_4.Visible = false;

            checkBox2_1.Checked = false;
            checkBox2_2.Checked = false;
            checkBox2_3.Checked = false;
            checkBox2_4.Checked = false;

            tableLayoutPanel1.Visible = false;

            Page2_button.BackColor = SystemColors.ControlLightLight;

            // Panel 03
            label3_1.Visible = false;
            label3_2.Visible = false;
            label3_3.Visible = false;

            checkBox3_1.Checked = false;
            checkBox3_2.Checked = false;
            checkBox3_3.Checked = false;

            P3_Button_StartSelected.Visible = false;
            tableLayoutPanel2.Visible = false;

            Page3_button.BackColor = SystemColors.ControlLightLight;

            // Panel 04
            label4_1.Visible = false;
            label4_2.Visible = false;
            label4_3.Visible = false;

            checkBox4_1.Checked = true;
            checkBox4_2.Checked = true;
            checkBox4_3.Checked = true;

            P4_Button_StartSelected.Visible = true;
            tableLayoutPanel3.Visible = true;

            Page4_button.BackColor = SystemColors.GradientActiveCaption;

            // Panel 05
            label1.Visible = false;
            textBox2.Visible = false;
            P5_Button_AddPoint.Visible = false;

            Page5_button.BackColor = SystemColors.ControlLightLight;

        }

        public void Panel05_active()
        {
            // Panel 01
            textBox1.Visible = true;
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            pictureBox3.Visible = false;
            label8.Visible = true;

            CollectGW_PointList.Visible = false;
            GW_XML_INI_Creator.Visible = false;
            ReadBackXML.Visible = false;

            Page1_button.BackColor = SystemColors.ControlLightLight;

            // Panel 02
            P2_Button_StartSelected.Visible = false;

            checkBox2_1.Visible = false;
            checkBox2_2.Visible = false;
            checkBox2_3.Visible = false;
            checkBox2_4.Visible = false;
            label2_1.Visible = false;
            label2_2.Visible = false;
            label2_3.Visible = false;
            label2_4.Visible = false;

            checkBox2_1.Checked = false;
            checkBox2_2.Checked = false;
            checkBox2_3.Checked = false;
            checkBox2_4.Checked = false;

            tableLayoutPanel1.Visible = false;

            Page2_button.BackColor = SystemColors.ControlLightLight;

            // Panel 03
            label3_1.Visible = false;
            label3_2.Visible = false;
            label3_3.Visible = false;

            checkBox3_1.Checked = false;
            checkBox3_2.Checked = false;
            checkBox3_3.Checked = false;

            P3_Button_StartSelected.Visible = false;
            tableLayoutPanel2.Visible = false;

            Page3_button.BackColor = SystemColors.ControlLightLight;

            // Panel 04
            label4_1.Visible = false;
            label4_2.Visible = false;
            label4_3.Visible = false;

            checkBox4_1.Checked = false;
            checkBox4_2.Checked = false;
            checkBox4_3.Checked = false;

            P4_Button_StartSelected.Visible = false;
            tableLayoutPanel3.Visible = false;

            Page4_button.BackColor = SystemColors.ControlLightLight;

            // Panel 05
            label1.Visible = true;
            textBox2.Visible = true;
            P5_Button_AddPoint.Visible = true;

            Page5_button.BackColor = SystemColors.GradientActiveCaption;
        }

        
    }
}
