using Scada.AddIn.Contracts;
using Scada.AddIn.Contracts.Variable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GW_EditorTool
{
    public class Creator_Class
    {
        IProject thisProject;
        RichTextBox ItemCreatorLogBox;
        
        public Creator_Class(IProject project, RichTextBox thisLogBox)
        {
            if (project != null) { thisProject = project; }
            ItemCreatorLogBox = thisLogBox;
        }

        public void VariableCreatorPLC(string varName, string DriverID, string dataType)
        {
            if (varName.Length <= 2)
            { ItemCreatorLogBox.AppendText("Variable name error\n"); ItemCreatorLogBox.ScrollToCaret(); return; }

            ChannelType varChannel = (ChannelType)Enum.Parse(typeof(ChannelType), "PlcMarker");

            if (DriverID.Length < 3)
            { DriverID = "Driver for internal variables"; varChannel = (ChannelType)Enum.Parse(typeof(ChannelType), "SystemDriverVariable"); }

            IDriver varDriver = thisProject.DriverCollection[DriverID];
            if (varDriver == null)
            { ItemCreatorLogBox.AppendText("Driver error for creating " + varName + "\n"); ItemCreatorLogBox.ScrollToCaret(); return; }

            IDataType varDataType = thisProject.DataTypeCollection[dataType];
            if (varDataType == null)
            { ItemCreatorLogBox.AppendText("Data type error for creating " + varName + "\n"); ItemCreatorLogBox.ScrollToCaret(); return; }

            thisProject.VariableCollection.Create(varName, varDriver, varChannel, varDataType);
            ItemCreatorLogBox.AppendText(varName + " variable created.\n");
            ItemCreatorLogBox.ScrollToCaret();
        }

        public void VariableCreatorDriverM(string varName, string DriverID, string dataType)
        {
            if (varName.Length <= 2)
            { ItemCreatorLogBox.AppendText("Variable name error\n"); ItemCreatorLogBox.ScrollToCaret(); return; }

            ChannelType varChannel = (ChannelType)Enum.Parse(typeof(ChannelType), "DriverVariable");

            IDriver varDriver = thisProject.DriverCollection[DriverID];
            if (varDriver == null)
            { ItemCreatorLogBox.AppendText("Driver error for creating " + varName + "\n"); ItemCreatorLogBox.ScrollToCaret(); return; }

            IDataType varDataType = thisProject.DataTypeCollection[dataType];
            if (varDataType == null)
            { ItemCreatorLogBox.AppendText("Data type error for creating " + varName + "\n"); ItemCreatorLogBox.ScrollToCaret(); return; }

            thisProject.VariableCollection.Create(varName, varDriver, varChannel, varDataType);
            ItemCreatorLogBox.AppendText(varName + " variable created.\n");
            ItemCreatorLogBox.ScrollToCaret();
        }

        public void VariableCreatorIV(string varName, string dataType)
        {
            if (varName.Length <= 2)
            { ItemCreatorLogBox.AppendText("Variable name error\n"); ItemCreatorLogBox.ScrollToCaret(); return; }

            IDriver InternalDriver = thisProject.DriverCollection["Driver for internal variables"];
            ChannelType varChannel = (ChannelType)Enum.Parse(typeof(ChannelType), "SystemDriverVariable");

            IDataType varDataType = thisProject.DataTypeCollection[dataType];
            if (varDataType == null)
            { ItemCreatorLogBox.AppendText("Data type error for creating " + varName + "\n"); ItemCreatorLogBox.ScrollToCaret(); return; }

            thisProject.VariableCollection.Create(varName, InternalDriver, varChannel, varDataType);
            ItemCreatorLogBox.AppendText(varName + " variable created.\n");
            ItemCreatorLogBox.ScrollToCaret();

        }

        public void VariableModifierPLC(string varName, int netA, string varDescription, string varUnit, string SymbolicAdd)
        {
            IVariable variable = thisProject.VariableCollection[varName];
            if (variable != null)
            {
                variable.NetAddress = netA;
                variable.Identification = varDescription;
                variable.Unit = varUnit;
                if (variable.DataType == thisProject.DataTypeCollection["REAL"]) { variable.Digits = 2; }
                if (SymbolicAdd != null) { variable.SetDynamicProperty("SymbAddr", SymbolicAdd); }

                ItemCreatorLogBox.AppendText(variable.Name + " variable modified.\n");
                ItemCreatorLogBox.ScrollToCaret();
            }
        }

        public void VariableModifierDriverM(string varName, int netA, int Voffset)
        {
            IVariable variable = thisProject.VariableCollection[varName];
            if (variable != null)
            {
                variable.NetAddress = netA;
                variable.Offset = Voffset;

                ItemCreatorLogBox.AppendText(variable.Name + " variable modified.\n");
                ItemCreatorLogBox.ScrollToCaret();
            }
        }

        public void VariableModifierIV(string varName, string varDescription, string varUnit)
        {
            IVariable variable = thisProject.VariableCollection[varName];
            if (variable != null)
            {
                variable.Identification = varDescription;
                variable.Unit = varUnit;

                ItemCreatorLogBox.AppendText(variable.Name + " variable modified.\n");
                ItemCreatorLogBox.ScrollToCaret();
            }
        }
    }
}
