using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unicoen.Model;

namespace Unicoen.Apps.Findbug.Util {
	internal class DataStructures {
		public static Node DataGraph(Node node, IEnumerable<UnifiedBlock> blocks, UnifiedBlock oneBlock) {
			if(blocks == null && oneBlock != null) {
				node = BlockAnalyzer(node, oneBlock);
			}
			else if(blocks != null){
				foreach (var block in blocks) {
					node = BlockAnalyzer(node, block);
				}
			}
			return node;
		}

		public static Node BlockAnalyzer(Node node, UnifiedBlock block) {
			var children = block.Elements();
			foreach (var child in children) {
				var childDef = child as UnifiedVariableDefinitionList;
				if (childDef != null) {
					node.NodeStatement = childDef;
					node.NextNodes.Add(new Node(node));
					node = node.NextNodes.First();
				}

				var childBin = child as UnifiedBinaryExpression;
				if (childBin != null) {
					node.NodeStatement = childBin;
					node.NextNodes.Add(new Node(node));
					node = node.NextNodes.First();
				}

				var childUnary = child as UnifiedUnaryExpression;
				if (childUnary != null) {
					node.NodeStatement = childUnary;
					node.NextNodes.Add(new Node(node));
					node = node.NextNodes.First();
				}

				var childCall = child as UnifiedCall;
				if (childCall != null) {
					node.NodeStatement = childCall;
					node.NextNodes.Add(new Node(node));
					node = node.NextNodes.First();
				}
				
				var childIf = child as UnifiedIf;
				if (childIf != null) {
					node.NodeStatement = childIf.Condition;
					node.NextNodes.Add(new Node(node));
					node.BranchPoint = true;
					var ifBodyNode = DataGraph(
							node.NextNodes.First(), null, childIf.Body);
					if (childIf.ElseBody != null) {
						node.NextNodes.Add(new Node(node));
						var elseBodyNodes = DataGraph(
								node.NextNodes.Last(), null, childIf.ElseBody);
						elseBodyNodes.FirstPrevious.NextNodes.Clear();
						elseBodyNodes.FirstPrevious.NextNodes.Add(ifBodyNode);
						ifBodyNode.PreviousList.Add(elseBodyNodes);
						ifBodyNode.EndOfBranch = true;
						node = ifBodyNode;
					} else {
						node.NextNodes.Insert(0, ifBodyNode);
						ifBodyNode.PreviousList.Add(node);
						ifBodyNode.EndOfBranch = true;
						node = ifBodyNode;
					}
				}

				var childWhile = child as UnifiedWhile;
				if (childWhile != null) {
					node.NodeStatement = childWhile.Condition;
					node.NextNodes.Add(new Node(node));
					node.BranchPoint = true;
					var whileBodyNode = DataGraph(
							node.NextNodes.First(), null, childWhile.Body);

					node.NextNodes.Insert(0, whileBodyNode);
					whileBodyNode.PreviousList.Add(node);
					whileBodyNode.EndOfBranch = true;
					node = whileBodyNode;
				}

				var childFor = child as UnifiedFor;
				if (childFor != null) {
					node.NodeStatement = childFor.Initializer;
					node.NextNodes.Add(new Node(node));
					node.BranchPoint = true;
					node = node.NextNodes.First();
					node.NodeStatement = childFor.Condition;
					node.NextNodes.Add(new Node(node));
					var forBodyNode = DataGraph(
							node.NextNodes.First(), null, childFor.Body);

					node.NextNodes.Insert(0, forBodyNode);
					forBodyNode.PreviousList.Add(node);
					forBodyNode.EndOfBranch = true;
					node = forBodyNode;
				}

				var childReturn = child as UnifiedReturn;
				if (childReturn != null) {
					node.NodeStatement = childReturn.Value;
					node.NextNodes.Add(new Node(node));
					node = node.NextNodes.First();
				}
			}
			return node;
		}
	}
}
