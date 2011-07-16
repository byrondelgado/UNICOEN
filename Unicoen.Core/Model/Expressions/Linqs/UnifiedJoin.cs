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
	///   LINQのクエリ式を構成するjoin句を表します。
	///   e.g. C#における<c>join q in array2 on p.X equals q.X</c>
	/// </summary>
	public class UnifiedJoin : UnifiedLinqPart {
		private UnifiedVariableIdentifier _receiver;
		private IUnifiedExpression _joinSource;
		private IUnifiedExpression _firstEqualsKey;
		private IUnifiedExpression _secondEqualsKey;

		/// <summary>
		///   クエリ式を継続するために要素を受け取る変数を取得もしくは設定します．
		///   e.g. C#における<c>group p.W in p.X into g</c>の<c>g</c>
		/// </summary>
		public UnifiedVariableIdentifier Receiver {
			get { return _receiver; }
			set { _receiver = SetChild(value, _receiver); }
		}

		/// <summary>
		///   結合対象となる式を取得もしくは設定します．
		///   e.g. C#における<c>join q in array2 on p.X equals q.X</c>の<c>array2</c>
		/// </summary>
		public IUnifiedExpression JoinSource {
			get { return _joinSource; }
			set { _joinSource = SetChild(value, _joinSource); }
		}

		/// <summary>
		///   結合条件となる比較対象のクエリ式の入力元の要素の式を取得もしくは設定します．
		///   e.g. C#における<c>join q in array2 on p.X equals q.X</c>の<c>array2</c>
		/// </summary>
		public IUnifiedExpression FirstEqualsKey {
			get { return _firstEqualsKey; }
			set { _firstEqualsKey = SetChild(value, _firstEqualsKey); }
		}

		/// <summary>
		///   結合条件となる比較対象の結合対象の要素の式を取得もしくは設定します．
		///   e.g. C#における<c>join q in array2 on p.X equals q.X</c>の<c>array2</c>
		/// </summary>
		public IUnifiedExpression SecondEqualsKey {
			get { return _secondEqualsKey; }
			set { _secondEqualsKey = SetChild(value, _secondEqualsKey); }
		}

		protected UnifiedJoin() {}

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

		public static UnifiedJoin Create(
				UnifiedVariableIdentifier receiver, IUnifiedExpression joinSource,
				IUnifiedExpression firstEqualsKey, IUnifiedExpression secondEqualsKey) {
			return new UnifiedJoin {
					Receiver = receiver,
					JoinSource = joinSource,
					FirstEqualsKey = firstEqualsKey,
					SecondEqualsKey = secondEqualsKey,
			};
		}
	}
}