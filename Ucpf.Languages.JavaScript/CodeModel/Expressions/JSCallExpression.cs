﻿using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Ucpf.Languages.JavaScript.CodeModel {
	//TODO implementation is not complete
	public class JSCallExpression : JSExpression {
		private XElement _node;

		public JSCallExpression(XElement xElement)
			: base(xElement) {
			_node = xElement;
		}

		public String Identifier {
			get { return null; }
		}

		public IEnumerable<JSExpression> Arguments {
			get { return null; }
		}
	}
}