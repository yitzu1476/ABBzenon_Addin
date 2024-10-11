using Scada.AddIn.Contracts;
using Scada.AddIn.Contracts.Variable;
using System;
using System.Windows.Forms;

namespace IEC61850_VariableDiagnosis_Container_81
{
    /// <summary>
    /// Description of Project Service Extension.
    /// </summary>
    [AddInExtension("IEC 61850 Variable Diagnosis Container", "IEC 61850 Variable Diagnosis Runtime Service (81)")]
    public class ProjectServiceExtension : IProjectServiceExtension
    {
        #region IProjectServiceExtension implementation
        IOnlineVariableContainer onlineContainer;
        IOnlineVariableContainer TempContainer;

        IProject thisProject;
        bool Container_active = false;

        public void Start(IProject context, IBehavior behavior)
        {
            thisProject = context;

            onlineContainer = context.OnlineVariableContainerCollection.Create("Test");
            onlineContainer.AddVariable("ABB_Diagnosis_VarList");
            onlineContainer.AddVariable("ABB_Diagnosis_ProfileList");

            IVariableCollection variableCollection = thisProject.VariableCollection;
            foreach (IVariable FindV in variableCollection)
            {
                if (FindV.Name.Contains("AddCause"))
                {
                    onlineContainer.AddVariable(FindV.Name);
                }
            }

            onlineContainer.ActivateBulkMode();
            onlineContainer.Activate();
            onlineContainer.BulkChanged += DiagnosisContainer;

        }

        // Container for default variables
        public void DiagnosisContainer(object sender, BulkChangedEventArgs e)
        {
            foreach (IVariable thisV in e.Variables)
            {
                if (thisV.Name != "ABB_Diagnosis_VarList") { continue; }

                string thisValue = thisV.GetValue(0).ToString();

                if (thisValue.Length > 2 && thisValue != "empty")
                {
                    if (Container_active == false)
                    {
                        TempContainer = thisProject.OnlineVariableContainerCollection.Create("Temp");

                        // Read all selected variable names from ABB_Diagnosis_VarList, and add to temp container
                        string[] allVarN = thisValue.Split(',');
                        foreach (string TempV in allVarN)
                        {
                            if (TempV.Length > 1)
                            {
                                TempContainer.AddVariable(TempV);
                                GetConnectionState_V(TempV);
                                GetDriver61(TempV);
                            }
                        }

                        TempContainer.ActivateBulkMode();
                        TempContainer.Activate();

                        Container_active = true;
                    }
                    else
                    {
                        // If temp container is already activated, deactivated it first
                        TempContainer.Deactivate();
                        thisProject.OnlineVariableContainerCollection.Delete(TempContainer.Name);
                        Container_active = false;

                        // Activate temp container again and add variables again
                        TempContainer = thisProject.OnlineVariableContainerCollection.Create("Temp");

                        string[] allVarN = thisValue.Split(',');
                        foreach (string TempV in allVarN)
                        {
                            if (TempV.Length > 1)
                            {
                                TempContainer.AddVariable(TempV);
                                GetConnectionState_V(TempV);
                                GetDriver61(TempV);
                            }
                        }

                        TempContainer.ActivateBulkMode();
                        TempContainer.Activate();

                        Container_active = true;

                    }
                }

                // Deactivate temp container if Diagnosis tool is closed
                if (thisValue.Trim() == "empty")
                {
                    TempContainer.Deactivate();
                    thisProject.OnlineVariableContainerCollection.Delete(TempContainer.Name);

                    Container_active = false;
                }

                MessageBox.Show("Variable container set.");
            }
        }

        // Get connection state from !CommecationState variable of each server
        public void GetConnectionState_V(string TempV_Name)
        {
            IVariableCollection variableCollection = thisProject.VariableCollection;
            IVariable TempV = variableCollection[TempV_Name];

            string connVarName = "";

            if (TempV.Driver.Name == "IEC850")
            {
                if (TempV.GetDynamicProperty("SymbAddr").ToString() == "*!ConnectionState") { return; }
                else
                {
                    foreach (IVariable thisV in variableCollection)
                    {
                        if (thisV.Name.Contains("!ConnectionState") == false) { continue; }


                        if (thisV.Driver.Identification == TempV.Driver.Identification)
                        {
                            if (thisV.NetAddress == TempV.NetAddress)
                            {
                                if (thisV.GetDynamicProperty("SymbAddr").ToString() == "*!ConnectionState")
                                {
                                    connVarName = thisV.Name;
                                    TempContainer.AddVariable(connVarName);
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }

        // Get driver connection of each driver
        public void GetDriver61(string thisVarName)
        {
            IVariableCollection variableCollection = thisProject.VariableCollection;
            IVariable thisVar = variableCollection[thisVarName];

            if (thisVar.GetDynamicProperty("ID_DriverTyp").ToString() == "35") { return; }

            if (thisVar.Driver.Name == "IEC850")
            {
                string thisComm_VarName = thisVar.Driver.Identification + "!Communication";
                TempContainer.AddVariable(thisComm_VarName);
            }
        }

        public void Stop()
        {
            onlineContainer.BulkChanged -= DiagnosisContainer;
            onlineContainer.Deactivate();
            thisProject.OnlineVariableContainerCollection.Delete(onlineContainer.Name);
        }

        #endregion
    }
}