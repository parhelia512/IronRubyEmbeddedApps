using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using IronRuby;
using IronRuby.Runtime;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting.Hosting.Providers;
using SERFS;

namespace IREmbeddedApp {
    public class EmbeddedRuby {
        private readonly Serfs _serfs;
        private ScriptRuntime _runtime;
        private ScriptEngine _engine;
        private RubyContext _context;

        /// <summary>
        /// Create new instance of Embedded Ruby
        /// </summary>
        public EmbeddedRuby() {
            _serfs = new Serfs(null) {
                IgnoreMissingAssemblies = true
            };
            AddAssembly("IREmbeddedApp");
#if NET5_0_OR_GREATER
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // Bring in extra encodings
#endif
            Reset();
        }

        /// <summary>
        /// Reset instance - creates new runtime and engine
        /// </summary>
        public void Reset() {
            _runtime = Ruby.CreateRuntime();
            _engine = _runtime.GetEngine("rb");
            _context = (RubyContext)HostingHelpers.GetLanguageContext(_engine);
        }

        public IStreamDecoder Decoder {
            set { _serfs.Decoder = value; }
        }

        /// <summary>
        /// Add a folder in the calling assembly to the SERFS search path
        /// </summary>
        /// <param name="topFolder">Path relative to the calling assembly</param>
        /// <returns></returns>
        public AssemblyInfo Mount(string topFolder) {
            AssemblyInfo info = _serfs.AddAssembly(Assembly.GetCallingAssembly().GetName().Name);
            info.Mount(topFolder);
            return info;
        }

        public AssemblyInfo AddAssembly(string name) {
            return _serfs.AddAssembly(name);
        }

        /// <summary>
        /// Add a folder in a specified assembly to the SERFS search path
        /// </summary>
        /// <param name="name">Assembly</param>
        /// <param name="folder">Path relative to the passed assembly</param>
        /// <returns></returns>
        public AssemblyInfo AddAssembly(string name, string folder) {
            AssemblyInfo info = _serfs.AddAssembly(name);
            info.Mount(folder);
            return info;
        }

        /// <summary>
        /// Pass a .NET object into a Ruby constant
        /// </summary>
        /// <param name="name"></param>
        /// <param name="obj"></param>
        public void SetConstant(string name, object obj) {
            _context.ObjectClass.SetConstant(name, obj);
        }

        /// <summary>
        /// Run a Ruby file
        /// </summary>
        /// <param name="app">Name of SERFs-accessible file</param>
        /// <returns>exit status</returns>
        public int Run(string app) {
            return Run(app, null);
        }

        /// <summary>
        /// Run a Ruby file
        /// </summary>
        /// <param name="app">Name of SERFs-accessible file</param>
        /// <param name="args">ARGV equivalent</param>
        /// <returns>exit status</returns>
        public int Run(string app, string[] args) {
            SetConstant("SerfsInstance", _serfs);

            Assembly exe = Assembly.GetEntryAssembly();
            string serfs_assy = exe.FullName;

            try {
                serfs_assy = Assembly.Load("Serfs").FullName;
            }
            catch (System.IO.FileNotFoundException) {
                // Allowed : it means whe have been ILMerged
            }

            // Sort out ARGV
            string argv;
            if ((args != null) && (args.Length > 0)) {
                List<string> quoted_args = new List<string>();
                foreach (string s in args) {
                    quoted_args.Add("'" + s + "'");
                }
                argv = String.Format("ARGV << {0}\r\n", String.Join(" << ", quoted_args.ToArray()));
            } else {
                argv = "";
            }
            // Create boot up script
            // Why add the /? string boot = String.Format(@"$0='/{0}' '
            string boot = String.Format(@"$0 ='{0}'
SerfsDll = '{3}'
{1}{2}
IS_LINUX = !!(PLATFORM =~ /linux/)
IS_WINDOWS = (PLATFORM !~ /linux/)
require 'EmbeddedRuby/LoadSupport'
require 'EmbeddedRuby/AutoloadSupport'
require 'EmbeddedRuby/IOSupport'
require 'EmbeddedRuby/FileSupport'
require 'EmbeddedRuby/Misc'
require 'EmbeddedRuby/AppBoot' if File.exist?('EmbeddedRuby/AppBoot.rb')
load $0.dup if $0
"
                , app, argv, _serfs.Read("EmbeddedRuby/RequireSupport.rb"), serfs_assy);

            var source = _engine.CreateScriptSourceFromString(boot, "RequireSupport.rb", SourceCodeKind.File);
            var ex = source.ExecuteProgram();
            _context.Shutdown();
            return ex;
        }

        /// <summary>
        /// Get the active Ruby engine
        /// </summary>
        /// <returns>The engine instance</returns>
        public ScriptEngine Engine() {
            return _engine;
        }

        /// <summary>
        /// Get the active Runtime
        /// </summary>
        /// <returns>The runtime instance</returns>
        public ScriptRuntime Runtime() {
            return _runtime;
        }

        /// <summary>
        /// Get the active Context
        /// </summary>
        /// <returns>The context instance</returns>
        public RubyContext Context() {
            return _context;
        }

        /// <summary>
        /// Execute a ruby script
        /// </summary>
        /// <param name="script">Ruby code</param>
        /// <returns>Return from ruby</returns>
        public dynamic Execute(string script) {
            return _engine.Execute(script);
        }
    }
}
