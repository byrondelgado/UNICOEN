using System;
using System.Linq;
using Code2Xml.Languages.Java.CodeToXmls;
using Unicoen.Languages.Java.ProgramGenerators;
using Unicoen.Languages.CSharp.ProgramGenerators;
using Unicoen.Languages.Python2.ProgramGenerators;
using Unicoen.Model;
using Unicoen.Tests;
using System.Collections.Generic;

namespace Unicoen.Apps.Findbug {
    class Program {
        public static IEnumerable<UnifiedIdentifier> GetVariables(string name, UnifiedProgram codeObj) {
            var ids = codeObj.Descendants<UnifiedIdentifier>();
            foreach(var id in ids) {
                if (id.Name.Equals(UnifiedIdentifier.CreateVariable(name).Name)) {
                    yield return id;
                }
            }
        }
        
        static void Main(string[] args) {
            try {
                var inputPath = FixtureUtil.GetInputPath("Java", "BugPatterns", "GraphSample.java");
                var javaCodeObj = new JavaProgramGenerator().GenerateFromFile(inputPath);
                var bodyJ = javaCodeObj.Body;

                var inputPathP = FixtureUtil.GetInputPath("Python2", "BugPatterns", "NP_SOMEPATH.py");
                var pythonCodeObj = new Python2ProgramGenerator().GenerateFromFile(inputPathP);
                var bodyP = pythonCodeObj.Body;

                var inputPathCs = FixtureUtil.GetInputPath("CSharp", "BugPatterns", "NP_SOMEPATH.cs");
                var csharpCodeObj = new CSharpProgramGenerator().GenerateFromFile(inputPathCs);
                var bodyCs = csharpCodeObj.Body;

                var javaBlocks = javaCodeObj.Descendants<UnifiedBlock>();
                var pythonBlocks = pythonCodeObj.Descendants<UnifiedBlock>();
                var csharpBlocks = csharpCodeObj.Descendants<UnifiedBlock>();
                CreateGraph.Graph(javaBlocks);
                //CreateGraph.Graph(pythonBlocks);
                //CreateGraph.Graph(csharpBlocks);

                //多言語対応可能かの実験
                /*var a = DefUseAnalyzer.FindDefines(bodyJ);
                foreach (var aaa in a) {
                    Console.WriteLine("def: \n{0}", aaa);
                }
                var b = DefUseAnalyzer.FindUses(bodyJ);
                foreach (var bbb in b) {
                    Console.WriteLine("use: \n{0}", bbb);
                }
                var c = DefUseAnalyzer.FindNullDefines(bodyJ);
                Console.WriteLine("END.");

                var ap = DefUseAnalyzer.FindDefines(bodyP);
                foreach (var app in ap) {
                    Console.WriteLine("def: \n{0}", app);
                }
                var bp = DefUseAnalyzer.FindUses(bodyP);
                foreach (var bpp in bp) {
                    Console.WriteLine("use: \n{0}", bpp);
                }
                var cp = DefUseAnalyzer.FindNullDefines(bodyP);
                Console.WriteLine("END.");

                var acs = DefUseAnalyzer.FindDefines(bodyCs);
                foreach (var acss in acs) {
                    Console.WriteLine("def: \n{0}", acss);
                }
                var bcs = DefUseAnalyzer.FindUses(bodyCs);
                foreach (var bcss in bcs) {
                    Console.WriteLine("use: \n{0}", bcss);
                }
                var ccs = DefUseAnalyzer.FindNullDefines(bodyCs);
                Console.WriteLine("END.");
                */
                /*var e = DefUseAnalyzer.CreateGraph(codeObject);
                foreach (var eee in e) {
                    Console.WriteLine(eee);
                }*/

                //FindUsesDefine(codeObj);

                /*Console.WriteLine("{0}: " + idSet.Count(), idName);
                var nulls = codeObj.Descendants<UnifiedNullLiteral>();
                Console.WriteLine("null: " + nulls.Count());*/
            }
            catch (Exception e) {
                Console.WriteLine(e);
            }
        }
    }
}
