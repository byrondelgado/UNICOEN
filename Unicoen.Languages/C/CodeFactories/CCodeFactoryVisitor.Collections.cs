﻿#region License

// Copyright (C) 2011 The Unicoen Project
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

using System;
using Unicoen.Core.Model;
using Unicoen.Core.Processor;

namespace Unicoen.Languages.C.CodeFactories {
	public partial class CCodeFactoryVisitor {
		public void VisitCollection<T, TSelf>(
				UnifiedElementCollection<T, TSelf> elements, VisitorArgument arg)
				where T : class, IUnifiedElement
				where TSelf : UnifiedElementCollection<T, TSelf> {
			var decoration = arg.Decoration;
			arg.Write(decoration.MostLeft);
			var splitter = "";
			foreach (var e in elements) {
				arg.Write(splitter);
				arg.Write(decoration.EachLeft);
				e.TryAccept(this, arg);
				arg.Write(decoration.EachRight);
				splitter = decoration.Delimiter;
			}
			arg.Write(decoration.MostRight);
		}

		public override bool Visit(
				UnifiedArgumentCollection element, VisitorArgument arg) {
			VisitCollection(element, arg);
			return false;
		}

		public override bool Visit(
				UnifiedParameterCollection element, VisitorArgument arg) {
			VisitCollection(element, arg.Set(Paren));
			return false;
		}

		public override bool Visit(
				UnifiedModifierCollection element, VisitorArgument arg) {
			VisitCollection(element, arg.Set(SpaceDelimiter));
			return false;
		}

		public override bool Visit(UnifiedVariableDefinitionList element, VisitorArgument arg) {

			foreach (var variableDefinition in element) {
				arg.Write(variableDefinition.Modifiers.TryAccept(this, arg));
				arg.Write(" ");
				arg.Write(variableDefinition.Type.TryAccept(this, arg));
				arg.Write(" ");
				arg.Write(variableDefinition.Name.TryAccept(this, arg));

				if (variableDefinition.InitialValue != null) {
					arg.Write( " = ");
					arg.Write(variableDefinition.InitialValue);
				}
				arg.WriteLine(";");
			}

			return true;
		}

		public override bool Visit(
				UnifiedExpressionCollection element, VisitorArgument arg) {
			throw new InvalidOperationException();
		}

		public override bool Visit(
				UnifiedGenericArgumentCollection element, VisitorArgument arg) {
			arg.WriteLine("/* ElementNotInC */");
			arg.WriteLine("/* " + element + " */");
			return false;
		}

		public override bool Visit(UnifiedCaseCollection element, VisitorArgument arg) {
			arg = arg.IncrementDepth();
			foreach (var caseElement in element) {
				arg.WriteIndent();
				caseElement.TryAccept(this, arg);
			}

			return false;
		}

		public override bool Visit(UnifiedTypeCollection element, VisitorArgument arg) {
			arg.WriteLine("/* ElementNotInC */");
			arg.WriteLine("/* " + element + " */");
			return false;
		}

		public override bool Visit(
				UnifiedCatchCollection element, VisitorArgument arg) {
			arg.WriteLine("/* ElementNotInC */");
			arg.WriteLine("/* " + element + " */");
			return false;
		}

		public override bool Visit(
				UnifiedGenericParameterCollection element, VisitorArgument arg) {
			arg.WriteLine("/* ElementNotInC */");
			arg.WriteLine("/* " + element + " */");
			return false;
		}

		public override bool Visit(
				UnifiedTypeConstrainCollection element, VisitorArgument arg) {
			arg.WriteLine("/* ElementNotInC */");
			arg.WriteLine("/* " + element + " */");
			return false;
		}

		public override bool Visit(
				UnifiedIdentifierCollection element, VisitorArgument arg) {
			// 親要素が処理するので辿りついてはいけない
			throw new InvalidOperationException();
		}

		public override bool Visit(UnifiedSimpleType element, VisitorArgument arg) {
			element.BasicType.TryAccept(this, arg);
			return true;
		}


	}
}