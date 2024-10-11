using Scada.AddIn.Contracts;
using Scada.AddIn.Contracts.Variable;
using System;
using System.Collections.Generic;

namespace IEC61850_VariableDiagnosis_81
{
    /// <summary>
    /// Description of Project Wizard Extension.
    /// </summary>
    [AddInExtension("IEC 61850 Variable Diagnosis", "IEC 61850 Variable Diagnosis Viewer (81)")]
    public class ProjectWizardExtension : IProjectWizardExtension
    {
        #region IProjectWizardExtension implementation
        //public List<SelectedVar> SelectedVars_main = new List<SelectedVar>();
        List<ProfileID> Nprofiles = new List<ProfileID>();

        public void Run(IProject context, IBehavior behavior)
        {
            IProject thisProject = context;

            // Collect profile content
            IVariable ABB_Diagnosis_ProfileList = thisProject.VariableCollection["ABB_Diagnosis_ProfileList"];
            IVariableCollection variableCollection = thisProject.VariableCollection;

            string oldProfile = ABB_Diagnosis_ProfileList.GetValue(0).ToString();
            string[] Profile_AllC = oldProfile.Split(';');

            foreach (string Profile_C in Profile_AllC)
            {
                string[] Profile_C_split = Profile_C.Split(',');
                if (Profile_C_split.Length > 1)
                {
                    string ProfileTitle = Profile_C_split[0];
                    for (int i = 1; i < Profile_C_split.Length; i++)
                    {
                        if (Profile_C_split[i].Length < 1) { continue; }
                        int Profile_var_id = int.Parse(Profile_C_split[i]);
                        foreach (IVariable thisV in variableCollection)
                        {
                            if (thisV.Id == Profile_var_id)
                            {
                                string profile_varName = thisV.Name;
                                GlobalItems.ProfileList.Add(new GlobalItems.ProfileContent
                                {
                                    ProfileName = ProfileTitle,
                                    VarName = profile_varName,
                                });
                            }
                        }
                    }
                }
            }




            Form1 form1 = new Form1(thisProject);
            form1.ShowDialog();
            form1.Activate();

            IVariable ABB_Diagnosis_VarList = thisProject.VariableCollection["ABB_Diagnosis_VarList"];
            ABB_Diagnosis_VarList.SetValue(0, "empty");


            // Write to profile saving
            string NewProfile = "";


            foreach (var ProfileItem in GlobalItems.ProfileList)
            {
                if (Nprofiles.Exists(x => x.ProfileN == ProfileItem.ProfileName))
                {
                    string pre_ids = Nprofiles.Find(x => x.ProfileN == ProfileItem.ProfileName).VarIDs;

                    IVariable temp_Var = variableCollection[ProfileItem.VarName];
                    string temp_id = "," + temp_Var.Id.ToString();

                    pre_ids = pre_ids + temp_id;
                    Nprofiles.Find(x => x.ProfileN == ProfileItem.ProfileName).VarIDs = pre_ids;
                }
                else
                {
                    IVariable temp_Var = variableCollection[ProfileItem.VarName];
                    string temp_id = "," + temp_Var.Id.ToString();

                    Nprofiles.Add(new ProfileID
                    {
                        ProfileN = ProfileItem.ProfileName,
                        VarIDs = temp_id
                    });
                }
            }

            foreach (var Profile_str in Nprofiles)
            {
                NewProfile = NewProfile + Profile_str.ProfileN + Profile_str.VarIDs + ";";
            }

            ABB_Diagnosis_ProfileList.SetValue(0, NewProfile);

        }

        public class ProfileID
        {
            public string ProfileN { get; set; }
            public string VarIDs { get; set; }
        }

        #endregion
    }

}