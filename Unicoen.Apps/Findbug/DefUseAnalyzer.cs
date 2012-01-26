using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Unicoen.Model;

namespace Unicoen.Apps.Findbug {
    //ToDo:すっきりまとめる
    public class DefUseAnalyzer {
        public static IEnumerable<IUnifiedElement> FindDefines(
                UnifiedBlock codeObj) {
            var binaryExpressions =
                    codeObj.Descendants<UnifiedBinaryExpression>();
            foreach (var be in binaryExpressions) {
                if (be.Operator.Kind == UnifiedBinaryOperatorKind.Assign) {
                    var left = be.LeftHandSide as UnifiedVariableIdentifier;
                    if (left != null) {
                        //Console.WriteLine("Identifier: \n{0}", left);
                        yield return left;
                    }
                }
            }
        }

        public static IEnumerable<IUnifiedElement> FindUses(
                UnifiedBlock codeObj) {
            var binaryExpressions =
                    codeObj.Descendants<UnifiedBinaryExpression>();
            foreach (var be in binaryExpressions) {
                if (be.Operator.Kind == UnifiedBinaryOperatorKind.Assign) {
                    var right = be.RightHandSide as UnifiedVariableIdentifier;
                    if (right != null) {
                        yield return right;
                    }
                    var right2 = be.RightHandSide as UnifiedLiteral;
                    if (right2 != null) {
                        yield return right2;
                    }
                }
            }
        }

        //new
        public static IEnumerable<string> FindNullDefines(UnifiedBlock codeObj) {
            var binaryExpressions =
                    codeObj.Descendants<UnifiedBinaryExpression>();
            var definition = codeObj.Descendants<UnifiedVariableDefinition>();
            var nameList = new LinkedList<string>();
            foreach (var def in definition) {
                var nullDefinition = def.InitialValue as UnifiedNullLiteral;
                if (nullDefinition != null) {
                    Console.WriteLine("{0} defines null", def.Name.Name);
                    nameList.AddFirst(def.Name.Name);
                }
            }

            foreach (var be in binaryExpressions) {
                if (be.Operator.Kind == UnifiedBinaryOperatorKind.Assign) {
                    var right = be.RightHandSide as UnifiedNullLiteral;
                    var nullId = be.LeftHandSide as UnifiedVariableIdentifier;
                    if (right != null && nullId != null) {
                        Console.WriteLine("{0} will be null", nullId.Name);
                        nameList.AddFirst(nullId.Name);
                    }
                }
            }
            return nameList;
        }

        //new
        public static void FindUsesDefine(UnifiedBlock codeObj) {
            var defineNames = FindUses(codeObj);
            foreach (var defName in defineNames) {
                var name = defName;
                var elements = codeObj.DescendantsUntil(
                        e => {
                            bool b = name == e;
                            Console.WriteLine(b);
                            return b;
                        });
                /*var variableName = (UnifiedVariableIdentifier)defName;
                foreach (var element in (IEnumerable<UnifiedBinaryExpression>)elements) {
                    var left = element.LeftHandSide as UnifiedVariableIdentifier;
                    var right = element.RightHandSide as UnifiedNullLiteral;
                    if (left != null && left.Name.Equals(variableName.Name)) {
                        Console.WriteLine("{0} is {1}", left.Name, element.RightHandSide);
                    }
                }*/
            }
        }

        public static void FindNodeDefines(IUnifiedElement binaryExpression) {
            var be = binaryExpression as UnifiedBinaryExpression;
            if (be != null) {
                if (be.Operator.Kind == UnifiedBinaryOperatorKind.Assign) {
                    var identifier = be.LeftHandSide as UnifiedVariableIdentifier;
                    if (identifier != null) {
                        Console.WriteLine("Def: \n{0}", identifier);
                    }
                    var usedId = be.RightHandSide as UnifiedVariableIdentifier;
                    if (usedId != null) {
                        Console.WriteLine("Use: \n{0}", usedId);
                    }
                    var literal = be.RightHandSide as UnifiedLiteral;
                    if (literal != null) {
                        Console.WriteLine("Use: \n{0}", literal);
                    }
                }
            }
            var definition = binaryExpression as UnifiedVariableDefinitionList;
            if (definition != null) {
                Console.WriteLine("Def: \n{0}", definition.First().Name);
                Console.WriteLine("Use: \n{0}", definition.First().InitialValue);
            }
        }
    }
}
