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
    public partial class Form3 : Form
    {

        string[] AllCloumnName = { "Variable Name" , "Driver Name" , "Last Update Time", "TCP Connection Status" , "mms Status" ,
                "Driver Connection" , "Actual Value" , "Cause of Transmission" , "Timestamp Code" , "Variable Status" , "Quality"};

        string[] AllColumnName2 = { "Variable Name", "Driver Name", "Last Update Time", "TCP Connection Status", "Actual Value",
            "Set Value", "Cause of Transmission", "AddCause" };

        public Form3()
        {
            InitializeComponent();
            checkedListBox1.Items.AddRange(AllCloumnName);
            checkedListBox2.Items.AddRange(AllColumnName2);

            // Add all column name to list
            if (GlobalItems.ColumnList.Count == 0)
            {
                for (int i = 0; i < AllCloumnName.Length; i++)
                {
                    string ColumnN = AllCloumnName[i];
                    GlobalItems.ColumnList.Add(new GlobalItems.ColumnAll
                    {
                        ColumnName = ColumnN,
                        ColumnCheck = true
                    });

                    checkedListBox1.SetItemChecked(i, true);
                }
            }
            else
            {
                // Update check status
                for (int j = 0; j < AllCloumnName.Length; j++)
                {
                    if (GlobalItems.ColumnList[j].ColumnCheck == true)
                    {
                        checkedListBox1.SetItemChecked(j, true);
                    }
                    else { checkedListBox1.SetItemChecked(j, false); }
                }
            }

            // Add all column name to list
            if (GlobalItems.ColumnList2.Count == 0)
            {
                for (int k = 0; k < AllColumnName2.Length; k++)
                {
                    string ColumnN = AllColumnName2[k];
                    GlobalItems.ColumnList2.Add(new GlobalItems.ColumnAll
                    {
                        ColumnName = ColumnN,
                        ColumnCheck = true
                    });

                    checkedListBox2.SetItemChecked(k, true);
                }
            }
            else
            {
                // Update check status
                for (int m = 0; m < AllColumnName2.Length; m++)
                {
                    if (GlobalItems.ColumnList2[m].ColumnCheck == true)
                    {
                        checkedListBox2.SetItemChecked(m, true);
                    }
                    else { checkedListBox2.SetItemChecked(m, false); }
                }
            }

        }

        // Update check status to globalitem
        private void Confirm_Click(object sender, EventArgs e)
        {
            for (int k = 0; k < AllCloumnName.Length; k++)
            {
                if (checkedListBox1.GetItemChecked(k) == true)
                {
                    GlobalItems.ColumnList[k].ColumnCheck = true;
                }
                else { GlobalItems.ColumnList[k].ColumnCheck = false; }
            }

            for (int n = 0; n < AllColumnName2.Length; n++)
            {
                if (checkedListBox2.GetItemChecked(n) == true)
                {
                    GlobalItems.ColumnList2[n].ColumnCheck = true;
                }
                else { GlobalItems.ColumnList2[n].ColumnCheck = false; }
            }

            this.Close();
        }
    }
}
