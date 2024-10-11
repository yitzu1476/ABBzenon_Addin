using Scada.AddIn.Contracts;
using Scada.AddIn.Contracts.Function;
using Scada.AddIn.Contracts.Variable;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static IEC61850_FileTransfer.ProjectWizardExtension;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;

namespace IEC61850_FileTransfer
{

    public partial class Form1 : Form
    {
        IProject formProject = ProjectWizardExtension.thisProject;
        List<DRitems> DR_allItem = new List<DRitems>();
        List<BayName> Bays = new List<BayName>();
        CultureInfo provider = CultureInfo.InvariantCulture;

        string thisbay_name;
        string thisbay_COM;
        string thisbay_DIR;
        IVariable curr_COM;
        IVariable curr_DIR;
        bool GetDR;
        string thisbay_driver_path;

        IOnlineVariableContainer onlineContainer = null;

        public Form1()
        {
            InitializeComponent();

            comboBox1.Items.Add("All");
            comboBox1.Items.Add("CFG");
            comboBox1.Items.Add("DAT");

            dateTimePicker1.Value = DateTime.Now;
            dateTimePicker2.Value = DateTime.Now;

            dateTimePicker1.CustomFormat = "yyyy-MM-dd , HH:mm:ss";
            dateTimePicker2.CustomFormat = "yyyy-MM-dd , HH:mm:ss";

            comboBox1.SelectedValueChanged += ComboSelection;
        }

        private void BayCollecting_Click(object sender, EventArgs e)
        {
            // Show loading popup screen
            Form2 form_pop = new Form2();
            form_pop.Activate();
            form_pop.Show();

            // Clear list box
            listBox1.Items.Clear();
            listBox1.Items.Add("Collecting...");

            // Find !Command variables from project, and add them to list "Bays"
            IVariableCollection variableCollection = formProject.VariableCollection;
            Bays.Clear();
            foreach (IVariable variable in variableCollection)
            {
                int var_leng = variable.Name.Length;
                if (var_leng > 8 && variable.Name.Substring(var_leng - 8, 8) == "!Command")
                {
                    Bays.Add(new BayName { Bay = variable.Name.Substring(0, var_leng - 8), VarName = variable.Name });
                }
            }

            // Write items in list "Bays" to list box
            listBox1.Items.Clear();
            foreach (var item in Bays)
            {
                listBox1.Items.Add(item.Bay);
            }
            
            // Close loading popup screen
            form_pop.Close();
        }

        private void SetTime_Click(object sender, EventArgs e)
        {
            comboBox1.SelectedItem = "All";
            comboBox1.SelectedItem = "CFG";
        }

        // Event triggered when selection of combo box is changed
        public void ComboSelection(object sender, EventArgs e)
        {
            // Clear list box 2
            listBox2.SelectedItems.Clear();

            // If time filter not enable
            if (EnableTimeFilter.Checked == false)
            {
                if (comboBox1.SelectedItem.ToString() == "All")
                {
                    if (DR_allItem.Count > 0)
                    {
                        listBox2.Items.Clear();
                        foreach (var DR in DR_allItem)
                        {
                            string temp = DR.DR_time + "  |  " + DR.DR_name;
                            listBox2.Items.Add(temp);
                        }
                    }
                }

                if (comboBox1.SelectedItem.ToString() == "CFG")
                {
                    if (DR_allItem.Count > 0)
                    {
                        listBox2.Items.Clear();
                        foreach (var DR in DR_allItem)
                        {
                            int name_leng = DR.DR_name.Length;
                            if (name_leng > 3)
                            {
                                if (DR.DR_name.Substring(name_leng - 3, 3).Equals("cfg", StringComparison.OrdinalIgnoreCase))
                                {
                                    string temp = DR.DR_time + "  |  " + DR.DR_name;
                                    listBox2.Items.Add(temp);
                                }
                            }
                        }
                    }
                }

                if (comboBox1.SelectedItem.ToString() == "DAT")
                {
                    if (DR_allItem.Count > 0)
                    {
                        listBox2.Items.Clear();
                        foreach (var DR in DR_allItem)
                        {
                            int name_leng = DR.DR_name.Length;
                            if (name_leng > 3)
                            {
                                if (DR.DR_name.Substring(name_leng - 3, 3).Equals("dat", StringComparison.OrdinalIgnoreCase))
                                {
                                    string temp = DR.DR_time + "  |  " + DR.DR_name;
                                    listBox2.Items.Add(temp);
                                }
                            }
                        }
                    }
                }
            }

            // If time filter is enable
            else
            {
                if (comboBox1.SelectedItem.ToString() == "All")
                {
                    if (DR_allItem.Count > 0)
                    {
                        listBox2.Items.Clear();
                        foreach (var DR in DR_allItem)
                        {
                            DateTime DR_dt = DRtoDateTime(DR.DR_time);
                            if (DR_dt >= dateTimePicker1.Value && DR_dt <= dateTimePicker2.Value)
                            {
                                string temp = DR.DR_time + "  |  " + DR.DR_name;
                                listBox2.Items.Add(temp);
                            }
                        }
                    }
                }

                if (comboBox1.SelectedItem.ToString() == "CFG")
                {
                    if (DR_allItem.Count > 0)
                    {
                        listBox2.Items.Clear();
                        foreach (var DR in DR_allItem)
                        {
                            DateTime DR_dt = DRtoDateTime(DR.DR_time);
                            int name_leng = DR.DR_name.Length;
                            if (name_leng > 3)
                            {
                                if (DR.DR_name.Substring(name_leng - 3, 3).Equals("cfg", StringComparison.OrdinalIgnoreCase) && DR_dt >= dateTimePicker1.Value && DR_dt <= dateTimePicker2.Value)
                                {
                                    string temp = DR.DR_time + "  |  " + DR.DR_name;
                                    listBox2.Items.Add(temp);
                                }
                            }
                        }
                    }
                }

                if (comboBox1.SelectedItem.ToString() == "DAT")
                {
                    if (DR_allItem.Count > 0)
                    {
                        listBox2.Items.Clear();
                        foreach (var DR in DR_allItem)
                        {
                            DateTime DR_dt = DRtoDateTime(DR.DR_time);
                            int name_leng = DR.DR_name.Length;
                            if (name_leng > 3)
                            {
                                if (DR.DR_name.Substring(name_leng - 3, 3).Equals("dat", StringComparison.OrdinalIgnoreCase) && DR_dt >= dateTimePicker1.Value && DR_dt <= dateTimePicker2.Value)
                                {
                                    string temp = DR.DR_time + "  |  " + DR.DR_name;
                                    listBox2.Items.Add(temp);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void SelectBay_Click(object sender, EventArgs e)
        {
            // If SelectBay button is clicked
            GetDR = true;

            if (listBox1.SelectedItem != null)
            {
                listBox2.Items.Clear();

                // Set command for this bay
                thisbay_COM = listBox1.SelectedItem.ToString() + "!Command";
                thisbay_name = listBox1.SelectedItem.ToString();

                // Create online container for command variable of this bay
                onlineContainer = formProject.OnlineVariableContainerCollection.Create("VariableContainer");
                onlineContainer.AddVariable(thisbay_COM);
                onlineContainer.ActivateBulkMode();
                onlineContainer.Activate();

                // Event triggered when the value of command variable is changed
                onlineContainer.BulkChanged += LoadDRItems;

                // Write "DIR" command to command variable
                curr_COM = formProject.VariableCollection[thisbay_COM];
                if (curr_COM == null) { MessageBox.Show("command variable error"); }

                curr_COM.SetValue(0, "DIR");
            }
        }

        // Event triggered when the value of command variable is changed
        public void LoadDRItems(object sender, BulkChangedEventArgs e)
        {
            string e_value = e.Variables[0].GetValue(0).ToString();

            if (e_value == "DIR BUSY")
            {
                listBox2.Items.Add("Collecting...");
            }

            // if SelectBay button is clicked, and command variable return "DIR OK"
            if (GetDR == true && e_value == "DIR OK")
            {
                DR_allItem.Clear();
                textBox1.Clear();

                // Set Directory variable for the bay, and get DIR return value
                thisbay_DIR = listBox1.SelectedItem.ToString() + "!Directory";
                curr_DIR = formProject.VariableCollection[thisbay_DIR];
                if (curr_DIR == null) { MessageBox.Show("directory variable error"); }

                string DIR_allitems = curr_DIR.GetValue(0).ToString();

                // Add all Directory item to list "DR_allItem"
                string[] DIR_allarray = DIR_allitems.Split('\n');
                foreach (string item in DIR_allarray)
                {
                    string[] DIR_item = item.Split(';');
                    if (DIR_item.Length == 3)
                    {
                        DR_allItem.Add(new DRitems
                        {
                            DR_name = DIR_item[0],
                            DR_size = DIR_item[1],
                            DR_time = DIR_item[2],
                        });
                    }
                }

                // Write items in list "DR_allItem" to list box 2
                if (DR_allItem.Count > 0)
                {
                    listBox2.Items.Clear();
                    foreach (var DR in DR_allItem)
                    {
                        string temp = DR.DR_time + "  |  " + DR.DR_name;
                        listBox2.Items.Add(temp);
                    }
                    comboBox1.SelectedItem = "All";
                }

                // Write default saving path to text box
                thisbay_driver_path = formProject.DriverCollection[curr_COM.Driver.Identification].GetDynamicProperty("DrvConfig.Options.DirectoryForFileTransfer").ToString();
                textBox1.Text = thisbay_driver_path;

                GetDR = false;

                // deactivate online container
                onlineContainer.BulkChanged -= LoadDRItems;
                onlineContainer.Deactivate();
                formProject.OnlineVariableContainerCollection.Delete(onlineContainer.Name);

                comboBox1.SelectedItem = "CFG";
            }
        }

        private void GET_Click(object sender, EventArgs e)
        {
            ListBox.SelectedObjectCollection SelectedDR = listBox2.SelectedItems;
            int cnt = 0;

            // Get selected items in list box 2
            foreach (var DR in SelectedDR)
            {
                string[] Item_text = DR.ToString().Split('|');
                string DR_N = Item_text[1].Trim();
                string Get_comm = "GET " + DR_N;

                // Write getting command to IED
                curr_COM.SetValue(0, Get_comm);

                if (cnt == 0) { MessageBox.Show("Getting item..."); }
                cnt = cnt + 1;

                // If GEt OK
                IVariable Get_var_after = formProject.VariableCollection[thisbay_COM];
                if (Get_var_after.GetValue(0).ToString() == "GET OK")
                {
                    Thread.Sleep(500);

                    // Get original filename
                    string filename = DR_N;
                    if (DR_N.Substring(0, 5) == "COMTR") { filename = DR_N.Substring(9); }
                    string old_path = thisbay_driver_path + "\\" + filename;

                    // If file saving path is changed, move to new path
                    if (textBox1.Text.Length > 3 && textBox1.Text != thisbay_driver_path)
                    {
                        string new_path = textBox1.Text + "\\" + thisbay_name + "_" + filename;
                        if (File.Exists(old_path))
                        {
                            if (File.Exists(new_path)) { MessageBox.Show(filename + " already existed."); File.Delete(old_path); continue; }
                            else { File.Move(old_path, new_path); }
                        }
                        else { MessageBox.Show("File local error"); }
                    }

                    // Rename file with IED name
                    else
                    {
                        string newname_path = thisbay_driver_path + "\\" + thisbay_name + "_" + filename;
                        if (File.Exists(newname_path)) { MessageBox.Show(filename + " already existed."); File.Delete(old_path); continue; }
                        else { File.Move(old_path, newname_path); }
                    }
                    MessageBox.Show(Get_comm + " OK!");
                }
            }
            Process.Start(textBox1.Text);

        }

        private void SelectAllItem_Click(object sender, EventArgs e)
        {
            int ItemCount = listBox2.Items.Count;
            for (int i = 0; i < ItemCount; i++) { listBox2.SetSelected(i, true); }
        }

        public DateTime DRtoDateTime(string DRtime)
        {
            DateTime DR_DateTime = DateTime.ParseExact(DRtime, "yyyy-MM-dd HH:mm:ss", provider);
            return DR_DateTime;
        }

        public class BayName
        {
            public string Bay { get; set; }
            public string VarName { get; set; }
        }

        public class DRitems
        {
            public string DR_name { get; set; }
            public string DR_size { get; set; }
            public string DR_time { get; set; }
        }

        private void OpenFolder_Click(object sender, EventArgs e)
        {
            Process.Start(textBox1.Text);
        }

        
    }
}
