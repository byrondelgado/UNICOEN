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

using System.Diagnostics;
using Unicoen.Processor;

namespace Unicoen.Model {
	/// <summary>
	///   関数やメソッドなどのサブルーチンの定義部分を表します。
	///   e.g. Javaにおける<c>public void method(int a){...}</c>
	/// </summary>
	public class UnifiedFunctionDefinition : UnifiedElement, IUnifiedExpression {
		#region fields & properties

		private UnifiedAnnotationCollection _annotations;
		private UnifiedModifierCollection _modifiers;
		private UnifiedType _type;
		private UnifiedGenericParameterCollection _genericParameters;
		private UnifiedIdentifier _name;
		private UnifiedParameterCollection _parameters;
		private UnifiedTypeCollection _throws;
		private UnifiedBlock _body;

		/// <summary>
		///   付与されているアノテーションを取得もしくは設定します．
		/// </summary>
		public UnifiedAnnotationCollection Annotations {
			get { return _annotations; }
			set { _annotations = SetChild(value, _annotations); }
		}

		/// <summary>
		///   付与されている修飾子の集合を取得もしくは設定します．
		///   e.g. Javaにおける<c>public static void method(int a){...}</c>の<c>public static</c>
		/// </summary>
		public UnifiedModifierCollection Modifiers {
			get { return _modifiers; }
			set { _modifiers = SetChild(value, _modifiers); }
		}

		/// <summary>
		///   名前を取得もしくは設定します．
		///   e.g. Javaにおける<c>public void method(int a){...}</c>の<c>method</c>
		/// </summary>
		public UnifiedType Type {
			get { return _type; }
			set { _type = SetChild(value, _type); }
		}

		public UnifiedGenericParameterCollection GenericParameters {
			get { return _genericParameters; }
			set { _genericParameters = SetChild(value, _genericParameters); }
		}

		public UnifiedIdentifier Name {
			get { return _name; }
			set { _name = SetChild(value, _name); }
		}

		public UnifiedParameterCollection Parameters {
			get { return _parameters; }
			set { _parameters = SetChild(value, _parameters); }
		}

		public UnifiedTypeCollection Throws {
			get { return _throws; }
			set { _throws = SetChild(value, _throws); }
		}

		/// <summary>
		///   ブロックを取得もしくは設定します．
		/// </summary>
		public UnifiedBlock Body {
			get { return _body; }
			set { _body = SetChild(value, _body); }
		}

		#endregion

		protected UnifiedFunctionDefinition() {}

		[DebuggerStepThrough]
		public override void Accept(IUnifiedVisitor visitor) {
			visitor.Visit(this);
		}

		[DebuggerStepThrough]
		public override void Accept<TArg>(
				IUnifiedVisitor<TArg> visitor,
				TArg arg) {
			visitor.Visit(this, arg);
		}

		[DebuggerStepThrough]
		public override TResult Accept<TArg, TResult>(
				IUnifiedVisitor<TArg, TResult> visitor, TArg arg) {
			return visitor.Visit(this, arg);
		}

		public static UnifiedFunctionDefinition Create(
				UnifiedAnnotationCollection annotations = null,
				UnifiedModifierCollection modifiers = null,
				UnifiedType type = null,
				UnifiedGenericParameterCollection genericParameters = null,
				UnifiedIdentifier name = null,
				UnifiedParameterCollection parameters = null,
				UnifiedTypeCollection throws = null,
				UnifiedBlock body = null) {
			return new UnifiedFunctionDefinition {
					Name = name,
					Annotations = annotations,
					Type = type,
					GenericParameters = genericParameters,
					Modifiers = modifiers,
					Parameters = parameters,
					Throws = throws,
					Body = body,
			};
		}
	}
}