using Scada.AddIn.Contracts;
using Scada.AddIn.Contracts.Variable;
using System;

namespace ABB_EnergyWizard_EditorTool
{
    /// <summary>
    /// Description of Engineering Studio Wizard Extension.
    /// </summary>
    [AddInExtension("ABB Energy Variable Editor Tool", "ABB Energy Variable Template Editor Tool", "ABB Energy Wizard")]
    public class EngineeringStudioWizardExtension : IEditorWizardExtension
    {
        #region IEditorWizardExtension implementation
        IProject thisProject;

        public void Run(IEditorApplication context, IBehavior behavior)
        {
            thisProject = context.Workspace.ActiveProject;

            Form1 form1 = new Form1(thisProject);
            form1.ShowDialog();
            form1.Activate();
        }

        #endregion
    }

}