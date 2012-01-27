using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unicoen.Model;

namespace Unicoen.Apps.Findbug.Util {
	internal class NodeAnalyzer {
		public static void Graph(IEnumerable<UnifiedBlock> blocks) {
			var head = new Node(null);
			head.NextNodes.Add(new Node(head));
			var node = head.NextNodes.First();

			var func = Function(blocks);

			node = DataStructures.DataGraph(node, func, null);

			node.NextNodes.Add(null);
			node = head.NextNodes.First();
			
			Display(node);

			while (node != null) {
				var suspiciousNode = DefUseAnalyzer.FindNodeUses(node);
				if (suspiciousNode != null) {
					IList<Node> definitionPoints = new List<Node>();
					var nodeBack = suspiciousNode;
					while (true) {
						if (nodeBack.EndOfBranch == true) {
							for (var i = 0; i < nodeBack.PreviousList.Count(); i++) {
								var back = nodeBack.PreviousList[i];
								while (back.BranchPoint != true) {
									var identifier = DefUseAnalyzer.FindNodeDefines(back.NodeStatement);
									if (identifier != null && identifier.Name.Equals(suspiciousNode.Define.Name)) {
										definitionPoints.Add(back);
										break;
									}
									back = back.FirstPrevious;
								}
							}
							if (definitionPoints.Any()) {
								DefUseAnalyzer.Analyze(definitionPoints, suspiciousNode);
								//break;
							}
						}
						else {
							var back = nodeBack.FirstPrevious;
							if (back == null) {
								break;
							}
							var identifier = DefUseAnalyzer.FindNodeDefines(back.NodeStatement);
							if (identifier != null && identifier.Equals(suspiciousNode.Define)) {
								definitionPoints.Add(back);
								DefUseAnalyzer.Analyze(definitionPoints, suspiciousNode);
								break;
							}
						}
						nodeBack = nodeBack.FirstPrevious;
					}
					node = node.NextNodes[0];
				}
				else {
					node = node.NextNodes[0];
				}
			}
		}

		public static IEnumerable<UnifiedBlock> Function(
				IEnumerable<UnifiedBlock> blocks) {
			foreach (var block in blocks) {
				var functions = block.Elements<UnifiedFunctionDefinition>();
				foreach (var function in functions) {
					yield return function.Body;
				}
			}
		}

		public static Node Display(Node node) {
			while (node != null && node.NextNodes.First() != null) {
				Console.WriteLine(node.NodeStatement);
				//DefUseAnalyzer.FindNodeDefines(node.NodeStatement);
				if (node.NextNodes.Count() > 1) {
					Node last = null;
					for (var i = 0; i < node.NextNodes.Count(); i++) {
						last = BranchDisplay(node.NextNodes[i]);
					}
					node = last;
				} else {
					node = node.NextNodes[0];
				}
			}
			return node;
		}

		public static Node BranchDisplay(Node node) {
			while (node != null && node.EndOfBranch != true) {
				Console.WriteLine(node.NodeStatement);
				//DefUseAnalyzer.FindNodeDefines(node.NodeStatement);
				if (node.NextNodes.Count() > 1) {
					Node last = null;
					for (var i = 0; i < node.NextNodes.Count(); i++) {
						last = BranchDisplay(node.NextNodes[i]);
					}
					node = last;
				} else {
					node = node.NextNodes[0];
				}
			}
			return node;
		}
	}
}
