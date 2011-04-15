﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Code2Xml.Languages.Java.XmlGenerators;
using Paraiba.Xml.Linq;
using Ucpf.Core.Model;
using Ucpf.Core.Model.Extensions;

namespace Ucpf.Languages.Java.Model {
	public class OldJavaModelFactory {
		#region Expression

		public static IUnifiedExpression CreateExpression(XElement node) {
			Contract.Requires(node != null);
			//Contract.Requires(node.Name().ToLower().EndsWith("expression"));
			//UnaryExpressionの際に<primary>が来る可能性もある

			var binaryOperator = new[] { "+", "-", "*", "/", "%", "<", ">" };

			/* 
			 * in descendants of <expression> node, if
			 * it has more than 2 child-node OR
			 * it has only one child whose name is <IDENTIFIER> OR
			 * it has only one child whose name is <TOKEN> 
			 * these are some actual expression
			*/
			var expressionList =
					node.DescendantsAndSelf().Where(e => {
						var c = e.Elements().Count();
						return c > 1 || (c == 1 && e.Element("IDENTIFIER") != null) ||
						       (c == 1 && e.Element("TOKEN") != null);
					});

			//Ensure that node has some expression
			Debug.Assert(expressionList.Count() != 0);

			var topExpressionElement = expressionList.First();

			//case <primary>: child is IDENTIFIER or TOKEN
			if (topExpressionElement.Elements().Count() == 1) {
				//case true or false literal
				switch (topExpressionElement.Value) {
				case "true":
				case "false":
					return CreateBooleanLiteral(topExpressionElement);
				}

				//case Variable
				var regex = new Regex(@"^[a-zA-Z_]{1}[a-zA-Z0-9_]*$");
				if (regex.IsMatch(topExpressionElement.Value))
					return CreateVariable(topExpressionElement);

				//other cases
				return CreateLiteral(topExpressionElement);
			}

			//case BinaryExpression
			var binaryOperatorString = topExpressionElement.Elements().ElementAt(1).Value;
			if (binaryOperator.Contains(binaryOperatorString)) {
				return CreateBinaryExpression(topExpressionElement);
			}
			if (topExpressionElement.Name() == "expression") {
				return CreateBinaryExpression(topExpressionElement);
			}

			switch (topExpressionElement.Name()) {
			case "unaryExpression":
				return CreateUnaryExpression(topExpressionElement);
			case "unaryExpressionNotPlusMinus":
				return CreateUnaryExpressionNotPlusMinus(topExpressionElement);
			case "primary":
				//case CallExpression
				return CreatePrimary(topExpressionElement);
			case "parExpression":
				// expression を () で囲ったような場合
				return CreateExpression(topExpressionElement.Elements().ElementAt(1));
			case "castExpression":
				//case CastExpression
				return CreateCast(topExpressionElement);
			case "creator":
			case "arrayCreator":
				// "new"で始まるジェネリックや配列など
				return CreateNew(topExpressionElement);
			}

			throw new NotImplementedException();
		}

		public static UnifiedBinaryExpression CreateBinaryExpression(XElement node) {
			Contract.Requires(node != null);
			Contract.Requires(node.Elements().Count() == 3);
			/*
			 * AST上に<BinaryExpression>という名前の要素は存在しない
			   <multiplicativeExpression>などのいずれかが該当する
			   事前条件は"左辺","演算子","右辺"からなる子要素３つを持つこととする
			*/
			return UnifiedBinaryExpression.Create(CreateExpression(node.NthElement(0)),
					CreateBinaryOperator(node.NthElement(1)),
					CreateExpression(node.NthElement(2)));
		}

		public static IUnifiedExpression CreateUnaryExpressionNotPlusMinus(
				XElement node) {
			Contract.Requires(node != null);
			Contract.Requires(node.Name().StartsWith("unaryExpressionNotPlusMinus"));
			/*
			 * unaryExpressionNotPlusMinus 
			 *		: '~' unaryExpression
			 *		| '!' unaryExpression
			 *		| castExpression
			 *		| primary (selector)* ( '++' | '--' )?
			 */
			var firstElement = node.FirstElement();
			switch (firstElement.Name()) {
			case "castExpression":
				return CreateCast(firstElement);
			case "primary":
				var result = CreatePrimary(firstElement);

				result = node.Elements("selector")
						.Aggregate(result, CreateSelector);

				var lastNode = node.LastElement();
				if (!lastNode.HasElement()) {
					var ope = lastNode.Value;
					result = UnifiedUnaryExpression.Create(result,
							UnifiedUnaryOperator.Create(ope,
									ope == "++" ? UnifiedUnaryOperatorKind.PostIncrementAssign
											: UnifiedUnaryOperatorKind.PostDecrementAssign));
				}
				return result;
			}
			return UnifiedUnaryExpression.Create(
					CreateExpression(node.NthElement(1)),
					CreatePrefixUnaryOperator(firstElement.Value));
		}

		public static UnifiedCast CreateCast(XElement node) {
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "castExpression");
			/* 
			 * castExpression 
				:   '(' primitiveType ')' unaryExpression
				|   '(' type ')' unaryExpressionNotPlusMinus 
			 */
			var type = node.NthElement(1).Name() == "type"
			           		? CreateTypeOrCreatedName(node.NthElement(1))
			           		: CreatePrimitiveType(node.NthElement(1));
			return UnifiedCast.Create(type, CreateExpression(node.NthElement(3)));
		}

		public static IUnifiedExpression CreateSelector(IUnifiedExpression prefix,
		                                                XElement node) {
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "selector");
			/*
			 *  selector  
				:   '.' IDENTIFIER (arguments)?	// student.getName()
				|   '.' 'this'					// OuterClass.this
				|   '.' 'super' superSuffix		// Outer.super()
				|   innerCreator
				|   '[' expression ']' 
			 */
			var secondElement = node.NthElement(1);
			if (secondElement == null)
				return CreateInnerCreator(prefix, node.FirstElement());

			if (secondElement.Name() == "IDENTIFIER") {
				prefix = UnifiedProperty.Create(prefix, secondElement.Value, ".");
				var arguments = node.Element("arguments");
				if (arguments != null) {
					prefix = UnifiedCall.Create(prefix, CreateArguments(arguments));
				}
				return prefix;
			}

			throw new NotImplementedException();
		}

		private static IUnifiedExpression CreateInnerCreator(
				IUnifiedExpression prefix, XElement node) {
			throw new NotImplementedException();
		}

		public static IUnifiedExpression CreateUnaryExpression(XElement node) {
			Contract.Requires(node != null);
			Contract.Requires(node.Name().StartsWith("unaryExpression"));
			/*
			 * unaryExpression 
			 *	: '+' unaryExpression
			 *	| '-' unaryExpression
			 *	| '++' unaryExpression
			 *	| '--' unaryExpression
			 *	| unaryExpressionNotPlusMinus
			*/
			var firstElement = node.FirstElement();
			if (firstElement.Name() == "unaryExpressionNotPlusMinus") {
				return CreateUnaryExpressionNotPlusMinus(firstElement);
			}
			return UnifiedUnaryExpression.Create(
					CreateUnaryExpression(node.NthElement(1)),
					CreatePrefixUnaryOperator(firstElement.Value));
		}

		public static IUnifiedExpression CreatePrimary(XElement node) {
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "primary");
			/*
			 * primary 
			 * :   parExpression            
			 * |   'this' ('.' IDENTIFIER)* (identifierSuffix)?
			 * |   IDENTIFIER ('.' IDENTIFIER)* (identifierSuffix)?
			 * |   'super' superSuffix									// Outer.super()
			 * |   literal
			 * |   creator
			 * |   primitiveType ('[' ']')* '.' 'class'
			 * |   'void' '.' 'class'
			 */
			var first = node.FirstElement();
			if (first.HasContent("this") || first.Name() == "IDENTIFIER") {
				var variable = UnifiedIdentifier.CreateUnknown(first.Value);
				var prop = first.NextElements("IDENTIFIER")
						.Aggregate((IUnifiedExpression)variable,
								(e, v) => UnifiedProperty.Create(e, v.Value, "."));
				return CreateIdentifierSuffix(node.Element("identifierSuffix"), prop);
			}
			if (first.HasContent("super")) {
				var super = UnifiedIdentifier.CreateUnknown("super");
				return CreateSuperSuffix(node.Element("superSuffix"), super);
			}
			if (first.Name() == "literal") {
				return CreateLiteral(first);
			}
			if (first.Name() == "creator") {
				return CreateCreator(first);
			}
			if (first.Name() == "primitiveType") {
				var type = node.Elements()
						.Take(node.Elements().Count() - 2)
						.Aggregate("", (s, e) => s + e.Value);
				return UnifiedProperty.Create(UnifiedType.CreateUsingString(type), "class",
						".");
			}
			if (first.HasContent("void")) {
				return UnifiedProperty.Create(UnifiedIdentifier.CreateUnknown(first.Value),
						"class", ".");
			}
			throw new InvalidOperationException();
		}

		private static IUnifiedExpression CreateCreator(XElement first) {
			throw new NotImplementedException();
		}

		public static IUnifiedExpression CreateSuperSuffix(XElement node,
		                                                   IUnifiedExpression prefix) {
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "superSuffix");
			/*
			 * superSuffix  
			 * :   arguments
			 * |   '.' (typeArguments)? IDENTIFIER (arguments)?		// Outer.super()
			 */
			if (node.FirstElement().Name == "arguments") {
				return UnifiedCall.Create(prefix, CreateArguments(node.FirstElement()));
			}
			throw new NotImplementedException();
		}

		public static IUnifiedExpression CreateIdentifierSuffix(XElement node,
		                                                        IUnifiedExpression
		                                                        		prefix) {
			Contract.Requires(node == null || node.Name() == "identifierSuffix");
			/*
			 * identifierSuffix
			 * :   ('[' ']')+ '.' 'class'	// java.lang.String[].class
			 * |   ('[' expression ']' )+	// strs[10]
			 * |   arguments				// func(1, 2)
			 * |   '.' 'class'				// java.lang.String.class
			 *			// this.<Integer>m(1), super.<Integer>m(1)
			 * |   '.' nonWildcardTypeArguments IDENTIFIER arguments
			 * |   '.' 'this'				// Outer.this
			 * |   '.' 'super' arguments	// new Outer().super();
			 * |   innerCreator				// new Outer().new <Integer> Inner<String>(1);
			 */

			if (node == null) {
				return prefix;
			}
			var second = node.NthElementOrDefault(1);
			if (second != null && second.Name() == "expression") {
				return node.Elements("expression")
						.Select(CreateExpression)
						.Aggregate(prefix, (current, exp) =>
						                   UnifiedIndexer.Create(current,
						                   		UnifiedArgumentCollection.Create(
						                   				UnifiedArgument.Create(exp)))
						);
			}
			if (node.FirstElement().Name() == "arguments") {
				return UnifiedCall.Create(prefix, CreateArguments(node.FirstElement()));
			}
			throw new NotImplementedException();
		}

		private static UnifiedArgumentCollection CreateArguments(XElement node) {
			Contract.Requires(node.Name() == "arguments");
			/*
			 * arguments : '(' (expressionList)? ')'
			 */

			var expressionListNode = node.Element("expressionList");
			if (expressionListNode == null)
				return UnifiedArgumentCollection.Create();

			var args = CreateExpressionList(expressionListNode)
					.Select(e => {
						e.Remove();
						return UnifiedArgument.Create(e);
					});
			return UnifiedArgumentCollection.Create(args);
		}

		public static UnifiedExpressionList CreateExpressionList(XElement node) {
			Contract.Requires(node.Name() == "expressionList");
			/*
			 * expressionList : expression (',' expression )* ;
			 */

			var expressions = node.Elements("expression")
					.Select(CreateExpression);
			return expressions.ToExpressionList();
		}

		/* expressionList
		 * :   expression (',' expression)*
		 */

		/*
		 * innerCreator  
		 * :   '.' 'new' (nonWildcardTypeArguments)? IDENTIFIER (typeArguments)? classCreatorRest
		 */

		public static IUnifiedExpression CreateNew(XElement node) {
			Contract.Requires(node != null);
			Contract.Requires(node.Name().ToLower().EndsWith("creator"));
			/* 
			 * creator 
				:   'new' nonWildcardTypeArguments classOrInterfaceType classCreatorRest
				|   'new' classOrInterfaceType classCreatorRest
				|   arrayCreator

			   arrayCreator 
				:   'new' createdName
					'[' ']'
					( '[' ']' )*
					arrayInitializer
			    |   'new' createdName
					'[' expression ']'
					( '[' expression ']' )*
					( '[' ']' )*
			*/

			if (node.Name() == "creator") {
				if (
						node.Element("classCreatorRest").Element("arguments").Element(
								"expressionList") == null)
					return
							UnifiedNew.Create(
									CreateClassOrInterfaceType(node.Element("classOrInterfaceType")),
									UnifiedArgumentCollection.Create());

				return
						UnifiedNew.Create(
								CreateClassOrInterfaceType(node.Element("classOrInterfaceType")),
								UnifiedArgumentCollection.Create(node.Element("classCreatorRest")
										.Element("arguments")
										.Element("expressionList")
										.Elements("expression")
										.Select(CreateArgument))
								);
			} else {
				//case "arrayCreator"
				UnifiedExpressionList initialValues = null;
				UnifiedArgumentCollection args = null;
				if (node.HasContent("arrayInitializer")) {
					initialValues = node.Element("arrayInitializer")
							.Elements("variableInitializer")
							.Select(e => CreateExpression(e.Element("expression")))
							.ToExpressionList();
				}
				var type = CreateTypeOrCreatedName(node.Element("createdName"));
				foreach (var e in node.ElementsByContent("[")) {
					if (e.NextElement().Name() == "expression") {
						var expression = CreateExpression(e.NextElement());
						type.AddSupplement(
								UnifiedTypeSupplement.CreateArray(expression.ToArgument()));
					} else {
						type.AddSupplement(UnifiedTypeSupplement.CreateArray());
					}
				}
				return UnifiedNew.Create(type, args, initialValues);
			}
		}

		public static UnifiedIdentifier CreateVariable(XElement node) {
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "primary" || node.Name() == "literal");
			return UnifiedIdentifier.CreateUnknown(node.Value);
		}

		public static UnifiedLiteral CreateLiteral(XElement node) {
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "primary" || node.Name() == "literal");

			int i;
			if (Int32.TryParse(node.Value, NumberStyles.Any, null, out i)) {
				return UnifiedIntegerLiteral.Create(i);
			}

			decimal d;
			if (Decimal.TryParse(node.Value, NumberStyles.Any, null, out d)) {
				return UnifiedDecimalLiteral.Create(d);
			}

			var regex = new Regex(@"^""[a-zA-Z0-9_]*""$");
			if (regex.IsMatch(node.Value)) {
				var r = new Regex(@"[a-zA-Z_]{1}[a-zA-Z0-9_]*");
				var match = r.Match(node.Value);
				return UnifiedStringLiteral.Create(match.Value);
			}

			throw new NotImplementedException();
		}

		#endregion

		#region Operator

		public static UnifiedBinaryOperator CreateBinaryOperator(XElement node) {
			Contract.Requires(node != null);

			var name = node.Value;
			UnifiedBinaryOperatorKind kind;

			switch (name) {
				//Arithmetic
			case "+":
				kind = UnifiedBinaryOperatorKind.Add;
				break;
			case "-":
				kind = UnifiedBinaryOperatorKind.Subtract;
				break;
			case "*":
				kind = UnifiedBinaryOperatorKind.Multiply;
				break;
			case "/":
				kind = UnifiedBinaryOperatorKind.Divide;
				break;
			case "%":
				kind = UnifiedBinaryOperatorKind.Modulo;
				break;
				//Shift
			case "<<":
				kind = UnifiedBinaryOperatorKind.ArithmeticLeftShift;
				break;
			case ">>":
				kind = UnifiedBinaryOperatorKind.ArithmeticRightShift;
				break;
				//Comparison
			case ">":
				kind = UnifiedBinaryOperatorKind.GreaterThan;
				break;
			case ">=":
				kind = UnifiedBinaryOperatorKind.GreaterThanOrEqual;
				break;
			case "<":
				kind = UnifiedBinaryOperatorKind.LessThan;
				break;
			case "<=":
				kind = UnifiedBinaryOperatorKind.LessThanOrEqual;
				break;
			case "==":
				kind = UnifiedBinaryOperatorKind.Equal;
				break;
			case "!=":
				kind = UnifiedBinaryOperatorKind.NotEqual;
				break;
				//Logocal
			case "&&":
				kind = UnifiedBinaryOperatorKind.AndAlso;
				break;
			case "||":
				kind = UnifiedBinaryOperatorKind.OrElse;
				break;
				//Bit
			case "&":
				kind = UnifiedBinaryOperatorKind.And;
				break;
			case "|":
				kind = UnifiedBinaryOperatorKind.Or;
				break;
			case "^":
				kind = UnifiedBinaryOperatorKind.ExclusiveOr;
				break;
				//Assignment
			case "=":
				kind = UnifiedBinaryOperatorKind.Assign;
				break;
			case "+=":
				kind = UnifiedBinaryOperatorKind.AddAssign;
				break;
			case "-=":
				kind = UnifiedBinaryOperatorKind.SubtractAssign;
				break;
			case "*=":
				kind = UnifiedBinaryOperatorKind.MultiplyAssign;
				break;
			case "/=":
				kind = UnifiedBinaryOperatorKind.DivideAssign;
				break;
			case "%=":
				kind = UnifiedBinaryOperatorKind.ModuloAssign;
				break;
			default:
				throw new InvalidOperationException();
			}
			return UnifiedBinaryOperator.Create(name, kind);
		}

		public static UnifiedUnaryOperator CreatePrefixUnaryOperator(string name) {
			Contract.Requires(name != null);
			UnifiedUnaryOperatorKind kind;
			switch (name) {
			case "+":
				kind = UnifiedUnaryOperatorKind.UnaryPlus;
				break;
			case "-":
				kind = UnifiedUnaryOperatorKind.Negate;
				break;
			case "++":
				kind = UnifiedUnaryOperatorKind.PreIncrementAssign;
				break;
			case "--":
				kind = UnifiedUnaryOperatorKind.PreDecrementAssign;
				break;
			case "~":
				kind = UnifiedUnaryOperatorKind.OnesComplement;
				break;
			case "!":
				kind = UnifiedUnaryOperatorKind.Not;
				break;
			default:
				throw new InvalidOperationException();
			}
			return UnifiedUnaryOperator.Create(name, kind);
		}

		#endregion

		#region Statement

		public static IUnifiedExpression CreateStatement(XElement node) {
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "statement");
			/*
			 * statement 
			 * :   block
			 * |   ('assert') expression (':' expression)? ';'
			 * |   'assert'  expression (':' expression)? ';'            
			 * |   'if' parExpression statement ('else' statement)?          
			 * |   forstatement
			 * |   'while' parExpression statement
			 * |   'do' statement 'while' parExpression ';'
			 * |   trystatement
			 * |   'switch' parExpression '{' switchBlockStatementGroups '}'
			 * |   'synchronized' parExpression block
			 * |   'return' (expression )? ';'
			 * |   'throw' expression ';'
			 * |   'break' (IDENTIFIER )? ';'
			 * |   'continue' (IDENTIFIER)? ';'
			 * |   expression  ';'     
			 * |   IDENTIFIER ':' statement
			 * |   ';'
			 * ;*/
			var element = node.FirstElement();

			switch (element.Name()) {
			case "block":
				return CreateBlock(element);
			case "IF":
				return CreateIf(node);
			case "forstatement":
				return CreateForstatement(element);
			case "WHILE":
				return CreateWhile(node);
			case "DO":
				return CreateDo(node);
			case "SWITCH":
				return CreateSwitch(node);
			case "RETURN":
				return CreateReturn(node);
			case "BREAK":
				return CreateBreak(node);
			case "CONTINUE":
				return CreateContinue(node);
			case "expression":
				return CreateExpression(element);
			default:
				throw new NotImplementedException();
			}
		}

		public static UnifiedBlock CreateBlock(XElement node) {
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "block");
			/*
			 * block : '{' (blockStatement )* '}'
			 */

			var list = new List<IUnifiedExpression>();

			foreach (var e in node.Elements()) {
				if (e.Name() == "blockStatement") {
					list.Add(CreateBlockStatement(e));
				}
			}
			return UnifiedBlock.Create(list);
		}

		public static IUnifiedExpression CreateBlockStatement(XElement node) {
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "blockStatement");
			/*  blockStatement
			 * :   localVariableDeclarationStatement
			 * |   classOrInterfaceDeclaration
			 * |   statement
			 */
			var e = node.Elements().First();
			switch (e.Name()) {
			case "statement":
				return CreateStatement(e);
			case "localVariableDeclarationStatement":
				return CreateLocalVariableDeclarationStatement(e);
			case "classOrInterfaceDeclaration":
				throw new NotImplementedException();
			default:
				throw new InvalidOperationException();
			}
		}

		public static UnifiedIf CreateIf(XElement node) {
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "statement");
			Contract.Requires(node.Elements().First().Name() == "IF");
			/*  'if' parExpression statement ('else' statement)? */
			var trueBody = UnifiedBlock.Create(
					CreateStatement(node.Element("statement")));

			UnifiedBlock falseBody = null;
			if (node.Elements("statement").Count() == 2) {
				var falseNode = node.Elements("statement").ElementAt(1);
				falseBody = UnifiedBlock.Create(
						CreateStatement(falseNode));
			}
			return UnifiedIf.Create(CreateExpression(node
					.Element("parExpression")
					.Element("expression")),
					trueBody, falseBody);
		}

		public static UnifiedWhile CreateWhile(XElement node) {
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "statement");
			Contract.Requires(node.Elements().First().Name() == "WHILE");
			/* 'while' parExpression statement */
			return UnifiedWhile.Create(
					UnifiedBlock.Create(
							CreateStatement(node.Element("statement"))
							),
					CreateExpression(node.Element("parExpression").Element("expression"))
					);
		}

		public static UnifiedDoWhile CreateDo(XElement node) {
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "statement");
			Contract.Requires(node.Elements().First().Name() == "DO");
			/* 'do' statement 'while' parExpression ';' */
			return UnifiedDoWhile.Create(
					UnifiedBlock.Create(
							CreateStatement(node.Element("statement"))
							),
					CreateExpression(node.Element("parExpression").Element("expression"))
					);
		}

		public static IUnifiedExpression CreateForstatement(XElement forstatement) {
			Contract.Requires(forstatement != null);
			Contract.Requires(forstatement.Name() == "forstatement");
			/*	forstatement :   
			 * // enhanced for loop
			 *     'for' '(' variableModifiers type IDENTIFIER ':' expression ')' statement
			 * // normal for loop
			 * |   'for' '(' (forInit)? ';' (expression)? ';' (expressionList)? ')' statement
			 * ; */
			if (forstatement.NthElement(2).Name() == "variableModifiers") {
				return UnifiedForeach.Create(
						UnifiedVariableDefinition.CreateSingle(
								CreateTypeOrCreatedName(forstatement.Element("type")),
								CreateVariableModifiers(forstatement.Element("variableModifiers")),
								null,
								forstatement.Element("IDENTIFIER").Value
								),
						CreateExpression(forstatement.Element("expression")),
						UnifiedBlock.Create(
								CreateStatement(forstatement.Element("statement"))
								)
						);
			} else {
				var forInit = forstatement.Element("forInit");
				var initializer = forInit != null ? CreateForInit(forInit) : null;

				var expression = forstatement.Element("expression");
				var condition = expression != null ? CreateExpression(expression) : null;

				var expressionList = forstatement.Element("expressionList");
				var step = expressionList != null
				           		? CreateExpressionList(expressionList) : null;

				var body = CreateStatement(forstatement.Element("statement"));
				return UnifiedFor.Create(
						initializer,
						condition,
						step,
						UnifiedBlock.Create(body)
						);
			}
		}

		public static UnifiedSwitch CreateSwitch(XElement node) {
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "statement");
			Contract.Requires(node.HasElementByContent("switch"));
			/* 'switch' parExpression '{' switchBlockStatementGroups '}' */
			return
					UnifiedSwitch.Create(
							CreateExpression(node.Element("parExpression").Element("expression")),
							CreateCaseCollection(node.Element("switchBlockStatementGroups")));
		}

		public static UnifiedCaseCollection CreateCaseCollection(XElement node) {
			//Top node is <switchBlockStatementGroups>.
			return
					UnifiedCaseCollection.Create(
							node.Elements("switchBlockStatementGroup").Select(CreateCase));
		}

		public static UnifiedCase CreateCase(XElement node) {
			//Top node is <switchBlockStatementGroup>.
			var cond = node.Element("switchLabel").Element("expression");
			var body = UnifiedBlock.Create(
					CreateBlockStatement(node.Element("blockStatement"))
					);
			//var body = CreateBlock(node);
			if (cond == null) {
				return UnifiedCase.Create(null, body);
			}
			return UnifiedCase.Create(CreateExpression(cond), body);
		}

		public static UnifiedSpecialExpression CreateBreak(XElement node) {
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "statement");
			Contract.Requires(node.HasElementByContent("break"));
			/* 'break' (IDENTIFIER )? ';' */
			if (node.Elements().Count() > 2)
				throw new NotImplementedException();
			return UnifiedSpecialExpression.CreateBreak();
		}

		public static UnifiedSpecialExpression CreateContinue(XElement node) {
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "statement");
			Contract.Requires(node.HasElementByContent("continue"));
			/* 'continue' (IDENTIFIER)? ';' */
			if (node.Elements().Count() > 2)
				throw new NotImplementedException();
			return UnifiedSpecialExpression.CreateContinue();
		}

		private static IUnifiedExpression CreateForInit(XElement node) {
			Contract.Requires(node.Name() == "forInit");
			/* forInit : localVariableDeclaration | expressionList ;
		 */
			switch (node.FirstElement().Name()) {
			case "localVariableDeclaration":
				return CreateLocalVariableDeclarationOrFieldDeclaration(node.FirstElement());
			case "expressionList":
				return CreateExpressionList(node.FirstElement());
			}
			throw new InvalidOperationException();
		}

		private static UnifiedModifierCollection CreateVariableModifiers(
				XElement xElement) {
			/*
			 * variableModifiers : ( 'final' | annotation )* ;
			 */
			if (xElement.Elements().Count() == 0)
				return UnifiedModifierCollection.Create();
			else throw new NotImplementedException();
		}

		public static UnifiedSpecialExpression CreateReturn(XElement node) {
			Contract.Requires(node != null);
			IUnifiedExpression value = null;
			var i = node.Elements().Count();
			if (node.Elements().Count() == 3) {
				value = CreateExpression(node.Element("expression"));
			}
			return UnifiedSpecialExpression.CreateReturn(value);
		}

		#endregion

		#region Function

		/*
		 * methodDeclaration
		 * :
		 * // Constructor
		 *   modifiers (typeParameters)? IDENTIFIER formalParameters ('throws' qualifiedNameList)? '{' (explicitConstructorInvocation)? (blockStatement)* '}'
		 * // Method
		 * | modifiers (typeParameters)? (type | 'void') IDENTIFIER formalParameters ('[' ']')* ('throws' qualifiedNameList)? (block | ';');
		 * 
		 */

		public static IUnifiedExpression CreateDefineFunction(XElement node) {
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "methodDeclaration");

			if (node.Element("type") == null && node.Element("VOID") == null) {
				//case Constructor
				return UnifiedConstructorDefinition.Create(
						UnifiedBlock.Create(
								CreateBlockStatement(node.Element("blockStatement"))
								),
						CreateModifierCollection(node),
						CreateFormalParameters(node.Element("formalParameters"))
						);
			}
			//if (node.Element("IDENTIFIER").PreviousElement().Name() == "")
			return UnifiedFunctionDefinition.CreateFunction(CreateModifierCollection(node),
					CreateTypeOrCreatedName(node.Element("type")),
					node.Element("IDENTIFIER").Value, CreateFormalParameters(node.Element("formalParameters")), CreateBlock(node.Element("block")));
		}

		public static UnifiedModifier CreateVariableModifier(XElement node) {
			Contract.Requires(node != null);
			return UnifiedModifier.Create(node.Value);
		}

		public static UnifiedModifier CreateModifier(XElement node) {
			Contract.Requires(node != null);
			return UnifiedModifier.Create(node.Value);
		}

		public static UnifiedModifierCollection CreateModifierCollection(XElement node) {
			Contract.Requires(node != null);
			return UnifiedModifierCollection.Create(node
					.Element("modifiers")
					.Elements()
					.Select(CreateModifier));
		}

		public static UnifiedType CreateTypeOrCreatedName(XElement node) {
			Contract.Requires(node == null || node.Name() == "type" ||
			                  node.Name() == "createdName");
			/* 
			 * type 
				:   classOrInterfaceType ('[' ']')*
				|   primitiveType ('[' ']')*
			*/

			if (node == null)
				return UnifiedType.CreateUsingString("void");

			var firstNode = node.FirstElement();
			UnifiedType result;
			switch (firstNode.Name()) {
			case "classOrInterfaceType":
				result = CreateClassOrInterfaceType(firstNode);
				break;
			case "primitiveType":
				result = CreatePrimitiveType(firstNode);
				break;
			default:
				throw new InvalidOperationException();
			}

			foreach (var e in node.ElementsByContent("[")) {
				result.AddSupplement(UnifiedTypeSupplement.CreateArray());
			}
			return result;
		}

		public static UnifiedType CreateClassOrInterfaceType(XElement node) {
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "classOrInterfaceType");
			/* 
			 * classOrInterfaceType 
				:   IDENTIFIER (typeArguments)? ('.' IDENTIFIER (typeArguments)? )*
			  
			   typeArguments 
			 * :   '<' typeArgument (',' typeArgument )* '>'
			 */

			//-> とりあえず、CreateClassOrInterfaceType()の中で親の値に遡って代入する

			var name = node.Element("IDENTIFIER").Value;
			if (node.HasElement("typeArguments")) {
				return UnifiedType.CreateUsingString(name,
						UnifiedTypeArgumentCollection.Create(
								node.Element("typeArguments")
										.Elements("typeArgument")
										.Select(CreatTypeParameter)));
			}
			return UnifiedType.CreateUsingString(name);
		}

		public static UnifiedType CreatePrimitiveType(XElement node) {
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "primitiveType");
			/*
			 * primitiveType  
				:   'boolean' | 'char' | 'byte' | 'short' | 'int' | 'long' | 'float' | 'double' 
			 */
			return UnifiedType.CreateUsingString(node.Value);
		}

		public static UnifiedTypeArgument CreatTypeParameter(XElement node) {
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "typeArgument");
			/* 
			 * typeArgument 
				:   type
				|   '?' ( ('extends'|'super' ) type )?
			 */
			if (node.FirstElement().Name() == "type") {
				return
						UnifiedTypeArgument.Create(CreateTypeOrCreatedName(node.Element("type")),
								null);
			}
			throw new NotImplementedException();

			var modifier = node.NthElement(1) != null
			               		? UnifiedModifierCollection.Create() : null;
			var type = node.NthElement(2) != null
			           		? CreateTypeOrCreatedName(node.Element("type")) : null;
			return UnifiedTypeArgument.Create(type, modifier);
		}

		public static UnifiedParameterCollection CreateFormalParameters(XElement node) {
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "formalParameters");
			var element = node.Element("formalParameterDecls");
			if (element == null)
				return UnifiedParameterCollection.Create();

			return UnifiedParameterCollection.Create(element
					.Elements()
					.Select(e => {
						if (e.Name() == "normalParameterDecl")
							return CreateNormalParameterDecl(e);
						if (e.Name() == "ellipsisParameterDecl")
							return CreateEllipsisParameterDecl(e);
						return null;
					})
					.Where(e => e != null));
		}

		public static UnifiedParameter CreateNormalParameterDecl(XElement node) {
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "normalParameterDecl");
			/* 
			 * normalParameterDecl 
				:   variableModifiers type IDENTIFIER ('[' ']')* 
			 */
			return UnifiedParameter.Create(
					node.Element("IDENTIFIER").Value,
					CreateTypeOrCreatedName(node.Element("type")),
					UnifiedModifierCollection.Create(node
							.Element("variableModifiers")
							.Elements()
							.Select(CreateVariableModifier)));
		}

		public static UnifiedParameter CreateEllipsisParameterDecl(XElement node) {
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "ellipsisParameterDecl");
			throw new NotImplementedException();
		}

		public static UnifiedArgument CreateArgument(XElement node) {
			Contract.Requires(node != null);
			return UnifiedArgument.Create(CreateExpression(node));
		}

		public static UnifiedArgumentCollection CreateArgumentCollection(XElement node) {
			Contract.Requires(node != null);
			var element = node
					.Element("identifierSuffix")
					.Element("arguments")
					.Element("expressionList")
					.Elements()
					.Select(CreateArgument);
			return UnifiedArgumentCollection.Create(element);
		}

		#endregion

		public static UnifiedVariableDefinition
				CreateLocalVariableDeclarationStatement(XElement node) {
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "localVariableDeclarationStatement");
			return
					CreateLocalVariableDeclarationOrFieldDeclaration(
							node.Element("localVariableDeclaration"));
		}

		public static UnifiedVariableDefinition
				CreateLocalVariableDeclarationOrFieldDeclaration(XElement node) {
			Contract.Requires(node.Name() == "localVariableDeclaration" ||
			                  node.Name() == "fieldDeclaration");
			/* localVariableDeclaration 
			 *   :   variableModifiers type variableDeclarator (',' variableDeclarator )* ;
			 */

			/*
			 * fieldDeclaration 
			 *   :   modifiers type variableDeclarator (',' variableDeclarator )* ';' 
			 */

			var init =
					node.Element("variableDeclarator").Element("variableInitializer") != null
							? CreateExpression(node.Element("variableDeclarator")
							  		.Element("variableInitializer")
							  		.Element("expression")) : null;
			return UnifiedVariableDefinition.CreateSingle(
					/*InitialValue = CreateExpression(
						node.Element("variableDeclarator")
						.Element("variableInitializer")
						.Element("expression")), */
					CreateTypeOrCreatedName(node.Element("type")),
					UnifiedModifierCollection.Create(
							node.ElementAt(0)
									.Elements()
									.Select(CreateVariableModifier)),
					init,
					node.Element("variableDeclarator").Element("IDENTIFIER").Value
					);
		}

		public static UnifiedBooleanLiteral CreateBooleanLiteral(XElement node) {
			Contract.Requires(node != null);
			Contract.Requires(node.Elements().First().Value == "true" ||
			                  node.Elements().First().Value == "false");
			var tokenNode = node.FirstElement();
			return UnifiedBooleanLiteral.Create(tokenNode.Value == "true");
		}

		public static UnifiedClassDefinition CreateClassDeclaration(XElement node) {
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "classDeclaration");
			/*
			 * classDeclaration 
			    :   normalClassDeclaration
				|   enumDeclaration 
			 */
			if (node.HasElement("normalClassDeclaration")) {
				//var modifiers = CreateModifierCollection(node);
				var name = node
						.Element("normalClassDeclaration")
						.Element("IDENTIFIER")
						.Value;
				var body = CreateClassBody(node
						.Element("normalClassDeclaration")
						.Element("classBody"));
				return UnifiedClassDefinition.CreateClass(name, body);
			}
			throw new NotImplementedException();
		}

		public static UnifiedBlock CreateClassBody(XElement node) {
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "classBody");
			/*
			 * classBody 
			    :   '{' (classBodyDeclaration)* '}' 
			 */
			return UnifiedBlock.Create(node
					.Elements("classBodyDeclaration")
					.Select(CreateClassBodyDeclaration));
		}

		public static IUnifiedExpression CreateClassBodyDeclaration(XElement node) {
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "classBodyDeclaration");
			/*
			 * memberDecl 
				:    fieldDeclaration
				|    methodDeclaration
				|    classDeclaration
				|    interfaceDeclaration 
			 */
			var memType = node.Element("memberDecl").FirstElement();
			switch (memType.Name()) {
			case "fieldDeclaration":
				return CreateLocalVariableDeclarationOrFieldDeclaration(memType);
			case "methodDeclaration":
				return CreateDefineFunction(memType);
			case "classDeclaration":
				return CreateClassDeclaration(memType);
			default:
				throw new NotImplementedException();
			}
		}

		public static UnifiedProgram CreateProgram(XElement node) {
			Contract.Requires(node != null);
			var model = UnifiedProgram.Create(
					CreateClassDeclaration(node
							.Element("typeDeclaration")
							.Element("classOrInterfaceDeclaration")
							.Element("classDeclaration"))
					);
			model.Normalize();
			return model;
		}

		public static UnifiedProgram CreateModel(string source) {
			Contract.Requires(source != null);
			var ast = JavaXmlGenerator.Instance.Generate(source);
			return CreateProgram(ast);
		}
	}
}