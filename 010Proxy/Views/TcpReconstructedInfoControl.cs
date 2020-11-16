using _010Proxy.Forms;
using _010Proxy.Network;
using _010Proxy.Network.TCP;
using _010Proxy.Parsers;
using _010Proxy.Types;
using Be.Windows.Forms;
using Microsoft.CodeDom.Providers.DotNetCompilerPlatform;
using ProtoBuf;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace _010Proxy.Views
{
    public partial class TcpReconstructedInfoControl : ProxyTabControl
    {
        private ConnectionInfo _connectionInfo;
        private int _flowIndex = 1;
        private bool _stickToBottom = true;

        private TemplateParser _templateParser;

        private static readonly IEnumerable<string> DefaultNamespaces =
            new[]
            {
                "System",
                "System.IO",
                "System.Net",
                "System.Linq",
                "System.Text",
                "System.Text.RegularExpressions",
                "System.Collections.Generic"
            };

        public TcpReconstructedInfoControl()
        {
            InitializeComponent();
        }

        public void LoadFlows(ConnectionInfo connectionInfo)
        {
            _connectionInfo = connectionInfo;

            foreach (var flow in _connectionInfo.Flows.ToList())
            {
                DisplayFlow(flow);
            }

            _connectionInfo.OnNewFlow += OnNewFlow;
            _connectionInfo.OnFlowUpdate += OnFlowUpdate;
        }

        private void DisplayFlow(TcpFlow tcpFlow)
        {
            packetsTable.Invoke((MethodInvoker)delegate
            {
                var icon = tcpFlow.Sender == SenderEnum.Client ? Properties.Resources.IconClientSource_v2 : Properties.Resources.IconServerSource_v2;
                var firstPacketTime = tcpFlow.FirstPacketTime.Date.ToLocalTime().ToString("HH:mm:ss.ffffff");
                var lastPacketTime = tcpFlow.LastPacketTime.Date.ToLocalTime().ToString("HH:mm:ss.ffffff");

                var rowIndex = packetsTable.Rows.Add(icon, _flowIndex++, firstPacketTime, lastPacketTime, tcpFlow.PacketsInfo.Count, tcpFlow.FlowData.Count, "");

                packetsTable.Rows[rowIndex].Tag = tcpFlow;

                ApplyProtocol(rowIndex);

                if (_stickToBottom)
                {
                    packetsTable.FirstDisplayedScrollingRowIndex = packetsTable.RowCount - 1;
                }
            });
        }

        private void OnNewFlow(TcpFlow tcpFlow)
        {
            DisplayFlow(tcpFlow);
        }

        private void OnFlowUpdate(TcpFlow tcpFlow)
        {
            for (var i = packetsTable.Rows.Count - 1; i >= 0; i--)
            {
                if (packetsTable.Rows[i].Tag == tcpFlow)
                {
                    lock (packetsTable.Rows[i].Cells)
                    {
                        packetsTable.Rows[i].Cells[3].Value = tcpFlow.LastPacketTime.Date.ToLocalTime().ToString("HH:mm:ss.ffffff");
                        packetsTable.Rows[i].Cells[4].Value = tcpFlow.PacketsInfo.Count;
                        packetsTable.Rows[i].Cells[5].Value = tcpFlow.FlowData.Count;

                        ApplyProtocol(i);
                    }

                    break;
                }
            }
        }

        public void ApplyProtocol(RepositoryNode repository)
        {
            if (repository.Type != EntryType.Protocol)
            {
                return;
            }

            _templateParser = new TemplateParser();

            var compilerParams = new CompilerParameters
            {
                GenerateInMemory = true,
                TreatWarningsAsErrors = false,
                GenerateExecutable = false,
                CompilerOptions = "/optimize"
            };

            compilerParams.ReferencedAssemblies.AddRange(new[] { "System.dll", "mscorlib.dll", "System.Core.dll", Assembly.GetEntryAssembly()?.Location, typeof(ProtoContractAttribute).Assembly.Location });

            var provider = new CSharpCodeProvider();
            var codes = repository.GetFiles().Select(template => "using _010Proxy.Templates.Attributes; using _010Proxy.Templates; " + template).ToArray();

            var compile = provider.CompileAssemblyFromSource(compilerParams, codes);

            if (compile.Errors.HasErrors)
            {
                // TODO: conveniently display on form
                var text = compile.Errors.Cast<CompilerError>().Aggregate("Compile error: ", (current, ce) => current + ("rn" + ce.ToString()));

                throw new Exception(text);
            }

            _templateParser.ParseAssembly(compile.CompiledAssembly);

            var rowsCount = packetsTable.Rows.Count;

            for (var i = 0; i < rowsCount; i++)
            {
                ApplyProtocol(i);
            }
        }

        private void ApplyProtocol(int rowIndex)
        {
            if (_templateParser == null)
            {
                return;
            }

            if (packetsTable.Rows.Count < rowIndex)
            {
                return;
            }

            lock (packetsTable.Rows[rowIndex])
            {
                var tcpFlow = (TcpFlow)packetsTable.Rows[rowIndex].Tag;

                if (tcpFlow.FlowData.Count > 0)
                //foreach (var type in _rootTemplates)
                {
                    try
                    {
                        tcpFlow.Data = _templateParser.Parse(tcpFlow.FlowData);
                        packetsTable.Rows[rowIndex].Cells[6].Value = "";
                        //break;
                    }
                    catch (Exception e)
                    {
                        packetsTable.Rows[rowIndex].Cells[6].Value = e.Message;
                    }
                }
            }
        }

        private void packetsTable_SelectionChanged(object sender, System.EventArgs e)
        {
            var flowIndex = (int)packetsTable.Rows[packetsTable.CurrentCell.RowIndex].Cells[1].Value;
            var tcpFlow = _connectionInfo.Flows[flowIndex - 1];

            packetPreview.ByteProvider = new DynamicByteProvider(tcpFlow.FlowData);

            if (_templateParser != null && tcpFlow.FlowData.Count > 0)
            {
                var data = _templateParser.Parse(tcpFlow.FlowData);
                ParentForm.CreateFlowDataPreview(data);
            }
        }

        private void packetsTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var templateForm = new PacketInfoForm();
            templateForm.Show();
        }

        public override void OnClose()
        {
            if (_connectionInfo != null)
            {
                _connectionInfo.OnNewFlow -= OnNewFlow;
                _connectionInfo.OnFlowUpdate -= OnFlowUpdate;
            }
        }

        public override void OnShow()
        {
            UpdateMenu(true);
        }

        public override void OnHide()
        {
            UpdateMenu(false);
        }

        private void packetsTable_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation == ScrollOrientation.VerticalScroll)
            {
                _stickToBottom = packetsTable.Rows[packetsTable.Rows.Count - 1].Displayed;
            }
        }
    }
}
