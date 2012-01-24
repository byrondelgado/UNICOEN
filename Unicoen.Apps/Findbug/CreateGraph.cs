using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unicoen.Model;

namespace Unicoen.Apps.Findbug {
    class CreateGraph {
        public static void Graph(IEnumerable<UnifiedBlock> blocks) {
            /*
             * コメントアウトの部分は、以前に作成した部分です。
             * エッジごとにそれぞれ違うリストを用意しています。
             * そうでない部分は、同じリストに全部入れているものです。
             */
            
            /*var head = new Head();
            var edge1 = new Edge();
            var edge2 = new Edge();
            var edge3 = new Edge();
            var edge4 = new Edge();
            var edge5 = new Edge();
            var edge6 = new Edge();

            head.FirstHead.NextEdges.Add(edge1);
            edge1.NextEdges.Add(edge2);
            edge2.NextEdges.Add(edge3);
            edge3.NextEdges.Add(edge4);
            edge4.NextEdges.Add(edge5);
            edge5.NextEdges.Add(edge6);
            edge6.NextEdges.Add(null);


            var next = head.FirstHead.NextEdges.First();*/

            var edge = new Edge();
            var next = 1;
            
            edge.MainList.Add(new Expression(null, next));

            foreach (var block in blocks) {
                var children = block.Elements();
                foreach (var child in children) {
                    var childBin = child as UnifiedBinaryExpression;
                    if (childBin != null) {
                        next++;
                        edge.MainList.Add(new Expression(childBin, next));
                        //next.MainList.Add(childBin);
                        //next = next.NextEdges.Last();
                    }
                    var childDef = child as UnifiedVariableDefinitionList;
                    if (childDef != null) {
                        next++;
                        edge.MainList.Add(new Expression(childDef, next));
                        //next.MainList.Add(childDef);
                        //next = next.NextEdges.Last();
                    }
                }
            }
            //next.NextEdges.Add(null);
            var array = edge.MainList.ToArray();

            //next = head.FirstHead.NextEdges.Last();
            var i = array[0].Next;
            while (i < array.Count()) {
                //Console.WriteLine(next.MainList.First());
                Console.WriteLine(array[i].Statement);
                Console.WriteLine(array[i].Next);
                FindDefinesB(array[i].Statement);
                //next = next.NextEdges.First();
                i++;
            }
        }

        public static void FindDefinesB(IUnifiedElement binaryExpression) {
            var be = binaryExpression as UnifiedBinaryExpression;
            if (be != null) {
                if (be.Operator.Kind == UnifiedBinaryOperatorKind.Assign) {
                    var left = be.LeftHandSide as UnifiedVariableIdentifier;
                    if (left != null) {
                        Console.WriteLine("Def: \n{0}", left);
                    }
                    var right = be.RightHandSide as UnifiedVariableIdentifier;
                    if (right != null) {
                        Console.WriteLine("Use: \n{0}", right);
                    }
                    var right2 = be.RightHandSide as UnifiedLiteral;
                    if (right2 != null) {
                        Console.WriteLine("Use: \n{0}", right2);
                    }
                }
            }
            var beDef = binaryExpression as UnifiedVariableDefinitionList;
            if (beDef != null) {
                Console.WriteLine("Def: \n{0}", beDef.First().Name);
                Console.WriteLine("Use: \n{0}", beDef.First().InitialValue);
            }
        }
    }

    class Head {
        public Edge FirstHead = new Edge();
    }

    class Edge {
        public IList<Edge> NextEdges = new List<Edge>();
        public List<Expression> MainList = new List<Expression>();
    }

    class Expression {
        private readonly IUnifiedElement element;
        private readonly int number;
        public Expression(IUnifiedElement element, int number) {
            this.element = element;
            this.number = number;
        }

        public IUnifiedElement Statement {
            get { return element; }
        }
        public int Next {
            get { return number; }
        }
    }
}
