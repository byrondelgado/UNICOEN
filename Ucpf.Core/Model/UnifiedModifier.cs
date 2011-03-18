﻿using System.Collections.Generic;
using Ucpf.Core.Model.Visitors;

namespace Ucpf.Core.Model {
	public class UnifiedModifier : UnifiedElement {
		public string Name { get; set; }

		public override void Accept(IUnifiedModelVisitor visitor) {
			visitor.Visit(this);
		}

		public override TResult Accept<TData, TResult>(
			IUnifiedModelVisitor<TData, TResult> visitor, TData data) {
			return visitor.Visit(this, data);
		}

		public override IEnumerable<UnifiedElement> GetElements() {
			yield break;
		}

		public static UnifiedModifier Create(string name) {
			return new UnifiedModifier { Name = name };
		}
	}
}