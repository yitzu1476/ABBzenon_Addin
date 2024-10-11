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

namespace IEC61850_VariableDiagnosis_81
{
    public partial class Form2 : Form
    {

        IProject thisProject;
        RichTextBox richTextBox1_Form1;
        DataTable table02 = new DataTable();
        public List<GlobalItems.SelectedVar> SelectedVarList = new List<GlobalItems.SelectedVar>();

        string VarName_toVar = "";

        public Form2(IProject form1Project, RichTextBox form1RichTextBox)
        {
            InitializeComponent();
            thisProject = form1Project;
            richTextBox1_Form1 = form1RichTextBox;
            Refresh_button.Visible = false;
            label1.Visible = false;

            dataGridView1.AllowUserToAddRows = false;

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.DataSource = VarAll_Startup();
            dataGridView1.Columns["Select"].Width = 80;
            dataGridView1.Columns["Variable Name"].Width = 400;
            dataGridView1.Columns["Data Type"].Width = 120;

            dataGridView1.Columns["Select"].SortMode = DataGridViewColumnSortMode.Automatic;

            dataGridView1.CellValueChanged += SelectedChanges;
            dataGridView1.Sorted += SortChanges;

            // Update variable selection status
            foreach (var thisItem in GlobalItems.VarSelections)
            {
                if (thisItem.Selection == true)
                {
                    if (GlobalItems.SelectedVar_forall.Exists(x => x.VarName == thisItem.VarName) == false) { thisItem.Selection = false; }
                }
            }

            // Write all variables to table
            if (GlobalItems.VarSelections.Count > 0)
            {
                richTextBox1_Form1.AppendText("Start drawing table...\n");
                richTextBox1_Form1.ScrollToCaret();

                foreach (var thisItem in GlobalItems.VarSelections)
                {
                    VarAll_AddRow(table02, thisItem.VarName, thisItem.VarType, thisItem.VarID, thisItem.SymAddr);
                }
            }
            else
            {
                IVariableCollection variableCollection = thisProject.VariableCollection;
                foreach (IVariable variable in variableCollection)
                {
                    if (variable.Driver.Name.ToString() != "IEC850") { continue; }

                    string thisVarName = variable.Name;
                    string thisVarType = variable.DataType.Name;
                    string thisVarID = variable.Identification;
                    string thisSymbAdd = variable.GetDynamicProperty("SymbAddr").ToString();

                    VarAll_AddRow(table02, thisVarName, thisVarType, thisVarID, thisSymbAdd);

                    GlobalItems.VarSelections.Add(new GlobalItems.VarTable
                    {
                        VarName = thisVarName,
                        VarType = thisVarType,
                        VarID = thisVarID,
                        SymAddr = thisSymbAdd,
                        Selection = false
                    });

                    richTextBox1_Form1.AppendText("Add variable: " + thisVarName + ".\n");
                    richTextBox1_Form1.ScrollToCaret();
                }
            }

            // If some variables are already selected, show refresh button
            if (GlobalItems.SelectedVar_forall.Count > 0)
            {
                dataGridView1.Enabled = false;
                label1.Visible = true;
                Refresh_button.Visible = true;
            }

            richTextBox1_Form1.AppendText("--------\n");
            richTextBox1_Form1.AppendText("\n");
            richTextBox1_Form1.ScrollToCaret();

            richTextBox1_Form1.Visible = false;
            richTextBox1.Visible = false;
        }

        // Show checked state after sorting
        public void SortChanges(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                string thisVarN = row.Cells["Variable Name"].Value.ToString();

                if (GlobalItems.VarSelections.Find(x => x.VarName == thisVarN).Selection == true)
                {
                    row.Cells["Select"].Value = true;
                }
            }
        }

        // If new variable is selected or unselected
        public void SelectedChanges(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name != "Select") { return; }

            // Count selection
            CountSelection();

            // Add or remove variable from VarSelection list
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) { continue; }
                if (row.Cells["Select"].Value == null) { continue; }

                if (row.Cells["Select"].Value.ToString() == "True")
                {
                    string thisVarN = row.Cells["Variable Name"].Value.ToString();
                    GlobalItems.VarSelections.Find(x => x.VarName == thisVarN).Selection = true;
                }

                if (row.Cells["Select"].Value.ToString() == "False")
                {
                    string thisVarN = row.Cells["Variable Name"].Value.ToString();
                    GlobalItems.VarSelections.Find(x => x.VarName == thisVarN).Selection = false;
                }
            }
        }

        // Count and show variable selection
        public void CountSelection()
        {
            int cnt = 0;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow) { continue; }
                if (row.Cells["Select"].Value == null) { continue; }

                if (row.Cells["Select"].Value.ToString() == "True") { cnt = cnt + 1; }
            }

            Label_SelectedCount.Text = cnt.ToString();
        }

        // Update all check box
        private void Refresh_button_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow thisRow in dataGridView1.Rows)
            {
                if (thisRow.IsNewRow) { continue; }
                string thisVarN = thisRow.Cells["Variable Name"].Value.ToString();
                if (GlobalItems.SelectedVar_forall.Exists(x => x.VarName == thisVarN))
                {
                    thisRow.Cells["Select"].Value = true;
                }
            }

            label1.Visible = false;
            Refresh_button.Visible = false;
            dataGridView1.Enabled = true;
        }

        private void confirm_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow thisRow in dataGridView1.Rows)
            {
                if (thisRow.IsNewRow) { continue; }
                if (thisRow.Cells["Select"].Value == null) { continue; }

                if (thisRow.Cells["Select"].Value.ToString() == "True")
                {
                    if (GlobalItems.SelectedVar_forall.Exists(x => x.VarName == thisRow.Cells["Variable Name"].Value.ToString())) { continue; }
                    GlobalItems.SelectedVar_forall.Add(new GlobalItems.SelectedVar { VarName = thisRow.Cells["Variable Name"].Value.ToString() });
                    VarName_toVar = VarName_toVar + thisRow.Cells["Variable Name"].Value.ToString() + ",";
                }

                if (thisRow.Cells["Select"].Value.ToString() == "False")
                {
                    if (GlobalItems.SelectedVar_forall.Exists(x => x.VarName == thisRow.Cells["Variable Name"].Value.ToString()))
                    {
                        var ItemtoRemove = GlobalItems.SelectedVar_forall.Find(x => x.VarName == thisRow.Cells["Variable Name"].Value.ToString());
                        GlobalItems.SelectedVar_forall.Remove(ItemtoRemove);
                    }
                }
            }

            // Add all selected variable to ABB_Diagnosis_VarList, add to temp onlineContainer
            IVariable ABB_Diagnosis_VarList = thisProject.VariableCollection["ABB_Diagnosis_VarList"];
            ABB_Diagnosis_VarList.SetValue(0, VarName_toVar);

            MessageBox.Show("Variable Container Set.");

            this.Close();
        }

        private void SelectAll_Click(object sender, EventArgs e)
        {
            richTextBox1.Visible = true;

            foreach (DataGridViewRow thisRow in dataGridView1.Rows)
            {
                if (thisRow.IsNewRow) { continue; }
                thisRow.Cells["Select"].Value = true;

                richTextBox1.AppendText("Check " + thisRow.Cells["Variable Name"].Value + ".\n");
                richTextBox1.ScrollToCaret();
            }

            CountSelection();
            richTextBox1.Visible = false;
        }

        private void ClearAll_Click(object sender, EventArgs e)
        {

            foreach (DataGridViewRow thisRow in dataGridView1.Rows)
            {
                if (thisRow.IsNewRow) { continue; }
                if (thisRow.Cells["Select"].Value == null) { continue; }
                if (thisRow.Cells["Select"].Value.ToString() == "True")
                {
                    thisRow.Cells["Select"].Value = false;
                }
            }

            CountSelection();
        }

        public void VarAll_AddRow(DataTable table, string VarName, string VarType, string VarID, string symbAdd)
        {
            DataRow row = table.NewRow();
            row["Variable Name"] = VarName;
            row["Data Type"] = VarType;
            row["Identification"] = VarID;
            row["Symbolic Address"] = symbAdd;

            table.Rows.Add(row);
        }

        public DataTable VarAll_Startup()
        {
            DataGridViewCheckBoxColumn checkBoxColumn = new DataGridViewCheckBoxColumn();
            checkBoxColumn.Name = "Select";
            checkBoxColumn.HeaderText = "Select";
            checkBoxColumn.ValueType = typeof(bool);
            checkBoxColumn.SortMode = DataGridViewColumnSortMode.Automatic;

            dataGridView1.Columns.Add(checkBoxColumn);

            table02.Columns.Add("Variable Name");
            table02.Columns.Add("Data Type");
            table02.Columns.Add("Identification");
            table02.Columns.Add("Symbolic Address");

            return table02;
        }

        private void Filter_Include_Click(object sender, EventArgs e)
        {
            string FilterText = textBox1.Text;

            // Clear all table
            while (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
            }

            // Add variables if condition matched
            IVariableCollection variableCollection = thisProject.VariableCollection;
            foreach (IVariable variable in variableCollection)
            {
                string thisVarName = variable.Name;
                if (thisVarName.Contains(FilterText) == false) { continue; }

                string thisVarType = variable.DataType.Name;
                string thisVarID = variable.Identification;
                string thisSymbAdd = variable.GetDynamicProperty("SymbAddr").ToString();

                VarAll_AddRow(table02, thisVarName, thisVarType, thisVarID, thisSymbAdd);
            }

            // Update check box
            foreach (DataGridViewRow thisRow in dataGridView1.Rows)
            {
                if (thisRow.IsNewRow) { continue; }
                string thisVarN = thisRow.Cells["Variable Name"].Value.ToString();
                if (GlobalItems.SelectedVar_forall.Exists(x => x.VarName == thisVarN))
                {
                    thisRow.Cells["Select"].Value = true;
                }
            }

            // Count selection
            CountSelection();
        }

        private void Filter_notInclude_Click(object sender, EventArgs e)
        {
            string FilterText = textBox1.Text;

            while (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
            }

            IVariableCollection variableCollection = thisProject.VariableCollection;
            foreach (IVariable variable in variableCollection)
            {
                string thisVarName = variable.Name;
                if (thisVarName.Contains(FilterText) == true) { continue; }

                string thisVarType = variable.DataType.Name;
                string thisVarID = variable.Identification;
                string thisSymbAdd = variable.GetDynamicProperty("SymbAddr").ToString();

                VarAll_AddRow(table02, thisVarName, thisVarType, thisVarID, thisSymbAdd);
            }

            foreach (DataGridViewRow thisRow in dataGridView1.Rows)
            {
                if (thisRow.IsNewRow) { continue; }
                string thisVarN = thisRow.Cells["Variable Name"].Value.ToString();
                if (GlobalItems.SelectedVar_forall.Exists(x => x.VarName == thisVarN))
                {
                    thisRow.Cells["Select"].Value = true;
                }
                if (GlobalItems.VarSelections.Exists(x => (x.VarName == thisVarN) && (x.Selection == true))) { thisRow.Cells["Select"].Value = true; }
            }

            CountSelection();
        }

        private void Filter_ClearFilter_Click(object sender, EventArgs e)
        {
            // Remove all table
            while (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
            }

            // Add all to table
            foreach (var thisItem in GlobalItems.VarSelections)
            {
                VarAll_AddRow(table02, thisItem.VarName, thisItem.VarType, thisItem.VarID, thisItem.SymAddr);
            }

            // Update check box
            foreach (DataGridViewRow thisRow in dataGridView1.Rows)
            {
                if (thisRow.IsNewRow) { continue; }
                string thisVarN = thisRow.Cells["Variable Name"].Value.ToString();
                if (GlobalItems.SelectedVar_forall.Exists(x => x.VarName == thisVarN))
                {
                    thisRow.Cells["Select"].Value = true;
                }
            }

            // Sort with variable name
            dataGridView1.Sort(dataGridView1.Columns["Variable Name"], ListSortDirection.Ascending);
        }

        private void SelectAll_Selection_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow thisRow in dataGridView1.SelectedRows)
            {
                if (thisRow.IsNewRow) { continue; }
                thisRow.Cells["Select"].Value = true;
            }

            CountSelection();
        }

        // Only show the selected items
        private void ShowAllSelected_Click(object sender, EventArgs e)
        {
            while (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
            }

            foreach (var thisItem in GlobalItems.VarSelections)
            {
                if (thisItem.Selection == true)
                {
                    VarAll_AddRow(table02, thisItem.VarName, thisItem.VarType, thisItem.VarID, thisItem.SymAddr);
                }
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.Cells["Select"].Value = true;
            }
        }
    }
}
