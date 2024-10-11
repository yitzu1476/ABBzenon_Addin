using GW_EditorTool;
using Scada.AddIn.Contracts;
using Scada.AddIn.Contracts.Variable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gateway_EditorTool
{
    internal class TCP_Status
    {
        IProject thisProject;
        RichTextBox thisRichTextBox;
        List<TCP_Vars> TCP_variables = new List<TCP_Vars>();

        public TCP_Status(IProject mainProject, RichTextBox mainRichTextBox)
        {
            thisProject = mainProject;
            thisRichTextBox = mainRichTextBox;
        }

        public List<TCP_Vars> TCP_Status_Creator(List<Form1.Station_List> allStations)
        {
            IVariableCollection variableCollection = thisProject.VariableCollection;
            IDriver InternalDriver = thisProject.DriverCollection["Driver for internal variables"];
            ChannelType varChannel = (ChannelType)Enum.Parse(typeof(ChannelType), "SystemDriverVariable");

            IDataType FUDataType = thisProject.DataTypeCollection["UDINT"];
            IDataType StatusDataType = thisProject.DataTypeCollection["BOOL"];

            foreach (var station in allStations)
            {
                if (station.Channel == "TCP/IP")
                {
                    string thisFrameRe_VarName = station.StationName + "_AccessDNP3_SG_master0_DL_FramesReceived";
                    string thisTCP_Update_VarName = "GW_" + station.StationName + "_TCP_Update";
                    string thisTCP_Status_VarName = "GW_" + station.StationName + "_TCP_Status";
                    string thisTCP_Temp_VarName = "GW_" + station.StationName + "_TCP_Temp";
                    string thisTCP_Cnt_VarName = "GW_" + station.StationName + "_TCP_Cnt";

                    TCP_variables.Add(new TCP_Vars
                    {
                        FrameReceived = thisFrameRe_VarName,
                        Update_diff = thisTCP_Update_VarName,
                        Temp_cnt = thisTCP_Temp_VarName,
                        Status_var = thisTCP_Status_VarName,
                        FrameCnt = thisTCP_Cnt_VarName
                    });

                    if (variableCollection[thisFrameRe_VarName] == null)
                    {
                        variableCollection.Create(thisFrameRe_VarName, InternalDriver, varChannel, FUDataType);
                        IVariable thisFrameRe_Var = variableCollection[thisFrameRe_VarName];
                        thisFrameRe_Var.SetDynamicProperty("ExternVisible", true);

                        thisRichTextBox.AppendText(thisFrameRe_VarName + " variable created.\n");
                        thisRichTextBox.ScrollToCaret();
                    }

                    if (variableCollection[thisTCP_Update_VarName] == null)
                    {
                        variableCollection.Create(thisTCP_Update_VarName, InternalDriver, varChannel, FUDataType);
                        IVariable thisTCP_Update_Var = variableCollection[thisTCP_Update_VarName];
                        thisTCP_Update_Var.SetDynamicProperty("ExternVisible", true);

                        thisRichTextBox.AppendText(thisTCP_Update_VarName + " variable created.\n");
                        thisRichTextBox.ScrollToCaret();
                    }

                    if (variableCollection[thisTCP_Status_VarName] == null)
                    {
                        variableCollection.Create(thisTCP_Status_VarName, InternalDriver, varChannel, StatusDataType);
                        IVariable thisTCP_Status_Var = variableCollection[thisTCP_Status_VarName];
                        thisTCP_Status_Var.SetDynamicProperty("ExternVisible", true);

                        thisRichTextBox.AppendText(thisTCP_Status_VarName + " variable created.\n");
                        thisRichTextBox.ScrollToCaret();
                    }

                    if (variableCollection[thisTCP_Temp_VarName] == null)
                    {
                        variableCollection.Create(thisTCP_Temp_VarName, InternalDriver, varChannel, FUDataType);
                        IVariable thisTCP_Temp_Var = variableCollection[thisTCP_Temp_VarName];
                        thisTCP_Temp_Var.SetDynamicProperty("ExternVisible", true);

                        thisRichTextBox.AppendText(thisTCP_Temp_VarName + " variable created.\n");
                        thisRichTextBox.ScrollToCaret();
                    }

                    if (variableCollection[thisTCP_Cnt_VarName] == null)
                    {
                        variableCollection.Create(thisTCP_Cnt_VarName, InternalDriver, varChannel, FUDataType);
                        IVariable thisTCP_Cnt_Var = variableCollection[thisTCP_Cnt_VarName];
                        thisTCP_Cnt_Var.SetDynamicProperty("ExternVisible", true);

                        thisRichTextBox.AppendText(thisTCP_Cnt_VarName + " variable created.\n");
                        thisRichTextBox.ScrollToCaret();
                    }
                }
            }

            return TCP_variables;
        }

        public string Serial_LogicContent()
        {
            string Serial_content = "// Serial\r\n" +
                "Inst_PLS1( True, t#5s );\r\n" +
                "Q1 := Inst_PLS1.Q;\r\n\r\n" +
                "if (Q1 = true) then\r\n    " +
                "GW_Timer := true;\r\n" +
                "end_if;\r\n\r\n" +
                "if (Q1 = false) then\r\n    " +
                "GW_Timer := false;\r\n" +
                "end_if;\r\n";

            return Serial_content;
        }

        public string TCP_LogicContent(List<TCP_Vars> allTCP)
        {
            string TCP_GeneralA = "// TCP\r\n" +
                "Inst_PLS( True, t#1s );\r\n" +
                "Q := Inst_PLS.Q;\r\n\r\n" +
                "if (Q = true) then\r\n";
            string TCP_GeneralB = "end_if;\r\n\r\n\r\n";

            string TCP_AllContent = TCP_GeneralA;

            foreach(var TCP_station in allTCP)
            {
                string TCP_Station_Con =
                "    // " + TCP_station.FrameReceived.Substring(0,TCP_station.FrameReceived.Length-40) + "\r\n" + 
                "    " + TCP_station.Update_diff + " := " + TCP_station.FrameReceived + " - " + TCP_station.Temp_cnt + ";\r\n    \r\n    " +
                "if ( " + TCP_station.Update_diff + " &lt;&gt; 0) then\r\n        " +
                TCP_station.FrameCnt + " := 0;\r\n        " +
                TCP_station.Status_var + " := true;\r\n    " +
                "end_if;\r\n    \r\n    " +
                "if (" + TCP_station.Update_diff + " = 0) then\r\n        " +
                TCP_station.FrameCnt + " := " + TCP_station.FrameCnt + " + 1;\r\n    " +
                "end_if;\r\n    \r\n    " +
                "if (" + TCP_station.FrameCnt + " &gt; 5) then\r\n        " +
                TCP_station.Status_var + " := false;\r\n    " +
                "end_if;\r\n    \r\n    " +
                TCP_station.Temp_cnt + " := " + TCP_station.FrameReceived + ";\r\n    \r\n";

                TCP_AllContent = TCP_AllContent + TCP_Station_Con;
            }

            TCP_AllContent = TCP_AllContent + TCP_GeneralB;

            return TCP_AllContent;
            
        }

        public class TCP_Vars
        {
            public string FrameReceived { get; set; }
            public string Update_diff { get; set; }
            public string Temp_cnt { get; set; }
            public string Status_var { get; set; }
            public string FrameCnt { get; set; }
        }
    }
}
