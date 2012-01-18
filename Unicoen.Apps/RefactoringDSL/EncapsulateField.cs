using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unicoen.Model;

namespace Unicoen.Apps.RefactoringDSL{
	/// <summary>
	/// EncapsulateField リファクタリングを行うためのクラス
	/// </summary>
	public class EncapsulateField{
		private UnifiedProgram Program { get; set; }

		// コンストラクタ
		public EncapsulateField(UnifiedProgram program) {
			this.Program = program;
		}

		public UnifiedProgram Refactor(String className) {
			// 一応コピー
			var model = Program.DeepCopy();
			var targetClasses = EncapsulateFieldHelper.FindByClassName(model, className);
			if (targetClasses.Count() <= 0) {
				// クラスがなかったら，なにもしないで返す
				return model;
			}
			var targetClass = targetClasses.First();
			var publicVariables = EncapsulateFieldHelper.FindPublicFields(targetClass);

			var getters = new List<UnifiedFunctionDefinition>();
			var setters = new List<UnifiedFunctionDefinition>();
			// ゲッタとセッタを生成
			foreach (var v in publicVariables) {
				getters.Add(EncapsulateFieldHelper.GenerateGetter(v));
				setters.Add(EncapsulateFieldHelper.GenerateSetter(v));
			}
			
			// フィールドを private に変更
			foreach (var pv in publicVariables) {
				EncapsulateFieldHelper.ChangeModifier(pv, "private");
			}

			var list = targetClass.Body.DeepCopy();
			// 元の要素群とアクセッサを結合
			foreach (var getter in getters) {
				list.Add(getter);
			}
			foreach (var setter in setters) {
				list.Add(setter);
			}
			
			targetClass.Body = list;

			return model;

		}
	}
}
