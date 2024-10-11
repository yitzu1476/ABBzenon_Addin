using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEC61850_VariableDiagnosis_81
{
    public class GlobalItems
    {
        internal static List<SelectedVar> SelectedVar_forall = new List<SelectedVar>();
        internal static List<ColumnAll> ColumnList = new List<ColumnAll>();
        internal static List<ColumnAll> ColumnList2 = new List<ColumnAll>();
        internal static List<ProfileContent> ProfileList = new List<ProfileContent>();
        internal static List<VarTable> VarSelections = new List<VarTable>();

        public class SelectedVar { public string VarName { get; set; } }

        public class ColumnAll
        {
            public string ColumnName { get; set; }
            public bool ColumnCheck { get; set; }
        }

        public class ProfileContent
        {
            public string ProfileName { get; set; }
            public string VarName { get; set; }
        }

        public class VarTable
        {
            public string VarName { get; set; }
            public string VarType { get; set; }
            public string VarID { get; set; }
            public string SymAddr { get; set; }
            public bool Selection { get; set; }
        }
    }
}
