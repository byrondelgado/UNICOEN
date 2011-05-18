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

using Unicoen.Core.Visitors;

namespace Unicoen.Core.Model {
	/// <summary>
	///   二項式を表します。
	///   e.g. JavaやCにおける<c>a + b</c>など
	/// </summary>
	public class UnifiedBinaryExpression : UnifiedElement, IUnifiedExpression {
		private IUnifiedExpression _leftHandSide;

		/// <summary>
		///   第1オペランドを表します
		///   e.g. <c>a + b</c>の<c>a</c>
		/// </summary>
		public IUnifiedExpression LeftHandSide {
			get { return _leftHandSide; }
			set { _leftHandSide = SetParentOfChild(value, _leftHandSide); }
		}

		private UnifiedBinaryOperator _operator;

		/// <summary>
		///   演算子を表します
		///   e.g. <c>a + b</c>の<c>+</c>
		/// </summary>
		public UnifiedBinaryOperator Operator {
			get { return _operator; }
			set { _operator = SetParentOfChild(value, _operator); }
		}

		private IUnifiedExpression _rightHandSide;

		/// <summary>
		///   第2オペランドを表します
		///   e.g. <c>a + b</c>の<c>b</c>
		/// </summary>
		public IUnifiedExpression RightHandSide {
			get { return _rightHandSide; }
			set { _rightHandSide = SetParentOfChild(value, _rightHandSide); }
		}

		private UnifiedBinaryExpression() {}

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

		public static UnifiedBinaryExpression Create(
				IUnifiedExpression leftHandSide,
				UnifiedBinaryOperator
						binaryOperator,
				IUnifiedExpression rightHandSide) {
			return new UnifiedBinaryExpression {
					LeftHandSide = leftHandSide,
					Operator = binaryOperator,
					RightHandSide = rightHandSide,
			};
		}
	}
}