using Scada.AddIn.Contracts;
using Scada.AddIn.Contracts.Historian;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Archive2CSV.ProjectWizardExtension;

namespace Archive2CSV
{
    public partial class Form1 : Form
    {
        // Get Project and reference lists from main program
        IProject FormProject = ProjectWizardExtension.thisProject;
        List<Archive_Info_Class> ArchiveInfo_Form = ProjectWizardExtension.ArchiveInfo;
        List<Archive_Variables_Class> ArchVariables_Form = ProjectWizardExtension.ArchVariables;

        public string Dura = "";
        public int DurationSelection = 0;
        public CultureInfo provider = CultureInfo.InvariantCulture;
        public string AllResult = "";
        public string Var_Category = "";

        // Main program for Form 1
        public Form1()
        {
            InitializeComponent();

            treeView1.CheckBoxes = true;
            treeView1.AfterCheck += ChildNode_check;    // After checking on nodes, check all child nodes or parent node
            treeView1.TabStop = false;
            treeView1.NodeMouseDoubleClick += DoubleClicktoCheck;  // Check the node if double clicked

            dateTimePicker1.CustomFormat = "yyyy/MM/dd";
            dateTimePicker2.CustomFormat = "HH:mm:ss";
            dateTimePicker3.CustomFormat = "yyyy/MM/dd";
            dateTimePicker4.CustomFormat = "HH:mm:ss";

            label6.Visible = false;
        }

        // Sub program for drawing Treeview
        public void DrawTreeView()
        {
            TreeNode SourceNode = null;
            TreeNode TempNodeV = null;
            int ArchCnt = ArchiveInfo_Form.Count;
            int cnt = 0;

            while (cnt < ArchCnt)
            {
                foreach (var item in ArchiveInfo_Form.OrderBy(ts => ts.ArchiveIdentifier))  // Sort the archives by Archive ID
                {
                    // Add all of the first level archive to the treeview
                    string treeArchItem = item.ArchiveIdentifier + " - " + item.ArchiveName;
                    if (item.ArchiveSource == "")
                    {
                        if (FindNode(treeArchItem, treeView1.Nodes) == false)   // Check if this node already exist
                        {
                            if (item.ArchiveRecordingCycle > DurationSelection && DurationSelection > 0) { cnt++; continue; }   // If saving cycle is larger than selected duration, skip
                            treeView1.Nodes.Add(treeArchItem);      // Add archive name to treeview
                            TempNodeV = GetNode(treeArchItem, treeView1.Nodes);
                            AddArchivevariable(TempNodeV, item);    // Add archive variable to treeview
                            cnt++;
                        }
                    }
                    else
                    {
                        SourceNode = null;
                        foreach (TreeNode tempnode in treeView1.Nodes)
                        {
                            // If this archive is second level, add to the corresponding first level node
                            string nodeID = tempnode.Text.Substring(0, 2);
                            if (item.ArchiveSource == nodeID && FindNode(treeArchItem, tempnode.Nodes) == false)    // Look for source archive and check if this node already exist
                            {
                                ProjectWizardExtension.Archive_Info_Class SourceA = ArchiveInfo_Form.Find(vv => vv.ArchiveSource == item.ArchiveSource);    // Find List item with Source ID
                                item.ArchiveRecordingCycle = SourceA.ArchiveSavingCycle;

                                if (item.ArchiveRecordingCycle > DurationSelection && DurationSelection > 0) { cnt++; continue; }   // If saving cycle is larger than selected duration, skip
                                tempnode.Nodes.Add(treeArchItem);       // Add archive name to treeview
                                tempnode.ExpandAll();
                                TempNodeV = GetNode(treeArchItem, tempnode.Nodes);
                                AddArchivevariable(TempNodeV, item);    // Add archive variable to treeview
                                cnt++;
                            }
                            else
                            {
                                // If source node cannot be found in the first attempt, try to match the source in second/third attempt
                                SourceNode = FindSource(item.ArchiveSource, tempnode);
                                if (SourceNode != null && FindNode(treeArchItem, SourceNode.Nodes) == false)    // Check if source node can be found
                                {
                                    ProjectWizardExtension.Archive_Info_Class SourceA = ArchiveInfo_Form.Find(vv => vv.ArchiveSource == item.ArchiveSource);    // Find List item with Source ID
                                    item.ArchiveRecordingCycle = SourceA.ArchiveSavingCycle;

                                    if (item.ArchiveRecordingCycle > DurationSelection && DurationSelection > 0) { cnt++; continue; }
                                    SourceNode.Nodes.Add(treeArchItem);     // Add archive name to treeview
                                    SourceNode.ExpandAll();
                                    TempNodeV = GetNode(treeArchItem, SourceNode.Nodes);
                                    AddArchivevariable(TempNodeV, item);    // Add archive variable to treeview
                                    cnt++;
                                }
                            }
                        }
                    }
                }
            }

            this.GrayEmptySource(treeView1.Nodes);

        }

        // Gray out items in archive treeview if no variable is available
        public void GrayEmptySource(TreeNodeCollection StartNode)
        {
            foreach (TreeNode thisNode in StartNode)
            {
                if (thisNode.Nodes.Count > 0)
                {
                    if (thisNode.Text.Contains("VAR") == false) { thisNode.ForeColor = Color.Gray; }
                    GrayEmptySource(thisNode.Nodes);
                }
                else
                {
                    if (thisNode.Text.Contains("VAR") == false)
                    {
                        thisNode.ForeColor = Color.Gray;
                    }
                }
            }
        }


        // Sub program for adding archive variables to treeview
        public void AddArchivevariable(TreeNode TempNodeV, ProjectWizardExtension.Archive_Info_Class item)
        {
            if (TempNodeV != null)
            {
                foreach (var ArchV in ArchVariables_Form)
                {
                    if (Var_Category.Length > 1)
                    {
                        if (Var_Category == "Value")
                        {
                            if (ArchV.ArchVarName.Contains("[") && ArchV.ArchVarName.Contains("]")) { continue; }
                        }
                        else
                        {
                            if (ArchV.ArchVarName.Contains(Var_Category) == false) { continue; }
                        }
                    }


                    if (ArchV.ArchiveID == item.ArchiveIdentifier)
                    {
                        string ArchVName = "[VAR]" + ArchV.ArchiveID + " - " + ArchV.ArchVarName;
                        TempNodeV.Nodes.Add(ArchVName);     // Add archive variable to treeview
                    }
                }
                TempNodeV.ExpandAll();
            }
        }

        // Sub program for finding the source node of a node by ID
        public TreeNode FindSource(string SourceID, TreeNode rootNode)
        {
            foreach (TreeNode node in rootNode.Nodes)
            {
                string nodeID = node.Text.Substring(0, 2);
                if (nodeID == SourceID) return node;    // If node is found, returen node
                TreeNode next = FindSource(SourceID, node);     // If node not found, down to next level
                if (next != null) return next;
            }
            return null;
        }

        // Sub program for checking if a node exist in a node collection
        public Boolean FindNode(string nodeText, TreeNodeCollection treeNodeCollection)
        {
            foreach (TreeNode childNode in treeNodeCollection)
            {
                if (childNode.Text == nodeText) return true;
            }
            return false;
        }

        // Sub program for getting a node in a node collection by node text
        public TreeNode GetNode(string nodeText, TreeNodeCollection treeNodeCollection)
        {
            foreach (TreeNode childNode in treeNodeCollection)
            {
                if (childNode.Text == nodeText) return childNode;
            }
            return null;
        }

        // Sub program for getting child nodes checked when the parent one is checked
        private void CheckAllChildNodes(TreeNode treenode, bool nodeChecked)
        {
            foreach (TreeNode childNode in treenode.Nodes)
            {
                childNode.Checked = nodeChecked;
                ArchVarCheckProp(childNode);
                if (childNode.Nodes.Count > 0) { this.CheckAllChildNodes(childNode, nodeChecked); }     // If child nodes exist, checked them all
            }
        }

        // Sub program for action of checking a child node
        public void ChildNode_check(object sender, TreeViewEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown)
            {
                if (e.Node.Nodes.Count > 0) { this.CheckAllChildNodes(e.Node, e.Node.Checked); }    // If child nodes exist, checked them all
                if (e.Node.Parent != null)
                {
                    Boolean AllChecked = e.Node.Checked;
                    foreach (TreeNode Collnode in e.Node.Parent.Nodes)
                    {
                        AllChecked = Collnode.Checked && AllChecked;
                    }
                    e.Node.Parent.Checked = AllChecked;     // Return the checking status of all child nodes to the parent node
                }
                ArchVarCheckProp(e.Node);
            }
        }

        // Sub program for getting the node checked if double clicked
        public void DoubleClicktoCheck(object sender, TreeNodeMouseClickEventArgs e)
        {
            e.Node.Checked = true;
            ArchVarCheckProp(e.Node);
        }

        // Sub program for changing checked property in variable list
        public void ArchVarCheckProp(TreeNode node)
        {
            string eName = node.Text.Substring(10);
            string eID = node.Text.Substring(5, 2);

            // Find archive variable form the list
            ProjectWizardExtension.Archive_Variables_Class varinList = ArchVariables_Form.Find(vv => (vv.ArchVarName == eName) && (vv.ArchiveID == eID));
            if (varinList != null) { varinList.ArchVarChecked = node.Checked; }
        }

        // Sub program for adding variables to listbox
        private void ConfirmSelection_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            foreach (ProjectWizardExtension.Archive_Variables_Class Var in ArchVariables_Form)
            {
                if (Var.ArchVarChecked == true)
                {
                    string itemName = Var.ArchiveID + " - " + Var.ArchVarName;
                    listBox1.Items.Add(itemName);
                }
            }
        }

        // Sub program for getting archive value
        public void GetAllArchivevalue()
        {
            // Create csv file
            StringBuilder csvF = new StringBuilder();

            string Title_text1 = "Duration: " + Dura;
            string Title_text2 = "DataType: " + Var_Category;
            csvF.AppendLine(Title_text1);
            csvF.AppendLine(Title_text2);
            

            // Get start & end timestamp for filter
            DateTimeOffset d1 = dateTimePicker1.Value.Date;
            DateTimeOffset d2 = dateTimePicker2.Value;
            DateTimeOffset d3 = dateTimePicker3.Value.Date;
            DateTimeOffset d4 = dateTimePicker4.Value;
            Int32 st = ArchiveFilterTime(d1, d2, d3, d4)[0];
            Int32 et = ArchiveFilterTime(d1, d2, d3, d4)[1];
            MessageBox.Show("Start time: " + Int2date(st)[0] + " - " + Int2date(st)[1]);
            MessageBox.Show("End time: " + Int2date(et)[0] + " - " + Int2date(et)[1]);

            csvF.AppendLine("Start time: " + Int2date(st)[0] + " - " + Int2date(st)[1]);
            csvF.AppendLine("End time: " + Int2date(et)[0] + " - " + Int2date(et)[1]);

            csvF.AppendLine("Time, Archive, Variable, Value");

            label6.Visible = true;

            foreach (string item in listBox1.Items)
            {
                string archID = item.Substring(0, 2);
                string archV_totalN = item.Substring(5);

                // Find archive variable from variable list
                ProjectWizardExtension.Archive_Variables_Class varinList = ArchVariables_Form.Find(vv => (vv.ArchVarName == archV_totalN) && (vv.ArchiveID == archID));
                string archV_name = varinList.ArchV;

                foreach (var ArchItem in ArchiveInfo_Form)
                {
                    if (archID == ArchItem.ArchiveIdentifier)
                    {
                        IRuntimeArchive runtimeArchive = FormProject.RuntimeArchiveCollection[ArchItem.ArchiveName];    // Get historian from archive ID, end program if null
                        if (runtimeArchive == null)
                        {
                            MessageBox.Show("No archive available!");
                            return;
                        }

                        IRuntimeArchiveFilter ArchiveFilter = runtimeArchive.FilterCollection.Create();     // Create filter for archive
                        ArchiveFilter.StartTime = st;   // Set filter start time
                        ArchiveFilter.EndTime = et;     // Set filter end time

                        ArchiveFilter.AddVariable(runtimeArchive.VariableCollection[archV_name]);   // Add variable to filter
                        IRuntimeArchiveFilterVariableCollection ArchiveVariables = ArchiveFilter.Query();   // confirm filter
                        IRuntimeArchiveFilterVariable ArchiveFilterVariable = ArchiveVariables[0];      // Get variable from filtered archive, end program if null
                        if (ArchiveFilterVariable == null)
                        {
                            MessageBox.Show("No filtered archive variable available!");
                            return;
                        }

                        foreach (IRuntimeArchiveValue archValue in ArchiveFilterVariable.ArchiveValueCollection)    // Get entries (value) of variable of the filtered archive
                        {
                            string arch_DT = Int2date(archValue.Time)[0] + " - " + Int2date(archValue.Time)[1];
                            csvF.AppendLine(arch_DT + "," + archID + "," + archV_name + "," + archValue.Value);     // Write to csv
                        }
                    }
                }
            }
            
            label6.Visible = false;
            AllResult = csvF.ToString();
        }

        // Export CSV
        public void CSV_Creation(string csvF)
        {
            string csv_folder = textBox1.Text;
            string csv_path = csv_folder + "\\" + DateTime.Now.ToString("yyyyMMddHHmmss") + "-" + Dura + "-" + Var_Category + "-" + textBox2.Text;
            File.WriteAllText(csv_path, csvF.ToString());   // Export csv file
            MessageBox.Show(csv_path + " created.");
        }

        // Sub program for changing int time unix to timestamp string
        public string[] Int2date(Int32 datetime_int)
        {
            string date_DT = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(datetime_int).AddHours(8).ToShortDateString();
            string time_DT = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(datetime_int).AddHours(8).ToLongTimeString();
            string[] datetime_result = { date_DT, time_DT };
            return datetime_result;
        }

        // Sub program for setting different starttime and endtime for different duration
        public Int32[] ArchiveFilterTime(DateTimeOffset d1, DateTimeOffset d2, DateTimeOffset d3, DateTimeOffset d4)
        {
            Int32 StartTime_int = 0;
            Int32 EndTime_int = 0;

            if (Dura == "Daily")
            {
                StartTime_int = Convert.ToInt32(d1.ToUnixTimeSeconds());
                EndTime_int = StartTime_int + 86399;
            }

            if (Dura == "Monthly")
            {
                string ST_year = d1.Year.ToString();
                int ST_month = d1.Month;
                string ST = ST_year + ST_month + "01000000";
                string ET = ST_year + ST_month + "01000000";

                if (ST_month < 10) { ST = ST_year + "0" + ST_month + "01000000"; ET = ST_year + "0" + ST_month + "30235959"; }
                else { ST = ST_year + ST_month + "01000000"; ET = ST_year + ST_month + "30235959"; }

                if (ST_month == 1 || ST_month == 3 || ST_month == 5 || ST_month == 7 || ST_month == 8 || ST_month == 10 || ST_month == 12)
                {
                    if (ST_month < 10) { ET = ST_year + "0" + ST_month + "31235959"; }
                    else { ET = ST_year + ST_month + "31235959"; }
                }

                if (ST_month == 2)
                {
                    if (Convert.ToInt32(ST_year) % 4 == 0) { ET = ST_year + "0" + ST_month + "29235959"; }
                    else { ET = ST_year + "0" + ST_month + "28235959"; }
                }

                // Convert string to datetime datatype
                DateTimeOffset ST_temp = DateTime.ParseExact(ST, "yyyyMMddHHmmss", provider);
                DateTimeOffset ET_temp = DateTime.ParseExact(ET, "yyyyMMddHHmmss", provider);
                StartTime_int = Convert.ToInt32(ST_temp.ToUnixTimeSeconds());
                EndTime_int = Convert.ToInt32(ET_temp.ToUnixTimeSeconds());
            }

            if (Dura == "Yearly")
            {
                string ST_year = d1.Year.ToString();
                string ST = ST_year + "0101000000";
                string ET = ST_year + "1231235959";

                DateTimeOffset ST_temp = DateTime.ParseExact(ST, "yyyyMMddHHmmss", provider);
                DateTimeOffset ET_temp = DateTime.ParseExact(ET, "yyyyMMddHHmmss", provider);
                StartTime_int = Convert.ToInt32(ST_temp.ToUnixTimeSeconds());
                EndTime_int = Convert.ToInt32(ET_temp.ToUnixTimeSeconds());
            }

            if (Dura == "Custom")
            {
                DateTimeOffset ST_Date = d2.Date;
                TimeSpan ST_Date_TimeSpan = d1.Subtract(ST_Date);
                int ST_TimeSpan = Convert.ToInt32(ST_Date_TimeSpan.TotalSeconds);
                StartTime_int = Convert.ToInt32(d2.ToUnixTimeSeconds()) + ST_TimeSpan;

                DateTimeOffset ET_Date = d4.Date;
                TimeSpan ET_Date_TimeSpan = d3.Subtract(ET_Date);
                int ET_TimeSpan = Convert.ToInt32(ET_Date_TimeSpan.TotalSeconds);
                EndTime_int = Convert.ToInt32(d4.ToUnixTimeSeconds()) + ET_TimeSpan;
            }
            Int32[] Result = { StartTime_int, EndTime_int };
            return Result;
        }

        // Sub program for clearing listbox
        public void ClearListBox()
        {
            listBox1.Items.Clear();
            foreach (var item in ArchVariables_Form) { item.ArchVarChecked = false; }
        }

        private void button_Daily_Click(object sender, EventArgs e)
        {
            Dura = "Daily";
            DurationSelection = 86400;
            if (Var_Category.Length > 0)
            {
                treeView1.Nodes.Clear();
                this.ClearListBox();
                this.DrawTreeView();
            }

            button_Daily.BackColor = SystemColors.ActiveCaption;
            button_Monthly.BackColor = SystemColors.Control;
            button_Yearly.BackColor = SystemColors.Control;
            button_Custom.BackColor = SystemColors.Control;

            dateTimePicker1.CustomFormat = "yyyy/MM/dd";
            dateTimePicker1.ShowUpDown = false;
            dateTimePicker2.Visible = false;
            dateTimePicker3.Visible = false;
            dateTimePicker4.Visible = false;
            label3.Text = "DateTime:";
            label4.Visible = false;
        }

        private void button_Monthly_Click(object sender, EventArgs e)
        {
            Dura = "Monthly";
            DurationSelection = 2592000;
            if (Var_Category.Length > 0)
            {
                treeView1.Nodes.Clear();
                this.ClearListBox();
                this.DrawTreeView();
            }

            button_Daily.BackColor = SystemColors.Control;
            button_Monthly.BackColor = SystemColors.ActiveCaption;
            button_Yearly.BackColor = SystemColors.Control;
            button_Custom.BackColor = SystemColors.Control;

            dateTimePicker1.CustomFormat = "yyyy/MM";
            dateTimePicker1.ShowUpDown = true;
            dateTimePicker2.Visible = false;
            dateTimePicker3.Visible = false;
            dateTimePicker4.Visible = false;
            label3.Text = "DateTime:";
            label4.Visible = false;
        }

        private void button_Yearly_Click(object sender, EventArgs e)
        {
            Dura = "Yearly";
            DurationSelection = 31104000;
            if (Var_Category.Length > 0)
            {
                treeView1.Nodes.Clear();
                this.ClearListBox();
                this.DrawTreeView();
            }

            button_Daily.BackColor = SystemColors.Control;
            button_Monthly.BackColor = SystemColors.Control;
            button_Yearly.BackColor = SystemColors.ActiveCaption;
            button_Custom.BackColor = SystemColors.Control;

            dateTimePicker1.CustomFormat = "yyyy";
            dateTimePicker1.ShowUpDown = true;
            dateTimePicker2.Visible = false;
            dateTimePicker3.Visible = false;
            dateTimePicker4.Visible = false;
            label3.Text = "DateTime:";
            label4.Visible = false;
        }

        private void button_Custom_Click(object sender, EventArgs e)
        {
            Dura = "Custom";
            DurationSelection = 0;
            if (Var_Category.Length > 0)
            {
                treeView1.Nodes.Clear();
                this.ClearListBox();
                this.DrawTreeView();
            }

            button_Daily.BackColor = SystemColors.Control;
            button_Monthly.BackColor = SystemColors.Control;
            button_Yearly.BackColor = SystemColors.Control;
            button_Custom.BackColor = SystemColors.ActiveCaption;

            dateTimePicker1.CustomFormat = "yyyy/MM/dd";
            dateTimePicker1.ShowUpDown = false;
            dateTimePicker2.Visible = true;
            dateTimePicker3.Visible = true;
            dateTimePicker4.Visible = true;
            label3.Text = "StartTime:";
            label4.Visible = true;
        }

        private void button_OK_Click(object sender, EventArgs e)
        {
            this.GetAllArchivevalue();
            this.CSV_Creation(AllResult);
        }

        private void Preview_Click(object sender, EventArgs e)
        {
            this.GetAllArchivevalue();

            Form2 form2 = new Form2(AllResult, Dura, Var_Category);
            form2.ShowDialog();
            form2.Activate();
        }

        private void button_Summary_Click(object sender, EventArgs e)
        {
            Var_Category = "Sum";
            if (Dura.Length > 0)
            {
                treeView1.Nodes.Clear();
                this.ClearListBox();
                this.DrawTreeView();
            }

            button_Summary.BackColor = SystemColors.ActiveCaption;
            button_Average.BackColor = SystemColors.Control;
            button_Minimum.BackColor = SystemColors.Control;
            button_Maximum.BackColor = SystemColors.Control;
            button_Value.BackColor = SystemColors.Control;
        }

        private void button_Average_Click(object sender, EventArgs e)
        {
            Var_Category = "Average";
            if (Dura.Length > 0)
            {
                treeView1.Nodes.Clear();
                this.ClearListBox();
                this.DrawTreeView();
            }

            button_Summary.BackColor = SystemColors.Control;
            button_Average.BackColor = SystemColors.ActiveCaption;
            button_Minimum.BackColor = SystemColors.Control;
            button_Maximum.BackColor = SystemColors.Control;
            button_Value.BackColor = SystemColors.Control;
        }

        private void button_Minimum_Click(object sender, EventArgs e)
        {
            Var_Category = "Minimum";
            if (Dura.Length > 0)
            {
                treeView1.Nodes.Clear();
                this.ClearListBox();
                this.DrawTreeView();
            }

            button_Summary.BackColor = SystemColors.Control;
            button_Average.BackColor = SystemColors.Control;
            button_Minimum.BackColor = SystemColors.ActiveCaption;
            button_Maximum.BackColor = SystemColors.Control;
            button_Value.BackColor = SystemColors.Control;
        }

        private void button_Maximum_Click(object sender, EventArgs e)
        {
            Var_Category = "Maximum";
            if (Dura.Length > 0)
            {
                treeView1.Nodes.Clear();
                this.ClearListBox();
                this.DrawTreeView();
            }

            button_Summary.BackColor = SystemColors.Control;
            button_Average.BackColor = SystemColors.Control;
            button_Minimum.BackColor = SystemColors.Control;
            button_Maximum.BackColor = SystemColors.ActiveCaption;
            button_Value.BackColor = SystemColors.Control;
        }

        private void button_Value_Click(object sender, EventArgs e)
        {
            Var_Category = "Value";
            if (Dura.Length > 0)
            {
                treeView1.Nodes.Clear();
                this.ClearListBox();
                this.DrawTreeView();
            }

            button_Summary.BackColor = SystemColors.Control;
            button_Average.BackColor = SystemColors.Control;
            button_Minimum.BackColor = SystemColors.Control;
            button_Maximum.BackColor = SystemColors.Control;
            button_Value.BackColor = SystemColors.ActiveCaption;
        }

        private void CSV_Folder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }
    }

}
