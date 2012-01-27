using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unicoen.Model;

namespace Unicoen.Apps.Findbug.Util {
	public class Node {
		public IList<Node> NextNodes = new List<Node>();
		public IUnifiedElement NodeStatement;

		internal bool BranchPoint;
		internal bool EndOfBranch;

		internal IList<Node> PreviousList = new List<Node>();
		public Node(Node previous) {
			PreviousList.Add(previous);
		}

		public UnifiedVariableIdentifier Define {
			get { return DefUseAnalyzer.FindNodeDefines(NodeStatement); }
		}

		public IUnifiedElement Use {
			get {
				var variable = NodeStatement as UnifiedBinaryExpression;
				if (variable != null) {
					return variable.RightHandSide;
				}
				return null;
			}
		}

		public Node FirstPrevious {
			get { return PreviousList[0]; }
		}
	}
}
