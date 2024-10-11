using Scada.AddIn.Contracts;
using Scada.AddIn.Contracts.Historian;
using System;
using System.Collections.Generic;

namespace Archive2CSV
{
    /// <summary>
    /// Description of Project Wizard Extension.
    /// </summary>
    [AddInExtension("Archive to CSV", "Create CSV Report for Archive")]
    public class ProjectWizardExtension : IProjectWizardExtension
    {
        #region IProjectWizardExtension implementation
        public static IProject thisProject = null;
        public static List<Archive_Info_Class> ArchiveInfo = new List<Archive_Info_Class>();
        public static List<Archive_Variables_Class> ArchVariables = new List<Archive_Variables_Class>();

        public void Run(IProject context, IBehavior behavior)
        {
            thisProject = context;

            // Load historian information to the list ArchiveInfo
            IRuntimeArchiveCollection runtimeArchives = thisProject.RuntimeArchiveCollection;
            foreach (IRuntimeArchive archive in runtimeArchives)
            {
                ArchiveInfo.Add(new Archive_Info_Class
                {
                    ArchiveName = archive.Name,
                    ArchiveIdentifier = archive.Identifier,
                    ArchiveSource = archive.GetDynamicProperty("SourceShortName").ToString(),
                    ArchiveRecordingCycle = archive.Cycle,
                    ArchiveSavingCycle = int.Parse(archive.GetDynamicProperty("Cycle").ToString()),
                    ArchiveLevel = 100

                });

                // Load all variables in historian to the list ArchVariables
                foreach (IRuntimeArchiveVariable ArchVar in archive.VariableCollection)
                {
                    string VarAT = "";
                    if (ArchVar.AggregationType.ToString() != "All") { VarAT = "[" + ArchVar.AggregationType.ToString() + "]"; }
                    string VarN = ArchVar.Name + VarAT;
                    ArchVariables.Add(new Archive_Variables_Class
                    {
                        ArchiveID = archive.Identifier,
                        ArchV = ArchVar.Name,
                        ArchVarName = VarN,
                        ArchVarChecked = false
                    });
                }
            }

            // Activate Form 1
            Form1 form_hmi = new Form1();
            form_hmi.Activate();
            form_hmi.ShowDialog();

        }

        // Create class for Historian information
        public class Archive_Info_Class
        {
            public string ArchiveName { get; set; }
            public string ArchiveIdentifier { get; set; }
            public string ArchiveSource { get; set; }
            public int ArchiveRecordingCycle { get; set; }
            public int ArchiveSavingCycle { get; set; }
            public int ArchiveLevel { get; set; }
        }

        // Create class for historian variable information
        public class Archive_Variables_Class
        {
            public string ArchiveID { get; set; }
            public string ArchV { get; set; }
            public string ArchVarName { get; set; }
            public bool ArchVarChecked { get; set; }

        }

        #endregion
    }

}