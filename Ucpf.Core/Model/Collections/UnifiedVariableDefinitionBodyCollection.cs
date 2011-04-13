﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ucpf.Core.Model.Visitors;

namespace Ucpf.Core.Model
{
	/// <summary>
	///   実引数の集合を表します。
	/// </summary>
	public class UnifiedVariableDefinitionBodyCollection
		: UnifiedElementCollection<UnifiedVariableDefinitionBody, UnifiedVariableDefinitionBodyCollection>
	{
		private UnifiedVariableDefinitionBodyCollection() {}

		private UnifiedVariableDefinitionBodyCollection(IEnumerable<UnifiedVariableDefinitionBody> elements)
			: base(elements) {}

		public override void Accept(IUnifiedModelVisitor visitor)
		{
			visitor.Visit(this);
		}

		public override TResult Accept<TData, TResult>(
			IUnifiedModelVisitor<TData, TResult> visitor, TData data)
		{
			return visitor.Visit(this, data);
		}

		public static UnifiedVariableDefinitionBodyCollection Create()
		{
			return new UnifiedVariableDefinitionBodyCollection();
		}

		public static UnifiedVariableDefinitionBodyCollection Create(
			params UnifiedVariableDefinitionBody[] elements)
		{
			return new UnifiedVariableDefinitionBodyCollection(elements);
		}

		public static UnifiedVariableDefinitionBodyCollection Create(
			IEnumerable<UnifiedVariableDefinitionBody> elements)
		{
			return new UnifiedVariableDefinitionBodyCollection(elements);
		}
	}
}