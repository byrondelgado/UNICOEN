using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unicoen.Model;

namespace Unicoen.Apps.RefactoringDSL.Sandbox {
	public class CollisionDetector {
		public static UnifiedElement DetectCollision(UnifiedElement src, UnifiedElement target) {
			foreach (var element in src.Descendants()) {
				if (element.GetType() != target.GetType()) {
					continue;
				}
				// 対象の型によって下に委譲
				if (target is UnifiedVariableDefinition) {
					return DetectVariableCollision(element as UnifiedVariableDefinition, target as UnifiedVariableDefinition);
				}

				if (target is UnifiedFunctionDefinition) {
					return DetectFunctionCollision(element as UnifiedFunctionDefinition, target as UnifiedFunctionDefinition);
				}

			}

			throw  new NotSupportedException("さぽーとしてないお(´・ω・｀)");

		}

		// src と dst の衝突を調べて，衝突していれば src を，そうでなければ null を返す
		private static UnifiedVariableDefinition DetectVariableCollision(UnifiedVariableDefinition src, UnifiedVariableDefinition dst) {
			var judges = new List<Boolean>();
			judges.Add(src.Name.Name == dst.Name.Name);

			if (judges.All(e => true)) {
				return null;
			}
			return src;
		}

		private static UnifiedFunctionDefinition DetectFunctionCollision(UnifiedFunctionDefinition src, UnifiedFunctionDefinition dst) {
			// Judge whether src and dst have same signature
			var judges = new List<Boolean>();
			judges.Add(src.Name.Name == dst.Name.Name);
			judges.Add(src.Parameters.All(e => dst.Parameters.Any(d => d.Equals(e))));

			if (judges.All(e => true)) {
				return null;
			}
			return src;
		}

	}
}
