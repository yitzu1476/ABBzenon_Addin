using Scada.AddIn.Contracts;
using Scada.AddIn.Contracts.Variable;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static GW_EditorTool.EngineeringStudioWizardExtension;
using Excel = Microsoft.Office.Interop.Excel;
using static GW_EditorTool.Form1;
using Microsoft.Office.Interop.Excel;
using System.Xml;

namespace GW_EditorTool
{
    public class StationCreator
    {
        IProject thisProject;
        List<PointList> DI_Points = new List<PointList>();
        List<PointList> CO_Points = new List<PointList>();
        List<PointList> AI_Points = new List<PointList>();
        List<PointList> ACC_Points = new List<PointList>();
        List<IED_Driver_Key> AllIED = new List<IED_Driver_Key>();
        List<Station_List> Allstations = new List<Station_List>();

        XML_Creator XMLBuilder = new XML_Creator();
        Creator_Class ItemCreator;

        RichTextBox StationRichTextBox;

        string[] DefaultVariation = new string[8];
        string[] StationSetting = new string[10];

        public StationCreator(List<PointList> DI, List<PointList> CO, List<PointList> AI, List<PointList> ACC, IProject thisIProject, Creator_Class IitemCreator, RichTextBox thisTextBox)
        {
            thisProject = thisIProject;
            DI_Points = DI;
            CO_Points = CO;
            AI_Points = AI;
            ACC_Points = ACC;
            ItemCreator = IitemCreator;

            StationRichTextBox = thisTextBox;
        }

        // Collect all points in excel to each list
        public void CollectAllPoints(Excel.Workbook XLworkBook)
        {
            DI_Points.Clear();
            AI_Points.Clear();
            CO_Points.Clear();
            ACC_Points.Clear();

            // DI
            Excel._Worksheet workSheet_DI = XLworkBook.Worksheets["GW_DI"];
            CollectPoints(workSheet_DI, DI_Points);
            Marshal.ReleaseComObject(workSheet_DI);

            // CO
            Excel._Worksheet workSheet_CO = XLworkBook.Worksheets["GW_CO"];
            CollectPoints(workSheet_CO, CO_Points);
            Marshal.ReleaseComObject(workSheet_CO);

            // AI
            Excel._Worksheet workSheet_AI = XLworkBook.Worksheets["GW_AI"];
            CollectPoints(workSheet_AI, AI_Points);
            Marshal.ReleaseComObject(workSheet_AI);

            // ACC
            Excel._Worksheet workSheet_ACC = XLworkBook.Worksheets["GW_ACC"];
            CollectPoints(workSheet_ACC, ACC_Points);
            Marshal.ReleaseComObject(workSheet_ACC);
        }

        // Get default setting of variation from excel
        public void GetDefaultVariation(Excel._Worksheet workSheet_station)
        {
            DefaultVariation[0] = workSheet_station.Cells[4, 3].Text;
            DefaultVariation[1] = workSheet_station.Cells[5, 3].Text;
            DefaultVariation[2] = workSheet_station.Cells[6, 3].Text;
            DefaultVariation[3] = workSheet_station.Cells[7, 3].Text;

            DefaultVariation[4] = workSheet_station.Cells[4, 5].Text;
            DefaultVariation[5] = workSheet_station.Cells[5, 5].Text;
            DefaultVariation[6] = workSheet_station.Cells[6, 5].Text;
            DefaultVariation[7] = workSheet_station.Cells[7, 5].Text;
        }

        // Collect all points in the template excel, and create variable in editor
        public void TemplateVariableCM(Excel.Workbook XLworkBook, List<IED_Driver_Key> tempAllIED)
        {
            // Collect all points in excel to lists
            CollectAllPoints(XLworkBook);

            AllIED = tempAllIED;

            // Create variables in editor from excel if they were not existed
            // BI
            foreach (var item in DI_Points)
            {
                if (item.Device == "")
                {
                    IVariable DI_variable = thisProject.VariableCollection[item.Variable_name];
                    if (DI_variable == null)
                    {
                        string varName = item.Variable_name;
                        string dataType = "BOOL";

                        ItemCreator.VariableCreatorIV(varName, dataType);
                    }
                    ItemCreator.VariableModifierIV(item.Variable_name, item.Description, item.Unit);
                }

                else
                {
                    int driver_netA = 0;
                    string driverID = "";
                    foreach (var driverItem in AllIED)
                    {
                        if (driverItem.TechnicalKey == item.Device)
                        {
                            driver_netA = driverItem.NetAddress;
                            driverID = driverItem.DriverName;
                        }
                    }

                    IVariable DI_variable = thisProject.VariableCollection[item.Variable_name];
                    if (DI_variable == null)
                    {
                        string varName = item.Variable_name;
                        //string driverID = item.Device;
                        string dataType = "BOOL";

                        ItemCreator.VariableCreatorPLC(varName, driverID, dataType);
                    }
                    ItemCreator.VariableModifierPLC(item.Variable_name, driver_netA, item.Description, item.Unit, item.Symbolic_address);
                }
            }

            // ACC
            foreach (var item in ACC_Points)
            {
                if (item.Device == "")
                {
                    IVariable ACC_variable = thisProject.VariableCollection[item.Variable_name];
                    if (ACC_variable == null)
                    {
                        string varName = item.Variable_name;
                        string dataType = "UDINT";

                        ItemCreator.VariableCreatorIV(varName, dataType);
                    }
                    ItemCreator.VariableModifierIV(item.Variable_name, item.Description, item.Unit);
                }

                else
                {
                    int driver_netA = 0;
                    string driverID = "";
                    foreach (var driverItem in AllIED)
                    {
                        if (driverItem.TechnicalKey == item.Device)
                        {
                            driver_netA = driverItem.NetAddress;
                            driverID = driverItem.DriverName;
                        }
                    }

                    IVariable ACC_variable = thisProject.VariableCollection[item.Variable_name];
                    if (ACC_variable == null)
                    {
                        string varName = item.Variable_name;
                        //string driverID = item.Device;
                        string dataType = "UDINT";

                        ItemCreator.VariableCreatorPLC(varName, driverID, dataType);
                    }
                    ItemCreator.VariableModifierPLC(item.Variable_name, driver_netA, item.Description, item.Unit, item.Symbolic_address);
                }
            }


            // AI
            foreach (var item in AI_Points)
            {
                if (item.Device == "")
                {
                    IVariable AI_variable = thisProject.VariableCollection[item.Variable_name];
                    if (AI_variable == null)
                    {
                        string varName = item.Variable_name;
                        string dataType = "REAL";

                        ItemCreator.VariableCreatorIV(varName, dataType);
                    }
                    ItemCreator.VariableModifierIV(item.Variable_name, item.Description, item.Unit);
                }

                else
                {
                    int driver_netA = 0;
                    string driverID = "";
                    foreach (var driverItem in AllIED)
                    {
                        if (driverItem.TechnicalKey == item.Device)
                        {
                            driver_netA = driverItem.NetAddress;
                            driverID = driverItem.DriverName;
                        }
                    }

                    IVariable AI_variable = thisProject.VariableCollection[item.Variable_name];
                    if (AI_variable == null)
                    {
                        string varName = item.Variable_name;
                        //string driverID = item.Device;
                        string dataType = "REAL";

                        ItemCreator.VariableCreatorPLC(varName, driverID, dataType);
                    }
                    ItemCreator.VariableModifierPLC(item.Variable_name, driver_netA, item.Description, item.Unit, item.Symbolic_address);
                }
            }


            // CO
            foreach (var item in CO_Points)
            {
                if (item.Device == "")
                {
                    IVariable CO_variable = thisProject.VariableCollection[item.Variable_name];
                    if (CO_variable == null)
                    {
                        string varName = item.Variable_name;
                        string dataType = "BOOL";

                        ItemCreator.VariableCreatorIV(varName, dataType);
                    }
                    ItemCreator.VariableModifierIV(item.Variable_name, item.Description, item.Unit);
                }

                else
                {
                    int driver_netA = 0;
                    string driverID = "";
                    foreach (var driverItem in AllIED)
                    {
                        if (driverItem.TechnicalKey == item.Device)
                        {
                            driver_netA = driverItem.NetAddress;
                            driverID = driverItem.DriverName;
                            break;
                        }
                    }

                    IVariable CO_variable = thisProject.VariableCollection[item.Variable_name];
                    if (CO_variable == null)
                    {
                        string varName = item.Variable_name;
                        //string driverID = item.Device;
                        string dataType = "BOOL";

                        ItemCreator.VariableCreatorPLC(varName, driverID, dataType);
                    }
                    ItemCreator.VariableModifierPLC(item.Variable_name, driver_netA, item.Description, item.Unit, item.Symbolic_address);
                }
            }
        }

        public void StationCollector(string path, string Station, string folder, List<IED_Driver_Key> AllIED, List<Station_List> Allstations)
        {
            // Activate excel workbook
            Excel.Application XLapp = new Excel.Application();
            Excel.Workbook XLworkBook = XLapp.Workbooks.Open(path);

            Excel._Worksheet workSheet_station = XLworkBook.Worksheets["Station_Setting"];
            GetDefaultVariation(workSheet_station);
            Marshal.ReleaseComObject(workSheet_station);

            // Add all points in excel to list
            CollectAllPoints(XLworkBook);

            XLworkBook.Close();
            Marshal.ReleaseComObject(XLworkBook);
            XLapp.Quit();
            Marshal.ReleaseComObject(XLapp);

            string xml_content;
            xml_content = XMLBuilder.AboveDataBase(Station, Allstations);
            xml_content = XML_DataBaseAll(xml_content);
            xml_content = xml_content + XMLBuilder.BelowDataBase();

            string SavingFilePath = folder + "\\AccessDNP3_SG-" + Station + ".xml";

            File.WriteAllText(SavingFilePath, xml_content, Encoding.Unicode);
            StationRichTextBox.AppendText(SavingFilePath + " xml file created.\n");
            StationRichTextBox.ScrollToCaret();

            string INIFilePath = folder + "\\" + Station + ".ini";
            string iniContent = XMLBuilder.INIcontent();
            File.WriteAllText(INIFilePath, iniContent);
            StationRichTextBox.AppendText(INIFilePath + " ini file created.\n");
            StationRichTextBox.ScrollToCaret();
        }

        // Collect point info from excel to certain list
        public void CollectPoints(Excel._Worksheet thisSheet, List<PointList> thisList)
        {
            Excel.Range this_XLrange = thisSheet.UsedRange;

            int rowCNT = this_XLrange.Rows.Count;

            for (int i = 2; i < rowCNT + 1; i++)
            {
                int temp_DNP_index;
                string temp_Station_variation;
                string temp_Event_variation;
                bool temp_Invert;
                bool temp_Shift;


                if (thisSheet.Cells[i, 1].Text != "") { temp_DNP_index = int.Parse(thisSheet.Cells[i, 1].Text); }
                else
                { StationRichTextBox.AppendText("DNP number error\n"); StationRichTextBox.ScrollToCaret(); return; }

                if (thisSheet.Cells[i, 10].Text != "") { temp_Station_variation = thisSheet.Cells[i, 10].Text; }
                else { temp_Station_variation = "default"; }

                if (thisSheet.Cells[i, 11].Text != "") { temp_Event_variation = thisSheet.Cells[i, 11].Text; }
                else { temp_Event_variation = "default"; }

                if (thisSheet.Cells[i, 12].Text != "") { temp_Invert = bool.Parse(thisSheet.Cells[i, 12].Text); }
                else { temp_Invert = false; }

                if (thisSheet.Cells[i, 13].Text != "") { temp_Shift = bool.Parse(thisSheet.Cells[i, 13].Text); }
                else { temp_Shift = false; }

                thisList.Add(new PointList
                {
                    DNP_index = temp_DNP_index,
                    Device = thisSheet.Cells[i, 2].Text,
                    Description = thisSheet.Cells[i, 6].Text,
                    Variable_name = thisSheet.Cells[i, 7].Text,
                    Symbolic_address = thisSheet.Cells[i,8].Text,
                    Unit = thisSheet.Cells[i, 5].Text,
                    Static_variation = temp_Station_variation,
                    Event_variation = temp_Event_variation,
                    Invert = temp_Invert,
                    Shift = temp_Shift
                });
            }

            StationRichTextBox.AppendText(thisSheet.Name + ": " + thisList.Count.ToString() + " variables found.\n");
            StationRichTextBox.ScrollToCaret();

            Marshal.ReleaseComObject(this_XLrange);
        }

        public void XML_DB_Modifier(string path, string xmlPath)
        {
            Excel.Application XLapp = new Excel.Application();
            Excel.Workbook XLworkBook = XLapp.Workbooks.Open(path);

            Excel._Worksheet workSheet_station = XLworkBook.Worksheets["Station_Setting"];
            GetDefaultVariation(workSheet_station);
            Marshal.ReleaseComObject(workSheet_station);

            CollectAllPoints(XLworkBook);

            XLworkBook.Close();
            Marshal.ReleaseComObject(XLworkBook);
            XLapp.Quit();
            Marshal.ReleaseComObject(XLapp);

            // XML
            string allXML = File.ReadAllText(xmlPath);
            string xml_aboveDB = allXML.Split(new string[] { "      <Database" }, StringSplitOptions.None)[0];
            string xml_belowDB = allXML.Split(new string[] { "</Database>\r\n" }, StringSplitOptions.None)[1];

            string xml_content;

            xml_content = xml_aboveDB + "      <Database Version=\"1\">\r\n";
            xml_content = XML_DataBaseAll(xml_content);
            xml_content = xml_content + "      </Database>\r\n" + xml_belowDB;

            File.WriteAllText(xmlPath, xml_content, Encoding.Unicode);

            StationRichTextBox.AppendText(xmlPath + " modified.\n");
            StationRichTextBox.ScrollToCaret();
        }

        public string XML_DataBaseAll(string xml_content)
        {
            // BI
            xml_content = xml_content + XMLBuilder.BinaryInputsA(DI_Points.Count, DefaultVariation);
            foreach (var item in DI_Points)
            {
                string point_content = XMLBuilder.BinaryInputs_point(item.DNP_index, item.Variable_name, item.Description, item.Static_variation, item.Event_variation, item.Invert, item.Shift);
                xml_content = xml_content + point_content;

            }
            xml_content = xml_content + XMLBuilder.BinaryInputsB();
            xml_content = xml_content + XMLBuilder.DoubleBitDI();

            // RC
            xml_content = xml_content + XMLBuilder.RunningCountersA(ACC_Points.Count, DefaultVariation);
            foreach (var item in ACC_Points)
            {
                string point_content = XMLBuilder.RunningCounters_point(item.DNP_index, item.Variable_name, item.Description, item.Static_variation, item.Event_variation, item.Shift);
                xml_content = xml_content + point_content;

            }
            xml_content = xml_content + XMLBuilder.RunningCountersB();


            // AI
            xml_content = xml_content + XMLBuilder.AnalogInputsA(AI_Points.Count, DefaultVariation);
            foreach (var item in AI_Points)
            {
                string point_content = XMLBuilder.AnalogInputs_point(item.DNP_index, item.Variable_name, item.Description, item.Static_variation, item.Event_variation, 1, item.Shift);
                xml_content = xml_content + point_content;

            }
            xml_content = xml_content + XMLBuilder.AnalogInputsB();


            // BO
            xml_content = xml_content + XMLBuilder.BinaryOutputsA(CO_Points.Count, DefaultVariation);
            foreach (var item in CO_Points)
            {
                string point_content = XMLBuilder.BinaryOutputs_point(item.DNP_index, item.Variable_name, item.Description, item.Static_variation, item.Event_variation, item.Shift);
                xml_content = xml_content + point_content;
            }
            xml_content = xml_content + XMLBuilder.BinaryOutputsB();
            xml_content = xml_content + XMLBuilder.RemainingDB();

            return xml_content;
        }

    }
}
