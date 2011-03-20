﻿using System;
using System.Collections.Generic;
using Ucpf.Core.Model.Visitors;

namespace Ucpf.Core.Model {
	public class UnifiedCall : UnifiedExpression {
		public UnifiedExpression Function { get; set; }
		public UnifiedArgumentCollection Arguments { get; set; }

		public UnifiedCall() {
			Arguments = new UnifiedArgumentCollection();
		}

		public override void Accept(IUnifiedModelVisitor visitor) {
			visitor.Visit(this);
		}

		public override TResult Accept<TData, TResult>(
			IUnifiedModelVisitor<TData, TResult> visitor, TData data) {
			return visitor.Visit(this, data);
		}

		public override IEnumerable<UnifiedElement> GetElements() {
			yield return Function;
			yield return Arguments;
		}

		public override IEnumerable<Tuple<UnifiedElement, Action<UnifiedElement>>> GetElementsAndSetters() {
			yield return Tuple.Create<UnifiedElement, Action<UnifiedElement>>
				(Function, v => Function = (UnifiedExpression)v);
			yield return Tuple.Create<UnifiedElement, Action<UnifiedElement>>
				(Arguments, v => Arguments = (UnifiedArgumentCollection)v);
		}
	}
}