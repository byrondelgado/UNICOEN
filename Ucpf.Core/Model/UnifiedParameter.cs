﻿using System.Collections.Generic;
using Ucpf.Core.Model.Visitors;

namespace Ucpf.Core.Model {
	public class UnifiedParameter : UnifiedElement {
		public UnifiedModifierCollection Modifiers { get; set; }
		public string Name { get; set; }
		public UnifiedType Type { get; set; }

		public UnifiedParameter() {
			Modifiers = new UnifiedModifierCollection();
		}

		public override void Accept(IUnifiedModelVisitor visitor) {
			visitor.Visit(this);
		}

		public override TResult Accept<TData, TResult>(
			IUnifiedModelVisitor<TData, TResult> visitor, TData data) {
			return visitor.Visit(this, data);
		}

		public override IEnumerable<UnifiedElement> GetElements() {
			yield return Modifiers;
			yield return Type;
		}
	}
}