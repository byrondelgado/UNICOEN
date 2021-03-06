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

using System.Diagnostics;
using Unicoen.Processor;

namespace Unicoen.Model {
	/// <summary>
	///   リソース管理に用いられるローンパターンを提供する構文を表します。 e.g. C#における <c>using(var r = new StreamReader(path)){...}</c> の <c>var r = new StreamReader(path)</c> e.g. Pythonにおける <c>with file(p1) as f1, file(p2) as f2:</c> の <c>file(p1) as f1</c>
	/// </summary>
	public class UnifiedUsing
			: UnifiedExpression {
		private UnifiedSet<UnifiedExpression> _expressions;
		private UnifiedBlock _body;

		/// <summary>
		///   リソース管理の対象となる式の集合を取得もしくは設定します． e.g. C#における <c>using(var r = new StreamReader(path)){...}</c> の <c>var r = new StreamReader(path)</c> e.g. Pythonにおける <c>with file(p1) as f1, file(p2) as f2:</c> の <c>file(p1) as f1, file(p2) as f2</c> なお，Pythonにおける <c>file(p1) as f1</c> は <c>f1 = file(p1)</c> という代入式だと見なします
		/// </summary>
		public UnifiedSet<UnifiedExpression> Expressions {
			get { return _expressions; }
			set { _expressions = SetChild(value, _expressions); }
		}

		/// <summary>
		///   ブロックを取得もしくは設定します．
		/// </summary>
		public UnifiedBlock Body {
			get { return _body; }
			set { _body = SetChild(value, _body); }
		}

		private UnifiedUsing() {}

		[DebuggerStepThrough]
		public override void Accept(UnifiedVisitor visitor) {
			visitor.Visit(this);
		}

		[DebuggerStepThrough]
		public override void Accept<TArg>(
				UnifiedVisitor<TArg> visitor,
				TArg arg) {
			visitor.Visit(this, arg);
		}

		[DebuggerStepThrough]
		public override TResult Accept<TArg, TResult>(
				UnifiedVisitor<TArg, TResult> visitor, TArg arg) {
			return visitor.Visit(this, arg);
		}

		public static UnifiedUsing Create(
				UnifiedSet<UnifiedExpression> expressions = null,
				UnifiedBlock body = null) {
			return new UnifiedUsing {
					Expressions = expressions,
					Body = body,
			};
		}
			}
}