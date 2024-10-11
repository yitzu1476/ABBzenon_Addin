using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gateway_EditorTool
{
    internal class Connection_XML_Creator
    {
        public Connection_XML_Creator() { }

        public string BasicSettingA(string ProgramName)
        {
            string BasicStringA = "<?xml version=\"1.0\" encoding=\"iso-8859-1\" standalone=\"yes\"?>\r\n" +
                "<K5project version=\"1.1\">\r\n   <programs>\r\n      " +
                "<pou name=\"" + ProgramName + "\" kind=\"program\" period=\"1\" phase=\"0\" lge=\"ST\" desc=\"Cyclic program\">\r\n         " +
                "<vargroup name=\"" + ProgramName + "\" kind=\"LOCAL\">\r\n";
            return BasicStringA;
        }

        public string VarSetting(string VarName, string VarType)
        {
            string VarString = "            <var name=\"" + VarName + "\" type=\"" + VarType + "\"/>\r\n";
            return VarString;
        }

        public string BasicSettingB(string ProgramName)
        {
            string BasicStringB = "         </vargroup>\r\n<defines name=\"" + ProgramName + "\"></defines>\r\n         " +
                "<srcdic>[CONTEXT]\r\nHEXADISPLAY=OFF\r\nEXPANDED=main,(Global)\r\nEXPANDED_SUBGROUPS=\r\nSORT_COL=0\r\nSORT_ASCENDING=ON\r\nNBCOL=12\r\n\r\n" +
                "[COL0]\r\nPOSITION=0\r\nNAME=Name\r\nTYPE=1\r\nTYPEEX=0\r\nWIDTH=612\r\nSHOW=ON\r\nFILTER=\r\nNBFILTER=0\r\n\r\n" +
                "[COL1]\r\nPOSITION=-1\r\nNAME=Value\r\nTYPE=8\r\nTYPEEX=0\r\nWIDTH=70\r\nSHOW=OFF\r\nFILTER=\r\nNBFILTER=0\r\n\r\n" +
                "[COL2]\r\nPOSITION=1\r\nNAME=Type\r\nTYPE=2\r\nTYPEEX=0\r\nWIDTH=80\r\nSHOW=ON\r\nFILTER=\r\nNBFILTER=0\r\n\r\n" +
                "[COL3]\r\nPOSITION=2\r\nNAME=Dim.\r\nTYPE=3\r\nTYPEEX=0\r\nWIDTH=40\r\nSHOW=ON\r\nFILTER=\r\nNBFILTER=0\r\n\r\n" +
                "[COL4]\r\nPOSITION=3\r\nNAME=Attrib.\r\nTYPE=4\r\nTYPEEX=0\r\nWIDTH=75\r\nSHOW=ON\r\nFILTER=\r\nNBFILTER=0\r\n\r\n" +
                "[COL5]\r\nPOSITION=4\r\nNAME=Syb.\r\nTYPE=9\r\nTYPEEX=0\r\nWIDTH=40\r\nSHOW=ON\r\nFILTER=\r\nNBFILTER=0\r\n\r\n" +
                "[COL6]\r\nPOSITION=5\r\nNAME=Init value\r\nTYPE=5\r\nTYPEEX=0\r\nWIDTH=60\r\nSHOW=ON\r\nFILTER=\r\nNBFILTER=0\r\n\r\n" +
                "[COL7]\r\nPOSITION=6\r\nNAME=User Group\r\nTYPE=14\r\nTYPEEX=0\r\nWIDTH=50\r\nSHOW=ON\r\nFILTER=\r\nNBFILTER=0\r\n\r\n" +
                "[COL8]\r\nPOSITION=7\r\nNAME=Tag\r\nTYPE=6\r\nTYPEEX=0\r\nWIDTH=50\r\nSHOW=ON\r\nFILTER=\r\nNBFILTER=0\r\n\r\n" +
                "[COL9]\r\nPOSITION=8\r\nNAME=Description\r\nTYPE=7\r\nTYPEEX=0\r\nWIDTH=500\r\nSHOW=ON\r\nFILTER=\r\nNBFILTER=0\r\n\r\n" +
                "[COL10]\r\nPOSITION=-1\r\nNAME=Properties\r\nTYPE=10\r\nTYPEEX=1\r\nWIDTH=100\r\nSHOW=OFF\r\nFILTER=\r\nNBFILTER=0\r\n\r\n" +
                "[COL11]\r\nPOSITION=-1\r\nNAME=Public\r\nTYPE=17\r\nTYPEEX=0\r\nWIDTH=50\r\nSHOW=OFF\r\nFILTER=\r\nNBFILTER=0\r\n\r\n" +
                "[FIND]\r\nWHAT=\r\nFLAGS=65537\r\n\r\n[REPLACE]\r\nWHAT=\r\nREPLACE=\r\nFLAGS=65537\r\nUPDATE=ON\r\n\r\n         </srcdic>\r\n";
            return BasicStringB;
        }

        public string ProgramContent(string ProgramOperation)
        {
            string ProgramA = "         <sourceSTIL>(* add your code here *)\r\n";
            string ProgramB = "         </sourceSTIL>\r\n      </pou>\r\n   </programs>\r\n</K5project>";
            string AllContent = ProgramA + ProgramOperation + ProgramB;

            return AllContent;
        }
    }
}
