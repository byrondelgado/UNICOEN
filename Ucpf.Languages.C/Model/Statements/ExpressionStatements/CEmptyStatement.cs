﻿using Ucpf.Common.Model.Visitors;
using Ucpf.Common.OldModel;
using Ucpf.Common.OldModel.Statements;

namespace Ucpf.Languages.C.Model.Statements.ExpressionStatements {
	public class CEmptyStatement : CExpressionStatement, IEmptyStatement {
		#region IEmptyStatement Members

		public void Accept(IModelToCode conv) {
			conv.Generate(this);
		}

		#endregion

		public override string ToString() {
			return ";";
		}
	}
}