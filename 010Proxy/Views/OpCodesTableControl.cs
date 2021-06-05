using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace _010Proxy.Views
{
    public partial class OpCodesTableControl : UserControl
    {
        public Type Type { get; }

        public OpCodesTableControl(Type type, List<object> ignoreList)
        {
            InitializeComponent();
            Type = type;
            DisplayOpCodes(type, ignoreList);
        }

        private void DisplayOpCodes(Type type, List<object> ignoreList)
        {
            var enumType = Enum.GetUnderlyingType(type);

            foreach (var opCode in Enum.GetValues(type))
            {
                opCodesTable.Rows.Add(1);
                opCodesTable.Rows[opCodesTable.RowCount - 1].Cells[0].Value = !ignoreList.Contains(opCode);
                opCodesTable.Rows[opCodesTable.RowCount - 1].Cells[1].Value = Convert.ChangeType(opCode, enumType);
                opCodesTable.Rows[opCodesTable.RowCount - 1].Cells[2].Value = opCode;
            }
        }

        public List<object> GetIgnoreList()
        {
            var ignoreList = new List<object>();

            foreach (DataGridViewRow row in opCodesTable.Rows)
            {
                if (Convert.ToBoolean(row.Cells["filter"].Value) == false)
                {
                    ignoreList.Add(row.Cells[2].Value);
                }
            }

            return ignoreList;
        }
    }
}
