using Scada.AddIn.Contracts;
using Scada.AddIn.Contracts.Historian;
using Scada.AddIn.Contracts.Variable;
using System;
using System.Windows.Forms;

namespace ReportCreator_EnergyArchiveTool
{
    /// <summary>
    /// Description of Engineering Studio Wizard Extension.
    /// </summary>
    [AddInExtension("ReportCreator Energy Archive Tool", "Energy Report Archive Tool", "Energy Wizard Archive Tool")]
    public class EngineeringStudioWizardExtension : IEditorWizardExtension
    {
        IProject thisProject;
        #region IEditorWizardExtension implementation

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