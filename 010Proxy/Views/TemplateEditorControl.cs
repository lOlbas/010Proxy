using _010Proxy.Types;
using ScintillaNET;
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace _010Proxy.Views
{
    public partial class TemplateEditorControl : ProxyTabControl
    {
        private TemplateNode _source;
        private int _maxLineNumberCharLength;

        public TemplateEditorControl()
        {
            InitializeComponent();

            // Configuring the default style with properties
            // we have common to every lexer style saves time.
            templateEditor.StyleResetDefault();
            templateEditor.Styles[Style.Default].Font = "Courier New";
            templateEditor.Styles[Style.Default].Size = 10;
            templateEditor.StyleClearAll();

            // Configure the CPP (C#) lexer styles
            templateEditor.Styles[Style.Cpp.Default].ForeColor = Color.Silver;
            templateEditor.Styles[Style.Cpp.Comment].ForeColor = Color.FromArgb(0, 128, 0); // Green
            templateEditor.Styles[Style.Cpp.CommentLine].ForeColor = Color.FromArgb(0, 128, 0); // Green
            templateEditor.Styles[Style.Cpp.CommentLineDoc].ForeColor = Color.FromArgb(128, 128, 128); // Gray
            templateEditor.Styles[Style.Cpp.Number].ForeColor = Color.Olive;
            templateEditor.Styles[Style.Cpp.Word].ForeColor = Color.Blue;
            templateEditor.Styles[Style.Cpp.Word2].ForeColor = Color.Blue;
            templateEditor.Styles[Style.Cpp.String].ForeColor = Color.FromArgb(163, 21, 21); // Red
            templateEditor.Styles[Style.Cpp.Character].ForeColor = Color.FromArgb(163, 21, 21); // Red
            templateEditor.Styles[Style.Cpp.Verbatim].ForeColor = Color.FromArgb(163, 21, 21); // Red
            templateEditor.Styles[Style.Cpp.StringEol].BackColor = Color.Pink;
            templateEditor.Styles[Style.Cpp.Operator].ForeColor = Color.Purple;
            templateEditor.Styles[Style.Cpp.Preprocessor].ForeColor = Color.Maroon;
            templateEditor.Lexer = Lexer.Cpp;

            // Set the keywords
            templateEditor.SetKeywords(0, "abstract as base break case catch checked continue default delegate do else event explicit extern false finally fixed for foreach goto if implicit in interface internal is lock namespace new null object operator out override params private protected public readonly ref return sealed sizeof stackalloc switch this throw true try typeof unchecked unsafe using virtual while");
            templateEditor.SetKeywords(1, "bool byte char class const decimal double enum float int long sbyte short static string struct uint ulong ushort void");
        }

        public void LoadTemplate(TemplateNode source)
        {
            _source = source;

            templateEditor.Text = source.Code;

            ParentTab.Text = _source.Name;
        }

        private void templateEditor_TextChanged(object sender, EventArgs e)
        {
            // Did the number of characters in the line number display change?
            // i.e. nnn VS nn, or nnnn VS nn, etc...

            _source.Code = templateEditor.Text;
            ParentTab.Text = _source.Name + "*";

            var maxLineNumberCharLength = templateEditor.Lines.Count.ToString().Length;
            if (maxLineNumberCharLength == _maxLineNumberCharLength)
            {
                return;
            }

            // Calculate the width required to display the last line number
            // and include some padding for good measure.
            const int padding = 2;
            templateEditor.Margins[0].Width = templateEditor.TextWidth(Style.LineNumber, new string('9', maxLineNumberCharLength + 1)) + padding;
            _maxLineNumberCharLength = maxLineNumberCharLength;
        }

        private void templateEditor_CharAdded(object sender, CharAddedEventArgs e)
        {
            //The '}' char.
            if (e.Char == 125)
            {
                int curLine = templateEditor.LineFromPosition(templateEditor.CurrentPosition);

                if (templateEditor.Lines[curLine].Text.Trim() == "}")
                { //Check whether the bracket is the only thing on the line.. For cases like "if() { }".
                    SetIndent(templateEditor, curLine, GetIndent(templateEditor, curLine) - 4);
                }
            }
        }

        private void templateEditor_InsertCheck(object sender, InsertCheckEventArgs e)
        {
            if ((e.Text.EndsWith("\n") || e.Text.EndsWith("\r")))
            {
                int startPos = templateEditor.Lines[templateEditor.LineFromPosition(templateEditor.CurrentPosition)].Position;
                int endPos = e.Position;
                string curLineText = templateEditor.GetTextRange(startPos, (endPos - startPos)); //Text until the caret so that the whitespace is always equal in every line.

                Match indent = Regex.Match(curLineText, "^[ \\t]*");
                e.Text = (e.Text + indent.Value);
                if (Regex.IsMatch(curLineText, "{\\s*$"))
                {
                    e.Text = (e.Text + "    ");
                }
            }
        }

        #region CodeIndent Handlers

        private const int SCI_SETLINEINDENTATION = 2126;
        private const int SCI_GETLINEINDENTATION = 2127;

        private void SetIndent(Scintilla scin, int line, int indent)
        {
            scin.DirectMessage(SCI_SETLINEINDENTATION, new IntPtr(line), new IntPtr(indent));
        }

        private int GetIndent(Scintilla scin, int line)
        {
            return (scin.DirectMessage(SCI_GETLINEINDENTATION, new IntPtr(line), new IntPtr()).ToInt32());
        }

        #endregion

        private void templateEditor_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
            {
                ParentForm.SaveConfig();
                ParentTab.Text = _source.Name;

                e.SuppressKeyPress = true;
            }
        }
    }
}
