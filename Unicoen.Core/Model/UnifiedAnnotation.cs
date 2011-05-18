﻿using System;
using System.Linq;
using System.Text;
using Unicoen.Core.Visitors;

namespace Unicoen.Core.Model {
	public class UnifiedAnnotation : UnifiedElement, IUnifiedExpression {
		private UnifiedType _type;

		public UnifiedType Type {
			get { return _type; }
			set { _type = SetParentOfChild(value, _type); }
		}

		private UnifiedArgumentCollection _arguments;

		/// <summary>
		///   実引数の集合を表します
		///   e.g. Javaにおける<c>method(a, b, c)</c>の<c>a, b, c</c>の部分
		/// </summary>
		public UnifiedArgumentCollection Arguments {
			get { return _arguments; }
			set { _arguments = SetParentOfChild(value, _arguments); }
		}

		private UnifiedAnnotation() { }

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

		public static UnifiedAnnotation Create(UnifiedType type, UnifiedArgumentCollection arguments) {
			return new UnifiedAnnotation {
					Type = type,
					Arguments = arguments,
			};
		}
	}
}
