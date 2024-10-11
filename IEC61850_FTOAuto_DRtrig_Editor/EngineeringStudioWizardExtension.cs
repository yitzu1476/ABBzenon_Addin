using Scada.AddIn.Contracts;
using Scada.AddIn.Contracts.Variable;
using System;
using System.Collections.Generic;
using System.IO;

namespace IEC61850_FTOAuto_DRtrig_Editor
{
    /// <summary>
    /// Description of Engineering Studio Wizard Extension.
    /// </summary>
    [AddInExtension("FTP Tool Editor Content for DRmade (with address selection)", "FTP Tool Editor Content for DRmade", "IEC61850 FTP Tool")]
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