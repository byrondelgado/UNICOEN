using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Unicoen.Model;

namespace Unicoen.Apps.Findbug.Util {
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
					/*var right2 = be.RightHandSide as UnifiedLiteral;
					if (right2 != null) {
						yield return right2;
					}*/
				}
			}
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

		public static UnifiedVariableIdentifier FindNodeDefines(IUnifiedElement statement) {
			var boolean = statement as UnifiedVariableIdentifier;
			if(boolean != null) {
				return boolean;
			}

			var binary = statement as UnifiedBinaryExpression;
			if (binary != null) {
				if (binary.Operator.Kind == UnifiedBinaryOperatorKind.Assign) {
					var identifier = binary.LeftHandSide as UnifiedVariableIdentifier;
					if (identifier != null) {
						return identifier;
					}
				}
			}

			if (statement is UnifiedVariableDefinitionList) {
				var definition = statement.Elements().First() as UnifiedVariableDefinition;
				if (definition != null) {
					var variableDef = definition.Name as UnifiedVariableIdentifier;
					if (variableDef != null) {
						return variableDef;
					}
				}	
			}
			return null;
		}

		public static Node FindNodeUses(Node node) {
			var variable = node.NodeStatement as UnifiedVariableIdentifier;
			if (variable != null) {
				return node;
			}
			return null;

			/*var binary = statement as UnifiedBinaryExpression;
			if (binary != null) {
				if (binary.Operator.Kind == UnifiedBinaryOperatorKind.Equal
				    || binary.Operator.Kind == UnifiedBinaryOperatorKind.NotEqual) {
					//
				}
				if (binary.RightHandSide is UnifiedVariableIdentifier) {
					//
				}
			}*/
		}

		public static void Analyze(IList<Node> definitionPoints, Node suspiciousNode ) {
			var count = 0;
			for (var i = 0; i < definitionPoints.Count(); i++) {
				if (definitionPoints[i].Use is UnifiedNullLiteral) {
					count++;
				}
			}
			if (count != 0 && count < definitionPoints.Count()) {
				Console.WriteLine("{0} is might be null pointer", suspiciousNode.Define);
			}
			else if (count == definitionPoints.Count()) {
				Console.WriteLine("{0} uses null pointer.", suspiciousNode.Define);
			}
		}
	}
}
