using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Unicoen.Languages.Java;
using Unicoen.Model;
using Unicoen.Tests;

namespace Unicoen.Apps.RefactoringDSL.Sandbox {
	public class CollisionDetectorTest
	{
		private UnifiedProgram _model;
		[SetUp]
		public void SetUp()
		{
            var inputPath = FixtureUtil.GetInputPath("Java", "default", "Collision.java");
            var code = File.ReadAllText(inputPath, Encoding.Default);
            _model = JavaFactory.GenerateModel(code);
		}

		[Test]
		public void 関数の衝突を検知できる()
		{
			
		}

		[Test]
		public void 変数の衝突を検知できる()
		{
			
		}
	}
}
