using Scada.AddIn.Contracts;
using System;

namespace OPC_VariableCreator
{
    /// <summary>
    /// Description of Engineering Studio Wizard Extension.
    /// </summary>
    [AddInExtension("OPC Variable Creator", "OPC variable creator from SPA", "OPC Tool")]
    public class EngineeringStudioWizardExtension : IEditorWizardExtension
    {
        #region IEditorWizardExtension implementation

        public void Run(IEditorApplication context, IBehavior behavior)
        {
            IProject thisProject = context.Workspace.ActiveProject;

            Form1 form1 = new Form1(thisProject);
            form1.ShowDialog();
            form1.Activate();

        }

        #endregion
    }

}