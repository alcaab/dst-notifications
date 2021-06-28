//
// TemplatingHost.cs
//
// Author:
//       Mikayla Hutchinson <m.j.hutchinson@gmail.com>
//
// Copyright (c) 2009 Novell, Inc. (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Desyco.T5Templating.TextTemplating.Microsoft.VisualStudio.TextTemplating;

namespace Desyco.T5Templating.TextTemplating
{
    public class TemplateGenerator : MarshalByRefObject, ITextTemplatingEngineHost, ITextTemplatingSessionHost, IDirectiveTemplatingHost
    {
        private static readonly Encoding BomlessUtf8Encoding = new UTF8Encoding(false);

        private readonly Dictionary<string, KeyValuePair<string, string>> directiveProcessors =
            new Dictionary<string, KeyValuePair<string, string>>();

        //host fields

        private readonly Dictionary<ParameterKey, string> parameters = new Dictionary<ParameterKey, string>();

        private Encoding encoding;

        //re-usable
        private TemplatingEngine engine;

        //per-run variables
        private string inputFile;

        private readonly List<Directive> directiveList = new List<Directive>();

        public TemplateGenerator()
        {
            Refs.Add(typeof(TextTransformation).Assembly.Location);
            Refs.Add(typeof(Uri).Assembly.Location);
            Imports.Add("System");
        }

        //host properties for consumers to access
        public CompilerErrorCollection Errors { get; } = new CompilerErrorCollection();

        public List<string> Refs { get; } = new List<string>();

        public List<string> Imports { get; } = new List<string>();

        public List<string> IncludePaths { get; } = new List<string>();

        public List<string> ReferencePaths { get; } = new List<string>();

        public string OutputFile { get; private set; }

        public bool UseRelativeLinePragmas { get; set; }

        protected TemplatingEngine Engine
        {
            get
            {
                if (engine == null)
                    engine = new TemplatingEngine();
                return engine;
            }
        }

        /// <summary>
        ///     If non-null, the template's Host property will be the full type of this host.
        /// </summary>
        public virtual Type SpecificHostType => null;

        public CompiledTemplate CompileTemplate(string content)
        {
            if (string.IsNullOrEmpty(content))
                throw new ArgumentNullException("content");

            Errors.Clear();
            encoding = Encoding.UTF8;

            return Engine.CompileTemplate(content, this);
        }

        public bool ProcessTemplate(string inputFile, string outputFile)
        {
            if (string.IsNullOrEmpty(inputFile))
                throw new ArgumentNullException("inputFile");
            if (string.IsNullOrEmpty(outputFile))
                throw new ArgumentNullException("outputFile");

            string content;
            try
            {
                content = File.ReadAllText(inputFile);
            }
            catch (IOException ex)
            {
                Errors.Clear();
                AddError("Could not read input file '" + inputFile + "':\n" + ex);
                return false;
            }

            string output;
            ProcessTemplate(inputFile, content, ref outputFile, out output);

            try
            {
                if (!Errors.HasErrors)
                    WriteAllTextToFile(outputFile, output, encoding);
            }
            catch (IOException ex)
            {
                AddError("Could not write output file '" + outputFile + "':\n" + ex);
            }

            return !Errors.HasErrors;
        }

        public bool ProcessTemplate(string inputFileName, string inputContent, ref string outputFileName,
            out string outputContent)
        {
            Errors.Clear();
            encoding = Encoding.UTF8;

            OutputFile = outputFileName;
            inputFile = inputFileName;
            outputContent = Engine.ProcessTemplate(inputContent, this);
            outputFileName = OutputFile;

            return !Errors.HasErrors;
        }

        public bool PreprocessTemplate(string inputFile, string className, string classNamespace,
            string outputFile, Encoding encoding, out string language, out string[] references)
        {
            language = null;
            references = null;

            if (string.IsNullOrEmpty(inputFile))
                throw new ArgumentNullException("inputFile");
            if (string.IsNullOrEmpty(outputFile))
                throw new ArgumentNullException("outputFile");

            string content;
            try
            {
                content = File.ReadAllText(inputFile);
            }
            catch (IOException ex)
            {
                Errors.Clear();
                AddError("Could not read input file '" + inputFile + "':\n" + ex);
                return false;
            }

            string output;
            PreprocessTemplate(inputFile, className, classNamespace, content, out language, out references, out output);

            try
            {
                if (!Errors.HasErrors)
                    WriteAllTextToFile(outputFile, output, encoding);
            }
            catch (IOException ex)
            {
                AddError("Could not write output file '" + outputFile + "':\n" + ex);
            }

            return !Errors.HasErrors;
        }

        private static void WriteAllTextToFile(string path, string contents, Encoding encoding)
        {
            File.WriteAllText(path, contents,
                (encoding is UTF8Encoding || "utf-8".Equals(encoding.WebName, StringComparison.OrdinalIgnoreCase))
                && encoding != BomlessUtf8Encoding
                    ? BomlessUtf8Encoding
                    : encoding);
        }

        public bool PreprocessTemplate(string inputFileName, string className, string classNamespace,
            string inputContent,
            out string language, out string[] references, out string outputContent)
        {
            Errors.Clear();
            encoding = Encoding.UTF8;

            inputFile = inputFileName;
            outputContent = Engine.PreprocessTemplate(inputContent, this, className, classNamespace, out language,
                out references);

            return !Errors.HasErrors;
        }

        private CompilerError AddError(string error)
        {
            var err = new CompilerError();
            err.ErrorText = error;
            Errors.Add(err);
            return err;
        }

        public void AddDirectiveProcessor(string name, string klass, string assembly)
        {
            directiveProcessors.Add(name, new KeyValuePair<string, string>(klass, assembly));
        }

        public void AddParameter(string processorName, string directiveName, string parameterName, string value)
        {
            parameters.Add(new ParameterKey(processorName, directiveName, parameterName), value);
        }

        /// <summary>
        ///     Parses a parameter and adds it.
        /// </summary>
        /// <returns>Whether the parameter was parsed successfully.</returns>
        /// <param name="unparsedParameter">Parameter in name=value or processor!directive!name!value format.</param>
        public bool TryAddParameter(string unparsedParameter)
        {
            string processor, directive, name, value;
            if (TryParseParameter(unparsedParameter, out processor, out directive, out name, out value))
            {
                AddParameter(processor, directive, name, value);
                CallContext.LogicalSetData(name, value);
                return true;
            }

            return false;
        }

        internal static bool TryParseParameter(string parameter, out string processor, out string directive,
            out string name, out string value)
        {
            processor = directive = name = value = "";

            var start = 0;
            var end = parameter.IndexOfAny(new[] { '=', '!' });
            if (end < 0)
                return false;

            //simple format n=v
            if (parameter[end] == '=')
            {
                name = parameter.Substring(start, end);
                value = parameter.Substring(end + 1);
                return !string.IsNullOrEmpty(name);
            }

            //official format, p!d!n!v
            processor = parameter.Substring(start, end);

            start = end + 1;
            end = parameter.IndexOf('!', start);
            if (end < 0)
            {
                //unlike official version, we allow you to omit processor/directive
                name = processor;
                value = parameter.Substring(start);
                processor = "";
                return !string.IsNullOrEmpty(name);
            }

            directive = parameter.Substring(start, end - start);


            start = end + 1;
            end = parameter.IndexOf('!', start);
            if (end < 0)
            {
                //we also allow you just omit the processor
                name = directive;
                directive = processor;
                value = parameter.Substring(start);
                processor = "";
                return !string.IsNullOrEmpty(name);
            }

            name = parameter.Substring(start, end - start);
            value = parameter.Substring(end + 1);

            return !string.IsNullOrEmpty(name);
        }

        internal static string ExpandParameters(string value, Dictionary<ParameterKey, string> parameters)
        {
            const char TokenStart = '$';
            const char TokenOpen = '(';
            const char TokenEnd = ')';

            var sb = new StringBuilder();
            for (var i = 0; i < value.Length; ++i)
                if (i < value.Length - 1
                    && value[i] == TokenStart
                    && value[i + 1] == TokenOpen)
                {
                    var endTokenIndex = i;
                    while (endTokenIndex < value.Length
                           && value[endTokenIndex] != TokenEnd)
                        ++endTokenIndex;

                    if (endTokenIndex >= value.Length
                        || value[endTokenIndex] != TokenEnd)
                    {
                        // We reached the end of the string
                        // Probably not a token, or not closed token
                        sb.Append(value.Substring(i));
                        break;
                    }

                    var parameterName = value.Substring(i + 2, endTokenIndex - i - 2);
                    var key = new ParameterKey(string.Empty, string.Empty, parameterName);
                    if (parameters.TryGetValue(key, out var parameterValue))
                        sb.Append(parameterValue);
                    else
                        sb.Append(value.Substring(i, endTokenIndex - i + 1));
                    i = endTokenIndex;
                }
                else
                {
                    sb.Append(value[i]);
                }

            return sb.ToString();
        }

        protected virtual bool LoadIncludeText(string requestFileName, out string content, out string location)
        {
            content = "";
            location = ResolvePath(requestFileName);

            if (location == null || !File.Exists(location))
                foreach (var path in IncludePaths)
                {
                    var f = Path.Combine(path, requestFileName);
                    if (File.Exists(f))
                    {
                        location = f;
                        break;
                    }
                }

            if (location == null)
                return false;

            try
            {
                content = File.ReadAllText(location);
                return true;
            }
            catch (IOException ex)
            {
                AddError("Could not read included file '" + location + "':\n" + ex);
            }

            return false;
        }

        /// <summary>
        ///     Gets any additional directive processors to be included in the processing run.
        /// </summary>
        public virtual IEnumerable<IDirectiveProcessor> GetAdditionalDirectiveProcessors()
        {
            yield break;
        }

        internal struct ParameterKey : IEquatable<ParameterKey>
        {
            public ParameterKey(string processorName, string directiveName, string parameterName)
            {
                this.processorName = processorName ?? "";
                this.directiveName = directiveName ?? "";
                this.parameterName = parameterName ?? "";
                unchecked
                {
                    hashCode = this.processorName.GetHashCode()
                               ^ this.directiveName.GetHashCode()
                               ^ this.parameterName.GetHashCode();
                }
            }

            private readonly string processorName;
            private readonly string directiveName;
            private readonly string parameterName;
            private readonly int hashCode;

            public override bool Equals(object obj)
            {
                return obj is ParameterKey && Equals((ParameterKey)obj);
            }

            public bool Equals(ParameterKey other)
            {
                return processorName == other.processorName && directiveName == other.directiveName &&
                       parameterName == other.parameterName;
            }

            public override int GetHashCode()
            {
                return hashCode;
            }
        }

        #region Virtual members

        public virtual object GetHostOption(string optionName)
        {
            switch (optionName)
            {
                case "UseRelativeLinePragmas":
                    return UseRelativeLinePragmas;
            }

            return null;
        }

        public virtual AppDomain ProvideTemplatingAppDomain(string content)
        {
            return null;
        }

        protected virtual string ResolveAssemblyReference(string assemblyReference)
        {
            if (Path.IsPathRooted(assemblyReference))
                return assemblyReference;
            foreach (var referencePath in ReferencePaths)
            {
                var path = Path.Combine(referencePath, assemblyReference);
                if (File.Exists(path))
                    return path;
            }

            var assemblyName = new AssemblyName(assemblyReference);
            if (assemblyName.Version != null) //Load via GAC and return full path
                return Assembly.Load(assemblyName).Location;

            if (!assemblyReference.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) &&
                !assemblyReference.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                return assemblyReference + ".dll";
            return assemblyReference;
        }

        protected virtual string ResolveParameterValue(string directiveId, string processorName, string parameterName)
        {
            var key = new ParameterKey(processorName, directiveId, parameterName);
            string value;
            if (parameters.TryGetValue(key, out value))
                return value;
            if (processorName != null || directiveId != null)
                return ResolveParameterValue(null, null, parameterName);
            return null;
        }

        protected virtual Type ResolveDirectiveProcessor(string processorName)
        {
            KeyValuePair<string, string> value;
            if (!directiveProcessors.TryGetValue(processorName, out value))
                throw new Exception(string.Format("No directive processor registered as '{0}'", processorName));
            var asmPath = ResolveAssemblyReference(value.Value);
            if (asmPath == null)
                throw new Exception(string.Format("Could not resolve assembly '{0}' for directive processor '{1}'",
                    value.Value, processorName));
            var asm = Assembly.LoadFrom(asmPath);
            return asm.GetType(value.Key, true);
        }

        protected virtual string ResolvePath(string path)
        {
            path = ExpandParameters(path, parameters);
            path = Environment.ExpandEnvironmentVariables(path);
            if (Path.IsPathRooted(path))
                return path;
            var dir = Path.GetDirectoryName(inputFile);
            var test = Path.Combine(dir, path);
            if (File.Exists(test) || Directory.Exists(test))
                return test;
            return path;
        }

        #endregion

        #region Explicit ITextTemplatingEngineHost implementation

        bool ITextTemplatingEngineHost.LoadIncludeText(string requestFileName, out string content, out string location)
        {
            return LoadIncludeText(requestFileName, out content, out location);
        }

        void ITextTemplatingEngineHost.LogErrors(CompilerErrorCollection errors)
        {
            Errors.AddRange(errors);
        }

        string ITextTemplatingEngineHost.ResolveAssemblyReference(string assemblyReference)
        {
            return ResolveAssemblyReference(assemblyReference);
        }

        string ITextTemplatingEngineHost.ResolveParameterValue(string directiveId, string processorName,
            string parameterName)
        {
            return ResolveParameterValue(directiveId, processorName, parameterName);
        }

        Type ITextTemplatingEngineHost.ResolveDirectiveProcessor(string processorName)
        {
            return ResolveDirectiveProcessor(processorName);
        }

        string ITextTemplatingEngineHost.ResolvePath(string path)
        {
            return ResolvePath(path);
        }

        void ITextTemplatingEngineHost.SetFileExtension(string extension)
        {
            extension = extension.TrimStart('.');
            if (Path.HasExtension(OutputFile))
                OutputFile = Path.ChangeExtension(OutputFile, extension);
            else
                OutputFile = OutputFile + "." + extension;
        }

        void ITextTemplatingEngineHost.SetOutputEncoding(Encoding encoding, bool fromOutputDirective)
        {
            this.encoding = encoding;
        }

        IList<string> ITextTemplatingEngineHost.StandardAssemblyReferences => Refs;

        IList<string> ITextTemplatingEngineHost.StandardImports => Imports;

        string ITextTemplatingEngineHost.TemplateFile => inputFile;

        #endregion

        // ITextTemplatingSession
        public ITextTemplatingSession Session { get; set; } = new TextTemplatingSession();
        public ITextTemplatingSession CreateSession() => new TextTemplatingSession();

        //IDirectiveTemplatingHost
        public List<Directive> GetNonContentDirectives() => directiveList;

        public void AddParameter(string name, string type, bool readOnly = true)
        {
            var par = new Directive("parameter", new Location())
            {
                Attributes = { { "name", name }, { "type", type } },
                State = readOnly ? DirectiveState.ReadOnly : DirectiveState.None
            };

            directiveList.Add(par);
        }
        public void AddParameter(string name, string type, object value, bool readOnly = true)
        {
            Session.Add(name, value);
            AddParameter(name,type,readOnly);
        }
    }
}