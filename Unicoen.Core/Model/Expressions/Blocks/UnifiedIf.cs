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
using System.Collections.Generic;
using System.Linq;
using Unicoen.Core.Visitors;

namespace Unicoen.Core.Model {
	/// <summary>
	///   if文を表します。
	///   e.g. Javaにおける<c>if(cond){...}else{...}</c>
	/// </summary>
	public class UnifiedIf : UnifiedExpressionWithBlock<UnifiedIf> {
		private IUnifiedExpression _condition;

		/// <summary>
		///   条件式を表します
		///   <c>if(cond){...}else{...}</c>の<c>con</c>
		/// </summary>
		public IUnifiedExpression Condition {
			get { return _condition; }
			set { _condition = SetParentOfChild(value, _condition); }
		}

		private UnifiedBlock _elseBody;

		public UnifiedBlock ElseBody {
			get { return _elseBody; }
			set { _elseBody = SetParentOfChild(value, _elseBody); }
		}

		private UnifiedIf() {}

		public UnifiedIf AddToFalseBody(IUnifiedExpression expression) {
			ElseBody.Add(expression);
			return this;
		}

		public UnifiedIf RemoveFalseBody() {
			ElseBody = null;
			return this;
		}

		public override void Accept(IUnifiedModelVisitor visitor) {
			visitor.Visit(this);
		}

		public override void Accept<TData>(
				IUnifiedModelVisitor<TData> visitor,
				TData state) {
			visitor.Visit(this, state);
		}

		public override TResult Accept<TData, TResult>(
				IUnifiedModelVisitor<TData, TResult> visitor, TData state) {
			return visitor.Visit(this, state);
		}

		public static UnifiedIf Create(UnifiedBlock body) {
			return new UnifiedIf {
					Body = body,
			};
		}

		public static UnifiedIf Create(
				IUnifiedExpression condition, UnifiedBlock body) {
			return new UnifiedIf {
					Body = body,
					Condition = condition,
			};
		}

		/// <summary>
		///   一個以上のelse if節によって構成されているif-else式に分解してモデルを構築します．
		/// </summary>
		/// <param name = "conditionAndBodies"></param>
		/// <param name = "lastElseBody"></param>
		/// <returns></returns>
		public static UnifiedIf Create(
				IEnumerable<Tuple<IUnifiedExpression, UnifiedBlock>> conditionAndBodies,
				UnifiedBlock lastElseBody) {
			var ifs = conditionAndBodies
					.Select(t => Create(t.Item1, t.Item2))
					.ToList();
			for (int i = 1; i < ifs.Count; i++) {
				ifs[i - 1].ElseBody = ifs[i].ToBlock();
			}
			if (lastElseBody != null) {
				ifs[ifs.Count - 1].ElseBody = lastElseBody;
			}
			return ifs[0];
		}

		public static UnifiedIf Create(IUnifiedExpression condition) {
			return new UnifiedIf {
					Condition = condition,
			};
		}

		public static UnifiedIf Create(
				IUnifiedExpression condition, UnifiedBlock body,
				UnifiedBlock falseBody) {
			return new UnifiedIf {
					Body = body,
					Condition = condition,
					ElseBody = falseBody,
			};
		}
	}
}