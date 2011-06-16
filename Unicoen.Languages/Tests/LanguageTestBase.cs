﻿#region License

// Copyright (C) 2011 The Unicoen Project
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System.Collections.Generic;
using NUnit.Framework;

namespace Unicoen.Languages.Tests {
	public abstract class LanguageTestBase {
		/// <summary>
		///   テストフィクスチャを取得します．
		/// </summary>
		protected abstract Fixture Fixture { get; }

		/// <summary>
		///   テスト対象のソースコードの集合を取得します．
		/// </summary>
		public IEnumerable<TestCaseData> TestCodes {
			get { return Fixture.TestCodes; }
		}

		/// <summary>
		///   テスト対象のソースコードのパスの集合を取得します．
		/// </summary>
		public IEnumerable<TestCaseData> TestFilePathes {
			get { return Fixture.TestFilePathes; }
		}

		/// <summary>
		///   テスト対象のプロジェクトのパスとコンパイルのコマンドと引数の集合を取得します．
		/// </summary>
		public IEnumerable<TestCaseData> TestProjectInfos {
			get { return Fixture.TestProjectInfos; }
		}
	}
}