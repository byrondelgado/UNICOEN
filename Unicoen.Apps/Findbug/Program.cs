using System;
using System.Linq;
using Unicoen.Languages.Java.ProgramGenerators;
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

        public static IEnumerable<IUnifiedElement> FindDefines(UnifiedProgram codeObj) {
            var binaryExpressions = codeObj.Descendants<UnifiedBinaryExpression>();

            foreach (var be in binaryExpressions) {
                if (be.Operator.Kind == UnifiedBinaryOperatorKind.Assign) {
                    var left = be.LeftHandSide as UnifiedVariableIdentifier;
                    if (left != null) {
                        Console.WriteLine("Identifier :\n{0}", left);
                    }
                    yield return left;
                }
            }
        }

        public static IEnumerable<IUnifiedElement> FindUses(UnifiedProgram codeObj) {
            var binaryExpressions = codeObj.Descendants<UnifiedBinaryExpression>();
            foreach (var be in binaryExpressions) {
                if (be.Operator.Kind == UnifiedBinaryOperatorKind.Assign) {
                    var right = be.RightHandSide as UnifiedVariableIdentifier;
                    if (right != null) {
                        var rightName = right.Name;
                        Console.WriteLine("{0} is used", rightName);
                        Console.WriteLine(right);
                        yield return right;
                    }
                }
            }
        }

        public static IEnumerable<string> FindNullDefines(UnifiedProgram codeObj) {
            var binaryExpressions = codeObj.Descendants<UnifiedBinaryExpression>();
            var definition = codeObj.Descendants<UnifiedVariableDefinition>();
            var nameList = new LinkedList<string>();
            foreach (var def in definition) {
                var nullDefinition = def.InitialValue as UnifiedNullLiteral;
                if (nullDefinition != null) {
                    Console.WriteLine("{0} defines null", def.Name.Name);
                    Console.WriteLine(nullDefinition);
                    nameList.AddLast(def.Name.Name);
                }
            }

            foreach (var be in binaryExpressions) {
                if (be.Operator.Kind == UnifiedBinaryOperatorKind.Assign) {
                    var right = be.RightHandSide as UnifiedNullLiteral;
                    var nullId = be.LeftHandSide as UnifiedVariableIdentifier;
                    if (right != null && nullId != null) {
                        Console.WriteLine("{0} will be null", nullId.Name);
                        Console.WriteLine(nullId);
                        nameList.AddLast(nullId.Name);
                    }
                }
            }
            return nameList;
        }

        public static void FindUsesDefine(UnifiedProgram codeObj) {
            var defineNames = FindUses(codeObj);
            var name = defineNames.First();
            var elements = codeObj.DescendantsUntil(e => {
                bool b = name == e;
                Console.WriteLine(b);
                return b;
            });
            /*foreach (var defName in defineNames) {
                var name = defName;
               */ 
                foreach (var element in elements) {
                    Console.WriteLine(element);
                }
            return;
            /*var variableName = (UnifiedVariableIdentifier)defName;
                foreach (var element in (IEnumerable<UnifiedBinaryExpression>)elements) {
                    var left = element.LeftHandSide as UnifiedVariableIdentifier;
                    var right = element.RightHandSide as UnifiedNullLiteral;
                    if (left != null && left.Name.Equals(variableName.Name)) {
                        Console.WriteLine("{0} is {1}", left.Name, element.RightHandSide);
                    }
                }*/
            //}
        }
        
        static void Main(string[] args) {
            try {
                var inputPath = FixtureUtil.GetInputPath("Java", "BugPatterns", "NP_NULL_ON_SOME_PATH.java");
                var codeObj = new JavaProgramGenerator().GenerateFromFile(inputPath);
                var a = FindDefines(codeObj);
                a.Count();
                var b = FindUses(codeObj);
                b.Count();
                var c = FindNullDefines(codeObj);
                foreach (var ccc in c) {
                    Console.WriteLine(ccc);
                }

                var e = DefUseAnalyzer.CreateGraph(codeObj);
                foreach (var eee in e) {
                    Console.WriteLine(eee);
                }

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
