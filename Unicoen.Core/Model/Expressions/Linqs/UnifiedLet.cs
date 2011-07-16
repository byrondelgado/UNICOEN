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
	///   LINQのクエリ式を構成するlet句を表します。
	///   e.g. C#における<c>let sumZ = p.Z.Sum()</c>
	/// </summary>
	public class UnifiedLet : UnifiedLinqPart {
		private UnifiedVariableIdentifier _variable;
		private IUnifiedExpression _expression;

		/// <summary>
		///   クエリ式中で計算した値を代入する変数を取得もしくは設定します．
		///   e.g. C#における<c>let sumZ = p.Z.Sum()</c>の<c>sumZ</c>
		/// </summary>
		public UnifiedVariableIdentifier Variable {
			get { return _variable; }
			set { _variable = SetChild(value, _variable); }
		}

		/// <summary>
		///   変数に代入する式を取得もしくは設定します．
		///   e.g. C#における<c>let sumZ = p.Z.Sum()</c>の<c>p.Z.Sum()</c>
		/// </summary>
		public IUnifiedExpression Expression {
			get { return _expression; }
			set { _expression = SetChild(value, _expression); }
		}

		protected UnifiedLet() {}

		[DebuggerStepThrough]
		public override void Accept(IUnifiedVisitor visitor) {
			visitor.Visit(this);
		}

		[DebuggerStepThrough]
		public override void Accept<TArg>(IUnifiedVisitor<TArg> visitor, TArg arg) {
			visitor.Visit(this, arg);
		}

		[DebuggerStepThrough]
		public override TResult Accept<TArg, TResult>(
				IUnifiedVisitor<TArg, TResult> visitor, TArg arg) {
			return visitor.Visit(this, arg);
		}

		public static UnifiedLet Create(
				UnifiedVariableIdentifier variable, IUnifiedExpression expression) {
			return new UnifiedLet {
					Variable = variable,
					Expression = expression
			};
		}
	}
}