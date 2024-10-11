using Scada.AddIn.Contracts;
using Scada.AddIn.Contracts.EquipmentModeling;
using Scada.AddIn.Contracts.Historian;
using Scada.AddIn.Contracts.Variable;
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

namespace ReportCreator_EquipmentModel
{
    public partial class Form1 : Form
    {
        IProject thisProject;
        int Page_num = 0;
        int Equipment_End = 0;
        int Duration_int = 0;
        string allContent = "";
        
        IVariableCollection variableCollection;
        IRuntimeArchiveCollection archiveCollection;
        CultureInfo provider = CultureInfo.InvariantCulture;
        List<AI_Content> AI_Items = new List<AI_Content>();
        List<AI_Final> AI_FinalList = new List<AI_Final>();
        List<Equipments> EquipmentModels = new List<Equipments>();
        List<items2Print> Items2Print_content = new List<items2Print>();

        List<ZEEEnergyVar> All_ZEEEnergy = new List<ZEEEnergyVar>();
        List<ArchiveInfo> ArchiveInfo_List = new List<ArchiveInfo>();
        List<ArchiveVarInfo> ArchiveVarInfo_List = new List<ArchiveVarInfo>();

        string[] AI_Content_Str = { "CurrentA", "CurrentB", "CurrentC", "ResCurrent", "VoltageAB", "VoltageBC", "VoltageCA", "ResVoltage", "Frequency", "TotPF", "TotW", "TotVAr", "kVA" };

        bool[] AI_Content_Bool;

        public Form1(IProject mainProject)
        {
            thisProject = mainProject;

            AI_Content_Bool = new bool[AI_Content_Str.Length];
            for (int z = 0; z < AI_Content_Bool.Length; z++) { AI_Content_Bool[z] = true; }

            InitializeComponent();

            variableCollection = thisProject.VariableCollection;
            archiveCollection = thisProject.RuntimeArchiveCollection;

            treeView1.TabStop = false;
            treeView1.CheckBoxes = true;
            treeView1.AfterCheck += ChildNode_check;

            comboBox1.SelectedIndexChanged += DurationSelection;

            Page1_Active();

        }

        // Collect all info from archives
        private void CollectArchiveItems_Click(object sender, EventArgs e)
        {
            IRuntimeArchiveCollection ArchiveCollection = thisProject.RuntimeArchiveCollection;
            foreach (IRuntimeArchive archive in ArchiveCollection)
            {
                richTextBox1.AppendText("Collecting " + archive.Name + "...\n");
                richTextBox1.ScrollToCaret();

                // Collect archive info
                ArchiveInfo_List.Add(new ArchiveInfo
                {
                    ArchiveName = archive.Name,
                    ArchiveIdentifier = archive.Identifier,
                    ArchiveSource = archive.GetDynamicProperty("SourceShortName").ToString(),
                    ArchiveRecordingCycle = archive.Cycle,
                    ArchiveSavingCycle = int.Parse(archive.GetDynamicProperty("Cycle").ToString())
                });

                foreach (IRuntimeArchiveVariable archiveVar in archive.VariableCollection)
                {
                    string VarName = archiveVar.Name;
                    IVariable thisVar = thisProject.VariableCollection[VarName];
                    string thisVar_Guid = thisVar.GetDynamicProperty("SystemModelGroup").ToString();        // Get Guid ID of the equipment model

                    string BayName = "";
                    if (VarName.Contains(".")) { BayName = VarName.Split('.')[0].Trim(); }
                    else { BayName = VarName.Split('!')[0].Trim(); }

                    // Collect values in this archive
                    ArchiveVarInfo_List.Add(new ArchiveVarInfo
                    {
                        ArchiveIdentifier = archive.Identifier,
                        ArchiveVarName = archiveVar.Name,
                        ArchiveDataType = archiveVar.AggregationType.ToString(),
                        ArchiveVarBayName = BayName,
                        ArchiveVarGuid = thisVar_Guid
                    });
                }
            }

            // Write source saving cycle as actual cycle
            foreach (var ArchItem in ArchiveInfo_List)
            {
                if (ArchItem.ArchiveSource == "") { ArchItem.ActualCycle = ArchItem.ArchiveRecordingCycle; }
                else { ArchItem.ActualCycle = ArchiveInfo_List.Find(x => x.ArchiveIdentifier == ArchItem.ArchiveSource).ArchiveSavingCycle; }
            }

            foreach (var ArchItem in ArchiveInfo_List)
            {
                if (ArchItem.ActualCycle == 1 && ArchItem.ArchiveSource != "")
                {
                    string thisSource = ArchItem.ArchiveSource;
                    if (ArchiveInfo_List.Find(x => x.ArchiveSource == thisSource).ActualCycle != 1)
                    {
                        int newCycle = 0;
                        int SourceCycle = ArchiveInfo_List.Find(x => x.ArchiveSource == thisSource).ActualCycle;
                        if (SourceCycle == 86400) { newCycle = 2073600; }
                        if (SourceCycle == 2073600) { newCycle = 49766400; }
                        ArchItem.ActualCycle = newCycle;
                    }
                }
            }

            foreach (var ArchItem in ArchiveInfo_List)
            {
                if (ArchItem.ActualCycle == 1 && ArchItem.ArchiveSource != "")
                {
                    string thisSource = ArchItem.ArchiveSource;
                    if (ArchiveInfo_List.Find(x => x.ArchiveIdentifier == thisSource).ActualCycle != 1)
                    {
                        int newCycle = 0;
                        string SourceCycle = ArchiveInfo_List.Find(x => x.ArchiveIdentifier == thisSource).ActualCycle.ToString().Trim();
                        if (SourceCycle == "86400") { newCycle = 2073600; }
                        if (SourceCycle == "2073600") { newCycle = 49766400; }
                        ArchItem.ActualCycle = newCycle;
                    }
                }
            }

            // Collect energy archive info and related energy variable name
            IVariableCollection variableCollection = thisProject.VariableCollection;
            foreach (IVariable thisVar in variableCollection)
            {
                if ((thisVar.Name.Contains("ZEE Energy Management.HistorianConfiguration")) && (thisVar.Name.Contains("ZEE_EnergyManagementBlockGroup")) && (thisVar.Name.Contains("Tag")))
                {
                    richTextBox1.AppendText("Collecting " + thisVar.Name + "...\n");
                    richTextBox1.ScrollToCaret();

                    string thisZEETitle = thisVar.Name.Substring(0, thisVar.Name.Length - 3);
                    string thisZEEVar = thisVar.GetDynamicProperty("Initial_value").ToString();
                    if (thisZEEVar.Length < 5) { continue; }

                    string[] thisVar_split = thisZEEVar.Split('.');
                    string BayName = thisVar_split[0].Trim();

                    string thisEnergyType = "";
                    string thisEnergyTypeN = thisVar_split[thisVar_split.Count() - 1].Trim();
                    if (thisEnergyTypeN == "ActiveEnergyFwd") { thisEnergyType = "kWh+"; }
                    if (thisEnergyTypeN == "ActiveEnergyRev") { thisEnergyType = "kWh-"; }
                    if (thisEnergyTypeN == "ReactiveEnergyFwd") { thisEnergyType = "kVArh+"; }
                    if (thisEnergyTypeN == "ReactiveEnergyRev") { thisEnergyType = "kVArh-"; }

                    All_ZEEEnergy.Add(new ZEEEnergyVar
                    {
                        ZEEEnergy_Title = thisZEETitle,
                        ZEEEnergy_Var = thisZEETitle + "DeltaValue",
                        actualVar = thisZEEVar,
                        Var_BayName = BayName,
                        Var_EnergyType = thisEnergyType
                    });
                }
            }

            // Draw items to treeview
            EM_Tree();
            treeView1.ExpandAll();
            CollectArchiveItems.Visible = false;

            richTextBox1.AppendText("-------- End of Operation --------\n");
            richTextBox1.AppendText("\n");
            richTextBox1.ScrollToCaret();

        }

        // Click to show archive values of the selected items
        private void Confirm_Click(object sender, EventArgs e)
        {
            allContent = "";
            AI_Items.Clear();
            Items2Print_content.Clear();

            // If page AI Valus is activated
            if (Page_num == 1)
            {
                // Open Form 4 for selecting the columns to show in Form 2
                Form4 form4 = new Form4(AI_Content_Str, AI_Content_Bool);
                form4.ShowDialog();
                form4.Activate();

                AI_Content_Bool = form4.All_Column_Bool;        // Use bool from Form 4 for disable visibility of Fomr 2

                bool EquipCheck = true;
                foreach (Equipments thisEquip in EquipmentModels)
                {
                    if (thisEquip.ModelChecked == true && thisEquip.ModelLevel == Equipment_End)        // check if this equipment is in final level
                    {
                        if (ArchiveVarInfo_List.Exists(x => (x.ArchiveVarGuid == thisEquip.ModelGuid) && x.ArchiveDataType == comboBox2.Text))       // Check if this equipment contains the target data type
                        {
                            bool CycleCheck = false;
                            foreach (ArchiveVarInfo ArchiveVarIn in ArchiveVarInfo_List)
                            {
                                if (ArchiveVarIn.ArchiveVarGuid == thisEquip.ModelGuid && ArchiveVarIn.ArchiveDataType == comboBox2.Text)
                                {
                                    string thisArchiveID = ArchiveVarIn.ArchiveIdentifier;
                                    if (ArchiveInfo_List.Find(x => x.ArchiveIdentifier == thisArchiveID).ActualCycle == Duration_int) { CycleCheck = true; break; }     // Check for duration
                                }
                            }

                            if (CycleCheck == false)
                            {
                                richTextBox1.SelectionColor = Color.Red;
                                richTextBox1.AppendText("ERROR >> " + thisEquip.ModelName + " duration type not found.\n");
                                richTextBox1.SelectionColor = Color.Black;
                                richTextBox1.ScrollToCaret();
                                EquipCheck = false;
                                continue;
                            }

                            //foreach (string thisvarType in AI_Content_Str)
                            //{
                            //    if (ArchiveVarInfo_List.Exists(x => x.ArchiveVarName.Contains(thisvarType))) { continue; }
                            //    else
                            //    {
                            //        richTextBox1.AppendText("ERROR >> " + thisEquip.ModelName + " without " + thisvarType + ".\n");
                            //        richTextBox1.ScrollToCaret();
                            //        EquipCheck = false;
                            //        continue;
                            //    }
                            //}


                            continue;
                        }
                        else
                        {
                            richTextBox1.SelectionColor = Color.Red;
                            richTextBox1.AppendText("ERROR >> " + thisEquip.ModelName + " data type not found.\n");
                            richTextBox1.SelectionColor = Color.Black;
                            richTextBox1.ScrollToCaret();
                            EquipCheck = false;
                            continue;
                        }
                    }
                }

                if (EquipCheck) { Open_Form2(); }       // If the above conditions are all met, show Form 2
                else
                {
                    richTextBox1.AppendText("-------- End of Operation --------\n");
                    richTextBox1.AppendText("\n");
                    richTextBox1.ScrollToCaret();
                }

            }

            // If page Energy Value is activated
            if (Page_num == 2)
            {
                bool Energy_EquipCheck = true;
                foreach (Equipments thisEquip in EquipmentModels)
                {
                    if (thisEquip.ModelChecked == true && thisEquip.ModelLevel == Equipment_End)
                    {
                        if (All_ZEEEnergy.Exists(x => x.Var_EquipName == thisEquip.ModelName)) { continue; }        // Check if this equipment contains energy variables
                        else
                        {
                            richTextBox1.SelectionColor = Color.Red;
                            richTextBox1.AppendText("ERROR >> " + thisEquip.ModelName + " energy not found.\n");
                            richTextBox1.SelectionColor = Color.Black;
                            richTextBox1.ScrollToCaret();
                            Energy_EquipCheck = false;
                            continue;
                        }
                    }
                }

                if (Energy_EquipCheck == true) { Open_Form3(); }        // If condition met, show Form 3
                else
                {
                    richTextBox1.AppendText("-------- End of Operation --------\n");
                    richTextBox1.AppendText("\n");
                    richTextBox1.ScrollToCaret();
                }
            }
        }

        public void Open_Form2()
        {
            allContent = "";
            AI_Items.Clear();
            Items2Print_content.Clear();

            foreach (Equipments thisEquip in EquipmentModels)
            {
                if (thisEquip.ModelChecked == true && thisEquip.ModelLevel == Equipment_End)
                {
                    //richTextBox1.AppendText("thisEquip: " + thisEquip.ModelName + "\n");
                    //richTextBox1.ScrollToCaret();

                    // Show differernt bay title depending on the equipment level
                    string EquipTitle = "";
                    if (Equipment_End == 3) { EquipTitle = thisEquip.ModelName; }
                    if (Equipment_End == 4) { EquipTitle = thisEquip.ModelParentName + "--" + thisEquip.ModelName; }

                    foreach (var VarItem in ArchiveVarInfo_List)
                    {
                        if (VarItem.ArchiveVarGuid == thisEquip.ModelGuid)      // Find all variables in archive variable list has the same Guid ID of the selected items
                        {
                            IVariable thisVar = variableCollection[VarItem.ArchiveVarName];
                            if (thisVar != null)
                            {
                                int thisVarLen = thisVar.Name.Split('.').Count();
                                string thisVarType = thisVar.Name.Split('.')[thisVarLen - 1];       // Get variable type of the variable

                                int thisActCycle = ArchiveInfo_List.Find(x => x.ArchiveIdentifier == VarItem.ArchiveIdentifier).ActualCycle;
                                if (thisActCycle == Duration_int)       // If duraiton match
                                {
                                    if (VarItem.ArchiveDataType != comboBox2.Text) { continue; }     //If data type doesn't match, continue

                                    richTextBox1.AppendText("Start collecting: " + VarItem.ArchiveVarName + "\n");
                                    richTextBox1.ScrollToCaret();

                                    string thisArchiveName = ArchiveInfo_List.Find(x => x.ArchiveIdentifier == VarItem.ArchiveIdentifier).ArchiveName;      // Get archive name
                                    IRuntimeArchive thisArchive = archiveCollection[thisArchiveName];

                                    richTextBox1.AppendText("Archive: " + thisArchiveName + "\n");
                                    richTextBox1.ScrollToCaret();

                                    IRuntimeArchiveFilter thisArchiveFilter = thisArchive.FilterCollection.Create();        // Create archive filter
                                    thisArchiveFilter.StartTime = DatePicker2Unix(dateTimePicker1.Value.Date)[0];           // Archive time filter
                                    thisArchiveFilter.EndTime = DatePicker2Unix(dateTimePicker1.Value.Date)[1];

                                    for (int k = 0; k < thisArchive.VariableCollection.Count; k++) { thisArchiveFilter.AddVariable(thisArchive.VariableCollection[k]); }       // Add all variables in this archive to filter

                                    IRuntimeArchiveFilterVariableCollection ArchiveVariables = thisArchiveFilter.Query();   // confirm filter

                                    IRuntimeArchiveFilterVariable ArchiveFilterVariable = ArchiveVariables[0];      // Get variable from filtered archive, end program if null
                                    if (ArchiveFilterVariable == null)
                                    {
                                        MessageBox.Show("No filtered archive variable available!");
                                        return;
                                    }

                                    foreach (IRuntimeArchiveFilterVariable thisFilterVariable in ArchiveVariables)
                                    {
                                        if (thisFilterVariable.ArchiveVariable.Name != VarItem.ArchiveVarName) { continue; }                    // Skip if wrong variable name
                                        if (thisFilterVariable.ArchiveVariable.AggregationType.ToString() != comboBox2.Text) { continue; }      // Skip if wrong data type

                                        // Add archive value to list for printing
                                        foreach (IRuntimeArchiveValue archiveValue in thisFilterVariable.ArchiveValueCollection)
                                        {
                                            AI_Items.Add(new AI_Content
                                            {
                                                timeStamp = archiveValue.Time,
                                                VarType = thisVarType,
                                                VarValue = archiveValue.Value.ToString(),
                                                DataType = VarItem.ArchiveDataType,
                                                BayTitle = EquipTitle
                                            });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }

            // If no archive value add to list for printing
            if (AI_Items.Count < 1)
            {
                richTextBox1.SelectionColor = Color.Red;
                richTextBox1.AppendText("ERROR >> No AI value.\n");
                richTextBox1.SelectionColor = Color.Black;

                richTextBox1.AppendText("-------- End of Operation --------\n");
                richTextBox1.AppendText("\n");
                richTextBox1.ScrollToCaret();
                return;
            }

            richTextBox1.AppendText("AI_Items collected.\n");
            richTextBox1.ScrollToCaret();

            allContent = allContent + "Duration: " + comboBox1.Text + "\n";
            allContent = allContent + "DataType: " + comboBox2.Text + "\n";
            allContent = allContent + "Date: " + dateTimePicker1.Text + "\n" + "\n";

            allContent = allContent + "Bay Name,Timestamp";
            foreach (string AI_Name in AI_Content_Str)
            {
                allContent = allContent + "," + AI_Name;
            }
            allContent = allContent + "\n";         // Write table title

            List<AI_Content> New_AI_Items = AI_Items.OrderBy(x => x.BayTitle).ThenBy(x => x.timeStamp).ToList();        // Order list for printing with timestamp

            foreach (var AIitem in New_AI_Items)
            {
                if (Items2Print_content.Exists(x => (x.Timestamp == AIitem.timeStamp) && (x.BayTitle_Print == AIitem.BayTitle))) { continue; }

                string thisTime = Int2date(AIitem.timeStamp)[0] + " - " + Int2date(AIitem.timeStamp)[1];
                string thisItem_content = AIitem.BayTitle + "," + thisTime;
                foreach (string AI_Name in AI_Content_Str)
                {
                    if (New_AI_Items.Exists(x => (x.timeStamp == AIitem.timeStamp) && (x.VarType == AI_Name) && (x.BayTitle == AIitem.BayTitle)) == false) { thisItem_content = thisItem_content + ",NA"; }
                    else
                    {
                        string AI_thisItem = New_AI_Items.Find(x => (x.timeStamp == AIitem.timeStamp) && (x.VarType == AI_Name) && (x.BayTitle == AIitem.BayTitle)).VarValue;
                        thisItem_content = thisItem_content + "," + AI_thisItem;        // Add value to text
                    }
                }

                thisItem_content = thisItem_content + "\n";

                // Add all items to list for printing
                Items2Print_content.Add(new items2Print
                {
                    BayTitle_Print = AIitem.BayTitle,
                    Timestamp = AIitem.timeStamp,
                    TextContent = thisItem_content
                });

            }

            richTextBox1.AppendText("Items2Print_content collected.\n");
            richTextBox1.ScrollToCaret();

            foreach (var PrintItem in Items2Print_content)
            {
                allContent = allContent + PrintItem.TextContent;
            }

            // Calculate Sum, average, minimum, maximum value
            string Calculation4 = Get4Calculation(New_AI_Items);

            richTextBox1.AppendText("Calculation4 collected.\n");
            richTextBox1.ScrollToCaret();

            string path = DateTime.Now.ToString("yyyyMMddHHmmss") + "_Report.csv";

            richTextBox1.AppendText("-------- End of Operation --------\n");
            richTextBox1.AppendText("\n");
            richTextBox1.ScrollToCaret();

            // Open Form 2
            Form2 form2 = new Form2(allContent, Calculation4, path, AI_Content_Bool, AI_Content_Str);
            form2.ShowDialog();
            form2.Activate();
        }

        public void Open_Form3()
        {
            AI_Items.Clear();

            // Pick archive name with duration setting
            string thisArchiveName = "";
            if (comboBox1.Text == "Daily") { thisArchiveName = "ENERGY MANAGEMENT - 1HOUR"; }
            if (comboBox1.Text == "Monthly") { thisArchiveName = "ENERGY MANAGEMENT - 1 DAY"; }
            if (comboBox1.Text == "Yearly") { thisArchiveName = "ENERGY MANAGEMENT - 1 MONTH"; }

            richTextBox1.AppendText("Duration: " + comboBox1.Text + "\n");
            richTextBox1.ScrollToCaret();

            IRuntimeArchive thisArchive = thisProject.RuntimeArchiveCollection[thisArchiveName];
            if (thisArchive == null)        // If archive doesn't exist
            {
                richTextBox1.SelectionColor = Color.Red;
                richTextBox1.AppendText("ERROR >> Archive: " + thisArchiveName + " no found.\n");
                richTextBox1.SelectionColor = Color.Black;

                richTextBox1.AppendText("-------- End of Operation --------\n");
                richTextBox1.AppendText("\n");
                richTextBox1.ScrollToCaret();
                return;
            }

            richTextBox1.AppendText("Archive: " + thisArchiveName + "\n");
            richTextBox1.ScrollToCaret();

            IRuntimeArchiveFilter thisArchiveFilter = thisArchive.FilterCollection.Create();        // Create archive filter
            thisArchiveFilter.StartTime = DatePicker2Unix(dateTimePicker1.Value.Date)[0];           // Set time filter
            thisArchiveFilter.EndTime = DatePicker2Unix(dateTimePicker1.Value.Date)[1];

            richTextBox1.AppendText("Archive time set.\n");
            richTextBox1.ScrollToCaret();

            for (int k = 0; k < thisArchive.VariableCollection.Count; k++)
            {
                if (thisArchive.VariableCollection[k].AggregationType.ToString() != comboBox2.Text) { continue; }           // Skip if wrong data type

                if (All_ZEEEnergy.Exists(x => x.ZEEEnergy_Var == thisArchive.VariableCollection[k].Name))
                {
                    string thisEquipName = All_ZEEEnergy.Find(x => x.ZEEEnergy_Var == thisArchive.VariableCollection[k].Name).Var_EquipName;        // Get equipment name of the archive variable
                    if (EquipmentModels.Exists(x => (x.ModelChecked == true) && (x.ModelName == thisEquipName)))        // If equipment modeol selected
                    {
                        if (All_ZEEEnergy.Exists(x => (x.Var_EnergyType == comboBox3.Text) && (x.ZEEEnergy_Var == thisArchive.VariableCollection[k].Name)))         // Check energy type
                        {
                            thisArchiveFilter.AddVariable(thisArchive.VariableCollection[k]);

                            richTextBox1.AppendText(thisArchive.VariableCollection[k].Name + " added\n");
                            richTextBox1.ScrollToCaret();
                        }
                    }
                }
            }
            IRuntimeArchiveFilterVariableCollection ArchiveVariables = thisArchiveFilter.Query();   // confirm filter

            IRuntimeArchiveFilterVariable ArchiveFilterVariable = ArchiveVariables[0];      // Get variable from filtered archive, end program if null
            if (ArchiveFilterVariable == null)
            {
                MessageBox.Show("No filtered archive variable available!");
                return;
            }

            foreach (IRuntimeArchiveFilterVariable thisFilterVariable in ArchiveVariables)
            {
                //if (thisFilterVariable.ArchiveVariable.Name != ZEEItem.ZEEEnergy_Var) { continue; }
                if (thisFilterVariable.ArchiveVariable.AggregationType.ToString() != comboBox2.Text) { continue; }

                richTextBox1.AppendText("Variable: " + thisFilterVariable.ArchiveVariable.Name + "\n");
                richTextBox1.ScrollToCaret();

                foreach (IRuntimeArchiveValue archiveValue in thisFilterVariable.ArchiveValueCollection)
                {
                    // Add archive value to list for printing
                    AI_Items.Add(new AI_Content
                    {
                        timeStamp = archiveValue.Time,
                        VarType = All_ZEEEnergy.Find(x => x.ZEEEnergy_Var == thisFilterVariable.ArchiveVariable.Name).Var_EnergyType,
                        VarValue = archiveValue.Value.ToString(),
                        BayTitle = All_ZEEEnergy.Find(x => x.ZEEEnergy_Var == thisFilterVariable.ArchiveVariable.Name).Var_EquipName
                    });
                }

            }

            // If no values are added to the printing list
            if (AI_Items.Count < 1)
            {
                richTextBox1.SelectionColor = Color.Red;
                richTextBox1.AppendText("ERROR >> No AI value.\n");
                richTextBox1.SelectionColor = Color.Black;

                richTextBox1.AppendText("-------- End of Operation --------\n");
                richTextBox1.AppendText("\n");
                richTextBox1.ScrollToCaret();
                return;
            }

            string Info_Content = "";
            Info_Content = Info_Content + "Duration: " + comboBox1.Text + "\n";
            Info_Content = Info_Content + "DataType: " + comboBox2.Text + "\n";
            Info_Content = Info_Content + "Date: " + dateTimePicker1.Text + "\n";
            Info_Content = Info_Content + "EnergyType: " + comboBox3.Text + "\n" + "\n";

            richTextBox1.AppendText("-------- End of Operation --------\n");
            richTextBox1.AppendText("\n");
            richTextBox1.ScrollToCaret();

            // Open Form 3
            Form3 form3 = new Form3(AI_Items, Info_Content);
            form3.ShowDialog();
            form3.Activate();
        }

        // Calculate summary, average, minimum, maximum value
        public string Get4Calculation(List<AI_Content> New_AI_Items)
        {
            AI_FinalList.Clear();

            foreach (var AIitem_cal in New_AI_Items)
            {
                // If this vartype not written yet
                if (AI_FinalList.Exists(x => x.Category == AIitem_cal.VarType) == false)
                {
                    // Set the first value as value for all datatype of this vartype
                    AI_FinalList.Add(new AI_Final
                    {
                        Category = AIitem_cal.VarType,
                        Min_final = float.Parse(AIitem_cal.VarValue),
                        Max_final = float.Parse(AIitem_cal.VarValue),
                        Sum_final = float.Parse(AIitem_cal.VarValue),
                        Ave_final = float.Parse(AIitem_cal.VarValue),
                        AI_count = 1
                    });
                }

                // If this vartype is written
                else
                {
                    AI_Final thisAI = AI_FinalList.Find(x => x.Category == AIitem_cal.VarType);
                    int newCount = thisAI.AI_count + 1;

                    float newMin = thisAI.Min_final;
                    if (float.Parse(AIitem_cal.VarValue) < newMin) { newMin = float.Parse(AIitem_cal.VarValue); }

                    float newMax = thisAI.Max_final;
                    if (float.Parse(AIitem_cal.VarValue) > newMax) { newMax = float.Parse(AIitem_cal.VarValue); }

                    float newAve = (thisAI.Sum_final + float.Parse(AIitem_cal.VarValue)) / newCount;
                    float newSum = thisAI.Sum_final + float.Parse(AIitem_cal.VarValue);

                    // Update value of all datatype of this vartype
                    thisAI.AI_count = newCount;
                    thisAI.Min_final = newMin;
                    thisAI.Max_final = newMax;
                    thisAI.Ave_final = newAve;
                    thisAI.Sum_final = newSum;
                }
            }

            string Min_finalText = "Minimum,";
            string Max_finalText = "Maximum,";
            string Ave_finalText = "Average,";
            string Sum_finalText = "Summary,";

            // write everything to text for printing
            foreach (string AI_Name in AI_Content_Str)
            {
                if (AI_FinalList.Exists(x => x.Category == AI_Name) == false)
                {
                    Min_finalText = Min_finalText + ",NA";
                    Max_finalText = Max_finalText + ",NA";
                    Ave_finalText = Ave_finalText + ",NA";
                    Sum_finalText = Sum_finalText + ",NA";
                }
                else
                {
                    Min_finalText = Min_finalText + "," + AI_FinalList.Find(x => x.Category == AI_Name).Min_final;
                    Max_finalText = Max_finalText + "," + AI_FinalList.Find(x => x.Category == AI_Name).Max_final;
                    Ave_finalText = Ave_finalText + "," + AI_FinalList.Find(x => x.Category == AI_Name).Ave_final;
                    Sum_finalText = Sum_finalText + "," + AI_FinalList.Find(x => x.Category == AI_Name).Sum_final;
                }
            }
            Min_finalText = Min_finalText + "\n";
            Max_finalText = Max_finalText + "\n";
            Ave_finalText = Ave_finalText + "\n";
            Sum_finalText = Sum_finalText + "\n";

            string Cal4 = Min_finalText + Max_finalText + Ave_finalText + Sum_finalText;
            return Cal4;
        }

        // Set duration cycle time when combobox changed
        public void DurationSelection(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Daily")
            {
                dateTimePicker1.CustomFormat = "yyyy/MM/dd";
                dateTimePicker1.ShowUpDown = false;
                Duration_int = 3600;
            }
            if (comboBox1.Text == "Monthly")
            {
                dateTimePicker1.CustomFormat = "yyyy/MM";
                dateTimePicker1.ShowUpDown = true;
                Duration_int = 86400;
            }
            if (comboBox1.Text == "Yearly")
            {
                dateTimePicker1.CustomFormat = "yyyy";
                dateTimePicker1.ShowUpDown = true;
                Duration_int = 2073600;
            }

            //richTextBox1.AppendText("Duration int: " + Duration_int + "\n");
            //richTextBox1.ScrollToCaret();

            //richTextBox1.AppendText("Equipment end: " + Equipment_End + "\n");
            //richTextBox1.ScrollToCaret();
        }

        // Draw treeview
        public void EM_Tree()
        {
            comboBox1.Items.Add("Daily");
            comboBox1.Items.Add("Monthly");
            comboBox1.Items.Add("Yearly");

            comboBox2.Items.Add("Sum");
            comboBox2.Items.Add("Average");
            comboBox2.Items.Add("Minimum");
            comboBox2.Items.Add("Maximum");

            comboBox3.Items.Add("kWh+");
            comboBox3.Items.Add("kWh-");
            comboBox3.Items.Add("kVArh+");
            comboBox3.Items.Add("kVArh-");


            IEquipmentModeling thisEquipmentModeling = thisProject.EquipmentModeling;
            // Draw all substation of equipment model
            foreach (IEquipmentModel thisSubstation in thisEquipmentModeling)
            {
                richTextBox1.AppendText("Drawing " + thisSubstation.GetDynamicProperty("Name").ToString() + "...\n");
                richTextBox1.ScrollToCaret();

                treeView1.Nodes.Add(thisSubstation.GetDynamicProperty("Name").ToString());
                EquipmentModels.Add(new Equipments
                {
                    ModelName = thisSubstation.GetDynamicProperty("Name").ToString(),
                    ModelGuid = thisSubstation.GetDynamicProperty("Guid").ToString(),
                    ModelLevel = 1,
                    ModelChecked = false,
                });
                TreeNode thisSubstation_Node = GetNode(thisSubstation.GetDynamicProperty("Name").ToString(), treeView1.Nodes);          // Get node of this substation equipment model
                thisSubstation_Node.ExpandAll();
                if (Equipment_End < 1) { Equipment_End = 1; }

                // Draw all voltage level of equipment model
                foreach (IEquipmentGroup thisVoltageLevel in thisSubstation)
                {
                    thisSubstation_Node.Nodes.Add(thisVoltageLevel.GetDynamicProperty("Name").ToString());
                    EquipmentModels.Add(new Equipments
                    {
                        ModelName = thisVoltageLevel.GetDynamicProperty("Name").ToString(),
                        ModelGuid = thisVoltageLevel.GetDynamicProperty("Guid").ToString(),
                        ModelLevel = 2,
                        ModelChecked = false
                    });
                    TreeNode thisVoltageLevel_Node = GetNode(thisVoltageLevel.GetDynamicProperty("Name").ToString(), thisSubstation_Node.Nodes);            // Get node of this voltage level equipment model
                    thisVoltageLevel_Node.ExpandAll();
                    if (Equipment_End < 2) { Equipment_End = 2; }

                    // Draw all bay of equipment model
                    foreach (IEquipmentGroup thisBay in thisVoltageLevel)
                    {
                        thisVoltageLevel_Node.Nodes.Add(thisBay.GetDynamicProperty("Name").ToString());
                        EquipmentModels.Add(new Equipments
                        {
                            ModelName = thisBay.GetDynamicProperty("Name").ToString(),
                            ModelGuid = thisBay.GetDynamicProperty("Guid").ToString(),
                            ModelLevel = 3,
                            ModelChecked = false
                        });
                        TreeNode thisBay_Node = GetNode(thisBay.GetDynamicProperty("Name").ToString(), thisVoltageLevel_Node.Nodes);        // Get node of this bay equipment model
                        thisBay_Node.ExpandAll();
                        if (Equipment_End < 3) { Equipment_End = 3; }

                        // Draw all device of equipment model
                        foreach (IEquipmentGroup thisDevice in thisBay)
                        {
                            thisBay_Node.Nodes.Add(thisDevice.GetDynamicProperty("Name").ToString());
                            EquipmentModels.Add(new Equipments
                            {
                                ModelName = thisDevice.GetDynamicProperty("Name").ToString(),
                                ModelGuid = thisDevice.GetDynamicProperty("Guid").ToString(),
                                ModelLevel = 4,
                                ModelChecked = false,
                                ModelParentName = thisBay.GetDynamicProperty("Name").ToString()
                            });
                            if (Equipment_End < 4) { Equipment_End = 4; }
                        }
                    }
                }
            }

            // Set equipment name to each Energy class
            foreach (var ZEEItem in All_ZEEEnergy)
            {
                string thisZEE_Equipt = "";
                foreach (var thisEquip in EquipmentModels)
                {
                    if (ZEEItem.Var_BayName.Contains(thisEquip.ModelName))
                    {
                        thisZEE_Equipt = thisEquip.ModelName;
                        ZEEItem.Var_EquipName = thisZEE_Equipt;
                        continue;
                    }
                }
            }
        }

        // Convert unixInt timestampe to string datetime
        public string[] Int2date(Int32 datetime_int)
        {
            string date_DT = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(datetime_int).AddHours(8).ToShortDateString();
            string time_DT = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(datetime_int).AddHours(8).ToLongTimeString();
            string[] datetime_result = { date_DT, time_DT };
            return datetime_result;
        }

        // Get datetime for filter depending on duration selection
        public Int32[] DatePicker2Unix(DateTimeOffset dateOffset)
        {
            string Dura = comboBox1.Text;

            Int32 StartTime_int = 0;
            Int32 EndTime_int = 0;

            if (Dura == "Daily")
            {
                StartTime_int = Convert.ToInt32(dateOffset.ToUnixTimeSeconds());
                EndTime_int = StartTime_int + 86399;
            }

            if (Dura == "Monthly")
            {
                string ST_year = dateOffset.Year.ToString();
                int ST_month = dateOffset.Month;
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
                string ST_year = dateOffset.Year.ToString();
                string ST = ST_year + "0101000000";
                string ET = ST_year + "1231235959";

                DateTimeOffset ST_temp = DateTime.ParseExact(ST, "yyyyMMddHHmmss", provider);
                DateTimeOffset ET_temp = DateTime.ParseExact(ET, "yyyyMMddHHmmss", provider);
                StartTime_int = Convert.ToInt32(ST_temp.ToUnixTimeSeconds());
                EndTime_int = Convert.ToInt32(ET_temp.ToUnixTimeSeconds());
            }

            Int32[] Result = { StartTime_int, EndTime_int };
            return Result;
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

                Equipments thisEquip = EquipmentModels.Find(x => x.ModelName == e.Node.Text);
                thisEquip.ModelChecked = e.Node.Checked;
            }
        }

        // Sub program for getting child nodes checked when the parent one is checked
        private void CheckAllChildNodes(TreeNode treenode, bool nodeChecked)
        {
            foreach (TreeNode childNode in treenode.Nodes)
            {
                childNode.Checked = nodeChecked;

                Equipments thisEquip = EquipmentModels.Find(x => x.ModelName == childNode.Text);
                thisEquip.ModelChecked = nodeChecked;

                if (childNode.Nodes.Count > 0) { this.CheckAllChildNodes(childNode, nodeChecked); }     // If child nodes exist, checked them all
            }
        }

        private void AI_Value_Page_Click(object sender, EventArgs e)
        {
            Page1_Active();
        }

        private void Energy_Value_Page_Click(object sender, EventArgs e)
        {
            Page2_Active();
        }

        public void Page1_Active()
        {
            AI_Value_Page.BackColor = SystemColors.GradientActiveCaption;
            Energy_Value_Page.BackColor = SystemColors.ControlLightLight;

            label_Page1.Visible = true;
            label_Page2.Visible = false;

            label5.Visible = false;
            comboBox3.Visible = false;

            // Uncheck all equipment on treeview
            foreach (TreeNode thisNode in treeView1.Nodes)
            {
                thisNode.Checked = false;
                Equipments thisEquip = EquipmentModels.Find(x => x.ModelName == thisNode.Text);
                thisEquip.ModelChecked = false;

                CheckAllChildNodes(thisNode, false);
            }

            Page_num = 1;
        }

        public void Page2_Active()
        {
            AI_Value_Page.BackColor = SystemColors.ControlLightLight;
            Energy_Value_Page.BackColor = SystemColors.GradientActiveCaption;

            label_Page1.Visible = false;
            label_Page2.Visible = true;

            label5.Visible = true;
            comboBox3.Visible = true;

            // Check all equipment on treeview
            foreach (TreeNode thisNode in treeView1.Nodes)
            {
                thisNode.Checked = true;
                Equipments thisEquip = EquipmentModels.Find(x => x.ModelName == thisNode.Text);
                thisEquip.ModelChecked = true;

                CheckAllChildNodes(thisNode, true);
            }

            Page_num = 2;
        }

        public class Equipments
        {
            public string ModelName { get; set; }
            public int ModelLevel { get; set; }
            public bool ModelChecked { get; set; }
            public string ModelGuid { get; set; }
            public string ModelParentName { get; set; }
        }

        public class AI_Content
        {
            public int timeStamp { get; set; }
            public string VarType { get; set; }
            public string VarValue { get; set; }
            public string DataType { get; set; }
            public string BayTitle { get; set; }
        }

        public class items2Print
        {
            public string BayTitle_Print { get; set; }
            public int Timestamp { get; set; }
            public string TextContent { get; set; }
        }

        public class AI_Final
        {
            public string Category { get; set; }
            public float Min_final { get; set; }
            public float Max_final { get; set; }
            public float Sum_final { get; set; }
            public float Ave_final { get; set; }
            public int AI_count { get; set; }
        }

        public class ArchiveInfo
        {
            public string ArchiveName { get; set; }
            public string ArchiveIdentifier { get; set; }
            public string ArchiveSource { get; set; }
            public int ArchiveRecordingCycle { get; set; }
            public int ArchiveSavingCycle { get; set; }
            public int ActualCycle { get; set; }
        }

        public class ArchiveVarInfo
        {
            public string ArchiveIdentifier { get; set; }
            public string ArchiveVarName { get; set; }
            public string ArchiveDataType { get; set; }
            public string ArchiveVarBayName { get; set; }
            public string ArchiveVarGuid { get; set; }
        }

        public class ZEEEnergyVar
        {
            public string ZEEEnergy_Title { get; set; }
            public string ZEEEnergy_Var { get; set; }
            public string actualVar { get; set; }
            public string Var_BayName { get; set; }
            public string Var_EnergyType { get; set; }
            public string Var_EquipName { get; set; }
        }        
    }
}
