using Unicoen.Core.Processor;

namespace Unicoen.Core.Model {
	/// <summary>
	///   文字列であるリテラルを表します。
	///   e.g. Javaにおける<c>char str = 'c'</c>の<c>'c'</c>の部分
	/// </summary>
	public class UnifiedCharLiteral : UnifiedTypedLiteral<string> {
		private UnifiedCharLiteral() { }

		public override void Accept(IUnifiedModelVisitor visitor) {
			visitor.Visit(this);
		}

		public override void Accept<TData>(
				IUnifiedModelVisitor<TData> visitor,
				TData arg) {
			visitor.Visit(this, arg);
		}

		public override TResult Accept<TData, TResult>(
				IUnifiedModelVisitor<TData, TResult> visitor, TData arg) {
			return visitor.Visit(this, arg);
		}

		public static UnifiedCharLiteral Create(string value) {
			return new UnifiedCharLiteral {
					Value = value,
			};
		}
	}
}