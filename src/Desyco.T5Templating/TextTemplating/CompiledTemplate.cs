//
// CompiledTemplate.cs
//
// Author:
//       Nathan Baulch <nathan.baulch@gmail.com>
//
// Copyright (c) 2009 Nathan Baulch
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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using Desyco.T5Templating.TextTemplating.Microsoft.VisualStudio.TextTemplating;

namespace Desyco.T5Templating.TextTemplating
{
    public sealed class CompiledTemplate : MarshalByRefObject, IDisposable
    {
        private readonly string[] assemblyFiles;
        private readonly CultureInfo culture;
        private ITextTemplatingEngineHost host;

        public CompiledTemplate(ITextTemplatingEngineHost host, CompilerResults results, string fullName,
            CultureInfo culture,
            string[] assemblyFiles)
        {
            AppDomain.CurrentDomain.AssemblyResolve += ResolveReferencedAssemblies;
            this.host = host;
            this.culture = culture;
            this.assemblyFiles = assemblyFiles;
            Load(results, fullName);
        }

        /*
         PARA PODER TENER EL TIPO DESDE FUERA
        */
        public object TextTransformation { get; private set; }

        public void Dispose()
        {
            if (host != null)
            {
                host = null;
                AppDomain.CurrentDomain.AssemblyResolve -= ResolveReferencedAssemblies;
            }
        }

        private void Load(CompilerResults results, string fullName)
        {
            var assembly = results.CompiledAssembly;
            //Type transformType = assembly.GetType (fullName);
            var name = fullName.Split('.').Last();
            var transformType = assembly.GetExportedTypes().Single(t => t.Name == name);
            //MS Templating Engine does not look on the type itself,
            //it checks only that required methods are exists in the compiled type
            TextTransformation = Activator.CreateInstance(transformType);

            //set the host property if it exists
            Type hostType = null;
            var gen = host as TemplateGenerator;
            if (gen != null) hostType = gen.SpecificHostType;
            var hostProp = transformType.GetProperty("Host", hostType ?? typeof(ITextTemplatingEngineHost));
            if (hostProp != null && hostProp.CanWrite)
                hostProp.SetValue(TextTransformation, host, null);

            var sessionHost = host as ITextTemplatingSessionHost;
            if (sessionHost != null)
            {
                //FIXME: should we create a session if it's null?
                var sessionProp = transformType.GetProperty("Session", typeof(IDictionary<string, object>));
                sessionProp.SetValue(TextTransformation, sessionHost.Session, null);
            }
        }

        public string Process()
        {
            var ttType = TextTransformation.GetType();

            var errorProp = ttType.GetProperty("Errors", BindingFlags.Instance | BindingFlags.NonPublic);
            if (errorProp == null)
                throw new ArgumentException("Template must have 'Errors' property");
            var errorMethod = ttType.GetMethod("Error", new[] {typeof(string)});
            if (errorMethod == null) throw new ArgumentException("Template must have 'Error(string message)' method");

            var errors = (CompilerErrorCollection) errorProp.GetValue(TextTransformation);
            errors.Clear();

            //set the culture
            if (culture != null)
                ToStringHelper.FormatProvider = culture;
            else
                ToStringHelper.FormatProvider = CultureInfo.InvariantCulture;

            string output = null;

            var initMethod = ttType.GetMethod("Initialize");
            var transformMethod = ttType.GetMethod("TransformText");

            if (initMethod == null)
                errorMethod.Invoke(TextTransformation,
                    new object[] {"Error running transform: no method Initialize()"});
            else if (transformMethod == null)
                errorMethod.Invoke(TextTransformation,
                    new object[] {"Error running transform: no method TransformText()"});
            else
                try
                {
                    initMethod.Invoke(TextTransformation, null);
                    output = (string) transformMethod.Invoke(TextTransformation, null);
                }
                catch (Exception ex)
                {
                    errorMethod.Invoke(TextTransformation, new object[] {"Error running transform: " + ex});
                }

            host.LogErrors(errors);

            ToStringHelper.FormatProvider = CultureInfo.InvariantCulture;
            return output;
        }


        private Assembly ResolveReferencedAssemblies(object sender, ResolveEventArgs args)
        {
            var asmName = new AssemblyName(args.Name);
            foreach (var asmFile in assemblyFiles)
                if (asmName.Name == Path.GetFileNameWithoutExtension(asmFile))
                    return Assembly.LoadFrom(asmFile);

            var path = host.ResolveAssemblyReference(asmName.Name + ".dll");
            if (File.Exists(path))
                return Assembly.LoadFrom(path);

            return null;
        }

        //public ITextTemplatingEngineHost Host => host;
    }
}