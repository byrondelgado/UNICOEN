﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ucpf.Core.Model.Visitors;

namespace Ucpf.Core.Model {
	/// <summary>
	/// UnifiedSpecialBlockの種類を表します。
	/// </summary>
	public enum UnifiedSpecialBlockType {
		Synchrnoized,
		Fix,
		Using,
	}

	/// <summary>
	/// synchronizedなど特殊なブロックを表します。
	/// </summary>
	public class UnifiedSpecialBlock : UnifiedExpressionWithBlock<UnifiedSpecialBlock> {
		public UnifiedSpecialBlockType Type { get; set; }

		public IUnifiedExpression Value { get; set; }

		private UnifiedSpecialBlock() { }

		public override void Accept(IUnifiedModelVisitor visitor) {
			visitor.Visit(this);
		}

		public override TResult Accept<TData, TResult>(IUnifiedModelVisitor<TData, TResult> visitor, TData data) {
			return visitor.Visit(this, data);
		}

		public override IEnumerable<IUnifiedElement> GetElements() {
			yield return Body;
		}

		public override IEnumerable<Tuple<IUnifiedElement, Action<IUnifiedElement>>> GetElementAndSetters() {
			yield return Tuple.Create<IUnifiedElement, Action<IUnifiedElement>>
					(Body, v => Body = (UnifiedBlock)v);
		}

		public override IEnumerable<Tuple<IUnifiedElement, Action<IUnifiedElement>>> GetElementAndDirectSetters() {
			yield return Tuple.Create<IUnifiedElement, Action<IUnifiedElement>>
					(_body, v => _body = (UnifiedBlock)v);
		}

		public static UnifiedSpecialBlock Create(UnifiedSpecialBlockType type, IUnifiedExpression value, UnifiedBlock body) {
			return new UnifiedSpecialBlock {
					Type = type,
					Value = value,
					Body = body,
			};
		}
	}
}