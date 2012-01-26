using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unicoen.Model;

namespace Unicoen.Apps.Findbug {
    class CreateGraph {
        public static void Graph(IEnumerable<UnifiedBlock> blocks) {
            
            var head = new Node(null);
            head.NextNodes.Add(new Node(head));
            var node = head.NextNodes.First();

            node = DataGraph(node, blocks, null);

            node.NextNodes.Add(null);

            node = head.NextNodes.First();
            Display(node);
        }

        public static void Display(Node node) {
            while (node.NextNodes.First() != null) {
                //Console.WriteLine(node.NodeStatement);
                DefUseAnalyzer.FindNodeDefines(node.NodeStatement);
                if (node.NextNodes.Count() > 1) {
                    Node last = null;
                    for(var i = 0; i < node.NextNodes.Count(); i++) {
                        last = BranchDisplay(node.NextNodes[i]);
                    }
                    node = last;
                }
                else {
                    node = node.NextNodes[0];
                }
            }
        }

        public static Node BranchDisplay(Node node) {
            while (node.EndOfBranch != true) {
                //Console.WriteLine(node.NodeStatement);
                DefUseAnalyzer.FindNodeDefines(node.NodeStatement);
                if (node.NextNodes.Count() > 1) {
                    var last = node;
                    for (var i = 0; i < node.NextNodes.Count(); i++) {
                        last = BranchDisplay(node.NextNodes[i]);
                    }
                    node = last;
                }
                else {
                    node = node.NextNodes[0];
                }
            }
            return node;
        }

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

                var childIf = child as UnifiedIf;
                if (childIf != null) {
                    node.NodeStatement = childIf.Condition;
                    node.NextNodes.Add(new Node(node));
                    var ifBodyNode = DataGraph(
                            node.NextNodes.First(), null, childIf.Body);
                    if (childIf.ElseBody != null) {
                        node.NextNodes.Add(new Node(node));
                        var elseBodyNodes = DataGraph(
                                node.NextNodes.Last(), null, childIf.ElseBody);
                        elseBodyNodes.Previous.NextNodes.Clear();
                        elseBodyNodes.Previous.NextNodes.Add(ifBodyNode);
                        node = ifBodyNode;
                        ifBodyNode.EndOfBranch = true;
                    } else {
                        node.NextNodes.Insert(0, ifBodyNode);
                        node = node.NextNodes.First();
                    }
                }

                var childWhile = child as UnifiedWhile;
                if (childWhile != null) {
                    node.NodeStatement = childWhile.Condition;
                    node.NextNodes.Add(new Node(node));
                    var whileBodyNode = DataGraph(
                            node.NextNodes.First(), null, childWhile.Body);
                    node.NextNodes.Insert(0, whileBodyNode);
                    node = node.NextNodes.First();
                    //whileBodyNode.EndOfBranch = true;
                }

                var childFor = child as UnifiedFor;
                if (childFor != null) {
                    node.NodeStatement = childFor.Initializer;
                    node.NextNodes.Add(new Node(node));
                    node = node.NextNodes.First();
                    node.NodeStatement = childFor.Condition;
                    node.NextNodes.Add(new Node(node));
                    var forBodyNode = DataGraph(
                            node.NextNodes.First(), null, childFor.Body);
                    node.NextNodes.Insert(0, forBodyNode);
                    node = node.NextNodes.First();
                    //forBodyNode.EndOfBranch = true;
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

    class Node {
        public IList<Node> NextNodes = new List<Node>();
        public IUnifiedElement NodeStatement;

        public bool EndOfBranch;

        private readonly Node _previous;
        public Node(Node previous) {
            _previous = previous;
        }

        public Node Previous{
            get { return _previous; }
        }
    }
}
