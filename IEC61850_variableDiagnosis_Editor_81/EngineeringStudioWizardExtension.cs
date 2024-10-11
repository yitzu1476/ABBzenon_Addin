using Scada.AddIn.Contracts;
using Scada.AddIn.Contracts.ReactionMatrix;
using Scada.AddIn.Contracts.Variable;
using System;
using System.Collections.Generic;
using System.IO;

namespace IEC61850_variableDiagnosis_Editor_81
{
    /// <summary>
    /// Description of Engineering Studio Wizard Extension.
    /// </summary>
    [AddInExtension("IEC 61850 Variable Diagnosis Editor Tool", "IEC 61850 Variable Diagnosis Item creator (81)", "IEC 61850 Variable Diagnosis")]
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