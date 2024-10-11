using Scada.AddIn.Contracts;
using Scada.AddIn.Contracts.ReactionMatrix;
using Scada.AddIn.Contracts.Variable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IEC61850_FTOAuto_DRtrig_Editor
{
    public class Creator
    {
        IProject CreatorProject;

        public Creator(IProject thisProject)
        {
            if (thisProject != null) { CreatorProject = thisProject; }
        }

        public void VariableCreator(string varName, string DriverID)
        {
            IDriver varDriver = CreatorProject.DriverCollection[DriverID];
            ChannelType varChannel = (ChannelType)Enum.Parse(typeof(ChannelType), "SpecialMarker");
            IDataType varDataType = CreatorProject.DataTypeCollection["STRING"];

            CreatorProject.VariableCollection.Create(varName, varDriver, varChannel, varDataType);
            CreatorProject.Parent.Parent.DebugPrint(varName + " variable created.", DebugPrintStyle.Standard);
        }

        public void VariableModify(IVariable thisVar, int NetA, string symbolA, int len)
        {
            thisVar.NetAddress = NetA;
            thisVar.StringLength = len;
            thisVar.SetDynamicProperty("SymbAddr", symbolA);
            thisVar.SetDynamicProperty("DDEActive", true);
            thisVar.SetDynamicProperty("SV_VBA", false);

            CreatorProject.Parent.Parent.DebugPrint(thisVar.Name + " variable modified.", DebugPrintStyle.Standard);
        }


        public void VariableCreator_Rcd(string varName, string DriverID)
        {
            IDriver varDriver = CreatorProject.DriverCollection[DriverID];
            ChannelType varChannel = (ChannelType)Enum.Parse(typeof(ChannelType), "PlcMarker");
            IDataType varDataType = CreatorProject.DataTypeCollection["LREAL"];

            CreatorProject.VariableCollection.Create(varName, varDriver, varChannel, varDataType);
            CreatorProject.Parent.Parent.DebugPrint(varName + " variable created.", DebugPrintStyle.Standard);
        }

        public void VariableModify_Rcd(IVariable thisVar, int NetA, string symbolA)
        {
            thisVar.NetAddress = NetA;
            thisVar.SetDynamicProperty("SymbAddr", symbolA);
            thisVar.SetDynamicProperty("DDEActive", true);

            CreatorProject.Parent.Parent.DebugPrint(thisVar.Name + " variable modified.", DebugPrintStyle.Standard);
        }
    }
}
