using Scada.AddIn.Contracts;
using Scada.AddIn.Contracts.Variable;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Generic;

namespace IEC61850_FileTransfer
{
    /// <summary>
    /// Description of Project Wizard Extension.
    /// </summary>
    [AddInExtension("IEC61850 FTP Tool [manually]", "Get DR files from IED")]
    public class ProjectWizardExtension : IProjectWizardExtension
    {
        #region IProjectWizardExtension implementation
        public static IProject thisProject;

        public void Run(IProject context, IBehavior behavior)
        {
            thisProject = context;

            Form1 form_UI = new Form1();
            form_UI.Activate();
            form_UI.ShowDialog();

        }
        #endregion
    }

}