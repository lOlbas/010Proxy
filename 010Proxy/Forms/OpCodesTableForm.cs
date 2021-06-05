using System;
using _010Proxy.Views;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace _010Proxy.Forms
{
    public partial class OpCodesTableForm : Form
    {
        public OpCodesTableForm()
        {
            InitializeComponent();
        }

        public void DisplayOpCodes(List<Type> opCodesTypes, Dictionary<Type, List<object>> ignoreList)
        {
            foreach (var type in opCodesTypes)
            {
                if (!ignoreList.TryGetValue(type, out var typeIgnoreList))
                {
                    typeIgnoreList = new List<object>();
                }

                var newTabPage = new TabPage(type.Name)
                {
                    Controls =
                    {
                        new OpCodesTableControl(type, typeIgnoreList)
                        {
                            Name = "OpCodesTableControl",
                            Dock = DockStyle.Fill
                        }
                    }
                };

                tabControl.TabPages.Add(newTabPage);
            }
        }

        public Dictionary<Type, List<object>> GetIgnoreList()
        {
            var ignoreList = new Dictionary<Type, List<object>>();

            foreach (TabPage page in tabControl.TabPages)
            {
                var ctrl = page.Controls.Find("OpCodesTableControl", true)[0];

                if (ctrl is OpCodesTableControl opCodesTableControl)
                {
                    ignoreList[opCodesTableControl.Type] = opCodesTableControl.GetIgnoreList();
                }
            }

            return ignoreList;
        }
    }
}
