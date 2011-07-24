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

using System.Diagnostics.Contracts;
using System.Xml.Linq;
using UniUni.Xml.Linq;
using Unicoen.Model;

// ReSharper disable InvocationIsSkipped

namespace Unicoen.Languages.Ruby18.Model {
	public partial class Ruby18ModelFactoryHelper {
		private static void InitializeDefinitions() {
			ExpressionFuncs["defn"] = CreateDefn;
		}

		public static IUnifiedExpression CreateDefn(XElement node) {
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "defn");
			return UnifiedFunctionDefinition.Create(
					null, null, null, null,
					CreateSymbol(node.NthElement(0)),
					CreateArgs(node.NthElement(1)), null,
					CreateScope(node.NthElement(2)));
		}

		public static UnifiedExtendConstrain CreateConst(XElement node) {
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "const");
			return UnifiedExtendConstrain.Create(
					UnifiedType.Create(node.Value));
		}

		public static UnifiedClassDefinition CreateClass(XElement node) {
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "class");
			var constNode = node.NthElement(1);
			var constrain = constNode.Name() != "nil"
			                		? CreateConst(constNode) : null;
			return UnifiedClassDefinition.Create(
					null, null, CreateSymbol(node.NthElement(0)), null,
					constrain.ToCollection(),
					CreateScope(node.NthElement(2)));
		}

		public static UnifiedClassDefinition CreateModule(XElement node) {
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "module");
			return UnifiedClassDefinition.Create(
					null, null, CreateSymbol(node.NthElement(0)), null,
					null,
					CreateScope(node.NthElement(2)));
		}

		public static UnifiedClassDefinition CreateModule(XElement node) {
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "module");
			return UnifiedClassDefinition.Create(
					null, null, CreateSymbol(node.NthElement(0)), null,
					null,
					CreateScope(node.NthElement(2)));
		}
	}
}