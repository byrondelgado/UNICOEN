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
using Unicoen.Core.Visitors;

namespace Unicoen.Core.Model {
	/// <summary>
	/// 3項式を表します
	/// Javaにおける<c>a ? b : c</c>
	/// </summary>
	public class UnifiedTernaryExpression : UnifiedElement, IUnifiedExpression {
		private IUnifiedExpression _firstExpression;

		/// <summary>
		/// 3項式の第1オペランドを表します
		/// </summary>
		public IUnifiedExpression FirstExpression {
			get { return _firstExpression; }
			set { _firstExpression = SetParentOfChild(value, _firstExpression); }
		}

		private UnifiedTernaryOperator _operator;

		/// <summary>
		/// 3項式の演算子を表します
		/// </summary>
		public UnifiedTernaryOperator Operator {
			get { return _operator; }
			set { _operator = SetParentOfChild(value, _operator); }
		}

		private IUnifiedExpression _secondExpression;

		/// <summary>
		/// 3項式の第2オペランドを表します
		/// Javaにおける<c>a ? b : c</c>の<c>b</c>
		/// </summary>
		public IUnifiedExpression SecondExpression {
			get { return _secondExpression; }
			set { _secondExpression = SetParentOfChild(value, _secondExpression); }
		}

		private IUnifiedExpression _lastExpression;

		/// <summary>
		/// 3項式の第3オペランドを表します
		/// Javaにおける<c>a ? b : c</c>の<c>c</c>
		/// </summary>
		public IUnifiedExpression LastExpression {
			get { return _lastExpression; }
			set { _lastExpression = SetParentOfChild(value, _lastExpression); }
		}

		private UnifiedTernaryExpression() {}

		public override void Accept(IUnifiedModelVisitor visitor) {
			visitor.Visit(this);
		}

		public override void Accept<TData>(
				IUnifiedModelVisitor<TData> visitor,
				TData data) {
			visitor.Visit(this, data);
		}

		public override TResult Accept<TData, TResult>(
				IUnifiedModelVisitor<TData, TResult> visitor, TData data) {
			return visitor.Visit(this, data);
		}

		public override IEnumerable<IUnifiedElement> GetElements() {
			yield return FirstExpression;
			yield return Operator;
			yield return SecondExpression;
			yield return LastExpression;
		}

		public override IEnumerable<Tuple<IUnifiedElement, Action<IUnifiedElement>>>
				GetElementAndSetters() {
			yield return Tuple.Create<IUnifiedElement, Action<IUnifiedElement>>
					(FirstExpression, v => FirstExpression = (IUnifiedExpression)v);
			yield return Tuple.Create<IUnifiedElement, Action<IUnifiedElement>>
					(Operator, v => Operator = (UnifiedTernaryOperator)v);
			yield return Tuple.Create<IUnifiedElement, Action<IUnifiedElement>>
					(SecondExpression, v => SecondExpression = (IUnifiedExpression)v);
			yield return Tuple.Create<IUnifiedElement, Action<IUnifiedElement>>
					(LastExpression, v => LastExpression = (IUnifiedExpression)v);
		}

		public override IEnumerable<Tuple<IUnifiedElement, Action<IUnifiedElement>>>
				GetElementAndDirectSetters() {
			yield return Tuple.Create<IUnifiedElement, Action<IUnifiedElement>>
					(_firstExpression, v => _firstExpression = (IUnifiedExpression)v);
			yield return Tuple.Create<IUnifiedElement, Action<IUnifiedElement>>
					(_operator, v => _operator = (UnifiedTernaryOperator)v);
			yield return Tuple.Create<IUnifiedElement, Action<IUnifiedElement>>
					(_secondExpression, v => _secondExpression = (IUnifiedExpression)v);
			yield return Tuple.Create<IUnifiedElement, Action<IUnifiedElement>>
					(_lastExpression, v => _lastExpression = (IUnifiedExpression)v);
		}
		
		public static UnifiedTernaryExpression Create(
				IUnifiedExpression firstExpression,
				UnifiedTernaryOperator ternaryOperator,
				IUnifiedExpression secondExpression,
				IUnifiedExpression lastExpression) {
			return new UnifiedTernaryExpression {
					FirstExpression = firstExpression,
					Operator = ternaryOperator,
					SecondExpression = secondExpression,
					LastExpression = lastExpression,
			};
		}
	}
}