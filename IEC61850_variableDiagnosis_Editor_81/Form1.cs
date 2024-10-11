using Scada.AddIn.Contracts;
using Scada.AddIn.Contracts.Variable;
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

namespace IEC61850_variableDiagnosis_Editor_81
{
    public partial class Form1 : Form
    {
        IProject thisProject;
        IVariableCollection variableCollection;
        List<IED_Driver_Key> AllIED = new List<IED_Driver_Key>();

        public Form1(IProject mainProject)
        {
            thisProject = mainProject;
            variableCollection = thisProject.VariableCollection;
            InitializeComponent();

            OK_Button.Visible = false;
        }

        private void Yes_Button_Click(object sender, EventArgs e)
        {
            Yes_Button.Visible = false;
            label1.Visible = false;

            CollectDriver();

            // Driver content
            foreach (var DriverItem in AllIED)
            {
                // ConnectionState for each IED
                string ConnectionState_VarName = DriverItem.TechnicalKey + "!ConnectionState";

                if (DriverItem.DriverName.Contains("_CO")) { ConnectionState_VarName = DriverItem.TechnicalKey + "_CO!ConnectionState"; }
                
                if (variableCollection[ConnectionState_VarName] == null)
                {
                    CreateConnectionState_NetDriver(DriverItem.DriverName, ConnectionState_VarName, DriverItem.NetAddress);
                }

                // Communication Info for each driver
                string Communication_VarName = DriverItem.DriverName + "!Communication";
                if (variableCollection[Communication_VarName] == null)
                {
                    CreateCommunication_Driver(DriverItem.DriverName, Communication_VarName);
                }

            }

            // Create internal variable for Add-in
            string ProfileList_VarName = "ABB_Diagnosis_ProfileList";
            if (variableCollection[ProfileList_VarName] == null) { Internal_Var(ProfileList_VarName); }

            string VarList_VarName = "ABB_Diagnosis_VarList";
            if (variableCollection[VarList_VarName] == null) { Internal_Var(VarList_VarName); }

            // Create AddCause variable
            foreach (IVariable thisVar in variableCollection)
            {
                if (thisVar.Driver.Name != "IEC850") { continue; }

                string thisVar_SymbAdd = thisVar.GetDynamicProperty("SymbAddr").ToString();
                if (thisVar_SymbAdd.Contains("CSWI") && thisVar_SymbAdd.Contains("Pos") && thisVar_SymbAdd.Contains("ctlVal[CO]"))
                {
                    string AddC_SymbAdd = thisVar_SymbAdd.Replace("ctlVal[CO]", "AddCause").Trim();
                    if (variableCollection[AddC_SymbAdd] == null)
                    {
                        AddCause_Var(AddC_SymbAdd, thisVar.Driver.Identification, thisVar.NetAddress, 0);
                    }
                }
            }

            richTextBox1.AppendText("-------- End of Operation. --------\n");
            richTextBox1.AppendText("\n");
            richTextBox1.ScrollToCaret();

            OK_Button.Visible = true;
        }

        private void OK_Button_Click(object sender, EventArgs e)
        {
            this.Close();
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

                richTextBox1.AppendText("Load driver " + fileName + " ...\n");
                richTextBox1.ScrollToCaret();

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

                                richTextBox1.AppendText("Load server " + technicalKey + " ...\n");
                                richTextBox1.ScrollToCaret();
                            }
                        }
                    }
                }
            }
        }

        public void CreateConnectionState_NetDriver(string DriverName, string VarName, int NetAddress)
        {
            IDriver thisDriver = thisProject.DriverCollection[DriverName];
            IDataType thisDatatype = thisProject.DataTypeCollection["UDINT"];

            variableCollection.Create(VarName, thisDriver, ChannelType.SpecialPlcMarker, thisDatatype);

            IVariable thisVar = variableCollection[VarName];
            thisVar.NetAddress = NetAddress;
            thisVar.SetDynamicProperty("SymbAddr", "*!ConnectionState");

            richTextBox1.AppendText(VarName + " variable created.\n");
            richTextBox1.ScrollToCaret();
        }

        public void CreateCommunication_Driver(string DriverName, string VarName)
        {
            IDriver thisDriver = thisProject.DriverCollection[DriverName];
            IDataType thisDatatype = thisProject.DataTypeCollection["STRING"];

            variableCollection.Create(VarName, thisDriver, ChannelType.DriverVariable, thisDatatype);

            IVariable thisVar = variableCollection[VarName];
            thisVar.Offset = 61;

            richTextBox1.AppendText(VarName + " variable created.\n");
            richTextBox1.ScrollToCaret();
        }

        public void AddCause_Var(string VarName, string DriverName, int NetAddress, int RM_Index)
        {
            IDriver thisDriver = thisProject.DriverCollection[DriverName];
            IDataType thisDatatype = thisProject.DataTypeCollection["SINT"];

            variableCollection.Create(VarName, thisDriver, ChannelType.Output, thisDatatype);

            IVariable thisVar = variableCollection[VarName];
            thisVar.NetAddress = NetAddress;
            thisVar.SetDynamicProperty("SymbAddr", VarName);

            richTextBox1.AppendText(VarName + " variable created.\n");
            richTextBox1.ScrollToCaret();
        }

        public void Internal_Var(string VarName)
        {
            IDriver thisDriver = thisProject.DriverCollection["Driver for internal variables"];
            IDataType thisDatatype = thisProject.DataTypeCollection["STRING"];

            variableCollection.Create(VarName, thisDriver, ChannelType.SystemDriverVariable, thisDatatype);

            IVariable thisVar = variableCollection[VarName];
            thisVar.StringLength = 65535;
            thisVar.SetDynamicProperty("SV_VBA", false);

            if (VarName == "ABB_Diagnosis_ProfileList") { thisVar.SetDynamicProperty("Remanenz", 2); }

            richTextBox1.AppendText(VarName + " variable created.\n");
            richTextBox1.ScrollToCaret();
        }

        public class IED_Driver_Key
        {
            public string DriverFilePath { get; set; }
            public string DriverName { get; set; }
            public int NetAddress { get; set; }
            public string TechnicalKey { get; set; }
        }

        public class LimitValue_Text
        {
            public int LimitValue { get; set; }
            public string LimitText { get; set; }
        }
    }
}
