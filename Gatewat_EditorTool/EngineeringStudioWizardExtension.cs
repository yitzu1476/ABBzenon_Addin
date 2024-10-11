using Microsoft.Office.Interop.Excel;
using OfficeOpenXml;
using Scada.AddIn.Contracts;
using Scada.AddIn.Contracts.Variable;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace GW_EditorTool
{
    /// <summary>
    /// Description of Engineering Studio Wizard Extension.
    /// </summary>
    [AddInExtension("Gateway Editor Tool", "Editor tool for gateway engineering", "Gateway Tool")]
    public class EngineeringStudioWizardExtension : IEditorWizardExtension
    {
        #region IEditorWizardExtension implementation
        public static IProject thisProject;

        public void Run(IEditorApplication context, IBehavior behavior)
        {
            thisProject = context.Workspace.ActiveProject;

            Form1 thisForm = new Form1();
            thisForm.Activate();
            thisForm.ShowDialog();

        }

        #endregion
    }

}