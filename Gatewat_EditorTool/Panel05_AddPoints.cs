using Microsoft.Office.Interop.Excel;
using Scada.AddIn.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace Gateway_EditorTool
{
    internal class Panel05_AddPoints
    {
        IProject thisProject;
        RichTextBox thisRichTextBox;
        string TemplateFile_path;
        string AddPointsFile_path;
        string SavingPath;

        public Panel05_AddPoints(IProject mainProject, RichTextBox mainRichTextBox, string TemplatePath, string AddPointsPath)
        {
            thisProject = mainProject;
            thisRichTextBox = mainRichTextBox;
            TemplateFile_path = TemplatePath;
            AddPointsFile_path = AddPointsPath;

            SavingPath = TemplateFile_path.Substring(0, TemplateFile_path.Length - 27) + "\\GW_PointList";
            if (Directory.Exists(SavingPath) == false)
            {
                thisRichTextBox.AppendText("Error: No GW_PointList Folder.\n");
                thisRichTextBox.ScrollToCaret();
                return;
            }
        }

        public void Editor_createVar()
        {
            Excel.Application XLapp = new Excel.Application();
            Excel.Workbook XLworkBook = XLapp.Workbooks.Open(AddPointsFile_path);

            Excel._Worksheet workSheet_station = XLworkBook.Worksheets["New_Points"];

        }
    }
}
