using Scada.AddIn.Contracts;
using Scada.AddIn.Contracts.Function;
using Scada.AddIn.Contracts.Interlocking;
using Scada.AddIn.Contracts.Screen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gateway_EditorTool
{
    internal class Panel03_ToDelete
    {
        IProject thisProject;
        RichTextBox thisRichTextBox;
        List<CP_Items> CP_Items2Delete = new List<CP_Items>();
        List<BayP_Items> BayP_Items2Delete = new List<BayP_Items>();
        List<Navi14_Items> Navi14_Items2Delete = new List<Navi14_Items>();

        public Panel03_ToDelete(IProject project, RichTextBox richtextbox)
        {
            thisProject = project;
            thisRichTextBox = richtextbox;
        }

        public void Delete_CommandProcessing()
        {
            // Screen
            IScreenCollection screenCollection = thisProject.ScreenCollection;
            foreach (IScreen screen in screenCollection)
            {
                if (screen.ScreenType.ToString() == "CommandProcessing")
                {
                    CP_Items2Delete.Add(new CP_Items { ScreenName = screen.Name });
                }

                if (screen.Name.Contains("Command Processing"))
                {
                    CP_Items2Delete.Add(new CP_Items { ScreenName = screen.Name });
                }
            }

            // Function
            IFunctionCollection functionCollection = thisProject.FunctionCollection;
            foreach (IFunction function in functionCollection)
            {
                if (function.Type.ToString() == "ScreenSwitch" && function.Parameter.Contains("Command Processing"))
                {
                    CP_Items2Delete.Add(new CP_Items { FunctionName = function.Name });
                }

                if (function.Name.Contains("CP"))
                {
                    if (function.Name.Contains("On") || function.Name.Contains("Off"))
                    {
                        CP_Items2Delete.Add(new CP_Items { FunctionName = function.Name });
                    }
                }
            }

            // to delete
            foreach (var CP_item in CP_Items2Delete)
            {
                if (CP_item.ScreenName != null)
                {
                    screenCollection.Delete(CP_item.ScreenName);
                    thisRichTextBox.AppendText("Screen " + CP_item.ScreenName + " deleted.\n");
                    thisRichTextBox.ScrollToCaret();
                }

                if (CP_item.FunctionName != null)
                {
                    functionCollection.Delete(CP_item.FunctionName);
                    thisRichTextBox.AppendText("Function " + CP_item.FunctionName + " deleted.\n");
                    thisRichTextBox.ScrollToCaret();
                }

            }

            CP_Items2Delete.Clear();
            thisRichTextBox.AppendText("-------- End of Operation. --------\n");
            thisRichTextBox.AppendText("\n");
            thisRichTextBox.ScrollToCaret();
        }

        public void Delete_BayPopup()
        {
            // Screen
            IScreenCollection screenCollection = thisProject.ScreenCollection;
            foreach (IScreen screen in screenCollection)
            {
                if (screen.Name.Contains("Bay Popup"))
                {
                    BayP_Items2Delete.Add(new BayP_Items { ScreenName = screen.Name });
                }
            }

            // Function
            IFunctionCollection functionCollection = thisProject.FunctionCollection;
            foreach (IFunction function in functionCollection)
            {
                if (function.Name.Contains("Bay Popup"))
                {
                    BayP_Items2Delete.Add(new BayP_Items { FunctionName = function.Name });
                }
            }

            // to delete
            foreach (var BayP_item in BayP_Items2Delete)
            {
                if (BayP_item.ScreenName != null)
                {
                    screenCollection.Delete(BayP_item.ScreenName);
                    thisRichTextBox.AppendText("Screen " + BayP_item.ScreenName + " deleted.\n");
                    thisRichTextBox.ScrollToCaret();
                }

                if (BayP_item.FunctionName != null)
                {
                    functionCollection.Delete(BayP_item.FunctionName);
                    thisRichTextBox.AppendText("Function " + BayP_item.FunctionName + " deleted.\n");
                    thisRichTextBox.ScrollToCaret();
                }

            }

            BayP_Items2Delete.Clear();
            thisRichTextBox.AppendText("-------- End of Operation. --------\n");
            thisRichTextBox.AppendText("\n");
            thisRichTextBox.ScrollToCaret();

        }

        public void Delete_Navi1to4()
        {
            // Screen
            IScreenCollection screenCollection = thisProject.ScreenCollection;
            for (int i = 1; i < 5; i++)
            {
                string navi_str = "ZEE Navigation " + i;
                foreach (IScreen screen in screenCollection)
                {
                    if (screen.Name.Contains(navi_str))
                    {
                        Navi14_Items2Delete.Add(new Navi14_Items { ScreenName = screen.Name });
                    }
                }
            }

            // Function
            IFunctionCollection functionCollection = thisProject.FunctionCollection;
            for (int i = 1; i < 5; i++)
            {
                string navi_str = "ZEE Activate page Navigation " + i;
                foreach (IFunction function in functionCollection)
                {
                    if (function.Name.Contains(navi_str))
                    {
                        Navi14_Items2Delete.Add(new Navi14_Items { FunctionName = function.Name });
                    }
                }
            }

            // to delete
            foreach (var Navi_item in Navi14_Items2Delete)
            {
                if (Navi_item.ScreenName != null)
                {
                    screenCollection.Delete(Navi_item.ScreenName);
                    thisRichTextBox.AppendText("Screen " + Navi_item.ScreenName + " deleted.\n");
                    thisRichTextBox.ScrollToCaret();
                }

                if (Navi_item.FunctionName != null)
                {
                    functionCollection.Delete(Navi_item.FunctionName);
                    thisRichTextBox.AppendText("Function " + Navi_item.FunctionName + " deleted.\n");
                    thisRichTextBox.ScrollToCaret();
                }

            }

            Navi14_Items2Delete.Clear();
            thisRichTextBox.AppendText("-------- End of Operation. --------\n");
            thisRichTextBox.AppendText("\n");
            thisRichTextBox.ScrollToCaret();
        }


        public class CP_Items
        {
            public string ScreenName { get; set; }
            public string FunctionName { get; set; }
        }

        public class BayP_Items
        {
            public string ScreenName { get; set;}
            public string FunctionName { get; set;}
        }

        public class Navi14_Items
        {
            public string ScreenName { get; set;}
            public string FunctionName { get; set;}
        }

    }
}
