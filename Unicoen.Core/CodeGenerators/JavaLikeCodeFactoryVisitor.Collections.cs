﻿#region License

// Copyright (C) 2011-2012 The Unicoen Project
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System.Linq;
using Unicoen.Model;
using Unicoen.Processor;

namespace Unicoen.CodeGenerators {
	public partial class JavaLikeCodeFactoryVisitor : UnifiedVisitor<VisitorArgument, bool> {
		public override bool Visit(
				UnifiedSet<UnifiedParameter> element, VisitorArgument arg) {
			VisitCollection(element, arg.Set(Paren));
			return false;
		}

		public override bool Visit(
				UnifiedSet<UnifiedModifier> element, VisitorArgument arg) {
			VisitCollection(element, arg.Set(SpaceEachRight));
			return false;
		}

		// e.g. throws E1, E2 ...
		public override bool Visit(
				UnifiedSet<UnifiedType> element, VisitorArgument arg) {
			VisitCollection(element, arg.Set(Throws));
			return false;
		}

		// e.g. {...}catch(Exception1 e1){...}catch{Exception2 e2}{....}... ?
		public override bool Visit(
				UnifiedSet<UnifiedCatch> element, VisitorArgument arg) {
			VisitCollection(element, arg.Set(NewLineDelimiter));
			return false;
		}

		// e.g. Foo<A, B> ?
		public override bool Visit(
				UnifiedSet<UnifiedGenericParameter> element, VisitorArgument arg) {
			VisitCollection(element, arg.Set(InequalitySignParen));
			return false;
		}

		/// <summary>
		/// e.g. class A implements B, C extends D, E
		/// </summary>
		/// <param name="element"></param>
		/// <param name="arg"></param>
		/// <returns></returns>
		public override bool Visit(
				UnifiedSet<UnifiedTypeConstrain> element, VisitorArgument arg) {
			UnifiedTypeConstrain last = null;
			foreach (var current in element) {
				if (last == null || last.GetType() != current.GetType()) {
					// implements
					current.TryAccept(this, arg.Set(NullDelimiter));
				} else {
					current.TryAccept(this, arg);
				}
				last = current;
			}
			return false;
		}

		public override bool Visit(
				UnifiedSet<UnifiedIdentifier> element, VisitorArgument arg) {
			VisitCollection(element, arg);
			return false;
		}

		public override bool Visit(
				UnifiedSet<UnifiedArgument> element, VisitorArgument arg) {
			VisitCollection(element, arg);
			return false;
		}

		public override bool Visit(
				UnifiedSet<UnifiedExpression> element, VisitorArgument arg) {
			VisitCollection(element, arg);
			return false;
		}

		public override bool Visit(
				UnifiedSet<UnifiedGenericArgument> element, VisitorArgument arg) {
			VisitCollection(element, arg.Set(InequalitySignParen));
			return false;
		}

		public override bool Visit(
				UnifiedSet<UnifiedCase> element, VisitorArgument arg) {
			arg = arg.IncrementDepth();
			foreach (var caseElement in element) {
				WriteIndent(arg.IndentDepth);
				caseElement.TryAccept(this, arg);
			}
			return false;
		}

		public override bool Visit(
				UnifiedAnnotation element, VisitorArgument arg) {
			Writer.Write("@");
			element.Name.TryAccept(this, arg);
			element.Arguments.TryAccept(this, arg.Set(Paren));
			Writer.WriteLine();
			return false;
		}

		public override bool Visit(
				UnifiedSet<UnifiedAnnotation> element, VisitorArgument arg) {
			VisitCollection(element, arg.Set(SpaceEachRight));
			return false;
		}

		public override bool Visit(
				UnifiedVariableDefinitionList element, VisitorArgument arg) {
			if (element.GrandParent() is UnifiedEnumDefinition) {
				var comma = "";
				foreach (var varDef in element) {
					Writer.Write(comma);
					varDef.Annotations.TryAccept(this, arg);
					varDef.Modifiers.TryAccept(this, arg);
					varDef.Type.TryAccept(this, arg);
					Writer.Write(" ");
					varDef.Name.TryAccept(this, arg);
					varDef.Arguments.TryAccept(this, arg.Set(Paren));
					if (varDef.InitialValue != null) {
						Writer.Write(" = ");
						varDef.InitialValue.TryAccept(this, arg.Set(Brace));
					}
					varDef.Body.TryAccept(this, arg.Set(ForBlock));
					comma = ", ";
				}
				return true;
			}

			// アノテーションの場合は String value() default "test";
			var setterSign =
					element.GrandParent() is UnifiedAnnotationDefinition
							? " default " : " = ";

			// 変数宣言を一つにまとめて出力
			// int a, b[];
			// ArrayList<Integer> a, b[];
			var commonTypeStr = "";
			var isFirst = true;
			foreach (var varDef in element) {
				if (isFirst) {
					varDef.Annotations.TryAccept(this, arg);
					varDef.Modifiers.TryAccept(this, arg);
					commonTypeStr = GetString(varDef.Type, arg);
					Writer.Write(commonTypeStr + " ");
					varDef.Name.TryAccept(this, arg);
					isFirst = false;
				} else {
					Writer.Write(", ");
					varDef.Name.TryAccept(this, arg);
					Writer.Write(
							GetString(varDef.Type, arg).Substring(
									commonTypeStr.Length));
				}
				varDef.Arguments.TryAccept(this, arg.Set(Paren));

				if (varDef.InitialValue != null) {
					if (varDef.Modifiers != null
					    && varDef.Modifiers.Any(m => m.Name == "static")) {
						Writer.Write(" = ");
					} else {
						Writer.Write(setterSign);
					}
					varDef.InitialValue.TryAccept(this, arg.Set(Brace));
				}
				varDef.Body.TryAccept(this, arg.Set(ForBlock));
			}
			return true;
		}

		public override bool Visit(
				UnifiedListLiteral element, VisitorArgument arg) {
			VisitCollection(element, arg);
			return false;
		}

		public override bool Visit(
				UnifiedIterableLiteral element, VisitorArgument arg) {
			VisitCollection(element, arg);
			return false;
		}

		public override bool Visit(
				UnifiedArrayLiteral element, VisitorArgument arg) {
			VisitCollection(element, arg.Set(Brace));
			return false;
		}

		public override bool Visit(
				UnifiedSetLiteral element, VisitorArgument arg) {
			VisitCollection(element, arg);
			return false;
		}

		public override bool Visit(
				UnifiedTupleLiteral element, VisitorArgument arg) {
			VisitCollection(element, arg);
			return false;
		}
	}
}