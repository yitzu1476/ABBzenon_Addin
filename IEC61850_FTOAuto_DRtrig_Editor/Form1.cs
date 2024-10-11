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

namespace IEC61850_FTOAuto_DRtrig_Editor
{
    public partial class Form1 : Form
    {
        IProject thisProject;
        List<IED_Driver_Key> AllIED = new List<IED_Driver_Key>();

        public Form1(IProject MainProject)
        {
            InitializeComponent();

            thisProject = MainProject;
        }

        

        private void button1_Click(object sender, EventArgs e)
        {

            // Selection for symbolic address
            string Relay_SymAddress = "";

            if (checkBox_ABB.Checked) { Relay_SymAddress = "DR/RDRE1/RcdMade/t[ST]"; }
            if (checkBox_ZIV.Checked) { Relay_SymAddress = "PROT/RDRE1/RcdMade/t[ST]"; }
            if (checkBox_Other.Checked) { Relay_SymAddress = textBox1.Text; }

            if (checkBox_ABB.Checked) { if (checkBox_ZIV.Checked || checkBox_Other.Checked) { MessageBox.Show("Please select one."); return; } }
            if (checkBox_ZIV.Checked) { if (checkBox_ABB.Checked || checkBox_Other.Checked) { MessageBox.Show("Please select one."); return; } }
            if (checkBox_Other.Checked) { if (checkBox_ZIV.Checked || checkBox_ABB.Checked) { MessageBox.Show("Please select one."); return; } }


            string ProjectID = thisProject.ProjectId;

            Creator ItemCreator = new Creator(thisProject);
            //ItemCreator.RMCreator();

            string DriverPath = "C:\\ProgramData\\ABB\\SQL2017\\" + ProjectID + "\\FILES\\zenon\\custom\\drivers";
            int DPath_len = DriverPath.Length + 1;

            // Get driver info from editor
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

            // Create variables and modify variables
            foreach (var item in AllIED)
            {
                string varCOM_name = item.TechnicalKey + "!Command";
                string varDIR_name = item.TechnicalKey + "!Directory";
                string varRCD_name = item.TechnicalKey + "!RcdMadeT";
                string varRCD_symb = item.TechnicalKey + "!" + item.TechnicalKey + Relay_SymAddress;

                IVariable varCOM_old = thisProject.VariableCollection[varCOM_name];
                IVariable varDIR_old = thisProject.VariableCollection[varDIR_name];
                IVariable varRCD_old = thisProject.VariableCollection[varRCD_name];

                if (varCOM_old == null) { ItemCreator.VariableCreator(varCOM_name, item.DriverName); }
                if (varDIR_old == null) { ItemCreator.VariableCreator(varDIR_name, item.DriverName); }
                if (varRCD_old == null) { ItemCreator.VariableCreator_Rcd(varRCD_name, item.DriverName); }

                IVariable varCOM = thisProject.VariableCollection[varCOM_name];
                IVariable varDIR = thisProject.VariableCollection[varDIR_name];
                IVariable varRCD = thisProject.VariableCollection[varRCD_name];

                ItemCreator.VariableModify(varCOM, item.NetAddress, varCOM_name, 1000);
                ItemCreator.VariableModify(varDIR, item.NetAddress, varDIR_name, 65535);
                ItemCreator.VariableModify_Rcd(varRCD, item.NetAddress, varRCD_symb);

            }

            // Create variable for collecting DR duration setting (unit: day)
            string TimeVarN = "DR_TimeSetting";

            IVariable TimeVar = thisProject.VariableCollection[TimeVarN];
            if (TimeVar == null)
            {
                IVariableCollection variableCollection = thisProject.VariableCollection;
                IDriver thisDriver = thisProject.DriverCollection["Driver for internal variables"];
                IDataType thisDataType = thisProject.DataTypeCollection["INT"];
                variableCollection.Create(TimeVarN, thisDriver, ChannelType.SystemDriverVariable, thisDataType);

                thisProject.Parent.Parent.DebugPrint(TimeVarN + " variable created.", DebugPrintStyle.Standard);
            }

            IVariable TimeVar_New = thisProject.VariableCollection[TimeVarN];
            if (TimeVar_New != null)
            {
                TimeVar_New.SetDynamicProperty("Remanenz", 2);
                TimeVar_New.SetDynamicProperty("Local", false);
                TimeVar_New.SetDynamicProperty("DDEActive", true);
                TimeVar_New.SetDynamicProperty("SV_VBA", false);

                thisProject.Parent.Parent.DebugPrint(TimeVar_New.Name + " variable modified.", DebugPrintStyle.Standard);
            }



        }

        public class IED_Driver_Key
        {
            public string DriverFilePath { get; set; }
            public string DriverName { get; set; }
            public int NetAddress { get; set; }
            public string TechnicalKey { get; set; }
        }
    }
}
