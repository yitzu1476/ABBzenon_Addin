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
    public partial class Form4 : Form
    {
        string TempProfile = "";
        string Temp_VartoRemove = "";

        public Form4()
        {
            InitializeComponent();

            foreach (var ProfileItem in GlobalItems.ProfileList)
            {
                if (listBox1.Items.Contains(ProfileItem.ProfileName) == false)
                {
                    listBox1.Items.Add(ProfileItem.ProfileName);
                }
            }
        }

        // Show the content of the selected profile
        private void SeeContent_Click(object sender, EventArgs e)
        {
            listBox2.Items.Clear();
            string thisProfile = listBox1.SelectedItem.ToString();
            TempProfile = thisProfile;

            foreach (var ProfileItem in GlobalItems.ProfileList)
            {
                if (ProfileItem.ProfileName == thisProfile) { listBox2.Items.Add(ProfileItem.VarName); }
            }
        }

        // Delete selected profile
        private void DeleteProfile_Click(object sender, EventArgs e)
        {
            string thisProfile = listBox1.SelectedItem.ToString();
            listBox1.Items.Remove(thisProfile);

            while (GlobalItems.ProfileList.Exists(x => x.ProfileName == thisProfile) == true)
            {
                var thisProfileItem = GlobalItems.ProfileList.Find(x => x.ProfileName == thisProfile);
                GlobalItems.ProfileList.Remove(thisProfileItem);
            }

            listBox2.Items.Clear();
        }

        // Delete selected content from selected profile
        private void DeleteVariables_Click(object sender, EventArgs e)
        {
            foreach (string thisV in listBox2.SelectedItems)
            {
                while (GlobalItems.ProfileList.Exists(x => (x.VarName == thisV) && (x.ProfileName == TempProfile)))
                {
                    var thisProfileItem = GlobalItems.ProfileList.Find(x => (x.VarName == thisV) && (x.ProfileName == TempProfile));
                    GlobalItems.ProfileList.Remove(thisProfileItem);
                }
                Temp_VartoRemove = Temp_VartoRemove + "," + thisV;
            }

            string[] VarstoRemove = Temp_VartoRemove.Split(',');
            foreach (string VartoRemove in VarstoRemove)
            {
                if (VartoRemove.Length > 1) { listBox2.Items.Remove(VartoRemove); }
            }
        }
    }
}
