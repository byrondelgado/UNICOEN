﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ucpf.Common.Model;
using Ucpf.Common.Model.Visitors;

namespace Ucpf.Common.Model {
	public class UnifiedBinaryExpression : UnifiedExpression {
		public UnifiedBinaryOperator Operator { get; set; }
		public UnifiedExpression LeftHandSide { get; set; }
		public UnifiedExpression RightHandSide { get; set; }

		public override void Accept(IUnifiedModelVisitor visitor) {
			visitor.Visit(this);
		}
	}
}
