using Scada.AddIn.Contracts;
using Scada.AddIn.Contracts.Variable;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ABB_EnergyWizard_EditorTool
{
    public partial class Form1 : Form
    {
        IProject thisProject;

        public Form1(IProject mainProject)
        {
            InitializeComponent();
            thisProject = mainProject;

            button_OK.Visible = false;
        }

        private void button_Yes_Click(object sender, EventArgs e)
        {
            button_Yes.Visible = false;
            label1.Visible = false;

            IVariableCollection variableCollection = thisProject.VariableCollection;

            foreach (IVariable thisVar in variableCollection)
            {
                // ActiveEnergyFwd
                if (thisVar.Name.Contains("DmdWh/actVal[ST]"))
                {
                    richTextBox1.AppendText("Found variable: " + thisVar.Name + ".\n");
                    richTextBox1.ScrollToCaret();

                    string thisTechnicalKey = thisVar.Name.Split('!')[0];
                    string NewVarName = thisTechnicalKey + ".MX.ActiveEnergyFwd";

                    if (variableCollection[NewVarName] != null) { continue; }

                    thisVar.Name = NewVarName;

                    richTextBox1.AppendText("Change to: " + thisVar.Name + ".\n");
                    richTextBox1.ScrollToCaret();
                }

                // ActiveEnergyRev
                if (thisVar.Name.Contains("SupWh/actVal[ST]"))
                {
                    richTextBox1.AppendText("Found variable: " + thisVar.Name + ".\n");
                    richTextBox1.ScrollToCaret();

                    string thisTechnicalKey = thisVar.Name.Split('!')[0];
                    string NewVarName = thisTechnicalKey + ".MX.ActiveEnergyRev";

                    if (variableCollection[NewVarName] != null) { continue; }

                    thisVar.Name = NewVarName;

                    richTextBox1.AppendText("Change to: " + thisVar.Name + ".\n");
                    richTextBox1.ScrollToCaret();
                }

                // ReactiveEnergyFwd
                if (thisVar.Name.Contains("DmdVArh/actVal[ST]"))
                {
                    richTextBox1.AppendText("Found variable: " + thisVar.Name + ".\n");
                    richTextBox1.ScrollToCaret();

                    string thisTechnicalKey = thisVar.Name.Split('!')[0];
                    string NewVarName = thisTechnicalKey + ".MX.ReactiveEnergyFwd";

                    if (variableCollection[NewVarName] != null) { continue; }

                    thisVar.Name = NewVarName;

                    richTextBox1.AppendText("Change to: " + thisVar.Name + ".\n");
                    richTextBox1.ScrollToCaret();
                }

                // ReactiveEnergyRev
                if (thisVar.Name.Contains("SupVArh/actVal[ST]"))
                {
                    richTextBox1.AppendText("Found variable: " + thisVar.Name + ".\n");
                    richTextBox1.ScrollToCaret();

                    string thisTechnicalKey = thisVar.Name.Split('!')[0];
                    string NewVarName = thisTechnicalKey + ".MX.ReactiveEnergyRev";

                    if (variableCollection[NewVarName] != null) { continue; }

                    thisVar.Name = NewVarName;

                    richTextBox1.AppendText("Change to: " + thisVar.Name + ".\n");
                    richTextBox1.ScrollToCaret();
                }



                if (thisVar.Name.Contains("ZEE Energy Management.HistorianConfiguration") && thisVar.Name.Contains("DeltaValue"))
                {
                    string EnergyProfile = thisVar.Name.Replace(".DeltaValue", "").Trim();

                    string EnergyName_VarName = EnergyProfile + ".Name";
                    string EnergyTag_VarName = EnergyProfile + ".Tag";
                    IVariable EnergyName_Var = variableCollection[EnergyName_VarName];
                    IVariable EnergyTag_Var = variableCollection[EnergyTag_VarName];

                    if (EnergyName_Var == null || EnergyTag_Var == null) { continue; }

                    string EnergyName_OriginalV = EnergyName_Var.GetDynamicProperty("Initial_value").ToString();
                    string OriginalV_TechnicalKey = EnergyName_OriginalV.Split('!')[0];

                    string OriginalV_NewName = "";
                    if (EnergyName_OriginalV.Contains("DmdWh/actVal[ST]")) { OriginalV_NewName = OriginalV_TechnicalKey + ".MX.ActiveEnergyFwd"; }
                    if (EnergyName_OriginalV.Contains("SupWh/actVal[ST]")) { OriginalV_NewName = OriginalV_TechnicalKey + ".MX.ActiveEnergyRev"; }
                    if (EnergyName_OriginalV.Contains("DmdVArh/actVal[ST]")) { OriginalV_NewName = OriginalV_TechnicalKey + ".MX.ReactiveEnergyFwd"; }
                    if (EnergyName_OriginalV.Contains("SupVArh/actVal[ST]")) { OriginalV_NewName = OriginalV_TechnicalKey + ".MX.ReactiveEnergyRev"; }


                    thisVar.Identification = OriginalV_NewName;
                    thisVar.ResourcesLabel = OriginalV_TechnicalKey;
                    EnergyName_Var.SetDynamicProperty("Initial_value", OriginalV_NewName);
                    EnergyTag_Var.SetDynamicProperty("Initial_value", OriginalV_NewName);

                    thisProject.Parent.Parent.DebugPrint(thisVar.Name + " variable modified.", DebugPrintStyle.Standard);

                    richTextBox1.AppendText(thisVar.Name + " variable modified.\n");
                    richTextBox1.ScrollToCaret();

                }
            }

            richTextBox1.AppendText("-------- Operation Completed --------");
            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("\n");
            richTextBox1.ScrollToCaret();

            button_OK.Visible = true;
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
