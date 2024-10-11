using GW_EditorTool;
using Scada.AddIn.Contracts;
using Scada.AddIn.Contracts.Function;
using Scada.AddIn.Contracts.License;
using Scada.AddIn.Contracts.Variable;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Gateway_EditorTool.Panel02_Driver;

namespace Gateway_EditorTool
{
    internal class Panel02_Driver
    {
        IProject thisProject;
        RichTextBox thisRichTextBox;
        Creator_Class ItemCreator;
        List<CO_variables> co_variables = new List<CO_variables>();

        public Panel02_Driver(IProject activeProject, RichTextBox activeRichTextBox)
        {
            thisProject = activeProject;
            thisRichTextBox = activeRichTextBox;
            ItemCreator = new Creator_Class(activeProject, thisRichTextBox);
        }

        public void DriverRCBModify()
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

                        fileContent = fileContent.Replace("Client1", "Client3");
                        fileContent = fileContent.Replace("Client2", "Client4");
                        fileContent = fileContent.Replace("01[", "03[");
                        fileContent = fileContent.Replace("02[", "04[");

                        File.WriteAllText(Dfile, fileContent);
                        thisRichTextBox.AppendText(driverName + " modified!\n");
                        thisRichTextBox.ScrollToCaret();
                    }
                }
            }

            thisRichTextBox.AppendText("-------- End of Operation. --------\n");
            thisRichTextBox.ScrollToCaret();

        }

        public void ControlDriverCreator()
        {
            string ProjectID = thisProject.ProjectId;

            IDriverCollection Drivers = thisProject.DriverCollection;
            foreach (IDriver driver in Drivers)
            {
                if (driver.Name != "IEC850") { continue; }

                string oldDriverName = driver.Identification.ToString();
                string newDriverName = driver.Identification.ToString() + "_CO";
                if (oldDriverName.Substring(oldDriverName.Length - 3, 3) == "_CO") { continue; }
                if (Drivers[newDriverName] != null) { continue; }
                Drivers.Create(newDriverName, "IEC850", false);

                thisRichTextBox.AppendText("Driver " + newDriverName + " created.\n");
                thisRichTextBox.ScrollToCaret();

                string oldDriverFile = "C:\\ProgramData\\ABB\\SQL2017\\" + ProjectID + "\\FILES\\zenon\\custom\\drivers\\IEC850_" + oldDriverName + ".txt";
                string newDriverFile = "C:\\ProgramData\\ABB\\SQL2017\\" + ProjectID + "\\FILES\\zenon\\custom\\drivers\\IEC850_" + newDriverName + ".txt";

                string DriverInfo = "";
                if (File.Exists(oldDriverFile))
                {
                    string fileContent = File.ReadAllText(oldDriverFile);
                    string[] fileLines = fileContent.Split('\n');
                    for (int i = 0; i < 6; i++)
                    {
                        DriverInfo = DriverInfo + fileLines[i] + "\n";
                    }
                }

                string DrivertxtAll = txtContent(DriverInfo);
                File.WriteAllText(newDriverFile, DrivertxtAll);

                driver.InitializeConfiguration();
                driver.SetDynamicProperty("DrvConfig.GenStopPassiveDrv", false);
                driver.EndConfiguration(true);

                thisRichTextBox.AppendText("Driver " + newDriverName + " server modified.\n");
                thisRichTextBox.ScrollToCaret();

            }

            thisRichTextBox.AppendText("-------- End of Operation. --------\n");
            thisRichTextBox.ScrollToCaret();

        }

        public string txtContent(string Driverinfo)
        {
            string txt_afterIP = "102\r\n1000\r\n12\r\n12\r\n1\r\n1\r\n1\r\n1\r\n1\r\n999\r\n999\r\n1\r\n-1\r\n1\r\n0\r\n-1\r\n0\r\n0\r\n0\r\n0\r\n0\r\n0\r\n0\r\n0\r\n10\r\n*\r\n0\r\n0\r\n0\r\n\r\n\r\n0\r\n1\r\n1\r\n0\r\n1\r\n1\r\n0\r\n7000\r\n500\r\n73\r\n7\r\n1\r\n300\r\n0\r\n0\r\n0\r\n2\r\n*** CLIENTCFG ***\r\n3\r\n$SCADA_SERVER1\r\n\r\n\r\n0\r\n0\r\n*** CLIENTCFG ***\r\n3\r\n$SCADA_SERVER2\r\n\r\n\r\n0\r\n0\r\n";
            return Driverinfo + txt_afterIP;
        }

        public void DriverModeVF(List<Form1.IED_Driver_Key> AllIED)
        {
            IVariableCollection variableCollection = thisProject.VariableCollection;

            foreach (var item in AllIED)
            {
                if (item.DriverName.Substring(item.DriverName.Length - 3, 3) == "_CO") { continue; }

                // Variable
                string ModeVarName = item.TechnicalKey + "_DriverMode";
                if (variableCollection[ModeVarName] == null)
                {
                    string DataStr = "BOOL";

                    ItemCreator.VariableCreatorDriverM(ModeVarName, item.DriverName, DataStr);
                    thisRichTextBox.AppendText("Variable " + ModeVarName + " created.\n");
                    thisRichTextBox.ScrollToCaret();
                }

                ItemCreator.VariableModifierDriverM(ModeVarName, item.NetAddress, 5);
                thisRichTextBox.AppendText("Variable " + ModeVarName + " modified.\n");
                thisRichTextBox.ScrollToCaret();

                // Function
                IVariable ModeVar = thisProject.VariableCollection[ModeVarName];
                int DriverInd = int.Parse(ModeVar.GetDynamicProperty("Driver").ToString());
                FunctionType varFunType = (FunctionType)Enum.Parse(typeof(FunctionType), "DriverCommand");

                string FunName_Hardware = item.TechnicalKey + "_DriverMode_H";
                string FunName_Simulation = item.TechnicalKey + "_DriverMode_S";

                IFunctionCollection functionCollection = thisProject.FunctionCollection;
                if (functionCollection[FunName_Hardware] == null)
                {
                    functionCollection.Create(FunName_Hardware, varFunType);
                    thisRichTextBox.AppendText("Function " + FunName_Hardware + " created.\n");
                    thisRichTextBox.ScrollToCaret();
                }

                if (functionCollection[FunName_Simulation] == null)
                {
                    functionCollection.Create(FunName_Simulation, varFunType);
                    thisRichTextBox.AppendText("Function " + FunName_Simulation + " created.\n");
                    thisRichTextBox.ScrollToCaret();
                }

                IFunction Fun_H = functionCollection[FunName_Hardware];
                IFunction Fun_S = functionCollection[FunName_Simulation];

                Fun_H.SetDynamicProperty("Command.Command", 4);
                Fun_H.SetDynamicProperty("Command.DrvIndex", DriverInd);

                Fun_S.SetDynamicProperty("Command.Command", 11);
                Fun_S.SetDynamicProperty("Command.DrvIndex", DriverInd);

                thisRichTextBox.AppendText("Function " + FunName_Hardware + " modified.\n");
                thisRichTextBox.AppendText("Function " + FunName_Simulation + " modified.\n");
                thisRichTextBox.ScrollToCaret();
            }

            // Script
            IScriptCollection scriptCollection = thisProject.ScriptCollection;
            if (scriptCollection["DriverMode_H"] == null && scriptCollection["DriverMode_S"] == null)
            {
                scriptCollection.Create("DriverMode_H");
                scriptCollection.Create("DriverMode_S");
                IScript DriverMode_H_Script = scriptCollection["DriverMode_H"];
                IScript DriverMode_S_Script = scriptCollection["DriverMode_S"];

                IFunctionCollection newFunCollection = thisProject.FunctionCollection;
                foreach (var function in newFunCollection)
                {
                    if (function.Name.Contains("_DriverMode_H"))
                    {
                        int funID = function.Id;
                        DriverMode_H_Script.AddFunction(funID);
                    }

                    if (function.Name.Contains("_DriverMode_S"))
                    {
                        int funID = function.Id;
                        DriverMode_S_Script.AddFunction(funID);
                    }
                }

                thisRichTextBox.AppendText("Script DriverMode_H created.\n");
                thisRichTextBox.AppendText("Script DriverMode_S created.\n");
                thisRichTextBox.ScrollToCaret();

                // Function for script
                FunctionType DM_funType = (FunctionType)Enum.Parse(typeof(FunctionType), "ExecuteScript");
                IFunctionCollection newfunctionColl = thisProject.FunctionCollection;

                newfunctionColl.Create("DriverMode_H_ScriptEXE", DM_funType);
                newfunctionColl.Create("DriverMode_S_ScriptEXE", DM_funType);

                IFunction DriverMode_H_ScriptE = newfunctionColl["DriverMode_H_ScriptEXE"];
                IFunction DriverMode_S_ScriptE = newfunctionColl["DriverMode_S_ScriptEXE"];

                DriverMode_H_ScriptE.SetDynamicProperty("Script", "DriverMode_H");
                DriverMode_S_ScriptE.SetDynamicProperty("Script", "DriverMode_S");

                thisRichTextBox.AppendText("Function DriverMode_H_ScriptEXE created.\n");
                thisRichTextBox.AppendText("Function DriverMode_S_ScriptEXE created.\n");
                thisRichTextBox.ScrollToCaret();
            }

            thisRichTextBox.AppendText("-------- End of Operation. --------\n");
            thisRichTextBox.ScrollToCaret();
        }

        public void ChangeCO_driver()
        {
            IVariableCollection variableCollection = thisProject.VariableCollection;
            foreach (var variable in variableCollection)
            {
                if (variable.Driver.Name != "IEC850") { continue; }

                if (variable.GetDynamicProperty("SymbAddr") == null) { continue; }

                string symbolicAdd = variable.GetDynamicProperty("SymbAddr").ToString();
                if (symbolicAdd.Contains("[CO]"))
                {
                    string DriverName = variable.Driver.Identification;
                    if (DriverName.Contains("_CO")) { continue; }

                    thisRichTextBox.AppendText("Found " + variable.Name + " \n");
                    thisRichTextBox.ScrollToCaret();

                    string varN = variable.Name;
                    int netAdd = variable.NetAddress;
                    string ident = variable.Identification;
                    string newDriverName = DriverName + "_CO";

                    variableCollection.Delete(variable.Name);
                    thisRichTextBox.AppendText("Delete " + varN + " \n");
                    thisRichTextBox.ScrollToCaret();

                    co_variables.Add(new CO_variables
                    {
                        varName = varN,
                        newDriverName = newDriverName,
                        netAddress = netAdd,
                        var_ident = ident,
                        symAddress = symbolicAdd
                    });
                }
            }

            if (co_variables.Count > 0) { MessageBox.Show("Changing drivers..."); }
            else
            {
                thisRichTextBox.AppendText("No control variable to change.\n");
                thisRichTextBox.ScrollToCaret();
            }
            

            IVariableCollection newVariableColl = thisProject.VariableCollection;
            foreach (var CO_var in co_variables)
            {
                if (newVariableColl[CO_var.varName] == null)
                {
                    ItemCreator.VariableCreatorPLC(CO_var.varName, CO_var.newDriverName, "BOOL");
                    ItemCreator.VariableModifierPLC(CO_var.varName, CO_var.netAddress, CO_var.var_ident, "", CO_var.symAddress);
                }
            }

            thisRichTextBox.AppendText("-------- End of Operation. --------\n");
            thisRichTextBox.ScrollToCaret();
        }

        public class CO_variables
        {
            public string varName { get; set; }
            public string newDriverName { get; set; }
            public int netAddress { get; set; }
            public string var_ident { get; set; }
            public string symAddress { get; set; }
        }
    }
}
