using Scada.AddIn.Contracts;
using Scada.AddIn.Contracts.Variable;
using System;
using System.IO;
using System.Runtime.Remoting.Contexts;
using System.Threading;
using System.Windows.Forms;

namespace IEC61850_FTPAuto_DRtrig
{
    /// <summary>
    /// Description of Project Service Extension.
    /// </summary>
    [AddInExtension("FTP Tool with Rcd Made", "DR being collected when DR triggered", DefaultStartMode = DefaultStartupModes.Auto)]
    public class ProjectServiceExtension : IProjectServiceExtension
    {
        #region IProjectServiceExtension implementation
        IProject thisProject;
        IOnlineVariableContainer onlineContainer;
        IOnlineVariableContainer onlineContainer_DIR;
        IOnlineVariableContainer onlineContainer_Server;
        IOnlineVariableContainer onlineContainer_Time;

        IApplication application;
        bool FTP_Active = false;

        string txtName = "C:\\Temp\\DisturbanceRecords\\DR_Log.txt";
        string TxtLogFile_content = "";

        int DR_TimeSet = 10;


        public void Start(IProject context, IBehavior behavior)
        {
            thisProject = context;
            application = thisProject.Parent.Parent;

            // Create folde if not existed
            Directory.CreateDirectory("C:\\TEMP");
            Directory.CreateDirectory("C:\\TEMP\\DisturbanceRecords");

            // txt file for download log
            if (File.Exists(txtName) == false)
            {
                TxtLogFile_content = TxtLogFile_content + "System timestamp,File name\n";
                File.WriteAllText(txtName, TxtLogFile_content);
            }
            else
            {
                TxtLogFile_content = File.ReadAllText(txtName);
            }


            // If network is not active
            if (thisProject.IsNetworkActive == false)
            {
                thisProject.ChronologicalEventList.AddEventEntry("Start FTP Tool");
                FTP_Active = true;

                // Add variables to online container
                onlineContainer = context.OnlineVariableContainerCollection.Create("RcdMadeT_var");
                onlineContainer_DIR = context.OnlineVariableContainerCollection.Create("Directory_var");

                // TimeSet container
                onlineContainer_Time = context.OnlineVariableContainerCollection.Create("Dr_Timeset");
                onlineContainer_Time.AddVariable("DR_TimeSetting");
                onlineContainer_Time.ActivateBulkMode();
                onlineContainer_Time.Activate();
                onlineContainer_Time.BulkChanged += TimeSet_Var;


                IVariableCollection variableCollection = context.VariableCollection;
                foreach (IVariable variable in variableCollection)
                {
                    int var_leng = variable.Name.Length;
                    if (var_leng > 9 && variable.Name.Substring(var_leng - 9, 9) == "!RcdMadeT")
                    {
                        string bayName = variable.Name.Substring(0, var_leng - 9);

                        onlineContainer.AddVariable(variable.Name);
                        string folderName = "C:\\TEMP\\DisturbanceRecords\\" + bayName;
                        Directory.CreateDirectory(folderName);
                    }

                    if (var_leng > 10 && variable.Name.Substring(var_leng - 10, 10) == "!Directory")
                    {
                        onlineContainer_DIR.AddVariable(variable.Name);
                    }
                }

                onlineContainer.ActivateBulkMode();
                onlineContainer.Activate();
                onlineContainer.BulkChanged += RecordedAction;

                onlineContainer_DIR.ActivateBulkMode();
                onlineContainer_DIR.Activate();
                onlineContainer_DIR.BulkChanged += DirectoryAction;
            }

            // If network is active
            else
            {
                // Add network variable to online
                onlineContainer_Server = context.OnlineVariableContainerCollection.Create("Primary_Server");
                onlineContainer_Server.AddVariable("[Network] Current Primary Server");

                onlineContainer_Server.ActivateBulkMode();
                onlineContainer_Server.Activate();
                onlineContainer_Server.BulkChanged += PrimaryServerAction;
            }

        }

        public void TimeSet_Var(object sender, BulkChangedEventArgs e)
        {
            foreach (IVariable timeV in e.Variables)
            {
                int timeSet_value = int.Parse(timeV.GetValue(0).ToString());
                if (timeSet_value >= 1)
                {
                    DR_TimeSet = timeSet_value;

                    string txt_thisLog0 = "DR logging time set: " + DR_TimeSet + " days.\n";
                    TxtLogFile_content = TxtLogFile_content + txt_thisLog0;
                    File.WriteAllText(txtName, TxtLogFile_content);
                }
            }
        }

        // When the value of network variable changes
        public void PrimaryServerAction(object sender, BulkChangedEventArgs e)
        {
            foreach (IVariable PriServer_V in e.Variables)
            {
                // If this computer is primary server
                if (PriServer_V.GetValue(0).ToString().Contains(application.ComputerName))
                {
                    thisProject.ChronologicalEventList.AddEventEntry("Start FTP Tool");
                    FTP_Active = true;

                    // Add variables to online container
                    onlineContainer = thisProject.OnlineVariableContainerCollection.Create("RcdMadeT_var");
                    onlineContainer_DIR = thisProject.OnlineVariableContainerCollection.Create("Directory_var");

                    // TimeSet container
                    onlineContainer_Time = thisProject.OnlineVariableContainerCollection.Create("Dr_Timeset");
                    onlineContainer_Time.AddVariable("DR_TimeSetting");
                    onlineContainer_Time.ActivateBulkMode();
                    onlineContainer_Time.Activate();
                    onlineContainer_Time.BulkChanged += TimeSet_Var;

                    IVariableCollection variableCollection = thisProject.VariableCollection;
                    foreach (IVariable variable in variableCollection)
                    {
                        int var_leng = variable.Name.Length;
                        if (var_leng > 9 && variable.Name.Substring(var_leng - 9, 9) == "!RcdMadeT")
                        {
                            string bayName = variable.Name.Substring(0, var_leng - 9);

                            onlineContainer.AddVariable(variable.Name);
                            string folderName = "C:\\TEMP\\DisturbanceRecords\\" + bayName;
                            Directory.CreateDirectory(folderName);
                        }

                        if (var_leng > 10 && variable.Name.Substring(var_leng - 10, 10) == "!Directory")
                        {
                            onlineContainer_DIR.AddVariable(variable.Name);
                        }
                    }

                    onlineContainer.ActivateBulkMode();
                    onlineContainer.Activate();
                    onlineContainer.BulkChanged += RecordedAction;

                    onlineContainer_DIR.ActivateBulkMode();
                    onlineContainer_DIR.Activate();
                    onlineContainer_DIR.BulkChanged += DirectoryAction;
                }

                // If computer down level to secondary server
                if (PriServer_V.GetValue(0).ToString().Contains(application.ComputerName) == false)
                {
                    // Deactive online containers
                    if (FTP_Active)
                    {
                        onlineContainer.BulkChanged -= RecordedAction;
                        onlineContainer.Deactivate();
                        thisProject.OnlineVariableContainerCollection.Delete(onlineContainer.Name);

                        onlineContainer_DIR.BulkChanged -= DirectoryAction;
                        onlineContainer_DIR.Deactivate();
                        thisProject.OnlineVariableContainerCollection.Delete(onlineContainer_DIR.Name);
                    }

                    FTP_Active = false;
                }
            }
        }

        // When the value of RcdMadeT variable changes
        public void RecordedAction(object sender, BulkChangedEventArgs e)
        {
            foreach (IVariable Rec_variable in e.Variables)
            {
                string bay_str = Rec_variable.Name.Substring(0, Rec_variable.Name.Length-9);

                string COM_str = bay_str + "!Command";
                IVariable COMvar = thisProject.VariableCollection[COM_str];

                // Set command to get directory info
                COMvar.SetValue(0, "DIR");

                string txt_thisLog = COMvar.Name + " set DIR.\n";
                TxtLogFile_content = TxtLogFile_content + txt_thisLog;
                File.WriteAllText(txtName, TxtLogFile_content);
            }

            // Remove all CFG & DAT files in C://Temp
            string[] TempFiles = Directory.GetFiles("C://TEMP");
            foreach (string file in TempFiles)
            {
                if (file.Substring(file.Length - 3, 3).Equals("cfg", StringComparison.OrdinalIgnoreCase)) { File.Delete(file); }
                if (file.Substring(file.Length - 3, 3).Equals("dat", StringComparison.OrdinalIgnoreCase)) { File.Delete(file); }
            }
        }

        // When the value of Directory variable changes
        public void DirectoryAction(object sender, BulkChangedEventArgs e)
        {
            foreach (IVariable DIR_variable in e.Variables)
            {
                string txt_thisLog = DIR_variable.Name + " DIR changed.\n";
                TxtLogFile_content = TxtLogFile_content + txt_thisLog;
                File.WriteAllText(txtName, TxtLogFile_content);

                string DIR_value = DIR_variable.GetValue(0).ToString();
                string[] DIR_allItem = DIR_value.Split('\n');

                if (DIR_allItem.Length > 3)
                {
                    if (DIR_variable.Name.Length <= 10) { continue; }
                    string bayName = DIR_variable.Name.Substring(0,DIR_variable.Name.Length-10);
                    string folderPath = "C:\\TEMP\\DisturbanceRecords\\" + bayName;

                    foreach (string item in DIR_allItem)
                    {
                        // each item content >> fileName;fileSize;fileTimestamp
                        string[] item_content = item.Split(';');
                        if (item_content.Length == 3)
                        {
                            string item_name = item_content[0].Trim();
                            string item_time = item_content[2];

                            // If file type is CFG or DAT
                            if ((item_name.Substring(item_name.Length-3,3).Equals("cfg", StringComparison.OrdinalIgnoreCase) == false) && (item_name.Substring(item_name.Length - 3, 3).Equals("dat", StringComparison.OrdinalIgnoreCase) == false)) { continue; }
                            item_time = item_time.Replace(' ', '_');
                            item_time = item_time.Replace(':', '-');

                            // Time compare
                            DateTime NowTime_DT = DateTime.Now;
                            string thisTime_Str = item_time.Split('_')[0];
                            DateTime thisTime_DT = DateTime.ParseExact(thisTime_Str, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                            int TimeCompare = (NowTime_DT - thisTime_DT).Days;

                            if (TimeCompare > DR_TimeSet) { continue; }

                            string[] item_nameS;
                            string short_name = item_name;
                            if (item_name.Contains("/")) { item_nameS = item_name.Split('/'); }
                            else { item_nameS = item_name.Split('\\'); }
                            
                            if (item_nameS.Length > 1) { short_name = item_nameS[1]; }

                            string newFileName = bayName.Trim() + "_" + item_time + "_" + short_name;
                            string newFilePath = folderPath.Trim() + "\\" + newFileName;

                            if (File.Exists(newFilePath)) { continue; }

                            // Collect file
                            string COMvar_str = bayName + "!Command";
                            IVariable COMvar = thisProject.VariableCollection[COMvar_str];
                            COMvar.SetValue(0, "GET " + item_name);

                            // txt log content
                            string thisTime = DateTime.Now.ToString();
                            string txt_thisLog2 = thisTime + "," + newFileName + "\n";
                            TxtLogFile_content = TxtLogFile_content + txt_thisLog2;
                            File.WriteAllText(txtName, TxtLogFile_content);

                            string oldFilePath = "C:\\TEMP\\" + short_name;

                            Thread.Sleep(10);

                            // If file is downloaded, move file to target folder; it file is not downloaded yet, wait 5ms; longest wait time: 500ms
                            bool Moved = false;
                            int cnt = 0;
                            while (cnt < 200)
                            {
                                if (File.Exists(oldFilePath) == true)
                                {
                                    File.Move(oldFilePath, newFilePath);

                                    string txt_thisLog3 = newFilePath + " moved successfully.\n";
                                    TxtLogFile_content = TxtLogFile_content + txt_thisLog3;
                                    File.WriteAllText(txtName, TxtLogFile_content);
                                    Moved = true;
                                    cnt = 500;
                                }
                                else
                                {
                                    Thread.Sleep(5); cnt = cnt + 1;
                                }
                            }

                            if (Moved == false)
                            {
                                string txt_thisLog4 = oldFilePath + " timeout error.\n";
                                TxtLogFile_content = TxtLogFile_content + txt_thisLog4;
                                File.WriteAllText(txtName, TxtLogFile_content);
                            }
                            
                        }
                    }
                }
            }

            // Remove all CFG & DAT files in C://Temp
            string[] TempFiles = Directory.GetFiles("C://TEMP");
            foreach (string file in TempFiles)
            {
                if (file.Substring(file.Length - 3, 3).Equals("cfg", StringComparison.OrdinalIgnoreCase)) { File.Delete(file); }
                if (file.Substring(file.Length - 3, 3).Equals("dat", StringComparison.OrdinalIgnoreCase)) { File.Delete(file); }
            }

        }


        // When stop the service, deactive all online containers
        public void Stop()
        {
            if (FTP_Active && thisProject.IsNetworkActive)
            {
                onlineContainer_Server.BulkChanged -= PrimaryServerAction;
                onlineContainer_Server.Deactivate();
                thisProject.OnlineVariableContainerCollection.Delete(onlineContainer_Server.Name);
            }


            if (FTP_Active)
            {
                onlineContainer.BulkChanged -= RecordedAction;
                onlineContainer.Deactivate();
                thisProject.OnlineVariableContainerCollection.Delete(onlineContainer.Name);

                onlineContainer_DIR.BulkChanged -= DirectoryAction;
                onlineContainer_DIR.Deactivate();
                thisProject.OnlineVariableContainerCollection.Delete(onlineContainer_DIR.Name);

                onlineContainer_Time.BulkChanged -= TimeSet_Var;
                onlineContainer_Time.Deactivate();
                thisProject.OnlineVariableContainerCollection.Delete(onlineContainer_Time.Name);
            }
        }

        #endregion
    }
}