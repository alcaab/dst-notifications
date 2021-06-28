//
// ITextTemplatingEngineHost.cs
//
// Author:
//       Mikayla Hutchinson <m.j.hutchinson@gmail.com>
//
// Copyright (c) 2009-2010 Novell, Inc. (http://www.novell.com)
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
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

[assembly: CLSCompliant(true)]
namespace Desyco.T5Templating.TextTemplating.Microsoft.VisualStudio.TextTemplating
{
    public interface IRecognizeHostSpecific
    {
        bool RequiresProcessingRunIsHostSpecific { get; }
        void SetProcessingRunIsHostSpecific(bool hostSpecific);
    }

    //[CLSCompliant(true)]
    //[Obsolete ("Use " + nameof(T5) + "."
    //           + nameof(T5.TextTemplating) + "."
    //           + nameof(T5.TextTemplating.TemplatingEngine) + " directly")]
    //public interface ITextTemplatingEngine
    //{
    //    string ProcessTemplate (string content, ITextTemplatingEngineHost host);
    //    string PreprocessTemplate (string content, ITextTemplatingEngineHost host, string className,
    //        string classNamespace, out string language, out string[] references);
    //}

    [CLSCompliant(true)]
    public interface ITextTemplatingEngineHost
    {
        IList<string> StandardAssemblyReferences { get; }
        IList<string> StandardImports { get; }
        string TemplateFile { get; }
        object GetHostOption(string optionName);
        bool LoadIncludeText(string requestFileName, out string content, out string location);
        void LogErrors(CompilerErrorCollection errors);
        AppDomain ProvideTemplatingAppDomain(string content);
        string ResolveAssemblyReference(string assemblyReference);
        Type ResolveDirectiveProcessor(string processorName);
        string ResolveParameterValue(string directiveId, string processorName, string parameterName);
        string ResolvePath(string path);
        void SetFileExtension(string extension);
        void SetOutputEncoding(Encoding encoding, bool fromOutputDirective);
    }

    [CLSCompliant(true)]
    public interface ITextTemplatingSession :
        IEquatable<ITextTemplatingSession>, IEquatable<Guid>, IDictionary<string, object>,
        ICollection<KeyValuePair<string, object>>,
        IEnumerable<KeyValuePair<string, object>>,
        IEnumerable, ISerializable
    {
        Guid Id { get; }
    }

    [CLSCompliant(true)]
    public interface ITextTemplatingSessionHost
    {
        ITextTemplatingSession Session { get; set; }
        ITextTemplatingSession CreateSession();
    }

    public interface IDirectiveProcessor
    {
        CompilerErrorCollection Errors { get; }
        bool RequiresProcessingRunIsHostSpecific { get; }

        void FinishProcessingRun();
        string GetClassCodeForProcessingRun();
        string[] GetImportsForProcessingRun();
        string GetPostInitializationCodeForProcessingRun();
        string GetPreInitializationCodeForProcessingRun();
        string[] GetReferencesForProcessingRun();
        CodeAttributeDeclarationCollection GetTemplateClassCustomAttributes(); //TODO
        void Initialize(ITextTemplatingEngineHost host);
        bool IsDirectiveSupported(string directiveName);
        //void ProcessDirective(string directiveName, IDictionary<string, string> arguments);
        void ProcessDirective(Directive directive);
        void SetProcessingRunIsHostSpecific(bool hostSpecific);

        void StartProcessingRun(CodeDomProvider languageProvider, string templateContents,
            CompilerErrorCollection errors);
    }

    public interface IDirectiveTemplatingHost 
    {
        List<Directive> GetNonContentDirectives();
        void AddParameter(string name, string type, bool readOnly = true);
        void AddParameter(string name, string type, object value, bool readOnly = true);
    }
}