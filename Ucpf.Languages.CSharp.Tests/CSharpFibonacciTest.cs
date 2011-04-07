﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Ucpf.Core.Model;
using Ucpf.Core.Tests;

namespace Ucpf.Languages.CSharp.Tests {

	[TestFixture]
	public class CSharpFibonacciTest {

		#region data

		private static readonly string Code =
			File.ReadAllText(Fixture.GetInputPath("CSharp", "Fibonacci.cs"));

		public static readonly UnifiedProgram Model = new UnifiedProgram {
				UnifiedClassDefinition.Create(
						"Fibonacci",
						UnifiedBlock.Create(new IUnifiedExpression[] {
								UnifiedFunctionDefinition.Create(
										"fibonacci",
										UnifiedType.Create("int"),
										UnifiedModifierCollection.Create(
												UnifiedModifier.Create("public"),
												UnifiedModifier.Create("static")
												),
										UnifiedParameterCollection.Create(
												UnifiedParameter.Create("n", UnifiedType.Create("int"))
												),
										UnifiedBlock.Create(
												UnifiedIf.Create(
														UnifiedBinaryExpression.Create(UnifiedVariable.Create("n"),
																UnifiedBinaryOperator.Create("<",
																		UnifiedBinaryOperatorType.LessThan),
																UnifiedIntegerLiteral.Create(2)),
														UnifiedBlock.Create(new[] {
																UnifiedJump.CreateReturn(UnifiedVariable.Create("n")),
														}),
														UnifiedBlock.Create(
																UnifiedJump.CreateReturn(
																		UnifiedBinaryExpression.Create(
																				UnifiedCall.Create(UnifiedVariable.Create("fibonacci"),
																						UnifiedArgumentCollection.Create(
																								UnifiedArgument.Create(UnifiedBinaryExpression.Create(
																												UnifiedVariable.Create("n"),
																												UnifiedBinaryOperator.Create("-",
																														UnifiedBinaryOperatorType.Subtract),
																												UnifiedIntegerLiteral.Create(1)
																												)))
																						),
																				UnifiedBinaryOperator.Create("+",
																						UnifiedBinaryOperatorType.Add),
																				UnifiedCall.Create(UnifiedVariable.Create("fibonacci"),
																						UnifiedArgumentCollection.Create(
																								UnifiedArgument.Create(
																										UnifiedBinaryExpression.Create(
																												UnifiedVariable.Create("n"),
																												UnifiedBinaryOperator.Create("-",
																														UnifiedBinaryOperatorType.Subtract),
																												UnifiedIntegerLiteral.Create(2)))
																								)
																						)
																				)
																		)
																)
														)
												)
										)
						}
								)
						)
		};

		#endregion

		[Test]
		public void CreateFibonacci() {
			var actual = CSharpModelFactory.CreateModel(Code);
			var expected = Model;
			Assert.That(actual,
				Is.EqualTo(expected).Using(StructuralEqualityComparerForDebug.Instance));
		}

		[Test]
		public void CreateClassDeclare() {
			const string code = "class Fibonacci{}";
			var expected = new UnifiedProgram(
				new[]{ UnifiedClassDefinition.Create("Fibonacci")});
			var actual = CSharpModelFactory.CreateModel(code);
			Assert.That(actual,
				Is.EqualTo(expected).Using(StructuralEqualityComparerForDebug.Instance));
		}

		[Test]
		public void CreateFuncDec() {
			const string code =
				@"
class Fibonacci {
	public static void fibonacci(int n) {
	}
}
";
			var expected = new UnifiedProgram(new[]{ UnifiedClassDefinition.Create(
				"Fibonacci",
				UnifiedBlock.Create(new IUnifiedExpression[] {
					UnifiedFunctionDefinition.Create(
						"fibonacci",
						UnifiedType.Create("void"),
						UnifiedModifierCollection.Create(
							UnifiedModifier.Create("public"),
							UnifiedModifier.Create("static")
						),
						UnifiedParameterCollection.Create(
							UnifiedParameter.Create(
								"n",
								UnifiedType.Create("int"))
						)
					)
				})
			)});
			var actual = CSharpModelFactory.CreateModel(code);
			Assert.That(actual,
				Is.EqualTo(expected).Using(StructuralEqualityComparerForDebug.Instance));
		}
	}
}