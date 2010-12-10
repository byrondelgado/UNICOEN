﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using NUnit.Framework;

namespace Ucpf.Languages.C.Tests
{
	public class CCodeModelTest_Fact
	{
		CFunction func_fact = new CFunction(
			CAstGenerator.Instance.GenerateFromFile("fact.c")
			.Descendants("function_definition")
			.First());

		
		[Test]
		public void メソッド返却値タイプが正しい()
		{
			Assert.That(func_fact.ReturnType.Name, Is.EqualTo("int"));
		}
		[Test]
		public void メソッド名が正しい()
		{
			Assert.That(func_fact.Name, Is.EqualTo("fact"));
		}

		// test whether the first parameter equals to (int, "n")
		[Test]
		public void パラメータリストが正しい()
		{
			Assert.That(func_fact.Parameters.ElementAt(0).Name, Is.EqualTo("n"));
			Assert.That(func_fact.Parameters.ElementAt(0).Type.Name, Is.EqualTo("int"));
		}

		[Test]
		public void IF文の条件式が正しい()
		{
			CIfStatement ifStatement = (CIfStatement)func_fact.Body.Statements.ElementAt(0);
			CBinaryExpression conditionalExpression = (CBinaryExpression)ifStatement.ConditionalExpression;
			Assert.That(ifStatement is CIfStatement);
			Assert.That(conditionalExpression is CBinaryExpression);
			CExpression leftExp = conditionalExpression.LeftExpression;
			CExpression rightExp = conditionalExpression.RightExpression;
			COperator ope = conditionalExpression.Operator;
			
			Assert.That(ope.ToString(), Is.EqualTo("=="));
			Assert.That(leftExp.ToString(), Is.EqualTo("n"));
			Assert.That(rightExp.ToString(), Is.EqualTo("1"));
			Assert.That(conditionalExpression.ToString(), Is.EqualTo("n==1"));

		}

		[Test]
		public void TrueBlockが正しく生成できる()
		{
			var firstStatement = ((CIfStatement)(func_fact.Body.Statements.ElementAt(0)))
				.TrueBlock
				.Statements
				.ElementAt(0);
			Assert.That(firstStatement is CReturnStatement);		// assert type
			Assert.That(firstStatement.Expressions.ElementAt(0).ToString(), Is.EqualTo("1"));		// assert body
		}

		[Test]
		public void ElseBlockが正しく生成できる()
		{
			var firstStatemenet = ((CIfStatement)(func_fact.Body.Statements.ElementAt(0)))
				.ElseBlock
				.Statements
				.ElementAt(0);
			CBinaryExpression exp = (CBinaryExpression)firstStatemenet.Expressions.ElementAt(0);
			Assert.That(firstStatemenet is CReturnStatement);
			CExpression leftExpression = exp.LeftExpression;
			CExpression rightExpression = exp.RightExpression;
			COperator ope = exp.Operator;

			Assert.That(leftExpression is CPrimaryExpression);
			Assert.That(rightExpression is CInvocationExpression);

			Assert.That(leftExpression.ToString(), Is.EqualTo("n"));
			Assert.That(ope is CMultiOperator);

			var rightFuncName = ((CInvocationExpression)rightExpression).FunctionName;
			var rightArg = (CBinaryExpression)((CInvocationExpression)rightExpression).Arguments.ElementAt(0);

			Assert.That(rightFuncName, Is.EqualTo("fact"));
			Assert.That(rightArg.LeftExpression.ToString(), Is.EqualTo("n"));
			Assert.That(rightArg.Operator.ToString(), Is.EqualTo("-"));
			Assert.That(rightArg.RightExpression.ToString(), Is.EqualTo("1"));
			Assert.That(rightArg.RightExpression is CNumber);

			Assert.That(exp.ToString(), Is.EqualTo("n*fact(n-1)"));
		}

	}
}