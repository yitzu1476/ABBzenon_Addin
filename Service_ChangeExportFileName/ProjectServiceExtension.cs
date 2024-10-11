using Scada.AddIn.Contracts;
using System;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Runtime.Remoting.Contexts;

namespace Service_ChangeExportFileName
{
    [AddInExtension("Export File Name Changing", "Auto change the name for exported files", DefaultStartMode = DefaultStartupModes.Auto)]

    public class ProjectServiceExtension : IProjectServiceExtension
    {
        #region IProjectServiceExtension implementation
        public static IProject thisProject = null;

        public void Start(IProject context, IBehavior behavior)
        {
            thisProject = context;

            FileSystemWatcher watcher = new FileSystemWatcher();

            string folder = context.GetFolderPath(FolderPath.Export);
            watcher.Path = folder;
            watcher.NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.FileName | NotifyFilters.LastWrite;

            watcher.Created += new FileSystemEventHandler(Event_triggered);
            watcher.EnableRaisingEvents = true;
        }

        public static void Event_triggered(object sender, FileSystemEventArgs e)
        {
            if (e.Name.Substring(0, 1) != "2")
            {
                string folder = thisProject.GetFolderPath(FolderPath.Export);
                string nowTime = DateTime.Now.ToString("yyyyMMddHHmmss");

                string new_name = folder + "\\" + nowTime + "-" +  e.Name;
                File.Move(e.FullPath, new_name);
            }
        }

        public void Stop()
        {
            // enter your code which should be executed when stopping the service for the SCADA Service Engine
        }

        #endregion
    }
}