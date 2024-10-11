using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static GW_EditorTool.Form1;
using Scada.AddIn.Contracts;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;
using Scada.AddIn.Contracts.Variable;

namespace GW_EditorTool
{
    public class ReadBackXML
    {
        IProject thisProject;
        RichTextBox thisTextBox;
        System.Windows.Forms.ListBox thisListbox;
        List<Station_List> TempStation = new List<Station_List>();
        List<PointList_fromXML> Points_fromXML = new List<PointList_fromXML>();
        List<Station_List> thisStation;
        List<IED_Driver_Key> thisAllIED;


        public ReadBackXML(IProject project, RichTextBox thisBox, System.Windows.Forms.ListBox thisListBox, List<Station_List> Allstations)
        {
            thisProject = project;
            thisTextBox = thisBox;
            thisListbox = thisListBox;
            thisStation = Allstations;

        }

        public List<Station_List> GetStations(string path)
        {
            thisTextBox.AppendText("Collecting stations...\n");
            thisTextBox.ScrollToCaret();

            thisListbox.Items.Clear();

            Excel.Application XLapp = new Excel.Application();
            Excel.Workbook XLworkBook = XLapp.Workbooks.Open(path);

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
                        thisListbox.Items.Add(thisStationName);

                        if (thisChannel == "TCP/IP")
                        {
                            if (tempStation[7] == "" || tempStation[8] == "" || tempStation[9] == "" || tempStation[10] == "" || tempStation[11] == "")
                            {
                                thisTextBox.AppendText(thisStationName + " setting error.\n");
                                thisTextBox.ScrollToCaret();
                                continue;
                            }

                            TempStation.Add(new Station_List
                            {
                                StationName = workSheet_station.Cells[i, 1].Text,
                                Channel = workSheet_station.Cells[i, 2].Text,
                                TPCListeningPort = int.Parse(workSheet_station.Cells[i, 7].Text),
                                NetworkCard = workSheet_station.Cells[i, 8].Text,
                                OutstationAdd = int.Parse(workSheet_station.Cells[i, 9].Text),
                                MasterAdd = int.Parse(workSheet_station.Cells[i, 10].Text),
                                MasterIP = workSheet_station.Cells[i, 11].Text
                            });
                        }

                        if (thisChannel == "Serial")
                        {
                            if (tempStation[3] == "" || tempStation[4] == "" || tempStation[5] == "" || tempStation[6] == "" || tempStation[9] == "" || tempStation[10] == "")
                            {
                                thisTextBox.AppendText(thisStationName + " setting error.\n");
                                thisTextBox.ScrollToCaret();
                                continue;
                            }

                            TempStation.Add(new Station_List
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
                        }
                    }
                }
            }

            Marshal.ReleaseComObject(workSheet_station);

            XLworkBook.Close();
            Marshal.ReleaseComObject(XLworkBook);
            XLapp.Quit();
            Marshal.ReleaseComObject(XLapp);

            thisStation = TempStation;

            return TempStation;
        }



        public void ReadXMLPoints(string PointList_path, string savingFolder)
        {
            if (thisStation.Count == 0 && thisListbox.SelectedItems.Count == 0)
            {
                thisTextBox.AppendText("No station, start collecting...\n");
                thisTextBox.ScrollToCaret();
                thisStation = GetStations(PointList_path);
            }

            if (thisListbox.SelectedItems.Count == 0)
            {
                thisTextBox.AppendText("Please select stations from Listbox.\n");
                thisTextBox.ScrollToCaret();
            }

            foreach (var StationN in thisListbox.SelectedItems)
            {
                string Station_str = StationN.ToString();

                foreach (var item in thisStation)
                {
                    if (item.StationName != Station_str) { continue; }

                    string SystemXML = "C:\\ProgramData\\ABB\\System\\AccessDNP3_SG-" + item.StationName + ".xml";
                    if (File.Exists(SystemXML))
                    {
                        XmlDocument xmlDoc = new XmlDocument();
                        xmlDoc.Load(SystemXML);

                        thisTextBox.AppendText("Reading xml from " + item.StationName + " ...\n");
                        thisTextBox.ScrollToCaret();

                        XmlNode dataBase = xmlDoc.DocumentElement.ChildNodes[0].ChildNodes[0]["Database"];
                        XmlNode BI_database = dataBase["BinaryInputs"]["Points"];
                        XmlNode CO_database = dataBase["BinaryOutputs"]["Points"];
                        XmlNode AI_database = dataBase["AnalogInputs"]["Points"];
                        XmlNode ACC_database = dataBase["RunningCounters"]["Points"];

                        WritetoPointList(BI_database, "DI");
                        WritetoPointList(CO_database, "CO");
                        WritetoPointList(AI_database, "AI");
                        WritetoPointList(ACC_database, "ACC");


                        string ExcelFile = savingFolder + "\\GW_PointList_" + item.StationName + ".xlsx";
                        if (File.Exists(ExcelFile))
                        {
                            Excel.Application XLapp = new Excel.Application();
                            Excel.Workbook XLworkBook = XLapp.Workbooks.Open(ExcelFile);

                            Excel._Worksheet workSheet_BI = XLworkBook.Worksheets["GW_DI"];
                            WritetoExcel(workSheet_BI, "DI");
                            Marshal.ReleaseComObject(workSheet_BI);

                            Excel._Worksheet workSheet_CO = XLworkBook.Worksheets["GW_CO"];
                            WritetoExcel(workSheet_CO, "CO");
                            Marshal.ReleaseComObject(workSheet_CO);

                            Excel._Worksheet workSheet_AI = XLworkBook.Worksheets["GW_AI"];
                            WritetoExcel(workSheet_AI, "AI");
                            Marshal.ReleaseComObject(workSheet_AI);

                            Excel._Worksheet workSheet_ACC = XLworkBook.Worksheets["GW_ACC"];
                            WritetoExcel(workSheet_ACC, "ACC");
                            Marshal.ReleaseComObject(workSheet_ACC);



                            XLworkBook.Save();
                            thisTextBox.AppendText(ExcelFile + " modified!\n");
                            thisTextBox.ScrollToCaret();

                            XLworkBook.Close();
                            Marshal.ReleaseComObject(XLworkBook);
                            XLapp.Quit();
                            Marshal.ReleaseComObject(XLapp);


                        }
                    }

                    else
                    {
                        thisTextBox.AppendText("No station XML file in System folder for " + Station_str + ".\n");
                        thisTextBox.AppendText("\n");
                        thisTextBox.ScrollToCaret();
                    }
                }
            }

            thisListbox.SelectedItems.Clear();

            thisTextBox.AppendText("-------- End of Operation. --------\n");
            thisTextBox.AppendText("\n");
            thisTextBox.ScrollToCaret();
        }

        public void AllIED_void(List<IED_Driver_Key> AllIED_form)
        {
            thisAllIED = AllIED_form;
        }

        public void WritetoPointList(XmlNode thisDataBase, string type)
        {
            foreach (XmlNode node in thisDataBase.ChildNodes)
            {
                bool point_shift = false;
                int DNP_id = int.Parse(node["PointIndex"].InnerText);
                if (node.Attributes["Index"].InnerText != node["PointIndex"].InnerText)
                {
                    point_shift = true;
                    DNP_id = DNP_id - 1;
                }

                bool invert_str = false;
                if (type == "DI") { invert_str = bool.Parse(node["InvertBIvalue"].InnerText); }

                string thisVarName = node["Name"].InnerText;
                if (thisVarName.Contains('#')) { thisVarName = thisVarName.Split('#')[1]; }

                Points_fromXML.Add(new PointList_fromXML
                {
                    PointType = type,
                    DNP_index = DNP_id,
                    Description = node["Identification"].InnerText,
                    Variable_name = thisVarName,
                    Static_variation = node["StaticVariation"].InnerText,
                    Event_variation = node["EventVariation"].InnerText,
                    Invert = invert_str,
                    Shift = point_shift
                });
            }

            thisTextBox.AppendText("Collecting " + type + " from XMl...\n");
            thisTextBox.ScrollToCaret();
        }

        public void WritetoExcel(Excel._Worksheet sheet, string type)
        {
            int DNP_end = 0;
            IVariableCollection variableColl = thisProject.VariableCollection;

            foreach (var point in Points_fromXML)
            {
                if (point.PointType == type)
                {
                    thisTextBox.AppendText(point.Variable_name + " written.\n");
                    thisTextBox.ScrollToCaret();

                    string thisTechnicalKey = "";

                    IVariable thisVar = variableColl[point.Variable_name];
                    string thisUnit = "";
                    string thisSymb = "";

                    if (thisVar != null)
                    {
                        thisUnit = thisVar.Unit;
                        thisSymb = thisVar.GetDynamicProperty("SymbAddr").ToString();

                        foreach (var item in thisAllIED)
                        {
                            if (item.DriverName == thisVar.Driver.Identification && item.NetAddress == thisVar.NetAddress) { thisTechnicalKey = item.TechnicalKey; }
                        }
                    }

                    int Row_index = point.DNP_index + 2;
                    sheet.Cells[Row_index, 1] = point.DNP_index;
                    sheet.Cells[Row_index, 2] = thisTechnicalKey;
                    sheet.Cells[Row_index, 4] = type;
                    sheet.Cells[Row_index, 5] = thisUnit;
                    sheet.Cells[Row_index, 6] = point.Description;
                    sheet.Cells[Row_index, 7] = point.Variable_name;
                    sheet.Cells[Row_index, 8] = thisSymb;
                    sheet.Cells[Row_index, 10] = point.Static_variation;
                    sheet.Cells[Row_index, 11] = point.Event_variation;
                    if (type == "DI") { sheet.Cells[Row_index, 12] = point.Invert; }
                    sheet.Cells[Row_index, 13] = point.Shift;

                    DNP_end++;
                }
            }

            bool cell_content = true;
            while (cell_content)
            {
                if (sheet.Cells[DNP_end + 2, 1].Text.Length > 0)
                {
                    for (int j = 1; j < 14; j++)
                    {
                        sheet.Cells[DNP_end + 2, j] = "";
                    }
                }
                else { cell_content = false; }

                DNP_end = DNP_end + 1;
            }

            thisTextBox.AppendText("Writing " + type + " to excel...\n");
            thisTextBox.ScrollToCaret();
        }

        public class PointList_fromXML
        {
            public string PointType { get; set; }
            public int DNP_index { get; set; }
            public int Item_index { get; set; }
            public string Device { get; set; }
            public string Description { get; set; }
            public string Variable_name { get; set; }
            public string Unit { get; set; }
            public string Static_variation { get; set; }
            public string Event_variation { get; set; }
            public bool Invert { get; set; }
            public bool Shift { get; set; }
        }

    }
}
