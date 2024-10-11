using GW_EditorTool;
using Scada.AddIn.Contracts;
using Scada.AddIn.Contracts.Function;
using Scada.AddIn.Contracts.ReactionMatrix;
using Scada.AddIn.Contracts.Variable;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace Gateway_EditorTool
{
    internal class Panel04_GWfunctions
    {
        IProject thisProject;
        RichTextBox thisRichTextBox;
        List<Form1.Station_List> Allstations = new List<Form1.Station_List>();
        List<TCP_Status.TCP_Vars> AllTCP_Vars = new List<TCP_Status.TCP_Vars>();

        public Panel04_GWfunctions(IProject mainProject, RichTextBox mainRichtextBox)
        {
            thisProject = mainProject;
            thisRichTextBox = mainRichtextBox;
        }

        public void GW_functions(List<Form1.Station_List> allstations)
        {
            Allstations = allstations;
            IFunctionCollection functionCollection = thisProject.FunctionCollection;

            StringBuilder csvF = new StringBuilder();
            csvF.AppendLine("StationName, SerialPort, MonitorPort, BaudRate, DataBits, StartBits, StopBits, Parity");

            foreach (var item in Allstations)
            {
                string GW_FunName = "GW_" + item.StationName;
                FunctionType GW_FunType = (FunctionType)Enum.Parse(typeof(FunctionType), "StartProgram");

                if (functionCollection[GW_FunName] == null)
                {
                    functionCollection.Create(GW_FunName, GW_FunType);

                    thisRichTextBox.AppendText(GW_FunName + " function created.\n");
                    thisRichTextBox.ScrollToCaret();
                }
                
                IFunction GW_Fun = functionCollection[GW_FunName];

                string GW_FunPara = "/ini:" + item.StationName + ".ini";
                GW_Fun.SetDynamicProperty("FileName", "C:\\Program Files (x86)\\ABB\\zenon 8.10\\zenProcGateway.exe");
                GW_Fun.SetDynamicProperty("FilePar", GW_FunPara);

                thisRichTextBox.AppendText(GW_FunName + " function modified.\n");
                thisRichTextBox.ScrollToCaret();

                // csv file
                if (item.Channel == "Serial")
                {
                    csvF.AppendLine(item.StationName + "," + item.SerialPort + "," + item.MonitorPort + "," + item.BaudRate + "," + item.PortSetting);
                }
            }

            // csv file
            string SerialSetting_file = "C:\\ProgramData\\ABB\\System\\SerialPort_Setting.csv";
            File.WriteAllText(SerialSetting_file, csvF.ToString());

            thisRichTextBox.AppendText("Write serial setting to C:\\ProgramData\\ABB\\System\\SerialPort_Setting.csv\n");
            thisRichTextBox.ScrollToCaret();


            thisRichTextBox.AppendText("-------- End of Operation. --------\n");
            thisRichTextBox.AppendText("\n");
            thisRichTextBox.ScrollToCaret();
        }

        public void GW_File2System(List<Form1.Station_List> allstations, string Template_path)
        {
            string PointList_folder = Template_path.Substring(0, Template_path.Length - 27) + "\\GW_PointList";
            string System_Folder = "C:\\ProgramData\\ABB\\System";

            foreach (var item in  allstations)
            {
                string oldXML_Path = PointList_folder + "\\AccessDNP3_SG-" + item.StationName + ".xml";
                string oldINI_Path = PointList_folder + "\\" + item.StationName + ".ini";

                string newXML_Path = System_Folder + "\\AccessDNP3_SG-" + item.StationName + ".xml";
                string newINI_Path = System_Folder + "\\" + item.StationName + ".ini";

                if (File.Exists(newXML_Path)) { File.Delete(newXML_Path); }
                File.Copy(oldXML_Path, newXML_Path);
                thisRichTextBox.AppendText("AccessDNP3_SG - " + item.StationName + ".xml" + " file copied to System folder.\n");
                thisRichTextBox.ScrollToCaret();

                if (File.Exists(newINI_Path)) { File.Delete(newINI_Path); }
                File.Copy(oldINI_Path, newINI_Path);
                thisRichTextBox.AppendText(item.StationName + ".ini" + " file copied to System folder.\n");
                thisRichTextBox.ScrollToCaret();
            }

            thisRichTextBox.AppendText("-------- End of Operation. --------\n");
            thisRichTextBox.AppendText("\n");
            thisRichTextBox.ScrollToCaret();
        }

        public void GW_LogicContent(List<Form1.Station_List> allstations, string Template_path)
        {
            string PointList_folder = Template_path.Substring(0, Template_path.Length - 27) + "\\GW_PointList";
            TCP_Status GW_Conntection = new TCP_Status(thisProject, thisRichTextBox);

            AllTCP_Vars = GW_Conntection.TCP_Status_Creator(allstations);
            string All_LogicContent = GW_Conntection.TCP_LogicContent(AllTCP_Vars) + GW_Conntection.Serial_LogicContent();

            //string txtFile_Path = PointList_folder + "\\GW_LogicContent.txt";
            //if (File.Exists(txtFile_Path)) { File.Delete(txtFile_Path); }
            //using (StreamWriter txtF = File.CreateText(txtFile_Path))
            //{
            //    txtF.WriteLine(All_LogicContent);
            //}

            //thisRichTextBox.AppendText("Logic Content file created, save in: " + PointList_folder + "\\GW_LogicContent.txt .\n");
            //thisRichTextBox.ScrollToCaret();



            // xml for logic
            Connection_XML_Creator ConnectionXML = new Connection_XML_Creator();
            string ProgramName = "DNP_Connection";
            string XMLName = PointList_folder + "\\DNP_Connection_Logic.xml";
            string XMLVariable = ConnectionXML.VarSetting("Inst_PLS", "PLS") + ConnectionXML.VarSetting("Q", "BOOL") + ConnectionXML.VarSetting("Inst_PLS1", "PLS") + ConnectionXML.VarSetting("Q1", "BOOL");

            string XMLContent = ConnectionXML.BasicSettingA(ProgramName) + XMLVariable + ConnectionXML.BasicSettingB(ProgramName) + ConnectionXML.ProgramContent(All_LogicContent);

            Encoding UTF8noBOM = new UTF8Encoding(false);
            File.WriteAllText(XMLName, XMLContent, UTF8noBOM);

            thisRichTextBox.AppendText("Logic Content file created, save in: " + XMLName + "\n");
            thisRichTextBox.ScrollToCaret();


            thisRichTextBox.AppendText("-------- End of Operation. --------\n");
            thisRichTextBox.AppendText("\n");
            thisRichTextBox.ScrollToCaret();
        }

        public void GW_Serial_Vars(List<Form1.Station_List> allstations)
        {
            // GW_Timer
            IVariableCollection variableCollection = thisProject.VariableCollection;

            IDriver InternalDriver = thisProject.DriverCollection["Driver for internal variables"];
            ChannelType varChannel = (ChannelType)Enum.Parse(typeof(ChannelType), "SystemDriverVariable");
            IDataType varDT_Bool = thisProject.DataTypeCollection["BOOL"];
            IDataType varDT_Str = thisProject.DataTypeCollection["STRING"];

            if (variableCollection["GW_Timer"] == null)
            {
                variableCollection.Create("GW_Timer", InternalDriver, varChannel, varDT_Bool);
                IVariable GW_Timer_var = variableCollection["GW_Timer"];
                GW_Timer_var.SetDynamicProperty("ExternVisible", true);

                thisRichTextBox.AppendText("GW_Timer variable created.\n");
                thisRichTextBox.ScrollToCaret();
            }

            // Serial Status, Update variables
            foreach (var Station in allstations)
            {
                if (Station.Channel == "Serial")
                {
                    string Update_varName = "GW_" + Station.StationName + "_Serial_Update";
                    string Status_varName = "GW_" + Station.StationName + "_Serial_Status";

                    if (variableCollection[Update_varName] == null)
                    {
                        variableCollection.Create(Update_varName, InternalDriver, varChannel, varDT_Str);
                        thisRichTextBox.AppendText(Update_varName + " variable created.\n");
                        thisRichTextBox.ScrollToCaret();
                    }

                    if (variableCollection[Status_varName] == null)
                    {
                        variableCollection.Create(Status_varName, InternalDriver, varChannel, varDT_Bool);
                        thisRichTextBox.AppendText(Status_varName + " variable created.\n");
                        thisRichTextBox.ScrollToCaret();
                    }
                }
            }
        }

        public void GW_ProgramStatus(List<Form1.Station_List> allstations)
        {
            foreach (var Station in allstations)
            {
                IVariableCollection variableCollection = thisProject.VariableCollection;
                IDriver InternalDriver = thisProject.DriverCollection["Driver for internal variables"];
                IDataType DT_BOOL = thisProject.DataTypeCollection["BOOL"];
                IDataType DT_UDINT = thisProject.DataTypeCollection["UDINT"];
                IFunctionCollection functionCollection = thisProject.FunctionCollection;
                IReactionMatrixCollection rmCollection = thisProject.ReactionMatrixCollection;

                // Input variable
                string Input_VarName = Station.StationName + "_AccessDNP3_SG_status";
                if (variableCollection[Input_VarName] == null)
                {
                    variableCollection.Create(Input_VarName, InternalDriver, ChannelType.SystemDriverVariable, DT_UDINT);
                    thisRichTextBox.AppendText(Input_VarName + " variable created.\n");
                    thisRichTextBox.ScrollToCaret();
                }

                // Output variable
                string Output_VarName = "GW_" + Station.StationName + "_ProgramStatus";
                if (variableCollection[Output_VarName] == null)
                {
                    variableCollection.Create(Output_VarName, InternalDriver, ChannelType.SystemDriverVariable, DT_BOOL);
                    thisRichTextBox.AppendText(Output_VarName + " variable created.\n");
                    thisRichTextBox.ScrollToCaret();
                }

                // Functions
                string Normal_FunName = "GW_" + Station.StationName + "_NormalStatus";
                string Alarm_FunName = "GW_" + Station.StationName + "_AlarmStatus";

                if (functionCollection[Normal_FunName] == null)
                {
                    functionCollection.Create(Normal_FunName, FunctionType.WriteSetValue);
                    IFunction Normal_Fun = functionCollection[Normal_FunName];
                    Normal_Fun.SetDynamicProperty("SetValue.IsDirect", true);
                    Normal_Fun.SetDynamicProperty("SetValue.Variable", Output_VarName);
                    Normal_Fun.SetDynamicProperty("SetValue.NumValue", 0);

                    thisRichTextBox.AppendText(Normal_FunName + " function created.\n");
                    thisRichTextBox.ScrollToCaret();
                }

                if (functionCollection[Alarm_FunName] == null)
                {
                    functionCollection.Create(Alarm_FunName, FunctionType.WriteSetValue);
                    IFunction Alarm_Fun = functionCollection[Alarm_FunName];
                    Alarm_Fun.SetDynamicProperty("SetValue.IsDirect", true);
                    Alarm_Fun.SetDynamicProperty("SetValue.Variable", Output_VarName);
                    Alarm_Fun.SetDynamicProperty("SetValue.NumValue", 1);

                    thisRichTextBox.AppendText(Alarm_FunName + " function created.\n");
                    thisRichTextBox.ScrollToCaret();
                }
                

                // Reaction Matrix
                string RM_Name = "GW_" + Station.StationName + "_StatusRM";
                if (rmCollection[RM_Name] == null)
                {
                    rmCollection.Create(ReactionMatrixType.MultiNumeric, RM_Name);
                    thisRichTextBox.AppendText(RM_Name + " reaction matrix created.\n");
                    thisRichTextBox.ScrollToCaret();

                    IReactionMatrix thisRM = rmCollection[RM_Name];
                    int Normal_FunID = functionCollection[Normal_FunName].Id;
                    int Alarm_FunID = functionCollection[Alarm_FunName].Id;

                    ConditionFlags Flag_Exe_Flash = (ConditionFlags)24;

                    for (int i = 0; i < 6; i++)
                    {
                        thisRM.CreateCondition();
                        if (thisRM.Count == 0)
                        {
                            thisRichTextBox.AppendText("Creation of Condition for " + RM_Name + " failed\n");
                            thisRichTextBox.ScrollToCaret();
                            return;
                        }

                        ICondition thisCondition = thisRM[i + 1];
                        thisCondition.ComparisonMethod = 3;
                        thisCondition.LimitValue = i;
                        if (i == 0)
                        {
                            thisCondition.LimitValueText = Station.StationName + " Process Gateway Not Starting";
                            thisCondition.FunctionId = Alarm_FunID;
                            thisCondition.Flags = ConditionFlags.ExecuteFunction;
                            thisCondition.LimitColor = Int32.Parse("2895598");
                        }

                        if (i == 1)
                        {
                            thisCondition.LimitValueText = Station.StationName + " Process Gateway Starting";
                            thisCondition.FunctionId = Normal_FunID;
                            thisCondition.Flags = Flag_Exe_Flash;
                            thisCondition.LimitColor = Int32.Parse("2895598");
                        }

                        if (i == 2)
                        {
                            thisCondition.LimitValueText = Station.StationName + " Process Gateway Running";
                            thisCondition.FunctionId = Normal_FunID;
                            thisCondition.Flags = ConditionFlags.ExecuteFunction;
                            thisCondition.LimitColor = 0;
                        }

                        if (i == 3)
                        {
                            thisCondition.LimitValueText = Station.StationName + " Process Gateway Running";
                            thisCondition.FunctionId = Normal_FunID;
                            thisCondition.Flags = ConditionFlags.ExecuteFunction;
                            thisCondition.LimitColor = 0;
                        }

                        if (i == 4)
                        {
                            thisCondition.LimitValueText = Station.StationName + " Process Gateway Restarting";
                            thisCondition.FunctionId = Normal_FunID;
                            thisCondition.Flags = Flag_Exe_Flash;
                            thisCondition.LimitColor = Int32.Parse("2895598");
                        }

                        if (i == 5)
                        {
                            thisCondition.LimitValueText = Station.StationName + " Process Gateway Shutting Down";
                            thisCondition.FunctionId = Normal_FunID;
                            thisCondition.Flags = Flag_Exe_Flash;
                            thisCondition.LimitColor = Int32.Parse("2895598");
                        }
                    }
                }

                int RM_int = 0;
                IReactionMatrixCollection newRMCollection = thisProject.ReactionMatrixCollection;
                int RM_IDs = newRMCollection.Count * 10;
                for (int i = 0; i < RM_IDs; i++)
                {
                    if (newRMCollection.GetItemById(i) == null) { continue; }
                    if (newRMCollection.GetItemById(i).Name == RM_Name) { RM_int = i; break; }
                }

                if (RM_int != 0)
                {
                    IVariable Input_Var = variableCollection[Input_VarName];
                    Input_Var.SetDynamicProperty("Rema", RM_int);
                    thisRichTextBox.AppendText(Input_VarName + " variable modified.\n");
                    thisRichTextBox.ScrollToCaret();
                }
                else
                {
                    thisRichTextBox.AppendText(Input_VarName + " reaction matrix not found, no settings set.\n");
                    thisRichTextBox.ScrollToCaret();
                }
                


            }

            thisRichTextBox.AppendText("-------- End of Operation. --------\n");
            thisRichTextBox.AppendText("\n");
            thisRichTextBox.ScrollToCaret();
        }
    }
}
