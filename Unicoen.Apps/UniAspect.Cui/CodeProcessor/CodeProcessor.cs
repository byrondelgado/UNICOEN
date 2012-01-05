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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Code2Xml.Languages.C.CodeToXmls;
using Code2Xml.Languages.Java.CodeToXmls;
using Code2Xml.Languages.JavaScript.CodeToXmls;
using Unicoen.Languages.C.ProgramGenerators;
using Unicoen.Languages.Java.ProgramGenerators;
using Unicoen.Languages.JavaScript.ProgramGenerators;
using Unicoen.Model;
using Unicoen.Languages.C;
using Unicoen.Languages.CSharp;
using Unicoen.Languages.Java;
using Unicoen.Languages.JavaScript;
using Unicoen.Languages.Python2;

namespace Unicoen.Apps.UniAspect.Cui.CodeProcessor {
	/// <summary>
	///   アスペクト指向プログラミングに必要なソースコードの加工処理メソッドを保有します。
	/// </summary>
	public class CodeProcessor {
		/// <summary>
		///   与えられたソースコードを共通モデルに変換します
		/// </summary>
		/// <param name = "ext">対象言語の拡張子</param>
		/// <param name = "code">対象ソースコードの中身</param>
		/// <returns></returns>
		public static UnifiedProgram CreateModel(string ext, string code) {
			switch (ext.ToLower()) {
			case ".cs":
				return CSharpFactory.GenerateModel(code);
			case ".java":
				return JavaFactory.GenerateModel(code);
			case ".js":
				return JavaScriptFactory.GenerateModel(code);
			case ".c":
			case ".h":
				return CFactory.GenerateModel(code);
			case ".py":
				return Python2Factory.GenerateModel(code);
			}
			//Ruby
			throw new InvalidOperationException("対応していない言語ファイルが指定されました");
		}

		/// <summary>
		///   与えられたコードを共通コードモデルとして生成します。
		/// </summary>
		/// <param name = "language">対象言語</param>
		/// <param name = "code">コード断片</param>
		/// <returns></returns>
		public static UnifiedBlock CreateAdvice(string language, string code) {
			//generate model from string advice (as UnifiedBlock)
			XElement ast = null;
			UnifiedBlock actual = null;
			code = "{ " + code + " }";

			switch (language) {
			case "Java":
				ast = JavaCodeToXml.Instance.Generate(code, p => p.block());
				actual = JavaProgramGeneratorHelper.CreateBlock(ast);
				break;
			case "JavaScript":
				ast = JavaScriptCodeToXml.Instance.Generate(code, p => p.statementBlock());
				actual = JavaScriptProgramGeneratorHelper.CreateStatementBlock(ast);
				break;
			case "C":
				//TODO Cでのアスペクト合成はこれで大丈夫か確認
				ast = CCodeToXml.Instance.Generate(code, p => p.compound_statement());
				actual = CProgramGeneratorHelper.CreateCompoundStatement(ast);
				break;
			default:
				//CSharp, Ruby, Python
				throw new InvalidOperationException("対応していない言語が指定されました");
			}
			actual.Normalize();

			return actual;
		}

		/// <summary>
		///   与えられたコードをインタータイプ宣言のために共通コードモデルとして生成します
		/// </summary>
		/// <param name = "language">対象言語</param>
		/// <param name = "code">コード断片</param>
		/// <returns></returns>
		public static List<IUnifiedExpression> CreateIntertype(
				string language, string code) {
			XElement ast = null;
			var actual = new List<IUnifiedExpression>();

			switch (language) {
			case "Java":
				//classBodyとしてパースするために中括弧を補う
				code = "{ " + code + " }";
				ast = JavaCodeToXml.Instance.Generate(code, p => p.classBody());
				var classBody = JavaProgramGeneratorHelper.CreateClassBody(ast);
				foreach (var e in classBody) {
					var method = e as UnifiedFunctionDefinition;
					var field = e as UnifiedVariableDefinitionList;
					if (field != null)
						actual.Add(field);
					if (method != null)
						actual.Add(method);
				}
				break;

			case "JavaScript":
				ast = JavaScriptCodeToXml.Instance.Generate(code, p => p.program());
				var program = JavaScriptProgramGeneratorHelper.CreateProgram(ast);
				actual.AddRange(program.Body);
				break;
			default:
				//TODO implement 他の言語についても実装する
				throw new NotImplementedException();
			}
			return actual;
		}

		/// <summary>
		///   指定されたクラスまたはプログラムに指定されたフィールドやメソッドを追加します。
		/// </summary>
		/// <param name = "program">メンバーを追加するモデルのルートノード</param>
		/// <param name = "name">対象となるクラスやプログラムを指定する名前</param>
		/// <param name = "members">挿入するメンバーのリスト</param>
		public static void AddIntertypeDeclaration(
				UnifiedProgram program, string name, List<IUnifiedExpression> members) {
			//クラスのリストを取得(Java, C#向け)
			var classes = program.Descendants<UnifiedClassDefinition>();
			if (classes.Count() > 0) {
				foreach (var c in classes) {
					//クラス名を取得
					var className = c.Name as UnifiedVariableIdentifier;
					if(className == null)
						continue;

					//受け取った名前を正規表現に変換し、クラス名が一致する場合は合成する
					var regex = new Regex(name == "*" ? ".*" : name);
					var m = regex.Match(className.Name);
					if (m.Success) {
						foreach (var e in members) {
							c.Body.Insert(0, e.DeepCopy());
						}
					}
				}
				return;
			}
			//TODO interfaceのようにUnifiedClassDefinitionがない場合はここまでくるのでどう対処するか
			//とりあえず応急処置
			var interfaces = program.Descendants<UnifiedInterfaceDefinition>();
			if(interfaces.Count() > 0) return;
			var enums = program.Descendants<UnifiedEnumDefinition>();
			if(enums.Count() > 0) return;

			//プログラムに対してメンバーを追加(JavaScript向け))
			if (program != null) {
				foreach (var e in members) {
					program.Body.Insert(0, e.DeepCopy());
				}
			}
		}

	}
}