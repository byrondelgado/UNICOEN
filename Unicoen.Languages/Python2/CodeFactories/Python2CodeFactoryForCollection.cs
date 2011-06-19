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

namespace Unicoen.Languages.Python2.CodeFactories {
	public partial class Python2CodeFactory {
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

		bool IUnifiedVisitor<bool, VisitorArgument>.Visit(
				UnifiedParameterCollection element, VisitorArgument arg) {
			VisitCollection(element, arg.Set(Paren));
			return false;
		}

		bool IUnifiedVisitor<bool, VisitorArgument>.Visit(
				UnifiedModifierCollection element, VisitorArgument arg) {
			VisitCollection(element, arg.Set(SpaceDelimiter));
			return false;
		}

		// e.g. throws E1, E2 ...
		bool IUnifiedVisitor<bool, VisitorArgument>.Visit(
				UnifiedTypeCollection element, VisitorArgument arg) {
			VisitCollection(element, arg);
			return false;
		}

		// e.g. {...}catch(Exception1 e1){...}catch{Exception2 e2}{....}... ?
		bool IUnifiedVisitor<bool, VisitorArgument>.Visit(
				UnifiedCatchCollection element, VisitorArgument arg) {
			VisitCollection(element, arg.Set(NewLineDelimiter));
			return false;
		}

		// e.g. Foo<A, B> ?
		bool IUnifiedVisitor<bool, VisitorArgument>.Visit(
				UnifiedTypeParameterCollection element, VisitorArgument arg) {
			VisitCollection(element, arg.Set(InequalitySignParen));
			return false;
		}

		bool IUnifiedVisitor<bool, VisitorArgument>.Visit(
				UnifiedIdentifierCollection element, VisitorArgument arg) {
			VisitCollection(element, arg);
			return false;
		}

		bool IUnifiedVisitor<bool, VisitorArgument>.Visit(
				UnifiedArgumentCollection element, VisitorArgument arg) {
			VisitCollection(element, arg);
			return false;
		}

		bool IUnifiedVisitor<bool, VisitorArgument>.Visit(
				UnifiedExpressionCollection element, VisitorArgument arg) {
			VisitCollection(element, arg);
			return false;
		}

		bool IUnifiedVisitor<bool, VisitorArgument>.Visit(
				UnifiedTypeArgumentCollection element, VisitorArgument arg) {
			VisitCollection(element, arg.Set(InequalitySignParen));
			return false;
		}

		bool IUnifiedVisitor<bool, VisitorArgument>.Visit(
				UnifiedCaseCollection element, VisitorArgument arg) {
			arg = arg.IncrementDepth();
			foreach (var caseElement in element) {
				arg.WriteIndent();
				caseElement.TryAccept(this, arg);
			}
			return false;
		}

		bool IUnifiedVisitor<bool, VisitorArgument>.Visit(
				UnifiedMatcherCollection element, VisitorArgument arg) {
			VisitCollection(element, arg);
			return false;
		}

		bool IUnifiedVisitor<bool, VisitorArgument>.Visit(
				UnifiedAnnotation element, VisitorArgument arg) {
			arg.Write("@");
			element.Name.TryAccept(this, arg);
			element.Arguments.TryAccept(this, arg.Set(Paren));
			arg.WriteLine();
			return false;
		}

		bool IUnifiedVisitor<bool, VisitorArgument>.Visit(
				UnifiedAnnotationCollection element, VisitorArgument arg) {
			VisitCollection(element, arg);
			return false;
		}

		bool IUnifiedVisitor<bool, VisitorArgument>.Visit(
				UnifiedVariableDefinitionList element, VisitorArgument arg) {
			var klass = element.GrandParent() as UnifiedEnum;
			if (klass != null) {
				VisitCollection(element, arg.Set(CommaDelimiter));
			} else {
				VisitCollection(element, arg.Set(SemiColonDelimiter));
			}
			return true;
		}

		bool IUnifiedVisitor<bool, VisitorArgument>.Visit(
				UnifiedSimpleType element, VisitorArgument arg) {
			element.NameExpression.TryAccept(this, arg);
			return true;
		}

		bool IUnifiedVisitor<bool, VisitorArgument>.Visit(
				UnifiedList element, VisitorArgument arg) {
			VisitCollection(element, arg);
			return false;
		}

		bool IUnifiedVisitor<bool, VisitorArgument>.Visit(
				UnifiedIterable element, VisitorArgument arg) {
			VisitCollection(element, arg);
			return false;
		}

		bool IUnifiedVisitor<bool, VisitorArgument>.Visit(
				UnifiedArray element, VisitorArgument arg) {
			VisitCollection(element, arg);
			return false;
		}

		bool IUnifiedVisitor<bool, VisitorArgument>.Visit(
				UnifiedSet element, VisitorArgument arg) {
			VisitCollection(element, arg);
			return false;
		}

		bool IUnifiedVisitor<bool, VisitorArgument>.Visit(
				UnifiedTuple element, VisitorArgument arg) {
			VisitCollection(element, arg);
			return false;
		}

		public bool Visit(UnifiedIterableComprehension element, VisitorArgument arg) {
			throw new NotImplementedException();
		}

		public bool Visit(UnifiedSetComprehension element, VisitorArgument arg) {
			throw new NotImplementedException();
		}

		public bool Visit(UnifiedInterface element, VisitorArgument arg) {
			throw new NotImplementedException();
		}

		public bool Visit(UnifiedClass element, VisitorArgument arg) {
			throw new NotImplementedException();
		}

		public bool Visit(UnifiedStruct element, VisitorArgument arg) {
			throw new NotImplementedException();
		}

		public bool Visit(UnifiedEnum element, VisitorArgument arg) {
			throw new NotImplementedException();
		}

		public bool Visit(UnifiedModule element, VisitorArgument arg) {
			throw new NotImplementedException();
		}

		public bool Visit(UnifiedUnion element, VisitorArgument arg) {
			throw new NotImplementedException();
		}

		public bool Visit(UnifiedAnnotationDefinition element, VisitorArgument arg) {
			throw new NotImplementedException();
		}
	}
}