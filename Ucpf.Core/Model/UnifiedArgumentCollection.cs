﻿using System;
using System.Collections.Generic;
using Ucpf.Core.Model.Visitors;

namespace Ucpf.Core.Model {
	public class UnifiedArgumentCollection
		: UnifiedElementCollection<UnifiedArgument> {
		public UnifiedArgumentCollection() {}

		public UnifiedArgumentCollection(IEnumerable<UnifiedArgument> elements)
			: base(elements) {}

		public override TResult Accept<TData, TResult>(IUnifiedModelVisitor<TData, TResult> visitor, TData data) {
			return visitor.Visit(this, data);
		}

		public override IEnumerable<UnifiedElement> GetElements() {
			return this;
		}
		}
}