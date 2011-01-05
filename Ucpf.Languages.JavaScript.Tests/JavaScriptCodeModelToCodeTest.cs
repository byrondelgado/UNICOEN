﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Ucpf.Common.Tests;
using Ucpf.Languages.JavaScript.CodeModel;

namespace Ucpf.Languages.JavaScript.Tests
{
	[TestFixture]
	public class JavaScriptCodeModelToCodeTest
	{
		private JSCodeModelToCode _generator;
		private StringWriter _writer;
		private JSFunctionDeclaration _func;
		private static readonly string InputPath =
			Path.Combine(Settings.GetInputDirPath("JavaScript"), "fibonacci.js");
		

		[SetUp]
		public void SetUp() 
		{
			_writer = new StringWriter();
			_generator = new JSCodeModelToCode(_writer, 0);

			var ast = JavaScriptAstGenerator.Instance.GenerateFromFile(InputPath);
            var root = ast.Descendants("functionDeclaration").First();
            _func = new JSFunctionDeclaration(root);
		}

		[Test]
		public void 関数が正しくコードに変換できる() 
		{
			_generator.Generate(_func);
			Console.Write(_writer.ToString());
			//Assert.That(_writer.ToString(), Is.EqualTo("hoge"));
		}

	}
}
