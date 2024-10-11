using Scada.AddIn.Contracts;
using Scada.AddIn.Contracts.Historian;
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

namespace ReportCreator_EnergyArchiveTool
{
    public partial class Form1 : Form
    {
        IProject thisProject;

        public Form1(IProject mainProject)
        {
            InitializeComponent();
            thisProject = mainProject;

            button2.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Visible = false;
            button1.Visible = false;

            IEditorArchiveCollection editorArchiveCollection = thisProject.EditorArchiveCollection;
            IVariableCollection variableCollection = thisProject.VariableCollection;

            int last_EID = 0;
            foreach (IVariable thisVar in variableCollection)
            {
                string thisVar_name = thisVar.Name;
                string[] thisVar_nameS = thisVar_name.Split('.');
                int thisVar_nameS_cnt = thisVar_nameS.Length;

                // Look for Energy variables
                if (thisVar_nameS[thisVar_nameS_cnt - 1] == "ActiveEnergyFwd" || thisVar_nameS[thisVar_nameS_cnt - 1] == "ActiveEnergyRev" || thisVar_nameS[thisVar_nameS_cnt - 1] == "ReactiveEnergyFwd" || thisVar_nameS[thisVar_nameS_cnt - 1] == "ReactiveEnergyRev")
                {
                    thisVar.SetDynamicProperty("ExternVisible", true);          // For SCADA Logic to use this variable

                    // Count the number of ZEE Energy variables
                    int EnergyVar_cnt = 0;
                    foreach (IVariable EnergyVar in variableCollection)
                    {
                        if (EnergyVar.Name.Contains("ZEE Energy Management.HistorianConfiguration")) { EnergyVar_cnt = EnergyVar_cnt + 1; }
                    }

                    int thisVar_EID = 0;
                    if (EnergyVar_cnt > 0) { thisVar_EID = EnergyVar_cnt / 2; }

                    CreateEnergyTag(thisVar_EID, thisVar_name);         // Create Energy Tag variable
                    CreateEnergyDelta(thisVar_EID);                     // Create Energy delta value variable
                    last_EID = thisVar_EID;

                }
            }

            // Add delta variables to archive E1
            IEditorArchive E1_archive = editorArchiveCollection["E1"];
            for (int i = 0; i < last_EID + 1; i++)
            {
                string thisTagName = "ZEE Energy Management.HistorianConfiguration[" + i + "].ZEE_EnergyManagementBlockGroup[" + i + "].DeltaValue";
                E1_archive.AddVariable(thisTagName, AggregationType.All, null);

                richTextBox1.AppendText(thisTagName + " added to archive E1.\n");
                richTextBox1.ScrollToCaret();
            }

            // Add delta variables with aggregation x4 to archive E2
            IEditorArchive E2_archive = editorArchiveCollection["E2"];
            for (int i = 0; i < last_EID + 1; i++)
            {
                string thisTagName = "ZEE Energy Management.HistorianConfiguration[" + i + "].ZEE_EnergyManagementBlockGroup[" + i + "].DeltaValue";
                E2_archive.AddVariable(thisTagName, AggregationType.Sum, E1_archive);
                E2_archive.AddVariable(thisTagName, AggregationType.Average, E1_archive);
                E2_archive.AddVariable(thisTagName, AggregationType.Minimum, E1_archive);
                E2_archive.AddVariable(thisTagName, AggregationType.Maximum, E1_archive);

                richTextBox1.AppendText(thisTagName + " added to archive E2.\n");
                richTextBox1.ScrollToCaret();
            }

            // Add delta variables with aggregation x4 to archive E3
            IEditorArchive E3_archive = editorArchiveCollection["E3"];
            for (int i = 0; i < last_EID + 1; i++)
            {
                string thisTagName = "ZEE Energy Management.HistorianConfiguration[" + i + "].ZEE_EnergyManagementBlockGroup[" + i + "].DeltaValue";
                E3_archive.AddVariable(thisTagName, AggregationType.Sum, E1_archive);
                E3_archive.AddVariable(thisTagName, AggregationType.Average, E1_archive);
                E3_archive.AddVariable(thisTagName, AggregationType.Minimum, E1_archive);
                E3_archive.AddVariable(thisTagName, AggregationType.Maximum, E1_archive);

                richTextBox1.AppendText(thisTagName + " added to archive E3.\n");
                richTextBox1.ScrollToCaret();
            }

            // Add delta variables with aggregation x4 to archive E4
            IEditorArchive E4_archive = editorArchiveCollection["E4"];
            for (int i = 0; i < last_EID + 1; i++)
            {
                string thisTagName = "ZEE Energy Management.HistorianConfiguration[" + i + "].ZEE_EnergyManagementBlockGroup[" + i + "].DeltaValue";
                E4_archive.AddVariable(thisTagName, AggregationType.Sum, E1_archive);
                E4_archive.AddVariable(thisTagName, AggregationType.Average, E1_archive);
                E4_archive.AddVariable(thisTagName, AggregationType.Minimum, E1_archive);
                E4_archive.AddVariable(thisTagName, AggregationType.Maximum, E1_archive);

                richTextBox1.AppendText(thisTagName + " added to archive E4.\n");
                richTextBox1.ScrollToCaret();
            }

            richTextBox1.AppendText("-------- Operation Completed --------");
            richTextBox1.AppendText("\n");
            richTextBox1.AppendText("\n");
            richTextBox1.ScrollToCaret();

            button2.Visible = true;
        }

        // Create Energy Tag variable
        public void CreateEnergyTag(int thisVar_EID, string thisVar_name)
        {
            string Etag_name = "ZEE Energy Management.HistorianConfiguration[" + thisVar_EID + "].ZEE_EnergyManagementBlockGroup[" + thisVar_EID + "].Tag";
            IDriver InternalDriver = thisProject.DriverCollection["Driver for internal variables"];
            IDataType thisDataType = thisProject.DataTypeCollection["STRING"];

            thisProject.VariableCollection.Create(Etag_name, InternalDriver, ChannelType.SystemDriverVariable, thisDataType);
            IVariable TagVar = thisProject.VariableCollection[Etag_name];

            if (TagVar != null)
            {
                TagVar.SetDynamicProperty("Initial_value", thisVar_name);       // Set original variable name as initial value

                richTextBox1.AppendText("Create variable " + Etag_name + " for " + thisVar_name + ".\n");
                richTextBox1.ScrollToCaret();
            }
        }

        // Create Energy delta variable
        public void CreateEnergyDelta(int thisVar_EID)
        {
            string Edetla_name = "ZEE Energy Management.HistorianConfiguration[" + thisVar_EID + "].ZEE_EnergyManagementBlockGroup[" + thisVar_EID + "].DeltaValue";
            IDriver InternalDriver = thisProject.DriverCollection["Driver for internal variables"];
            IDataType thisDataType = thisProject.DataTypeCollection["UDINT"];

            thisProject.VariableCollection.Create(Edetla_name, InternalDriver, ChannelType.SystemDriverVariable, thisDataType);
            IVariable EdetlaVar = thisProject.VariableCollection[Edetla_name];
            
            if (EdetlaVar != null)
            {
                EdetlaVar.SetDynamicProperty("ExternVisible", true);        // For SCADA Logic to use this variable

                richTextBox1.AppendText("Create variable " + Edetla_name + ".\n");
                richTextBox1.ScrollToCaret();
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
