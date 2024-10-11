using Scada.AddIn.Contracts;
using Scada.AddIn.Contracts.Historian;
using Scada.AddIn.Contracts.Variable;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ReportCreator_EquipmentModel
{
    /// <summary>
    /// Description of Project Wizard Extension.
    /// </summary>
    [AddInExtension("Report Creator with Equipment Modeling", "Report Creator with Equipment Modeling")]
    public class ProjectWizardExtension : IProjectWizardExtension
    {
        #region IProjectWizardExtension implementation

        public void Run(IProject context, IBehavior behavior)
        {
            IProject thisProject = context;

            Form1 form1 = new Form1(thisProject);
            form1.ShowDialog();
            form1.Activate();

        }

        #endregion
    }

}