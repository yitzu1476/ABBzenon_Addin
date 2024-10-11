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
    public partial class Form1 : Form
    {
        IProject thisProject;
        DataTable table01 = new DataTable();
        DataTable table02 = new DataTable();
        List<LimitValue_Text> TCP_mms_Status_Text = new List<LimitValue_Text>();
        List<LimitValue_Text> COT_Text = new List<LimitValue_Text>();
        List<LimitValue_Text> ValueState_Text = new List<LimitValue_Text>();
        List<LimitValue_Text> Quality_Text = new List<LimitValue_Text>();
        List<LimitValue_Text> AddCause_Text = new List<LimitValue_Text>();

        public Form1(IProject mainProject)
        {
            InitializeComponent();
            thisProject = mainProject;

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.DataSource = DataTable_Startup();

            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView2.DataSource = DataTable2_Startup();

            comboBox1.SelectedIndexChanged += ProfileSelected;

            // Create lists for all category
            AllList_Startup();

            // Collect profile info from profile variable
            if (GlobalItems.ProfileList.Count > 0)
            {
                foreach (var profileC in GlobalItems.ProfileList)
                {
                    if (comboBox1.Items.Contains(profileC.ProfileName) == false) { comboBox1.Items.Add(profileC.ProfileName); }
                }
            }

            richTextBox1.Visible = false;
        }

        private void SelectVar_Click(object sender, EventArgs e)
        {
            richTextBox1.Visible = true;

            richTextBox1.AppendText("Start collecting variables...\n");
            richTextBox1.ScrollToCaret();

            // Clear all datagridview
            if (GlobalItems.SelectedVar_forall.Count > 0)
            {
                while (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
                }

                while (dataGridView2.Rows.Count > 0)
                {
                    dataGridView2.Rows.RemoveAt(dataGridView2.Rows.Count - 1);
                }
            }

            Form2 form2 = new Form2(thisProject, richTextBox1);
            form2.ShowDialog();
            form2.Activate();

            IVariableCollection variableCollection = thisProject.VariableCollection;

            // Show CO variable in table 2, and the others in table 1
            foreach (var SelectedV in GlobalItems.SelectedVar_forall)
            {
                bool CO_var = false;
                string thisVarName = SelectedV.VarName;
                IVariable thisVar = variableCollection[thisVarName];

                if (thisVar.Driver.Name == "IEC850")
                {
                    if (thisVar.GetDynamicProperty("ID_DriverTyp").ToString() == "8")
                    {

                        if (thisVar.GetDynamicProperty("SymbAddr").ToString().Contains("ctlVal[CO]"))
                        {

                            CO_var = true;
                            DataTable2_AddRow(table02, thisVarName, thisVar.Driver.Identification, "", "", "", "", "", "");
                        }
                    }
                }

                if (CO_var == false)
                {
                    DataTable_AddRow(table01, thisVarName, thisVar.Driver.Identification, "", "", "", "", "", "", "", "", "", "");
                }
            }
        }

        private void Refresh_var_Click(object sender, EventArgs e)
        {
            richTextBox1.Visible = true;
            richTextBox1.AppendText("Start refreshing...\n");
            richTextBox1.ScrollToCaret();

            // Get all status of variables
            foreach (DataGridViewRow thisRow in dataGridView1.Rows)
            {
                if (thisRow.IsNewRow) { continue; }
                string thisVarN = thisRow.Cells["Variable Name"].Value.ToString();
                IVariable thisVar = thisProject.VariableCollection[thisVarN];
                string thisTime = thisVar.LastUpdateTime.ToString() + "." + thisVar.LastUpdateTimeMilliSeconds.ToString();

                richTextBox1.AppendText(thisVarN + ": \n");
                richTextBox1.ScrollToCaret();

                string[] tempConnection = ConnectionState_value(thisVarN);

                thisRow.Cells["Last Update Time"].Value = thisTime;
                thisRow.Cells["TCP Connection Status"].Value = tempConnection[0];
                thisRow.Cells["mms Status"].Value = tempConnection[1];
                thisRow.Cells["Driver Connection"].Value = ConnectionStates(thisVarN);
                thisRow.Cells["Actual Value"].Value = thisVar.GetValue(0);
                thisRow.Cells["Cause of Transmission"].Value = VariableCOT(thisVarN);
                thisRow.Cells["Timestamp Code"].Value = Timestamp_ErrorCode(thisVarN);
                thisRow.Cells["Variable Status"].Value = ValueState(thisVarN);
                thisRow.Cells["Quality"].Value = QualityState(thisVarN);
            }

            foreach (DataGridViewRow thisRow2 in dataGridView2.Rows)
            {
                if (thisRow2.IsNewRow) { continue; }
                string thisVarN = thisRow2.Cells["Variable Name"].Value.ToString();
                IVariable thisVar = thisProject.VariableCollection[thisVarN];
                string thisTime = thisVar.LastUpdateTime.ToString() + "." + thisVar.LastUpdateTimeMilliSeconds.ToString();

                richTextBox1.AppendText(thisVarN + ": \n");
                richTextBox1.ScrollToCaret();

                thisRow2.Cells["Last Update Time"].Value = thisTime;
                thisRow2.Cells["TCP Connection Status"].Value = ConnectionState_value(thisVarN)[0];
                thisRow2.Cells["Actual Value"].Value = thisVar.GetValue(0).ToString();
                thisRow2.Cells["Cause of Transmission"].Value = VariableCOT(thisVarN);
                thisRow2.Cells["AddCause"].Value = CO_AddCause(thisVarN);
            }

            richTextBox1.Visible = false;
        }

        // Collect all connection state (TCP, mms) from server !ConnectionState
        public string[] ConnectionState_value(string thisVarName)
        {
            richTextBox1.AppendText("Collecting connection statue...\n");
            richTextBox1.ScrollToCaret();

            IVariableCollection variableCollection = thisProject.VariableCollection;
            IVariable thisVar = variableCollection[thisVarName];
            string thisTCPStatus = "";
            string thismmsStatus = "";
            string[] TCP_mms_strings = new string[] { thisTCPStatus, thismmsStatus };

            // Find !ConnectionState of the server
            if (thisVar.Driver.Name == "IEC850")
            {
                if (thisVar.GetDynamicProperty("ID_DriverTyp").ToString() == "35") { return TCP_mms_strings; }

                int ConnValue = 0;
                if (thisVar.GetDynamicProperty("SymbAddr").ToString() == "*!ConnectionState")
                {
                    ConnValue = int.Parse(thisVar.GetValue(0).ToString());
                }
                else
                {
                    string ConnVarName = "";

                    foreach (var TempVar in variableCollection)
                    {
                        if (TempVar.Driver.Identification == thisVar.Driver.Identification)
                        {
                            if (TempVar.NetAddress == thisVar.NetAddress)
                            {
                                if (TempVar.GetDynamicProperty("SymbAddr").ToString() == "*!ConnectionState")
                                {
                                    ConnVarName = TempVar.Name;
                                }
                            }
                        }
                    }

                    IVariable ConnVar = variableCollection[ConnVarName];

                    ConnValue = int.Parse(ConnVar.GetValue(0).ToString());
                }

                string ConnValueBin = "";
                ConnValueBin = Convert.ToString(ConnValue, 2);
                ConnValueBin = Reverse(ConnValueBin);

                char[] ConnBin_char = ConnValueBin.ToCharArray();

                // Check status from list
                if (ConnBin_char.Length > 1)
                {
                    for (int i = 0; i < ConnBin_char.Length; i++)
                    {
                        if (ConnBin_char[i] == '1')
                        {
                            if (TCP_mms_Status_Text.Exists(x => (x.LimitValue == i) && (x.Category == "TCP")))
                            {
                                thisTCPStatus = thisTCPStatus + TCP_mms_Status_Text.Find(x => (x.LimitValue == i) && (x.Category == "TCP")).LimitText + " ";
                            }

                            if (TCP_mms_Status_Text.Exists(x => (x.LimitValue == i) && (x.Category == "mms")))
                            {
                                thismmsStatus = thismmsStatus + TCP_mms_Status_Text.Find(x => (x.LimitValue == i) && (x.Category == "mms")).LimitText + " ";
                            }
                        }
                    }
                }
            }

            TCP_mms_strings = new string[] { thisTCPStatus, thismmsStatus };
            return TCP_mms_strings;
        }

        // Get driver connection status from driver communication variable (Offset 61)
        public string ConnectionStates(string thisVarName)
        {
            richTextBox1.AppendText("Collecting connection state...\n");
            richTextBox1.ScrollToCaret();

            string Driver_ConnStates = "";

            IVariableCollection variableCollection = thisProject.VariableCollection;
            IVariable thisVar = variableCollection[thisVarName];

            if (thisVar.Driver.Name == "IEC850")
            {
                int thisNetA = thisVar.NetAddress;

                string thisComm_VarName = thisVar.Driver.Identification + "!Communication";
                IVariable thisComm_Var = variableCollection[thisComm_VarName];

                string Driver_Conn_Allstring = thisComm_Var.GetValue(0).ToString();
                string[] Driver_Conn_AllServer = Driver_Conn_Allstring.Split(';');

                foreach (string Driver_Conn_string in Driver_Conn_AllServer)
                {
                    if (Driver_Conn_string.Length > 2)
                    {
                        string[] Driver_Conn = Driver_Conn_string.Split(':');
                        if (Driver_Conn[0] == thisNetA.ToString())
                        {
                            if (Driver_Conn[1].Trim() == "0") { Driver_ConnStates = "Connection OK"; }
                            if (Driver_Conn[1].Trim() == "1") { Driver_ConnStates = "Connection failure"; }
                            if (Driver_Conn[1].Trim() == "2") { Driver_ConnStates = "Connection simulated"; }
                        }
                    }
                }
            }

            return Driver_ConnStates;
        }

        // Get COT of the variable from variable binary status
        public string VariableCOT(string thisVarName)
        {
            richTextBox1.AppendText("Collecting cause of transmission...\n");
            richTextBox1.ScrollToCaret();

            string COT_limittext = "NA";

            IVariable thisVar = thisProject.VariableCollection[thisVarName];
            int HighStatus = thisVar.HigherState;
            if (HighStatus == 0) { return COT_limittext; }

            string HighStatus_bin = Convert.ToString(HighStatus, 2);
            HighStatus_bin = Reverse(HighStatus_bin);

            double sum_bin = 0;
            char[] HighStatus_chars = HighStatus_bin.ToCharArray();
            for (int i = 0; i < HighStatus_chars.Length; i++)
            {
                if (i > 5) { continue; }
                if (HighStatus_chars[i] == '1')
                {
                    double thisP = Math.Pow(2, i);
                    sum_bin = sum_bin + thisP;
                }
            }

            // Check COT from list
            if (COT_Text.Exists(x => x.LimitValue == sum_bin))
            {
                COT_limittext = COT_Text.Find(x => x.LimitValue == sum_bin).LimitText;
            }
            else { COT_limittext = "COT error" + sum_bin; }

            return COT_limittext;
        }

        // Get timestamp code from variable binary status
        public string Timestamp_ErrorCode(string thisVarName)
        {
            richTextBox1.AppendText("Collecting timestamp code...\n");
            richTextBox1.ScrollToCaret();

            string Time_limittext = "";

            IVariable thisVar = thisProject.VariableCollection[thisVarName];
            int HighStatus = thisVar.HigherState;
            if (HighStatus == 0) { return Time_limittext; }

            string HighStatus_bin = Convert.ToString(HighStatus, 2);
            HighStatus_bin = Reverse(HighStatus_bin);

            char[] HighStatus_chars = HighStatus_bin.ToCharArray();
            if (HighStatus_chars.Length < 18) { return Time_limittext; }

            if (HighStatus_chars[17] == '1') { Time_limittext = Time_limittext + "T_INVAL"; }
            if (HighStatus_chars.Length < 22) { return Time_limittext; }
            else
            {
                if (HighStatus_chars[21] == '1') { Time_limittext = Time_limittext + "T_UNSYNC"; return Time_limittext; }
                else { return Time_limittext; }
            }
        }

        // Get value state from variable binary status
        public string ValueState(string thisVarName)
        {
            richTextBox1.AppendText("Collecting variable state...\n");
            richTextBox1.ScrollToCaret();

            string Value_state = "NA";

            IVariable thisVar = thisProject.VariableCollection[thisVarName];
            int LowerState = thisVar.LowerState;
            if (LowerState == 0) { return Value_state; }

            string LowerState_bin = Convert.ToString(LowerState, 2);
            LowerState_bin = Reverse(LowerState_bin);

            // Check value status from list
            char[] LowerState_chars = LowerState_bin.ToCharArray();
            for (int i = 0; i < LowerState_chars.Length; i++)
            {
                if (LowerState_chars[i] == '1')
                {
                    if (ValueState_Text.Exists(x => x.LimitValue == i))
                    {
                        Value_state = ValueState_Text.Find(x => x.LimitValue == i).LimitText;
                    }
                }
            }

            return Value_state;
        }

        // Get quality state from variable binary status
        public string QualityState(string thisVarName)
        {
            richTextBox1.AppendText("Collecting quality state...\n");
            richTextBox1.ScrollToCaret();

            string quality_state = "";
            IVariable thisVar = thisProject.VariableCollection[thisVarName];
            int HighState = thisVar.HigherState;
            if (HighState == 0) { return quality_state; }

            string HighState_bin = Convert.ToString(HighState, 2);
            HighState_bin = Reverse(HighState_bin);

            // Check quality from list
            char[] HighState_chars = HighState_bin.ToCharArray();
            for (int i = 0; i < HighState_chars.Length; i++)
            {
                if (HighState_chars[i] == '1')
                {
                    if (Quality_Text.Exists(x => x.LimitValue == i))
                    {
                        quality_state = quality_state + Quality_Text.Find(x => x.LimitValue == i).LimitText + ",";
                    }
                }
            }
            return quality_state;
        }

        // Get AddCasue of Control variable
        public string CO_AddCause(string thisVarName)
        {
            richTextBox1.AppendText("Collecting addCause...\n");
            richTextBox1.ScrollToCaret();

            string AddC_text = "";
            IVariableCollection variableCollection = thisProject.VariableCollection;
            IVariable thisVar = variableCollection[thisVarName];

            if (thisVar.GetDynamicProperty("SymbAddr").ToString().Contains("ctlVal[CO]"))
            {
                string thisVar_Symb = thisVar.GetDynamicProperty("SymbAddr").ToString();
                string AddC_VarName = thisVar_Symb.Replace("ctlVal[CO]", "AddCause").Trim();

                // Check AddCause from list
                IVariable AddC_Var = variableCollection[AddC_VarName];
                if (AddC_Var == null)
                {
                    AddC_text = "No AddCause Var";
                    return AddC_text;
                }

                int AddC_num = int.Parse(AddC_Var.GetValue(0).ToString());
                if (AddCause_Text.Exists(x => x.LimitValue == AddC_num))
                {
                    AddC_text = AddCause_Text.Find(x => x.LimitValue == AddC_num).LimitText;
                    return AddC_text;
                }

                AddC_text = "No AddCause Var";
                return AddC_text;
            }

            return AddC_text;
        }

        private void CreateProfile_Click(object sender, EventArgs e)
        {
            // Remove profile info if profile name is already existed
            string ProfileN = textBox1.Text;
            while (GlobalItems.ProfileList.Exists(x => x.ProfileName == ProfileN) == true)
            {
                var thisProfileItem = GlobalItems.ProfileList.Find(x => x.ProfileName == ProfileN);
                GlobalItems.ProfileList.Remove(thisProfileItem);

            }

            // Add table 1 variable to profile info
            foreach (DataGridViewRow thisRow in dataGridView1.Rows)
            {
                GlobalItems.ProfileList.Add(new GlobalItems.ProfileContent
                {
                    ProfileName = ProfileN,
                    VarName = thisRow.Cells["Variable Name"].Value.ToString()
                });
            }

            // Add table 2 variable to profile info
            foreach (DataGridViewRow thisRow in dataGridView2.Rows)
            {
                GlobalItems.ProfileList.Add(new GlobalItems.ProfileContent
                {
                    ProfileName = ProfileN,
                    VarName = thisRow.Cells["Variable Name"].Value.ToString()
                });
            }

            // Add profile name to combo box
            comboBox1.Items.Clear();
            foreach (var Proitem in GlobalItems.ProfileList)
            {
                if (comboBox1.Items.Contains(Proitem.ProfileName)) { continue; }
                comboBox1.Items.Add(Proitem.ProfileName);
            }
        }

        public void ProfileSelected(object sender, EventArgs e)
        {
            // Clear tables of datagridview
            while (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
            }

            while (dataGridView2.Rows.Count > 0)
            {
                dataGridView2.Rows.RemoveAt(dataGridView2.Rows.Count - 1);
            }

            GlobalItems.SelectedVar_forall.Clear();
            string NowProfile = comboBox1.Text;
            IVariableCollection variableCollection = thisProject.VariableCollection;

            string VarName_toVar = "";

            // Add variables to each table with the selected profile name
            foreach (var ProfileItem in GlobalItems.ProfileList)
            {
                if (ProfileItem.ProfileName == NowProfile)
                {
                    string thisVarName = ProfileItem.VarName;
                    IVariable thisVar = variableCollection[thisVarName];

                    bool CO_var = false;
                    if (thisVar.Driver.Name == "IEC850")
                    {
                        if (thisVar.GetDynamicProperty("ID_DriverTyp").ToString() == "8")
                        {
                            if (thisVar.GetDynamicProperty("SymbAddr").ToString().Contains("ctlVal[CO]"))
                            {
                                CO_var = true;
                                VarName_toVar = VarName_toVar + thisVarName + ",";
                                DataTable2_AddRow(table02, thisVarName, thisVar.Driver.Identification, "", "", "", "", "", "");
                            }
                        }
                    }

                    if (CO_var == false)
                    {
                        VarName_toVar = VarName_toVar + thisVarName + ",";
                        DataTable_AddRow(table01, thisVarName, thisVar.Driver.Identification, "", "", "", "", "", "", "", "", "", "");
                    }

                    if (GlobalItems.SelectedVar_forall.Exists(x => x.VarName == thisVarName) == false)
                    {
                        GlobalItems.SelectedVar_forall.Add(new GlobalItems.SelectedVar { VarName = thisVarName });
                    }
                }

            }

            // Set variables to temp onlineContainer
            IVariable ABB_Diagnosis_VarList = thisProject.VariableCollection["ABB_Diagnosis_VarList"];
            ABB_Diagnosis_VarList.SetValue(0, VarName_toVar);

            textBox1.Text = NowProfile;
        }

        // Open screen for profile manager
        private void Profile_Manager_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.ShowDialog();
            form4.Activate();

            comboBox1.Items.Clear();
            foreach (var Proitem in GlobalItems.ProfileList)
            {
                if (comboBox1.Items.Contains(Proitem.ProfileName)) { continue; }
                comboBox1.Items.Add(Proitem.ProfileName);
            }
        }

        // Select column for Form2
        private void ColumnSelection_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.ShowDialog();
            form3.Activate();

            if (GlobalItems.ColumnList.Count != 0)
            {
                foreach (var thisColumn in GlobalItems.ColumnList)
                {
                    if (thisColumn.ColumnCheck == true)
                    {
                        dataGridView1.Columns[thisColumn.ColumnName].Visible = true;
                    }
                    else
                    {
                        dataGridView1.Columns[thisColumn.ColumnName].Visible = false;
                    }
                }
            }

            if (GlobalItems.ColumnList2.Count != 0)
            {
                foreach (var thisColumn in GlobalItems.ColumnList2)
                {
                    if (thisColumn.ColumnCheck == true)
                    {
                        dataGridView2.Columns[thisColumn.ColumnName].Visible = true;
                    }
                    else
                    {
                        dataGridView2.Columns[thisColumn.ColumnName].Visible = false;
                    }
                }
            }
        }

        private void DeleteSelected_Click(object sender, EventArgs e)
        {
            // Add selected variable names to tmep string
            string RowstoDelete = "";

            foreach (DataGridViewRow SelectedRow in dataGridView1.SelectedRows)
            {
                string SelectedRow_Name = SelectedRow.Cells["Variable Name"].Value.ToString();
                RowstoDelete = RowstoDelete + SelectedRow_Name + ",";
            }

            foreach (DataGridViewRow SelectedRow in dataGridView2.SelectedRows)
            {
                string SelectedRow_Name = SelectedRow.Cells["Variable Name"].Value.ToString();
                RowstoDelete = RowstoDelete + SelectedRow_Name + ",";
            }

            // Remove all variables in the tables
            while (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows.RemoveAt(dataGridView1.Rows.Count - 1);
            }

            while (dataGridView2.Rows.Count > 0)
            {
                dataGridView2.Rows.RemoveAt(dataGridView2.Rows.Count - 1);
            }

            // Add back variables if they are not added to the remove string
            string[] RowtoDelete_split = RowstoDelete.Split(',');
            foreach (var thisData in GlobalItems.SelectedVar_forall)
            {
                bool toDelete = false;
                foreach (string Row_Name in RowtoDelete_split)
                {
                    if (Row_Name.Length < 1) { continue; }
                    if (Row_Name == thisData.VarName) { toDelete = true; break; }
                }

                if (toDelete == false)
                {
                    string thisVarName = thisData.VarName;
                    IVariable thisVar = thisProject.VariableCollection[thisVarName];

                    bool CO_var = false;
                    if (thisVar.Driver.Name == "IEC850")
                    {
                        if (thisVar.GetDynamicProperty("ID_DriverTyp").ToString() == "8")
                        {
                            if (thisVar.GetDynamicProperty("SymbAddr").ToString().Contains("ctlVal[CO]"))
                            {
                                CO_var = true;
                                DataTable2_AddRow(table02, thisVarName, thisVar.Driver.Identification, "", "", "", "", "", "");
                            }
                        }
                    }

                    if (CO_var == false)
                    {
                        DataTable_AddRow(table01, thisVarName, thisVar.Driver.Identification, "", "", "", "", "", "", "", "", "", "");
                    }
                }
            }

            // Remove selected variables from SelectedVar list
            foreach (string Row_Name in RowtoDelete_split)
            {
                while (GlobalItems.SelectedVar_forall.Exists(x => x.VarName == Row_Name))
                {
                    var ItemtoRemove = GlobalItems.SelectedVar_forall.Find(x => x.VarName == Row_Name);
                    GlobalItems.SelectedVar_forall.Remove(ItemtoRemove);
                }
            }
        }

        // Write selected variable value to actual variable
        private void Set_Selected_CO_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow SelectedRow in dataGridView2.SelectedRows)
            {
                string thisVarName = SelectedRow.Cells["Variable Name"].Value.ToString();
                string thisVarSetV = SelectedRow.Cells["Set Value"].Value.ToString();

                IVariable thisVar = thisProject.VariableCollection[thisVarName];
                IDataType thisDataType = thisVar.DataType;

                thisVar.SetValue(0, thisVarSetV);
            }
        }

        // Write all control variable value to actual variable
        private void Set_All_CO_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow thisRow in dataGridView2.Rows)
            {
                string thisVarName = thisRow.Cells["Variable Name"].Value.ToString();
                string thisVarSetV = thisRow.Cells["Set Value"].Value.ToString();

                IVariable thisVar = thisProject.VariableCollection[thisVarName];
                IDataType thisDataType = thisVar.DataType;

                thisVar.SetValue(0, thisVarSetV);
            }
        }

        public void DataTable_AddRow(DataTable table, string VarName, string DriverName, string LastTime, string TCPStatus, string mmsStatus, string DriverConn, string actualV, string COT_t, string Time_code, string Var_state, string Var_q, string CO_addC)
        {
            DataRow row = table.NewRow();
            row["Variable Name"] = VarName;
            row["Driver Name"] = DriverName;
            row["Last Update Time"] = LastTime;
            row["TCP Connection Status"] = TCPStatus;
            row["mms Status"] = mmsStatus;
            row["Driver Connection"] = DriverConn;
            row["Actual Value"] = actualV;
            row["Cause of Transmission"] = COT_t;
            row["Timestamp Code"] = Time_code;
            row["Variable Status"] = Var_state;
            row["Quality"] = Var_q;

            table.Rows.Add(row);
        }

        public void DataTable2_AddRow(DataTable table, string VarName, string DriverName, string LastTime, string TCPStatus, string ActualV, string SetValue, string COT_t, string AddC_t)
        {
            DataRow row = table.NewRow();
            row["Variable Name"] = VarName;
            row["Driver Name"] = DriverName;
            row["Last Update Time"] = LastTime;
            row["TCP Connection Status"] = TCPStatus;
            row["Actual Value"] = ActualV;
            row["Set Value"] = SetValue;
            row["Cause of Transmission"] = COT_t;
            row["AddCause"] = AddC_t;

            table.Rows.Add(row);
        }

        public void AllList_Startup()
        {
            TCP_mms_Status_Text.Add(new LimitValue_Text { LimitValue = 1, LimitText = "1: TCP connected", Category = "TCP"});
            TCP_mms_Status_Text.Add(new LimitValue_Text { LimitValue = 2, LimitText = "1: TCP connecting", Category = "TCP" });
            TCP_mms_Status_Text.Add(new LimitValue_Text { LimitValue = 3, LimitText = "1: TCP connect failed", Category = "TCP" });
            TCP_mms_Status_Text.Add(new LimitValue_Text { LimitValue = 16, LimitText = "1: mms associated", Category = "mms" });
            TCP_mms_Status_Text.Add(new LimitValue_Text { LimitValue = 17, LimitText = "1: mms rcb enable failed", Category = "mms" });

            TCP_mms_Status_Text.Add(new LimitValue_Text { LimitValue = 5, LimitText = "2: TCP connected", Category = "TCP" });
            TCP_mms_Status_Text.Add(new LimitValue_Text { LimitValue = 6, LimitText = "2: TCP connecting", Category = "TCP" });
            TCP_mms_Status_Text.Add(new LimitValue_Text { LimitValue = 7, LimitText = "2: TCP connect failed", Category = "TCP" });
            TCP_mms_Status_Text.Add(new LimitValue_Text { LimitValue = 24, LimitText = "2: mms associated", Category = "mms" });
            TCP_mms_Status_Text.Add(new LimitValue_Text { LimitValue = 25, LimitText = "2: mms rcb enable failed", Category = "mms" });

            COT_Text.Add(new LimitValue_Text { LimitValue = 1, LimitText = "Polling", Category = "COT" });
            COT_Text.Add(new LimitValue_Text { LimitValue = 2, LimitText = "Integrity", Category = "COT" });
            COT_Text.Add(new LimitValue_Text { LimitValue = 3, LimitText = "Report", Category = "COT" });
            COT_Text.Add(new LimitValue_Text { LimitValue = 7, LimitText = "Activation confirmation", Category = "COT" });
            COT_Text.Add(new LimitValue_Text { LimitValue = 10, LimitText = "Activation termination", Category = "COT" });
            COT_Text.Add(new LimitValue_Text { LimitValue = 20, LimitText = "General Interrogation", Category = "COT" });

            ValueState_Text.Add(new LimitValue_Text { LimitValue = 12, LimitText = "MAN_VAL", Category = "ValueState" });
            ValueState_Text.Add(new LimitValue_Text { LimitValue = 17, LimitText = "SPONT", Category = "ValueState" });
            ValueState_Text.Add(new LimitValue_Text { LimitValue = 18, LimitText = "INVALID", Category = "ValueState" });
            ValueState_Text.Add(new LimitValue_Text { LimitValue = 27, LimitText = "ALT_VAL", Category = "ValueState" });

            Quality_Text.Add(new LimitValue_Text { LimitValue = 7, LimitText = "Test", Category = "Q" });
            Quality_Text.Add(new LimitValue_Text { LimitValue = 12, LimitText = "Operator Blocked", Category = "Q" });
            Quality_Text.Add(new LimitValue_Text { LimitValue = 13, LimitText = "Substituted", Category = "Q" });
            Quality_Text.Add(new LimitValue_Text { LimitValue = 15, LimitText = "Overflow", Category = "Q" });
            Quality_Text.Add(new LimitValue_Text { LimitValue = 20, LimitText = "Out of Range", Category = "Q" });

            AddCause_Text.Add(new LimitValue_Text { LimitValue = 0, LimitText = "Unknown", Category = "AddC" });
            AddCause_Text.Add(new LimitValue_Text { LimitValue = 1, LimitText = "Not supported", Category = "AddC" });
            AddCause_Text.Add(new LimitValue_Text { LimitValue = 2, LimitText = "Blocked by switching hierarchy", Category = "AddC" });
            AddCause_Text.Add(new LimitValue_Text { LimitValue = 3, LimitText = "Select failed", Category = "AddC" });
            AddCause_Text.Add(new LimitValue_Text { LimitValue = 4, LimitText = "Invalid position", Category = "AddC" });
            AddCause_Text.Add(new LimitValue_Text { LimitValue = 5, LimitText = "Position reached", Category = "AddC" });
            AddCause_Text.Add(new LimitValue_Text { LimitValue = 6, LimitText = "Parameter change in execution", Category = "AddC" });
            AddCause_Text.Add(new LimitValue_Text { LimitValue = 7, LimitText = "Step limit", Category = "AddC" });
            AddCause_Text.Add(new LimitValue_Text { LimitValue = 8, LimitText = "Blocked by Mode", Category = "AddC" });
            AddCause_Text.Add(new LimitValue_Text { LimitValue = 9, LimitText = "Blocked by process", Category = "AddC" });
            AddCause_Text.Add(new LimitValue_Text { LimitValue = 10, LimitText = "Blocked by interlocking", Category = "AddC" });
            AddCause_Text.Add(new LimitValue_Text { LimitValue = 11, LimitText = "Blocked by synchrocheck", Category = "AddC" });
            AddCause_Text.Add(new LimitValue_Text { LimitValue = 12, LimitText = "Command already in execution", Category = "AddC" });
            AddCause_Text.Add(new LimitValue_Text { LimitValue = 13, LimitText = "Block by health", Category = "AddC" });
            AddCause_Text.Add(new LimitValue_Text { LimitValue = 14, LimitText = "1 of n control", Category = "AddC" });
            AddCause_Text.Add(new LimitValue_Text { LimitValue = 15, LimitText = "Abortion by cancel", Category = "AddC" });
            AddCause_Text.Add(new LimitValue_Text { LimitValue = 16, LimitText = "Time limit over", Category = "AddC" });
            AddCause_Text.Add(new LimitValue_Text { LimitValue = 17, LimitText = "Abortion by trip", Category = "AddC" });
            AddCause_Text.Add(new LimitValue_Text { LimitValue = 18, LimitText = "Object not selected", Category = "AddC" });
            AddCause_Text.Add(new LimitValue_Text { LimitValue = 19, LimitText = "Object already selected", Category = "AddC" });
            AddCause_Text.Add(new LimitValue_Text { LimitValue = 20, LimitText = "No access authority", Category = "AddC" });
            AddCause_Text.Add(new LimitValue_Text { LimitValue = 21, LimitText = "Ended with overshoot", Category = "AddC" });
            AddCause_Text.Add(new LimitValue_Text { LimitValue = 22, LimitText = "Abortion due to deviation", Category = "AddC" });
            AddCause_Text.Add(new LimitValue_Text { LimitValue = 23, LimitText = "Abortion by communication loss", Category = "AddC" });
            AddCause_Text.Add(new LimitValue_Text { LimitValue = 24, LimitText = "Blocked by command", Category = "AddC" });
            AddCause_Text.Add(new LimitValue_Text { LimitValue = 25, LimitText = "None", Category = "AddC" });
            AddCause_Text.Add(new LimitValue_Text { LimitValue = 26, LimitText = "Inconsistent parameters", Category = "AddC" });
            AddCause_Text.Add(new LimitValue_Text { LimitValue = 27, LimitText = "Locked by other client", Category = "AddC" });
        }

        public DataTable DataTable_Startup()
        {
            table01.Columns.Add("Variable Name");
            table01.Columns.Add("Driver Name");
            table01.Columns.Add("Last Update Time");
            table01.Columns.Add("TCP Connection Status");
            table01.Columns.Add("mms Status");
            table01.Columns.Add("Driver Connection");
            table01.Columns.Add("Actual Value");
            table01.Columns.Add("Cause of Transmission");
            table01.Columns.Add("Timestamp Code");
            table01.Columns.Add("Variable Status");
            table01.Columns.Add("Quality");

            return table01;
        }

        public DataTable DataTable2_Startup()
        {
            table02.Columns.Add("Variable Name");
            table02.Columns.Add("Driver Name");
            table02.Columns.Add("Last Update Time");
            table02.Columns.Add("TCP Connection Status");
            table02.Columns.Add("Actual Value");
            table02.Columns.Add("Set Value");
            table02.Columns.Add("Cause of Transmission");
            table02.Columns.Add("AddCause");

            return table02;
        }

        public static string Reverse(string s)
        {
            char[] charArray = s.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public class LimitValue_Text
        {
            public int LimitValue { get; set; }
            public string LimitText { get; set; }
            public string Category { get; set; }
            public string DeveloperText { get; set; }
        }

        
    }
}
