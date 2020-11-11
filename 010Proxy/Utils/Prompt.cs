using System.Windows.Forms;

namespace _010Proxy.Utils
{
    public static class Prompt
    {
        public static string ShowDialog(string text, string caption)
        {
            var prompt = new Form()
            {
                Width = 288,
                Height = 132,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            var textLabel = new Label() { Left = 12, Top = 12, Text = text };
            var textBox = new TextBox() { Left = 12, Top = 30, Width = 250 };
            var confirmation = new Button() { Text = "Add", Left = 100, Top = 60, Width = 74, DialogResult = DialogResult.OK };

            confirmation.Click += (sender, e) => { prompt.Close(); };

            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }
}
