using _010Proxy.Forms;
using System.Windows.Forms;

namespace _010Proxy.Views
{
    public class ProxyTabControl : UserControl
    {
        public new MainForm ParentForm { get; set; }
        public TabPage ParentTab { get; set; }

        public ProxyTabControl()
        {
            OnCreate();
        }

        public virtual void OnCreate() { }

        public virtual void OnClose() { }

        public virtual void OnShow() { }

        public virtual void OnHide() { }

        protected virtual void UpdateMenu(bool show = true) { }
    }
}
