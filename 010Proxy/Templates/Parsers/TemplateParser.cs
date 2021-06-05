using _010Proxy.Templates.Attributes;
using _010Proxy.Types;
using _010Proxy.Utils.Extensions;
using Microsoft.CodeDom.Providers.DotNetCompilerPlatform;
using ProtoBuf;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

    namespace _010Proxy.Templates.Parsers
    {
    public sealed class TemplateParser
    {
        private Assembly _assembly;

        public List<Type> RootEvents { get; } = new List<Type>();

        public Dictionary<object, Type> EventsMap { get; } = new Dictionary<object, Type>();

        public List<Type> OpCodesTypes { get; } = new List<Type>();

        public IProtocolConfig ProtocolConfig { get; private set; } = null;

        private static readonly IEnumerable<string> DefaultNamespaces = new[]
        {
            "_010Proxy.Templates",
            "_010Proxy.Templates.Attributes",
            "_010Proxy.Types",
            "BinarySerialization",
            "System",
            "System.IO",
            "System.Net",
            "System.Linq",
            "System.Text",
            "System.Text.RegularExpressions",
            "System.Collections.Generic"
        };

        public void LoadProtocol(RepositoryNode repository)
        {
            if (repository.Type == EntryType.Protocol)
            {
                CompileTemplates(repository);
                ParseAssembly();
            }
        }

        public void CompileTemplates(RepositoryNode repository)
        {
            var compilerParams = new CompilerParameters
            {
                GenerateInMemory = true,
                TreatWarningsAsErrors = false,
                GenerateExecutable = false,
                CompilerOptions = "/optimize"
            };

            compilerParams.ReferencedAssemblies.AddRange(new[]
            {
                "System.dll", "mscorlib.dll", "System.Core.dll",
                Assembly.GetEntryAssembly()?.Location,
                typeof(ProtoContractAttribute).Assembly.Location,
                typeof(BinarySerialization.BinarySerializer).Assembly.Location
            });

            var provider = new CSharpCodeProvider();
            var usingList = string.Join("", DefaultNamespaces.Select(u => $"using {u}; "));
            var codes = repository.GetFiles().Select(template => usingList + template).ToArray();

            var compile = provider.CompileAssemblyFromSource(compilerParams, codes);

            if (compile.Errors.HasErrors)
            {
                // TODO: conveniently display on form
                // TODO: map index of temp files to file names with path from `codes`
                var text = compile.Errors.Cast<CompilerError>().Aggregate("Compile error: ", (current, ce) => current + ("\r\n" + ce));

                throw new Exception(text);
            }

            _assembly = compile.CompiledAssembly;
        }

        private bool ParseAssembly()
        {
            foreach (var module in _assembly.GetModules())
            {
                foreach (var type in module.GetTypes())
                {
                    if (typeof(IProtocolConfig).IsAssignableFrom(type))
                    {
                        ProtocolConfig = (IProtocolConfig) Activator.CreateInstance(type);
                    }

                    if (type.HasAttribute<RootAttribute>(out var rootAttribute))
                    {
                        RootEvents.Add(type);
                    }

                    if (type.HasAttribute<OpCodeListAttribute>(out var opCodeListAttribute))
                    {
                        OpCodesTypes.Add(type);
                    }

                    var eventAttributes = type.GetCustomAttributes(typeof(EventAttribute), false);

                    foreach (EventAttribute attribute in eventAttributes)
                    {
                        EventsMap.Add(attribute.OpCode, type);
                    }
                }
            }

            RootEvents.Sort((x, y) =>
            {
                var xPriority = x.GetCustomAttribute<RootAttribute>().Priority;
                var yPriority = y.GetCustomAttribute<RootAttribute>().Priority;

                return xPriority - yPriority;
            });

            return RootEvents.Count != 0;
        }
    }
}
