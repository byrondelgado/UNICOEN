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

using Unicoen.Core.Model;

namespace Unicoen.Core.Processor {
	public interface IUnifiedVisitor {
		void Visit(UnifiedBinaryOperator element);
		void Visit(UnifiedUnaryOperator element);
		void Visit(UnifiedArgument element);
		void Visit(UnifiedArgumentCollection element);
		void Visit(UnifiedBinaryExpression element);
		void Visit(UnifiedBlock element);
		void Visit(UnifiedCall element);
		void Visit(UnifiedFunction element);
		void Visit(UnifiedIf element);
		void Visit(UnifiedParameter element);
		void Visit(UnifiedParameterCollection element);
		void Visit(UnifiedModifier element);
		void Visit(UnifiedModifierCollection element);
		void Visit(UnifiedImport element);
		void Visit(UnifiedProgram element);
		void Visit(UnifiedNew element);
		void Visit(UnifiedFor element);
		void Visit(UnifiedForeach element);
		void Visit(UnifiedUnaryExpression element);
		void Visit(UnifiedProperty element);
		void Visit(UnifiedExpressionCollection element);
		void Visit(UnifiedWhile element);
		void Visit(UnifiedDoWhile element);
		void Visit(UnifiedIndexer element);
		void Visit(UnifiedTypeArgument element);
		void Visit(UnifiedTypeArgumentCollection element);
		void Visit(UnifiedSwitch element);
		void Visit(UnifiedCaseCollection element);
		void Visit(UnifiedCase element);
		void Visit(UnifiedCatch element);
		void Visit(UnifiedTypeCollection element);
		void Visit(UnifiedCatchCollection element);
		void Visit(UnifiedTry element);
		void Visit(UnifiedCast element);
		void Visit(UnifiedTypeParameterCollection element);
		void Visit(UnifiedTypeConstrainCollection element);
		void Visit(UnifiedTypeParameter element);
		void Visit(UnifiedTernaryExpression element);
		void Visit(UnifiedIdentifierCollection element);
		void Visit(UnifiedLabel element);
		void Visit(UnifiedBooleanLiteral element);
		void Visit(UnifiedFractionLiteral element);
		void Visit(UnifiedIntegerLiteral element);
		void Visit(UnifiedStringLiteral element);
		void Visit(UnifiedNullLiteral element);
		void Visit(UnifiedMatcher element);
		void Visit(UnifiedMatcherCollection element);
		void Visit(UnifiedUsing element);
		void Visit(UnifiedListComprehension element);
		void Visit(UnifiedList element);
		void Visit(UnifiedKeyValue element);
		void Visit(UnifiedDictionaryComprehension element);
		void Visit(UnifiedDictionary element);
		void Visit(UnifiedSlice element);
		void Visit(UnifiedComment element);
		void Visit(UnifiedAnnotation element);
		void Visit(UnifiedAnnotationCollection element);
		void Visit(UnifiedVariableDefinitionList element);
		void Visit(UnifiedVariableDefinition element);
		void Visit(UnifiedArrayType element);
		void Visit(UnifiedGenericType element);
		void Visit(UnifiedSimpleType element);
		void Visit(UnifiedCharLiteral element);
		void Visit(UnifiedIterable element);
		void Visit(UnifiedArray element);
		void Visit(UnifiedSet element);
		void Visit(UnifiedTuple element);
		void Visit(UnifiedIterableComprehension element);
		void Visit(UnifiedSetComprehension element);
		void Visit(UnifiedInterface element);
		void Visit(UnifiedClass element);
		void Visit(UnifiedStruct element);
		void Visit(UnifiedEnum element);
		void Visit(UnifiedModule element);
		void Visit(UnifiedUnion element);
		void Visit(UnifiedAnnotationDefinition element);
		void Visit(UnifiedNamespace element);
		void Visit(UnifiedBreak element);
		void Visit(UnifiedContinue element);
		void Visit(UnifiedReturn element);
		void Visit(UnifiedGoto element);
		void Visit(UnifiedYieldReturn element);
		void Visit(UnifiedDelete element);
		void Visit(UnifiedThrow element);
		void Visit(UnifiedAssert element);
		void Visit(UnifiedExec element);
		void Visit(UnifiedStringConversion element);
		void Visit(UnifiedPass element);
		void Visit(UnifiedPrint element);
		void Visit(UnifiedPrintChevron element);
		void Visit(UnifiedWith element);
		void Visit(UnifiedFix element);
		void Visit(UnifiedSynchronized element);
		void Visit(UnifiedConstType element);
		void Visit(UnifiedPointerType element);
		void Visit(UnifiedUnionType element);
		void Visit(UnifiedStructType element);
		void Visit(UnifiedVolatileType element);
		void Visit(UnifiedReferenceType element);
		void Visit(UnifiedConstructor element);
		void Visit(UnifiedStaticInitializer element);
		void Visit(UnifiedInstanceInitializer element);
		void Visit(UnifiedLambda element);
		void Visit(UnifiedDefaultConstrain element);
		void Visit(UnifiedExtendConstrain element);
		void Visit(UnifiedImplementsConstrain element);
		void Visit(UnifiedReferenceConstrain element);
		void Visit(UnifiedSuperConstrain element);
		void Visit(UnifiedValueConstrain element);
		void Visit(UnifiedSuperIdentifier element);
		void Visit(UnifiedThisIdentifier element);
		void Visit(UnifiedLabelIdentifier element);
		void Visit(UnifiedSizeof element);
		void Visit(UnifiedTypeof element);
		void Visit(UnifiedVariableIdentifier element);
	}

	public interface IUnifiedVisitor<in TArg> {
		void Visit(UnifiedBinaryOperator element, TArg arg);
		void Visit(UnifiedUnaryOperator element, TArg arg);
		void Visit(UnifiedArgument element, TArg arg);
		void Visit(UnifiedArgumentCollection element, TArg arg);
		void Visit(UnifiedBinaryExpression element, TArg arg);
		void Visit(UnifiedBlock element, TArg arg);
		void Visit(UnifiedCall element, TArg arg);
		void Visit(UnifiedFunction element, TArg arg);
		void Visit(UnifiedIf element, TArg arg);
		void Visit(UnifiedParameter element, TArg arg);
		void Visit(UnifiedParameterCollection element, TArg arg);
		void Visit(UnifiedModifier element, TArg arg);
		void Visit(UnifiedModifierCollection element, TArg arg);
		void Visit(UnifiedImport element, TArg arg);
		void Visit(UnifiedProgram element, TArg arg);
		void Visit(UnifiedNew element, TArg arg);
		void Visit(UnifiedFor element, TArg arg);
		void Visit(UnifiedForeach element, TArg arg);
		void Visit(UnifiedUnaryExpression element, TArg arg);
		void Visit(UnifiedProperty element, TArg arg);
		void Visit(UnifiedExpressionCollection element, TArg arg);
		void Visit(UnifiedWhile element, TArg arg);
		void Visit(UnifiedDoWhile element, TArg arg);
		void Visit(UnifiedIndexer element, TArg arg);
		void Visit(UnifiedTypeArgument element, TArg arg);
		void Visit(UnifiedTypeArgumentCollection element, TArg arg);
		void Visit(UnifiedSwitch element, TArg arg);
		void Visit(UnifiedCaseCollection element, TArg arg);
		void Visit(UnifiedCase element, TArg arg);
		void Visit(UnifiedCatch element, TArg arg);
		void Visit(UnifiedTypeCollection element, TArg arg);
		void Visit(UnifiedCatchCollection element, TArg arg);
		void Visit(UnifiedTry element, TArg arg);
		void Visit(UnifiedCast element, TArg arg);
		void Visit(UnifiedTypeParameterCollection element, TArg arg);
		void Visit(UnifiedTypeConstrainCollection element, TArg arg);
		void Visit(UnifiedTypeParameter element, TArg arg);
		void Visit(UnifiedTernaryExpression element, TArg arg);
		void Visit(UnifiedIdentifierCollection element, TArg arg);
		void Visit(UnifiedLabel element, TArg arg);
		void Visit(UnifiedBooleanLiteral element, TArg arg);
		void Visit(UnifiedFractionLiteral element, TArg arg);
		void Visit(UnifiedIntegerLiteral element, TArg arg);
		void Visit(UnifiedStringLiteral element, TArg arg);
		void Visit(UnifiedNullLiteral element, TArg arg);
		void Visit(UnifiedMatcher element, TArg arg);
		void Visit(UnifiedMatcherCollection element, TArg arg);
		void Visit(UnifiedUsing element, TArg arg);
		void Visit(UnifiedListComprehension element, TArg arg);
		void Visit(UnifiedList element, TArg arg);
		void Visit(UnifiedKeyValue element, TArg arg);
		void Visit(UnifiedDictionaryComprehension element, TArg arg);
		void Visit(UnifiedDictionary element, TArg arg);
		void Visit(UnifiedSlice element, TArg arg);
		void Visit(UnifiedComment element, TArg arg);
		void Visit(UnifiedAnnotation element, TArg arg);
		void Visit(UnifiedAnnotationCollection element, TArg arg);
		void Visit(UnifiedVariableDefinitionList element, TArg arg);
		void Visit(UnifiedVariableDefinition element, TArg arg);
		void Visit(UnifiedArrayType element, TArg arg);
		void Visit(UnifiedGenericType element, TArg arg);
		void Visit(UnifiedSimpleType element, TArg arg);
		void Visit(UnifiedCharLiteral element, TArg arg);
		void Visit(UnifiedIterable element, TArg arg);
		void Visit(UnifiedArray element, TArg arg);
		void Visit(UnifiedSet element, TArg arg);
		void Visit(UnifiedTuple element, TArg arg);
		void Visit(UnifiedIterableComprehension element, TArg arg);
		void Visit(UnifiedSetComprehension element, TArg arg);
		void Visit(UnifiedInterface element, TArg arg);
		void Visit(UnifiedClass element, TArg arg);
		void Visit(UnifiedStruct element, TArg arg);
		void Visit(UnifiedEnum element, TArg arg);
		void Visit(UnifiedModule element, TArg arg);
		void Visit(UnifiedUnion element, TArg arg);
		void Visit(UnifiedAnnotationDefinition element, TArg arg);
		void Visit(UnifiedNamespace element, TArg arg);
		void Visit(UnifiedBreak element, TArg arg);
		void Visit(UnifiedContinue element, TArg arg);
		void Visit(UnifiedReturn element, TArg arg);
		void Visit(UnifiedGoto element, TArg arg);
		void Visit(UnifiedYieldReturn element, TArg arg);
		void Visit(UnifiedDelete element, TArg arg);
		void Visit(UnifiedThrow element, TArg arg);
		void Visit(UnifiedAssert element, TArg arg);
		void Visit(UnifiedExec element, TArg arg);
		void Visit(UnifiedStringConversion element, TArg arg);
		void Visit(UnifiedPass element, TArg arg);
		void Visit(UnifiedPrint element, TArg arg);
		void Visit(UnifiedPrintChevron element, TArg arg);
		void Visit(UnifiedWith element, TArg arg);
		void Visit(UnifiedFix element, TArg arg);
		void Visit(UnifiedSynchronized element, TArg arg);
		void Visit(UnifiedConstType element, TArg arg);
		void Visit(UnifiedPointerType element, TArg arg);
		void Visit(UnifiedUnionType element, TArg arg);
		void Visit(UnifiedStructType element, TArg arg);
		void Visit(UnifiedVolatileType element, TArg arg);
		void Visit(UnifiedReferenceType element, TArg arg);
		void Visit(UnifiedConstructor element, TArg arg);
		void Visit(UnifiedStaticInitializer element, TArg arg);
		void Visit(UnifiedInstanceInitializer element, TArg arg);
		void Visit(UnifiedLambda element, TArg arg);
		void Visit(UnifiedDefaultConstrain element, TArg arg);
		void Visit(UnifiedExtendConstrain element, TArg arg);
		void Visit(UnifiedImplementsConstrain element, TArg arg);
		void Visit(UnifiedReferenceConstrain element, TArg arg);
		void Visit(UnifiedSuperConstrain element, TArg arg);
		void Visit(UnifiedValueConstrain element, TArg arg);
		void Visit(UnifiedSuperIdentifier element, TArg arg);
		void Visit(UnifiedThisIdentifier element, TArg arg);
		void Visit(UnifiedLabelIdentifier element, TArg arg);
		void Visit(UnifiedSizeof element, TArg arg);
		void Visit(UnifiedTypeof element, TArg arg);
		void Visit(UnifiedVariableIdentifier element, TArg arg);
	}

	public interface IUnifiedVisitor<out TResult, in TArg> {
		TResult Visit(UnifiedBinaryOperator element, TArg arg);
		TResult Visit(UnifiedUnaryOperator element, TArg arg);
		TResult Visit(UnifiedArgument element, TArg arg);
		TResult Visit(UnifiedArgumentCollection element, TArg arg);
		TResult Visit(UnifiedBinaryExpression element, TArg arg);
		TResult Visit(UnifiedBlock element, TArg arg);
		TResult Visit(UnifiedCall element, TArg arg);
		TResult Visit(UnifiedFunction element, TArg arg);
		TResult Visit(UnifiedIf element, TArg arg);
		TResult Visit(UnifiedParameter element, TArg arg);
		TResult Visit(UnifiedParameterCollection element, TArg arg);
		TResult Visit(UnifiedModifier element, TArg arg);
		TResult Visit(UnifiedModifierCollection element, TArg arg);
		TResult Visit(UnifiedImport element, TArg arg);
		TResult Visit(UnifiedProgram element, TArg arg);
		TResult Visit(UnifiedNew element, TArg arg);
		TResult Visit(UnifiedFor element, TArg arg);
		TResult Visit(UnifiedForeach element, TArg arg);
		TResult Visit(UnifiedUnaryExpression element, TArg arg);
		TResult Visit(UnifiedProperty element, TArg arg);
		TResult Visit(UnifiedExpressionCollection element, TArg arg);
		TResult Visit(UnifiedWhile element, TArg arg);
		TResult Visit(UnifiedDoWhile element, TArg arg);
		TResult Visit(UnifiedIndexer element, TArg arg);
		TResult Visit(UnifiedTypeArgument element, TArg arg);
		TResult Visit(UnifiedTypeArgumentCollection element, TArg arg);
		TResult Visit(UnifiedSwitch element, TArg arg);
		TResult Visit(UnifiedCaseCollection element, TArg arg);
		TResult Visit(UnifiedCase element, TArg arg);
		TResult Visit(UnifiedCatch element, TArg arg);
		TResult Visit(UnifiedTypeCollection element, TArg arg);
		TResult Visit(UnifiedCatchCollection element, TArg arg);
		TResult Visit(UnifiedTry element, TArg arg);
		TResult Visit(UnifiedCast element, TArg arg);
		TResult Visit(UnifiedTypeParameterCollection element, TArg arg);
		TResult Visit(UnifiedTypeConstrainCollection element, TArg arg);
		TResult Visit(UnifiedTypeParameter element, TArg arg);
		TResult Visit(UnifiedTernaryExpression element, TArg arg);
		TResult Visit(UnifiedIdentifierCollection element, TArg arg);
		TResult Visit(UnifiedLabel element, TArg arg);
		TResult Visit(UnifiedBooleanLiteral element, TArg arg);
		TResult Visit(UnifiedFractionLiteral element, TArg arg);
		TResult Visit(UnifiedIntegerLiteral element, TArg arg);
		TResult Visit(UnifiedStringLiteral element, TArg arg);
		TResult Visit(UnifiedNullLiteral element, TArg arg);
		TResult Visit(UnifiedMatcher element, TArg arg);
		TResult Visit(UnifiedMatcherCollection element, TArg arg);
		TResult Visit(UnifiedUsing element, TArg arg);
		TResult Visit(UnifiedListComprehension element, TArg arg);
		TResult Visit(UnifiedList element, TArg arg);
		TResult Visit(UnifiedKeyValue element, TArg arg);
		TResult Visit(UnifiedDictionaryComprehension element, TArg arg);
		TResult Visit(UnifiedDictionary element, TArg arg);
		TResult Visit(UnifiedSlice element, TArg arg);
		TResult Visit(UnifiedComment element, TArg arg);
		TResult Visit(UnifiedAnnotation element, TArg arg);
		TResult Visit(UnifiedAnnotationCollection element, TArg arg);
		TResult Visit(UnifiedVariableDefinitionList element, TArg arg);
		TResult Visit(UnifiedVariableDefinition element, TArg arg);
		TResult Visit(UnifiedArrayType element, TArg arg);
		TResult Visit(UnifiedGenericType element, TArg arg);
		TResult Visit(UnifiedSimpleType element, TArg arg);
		TResult Visit(UnifiedCharLiteral element, TArg arg);
		TResult Visit(UnifiedIterable element, TArg arg);
		TResult Visit(UnifiedArray element, TArg arg);
		TResult Visit(UnifiedSet element, TArg arg);
		TResult Visit(UnifiedTuple element, TArg arg);
		TResult Visit(UnifiedIterableComprehension element, TArg arg);
		TResult Visit(UnifiedSetComprehension element, TArg arg);
		TResult Visit(UnifiedInterface element, TArg arg);
		TResult Visit(UnifiedClass element, TArg arg);
		TResult Visit(UnifiedStruct element, TArg arg);
		TResult Visit(UnifiedEnum element, TArg arg);
		TResult Visit(UnifiedModule element, TArg arg);
		TResult Visit(UnifiedUnion element, TArg arg);
		TResult Visit(UnifiedAnnotationDefinition element, TArg arg);
		TResult Visit(UnifiedNamespace element, TArg arg);
		TResult Visit(UnifiedBreak element, TArg arg);
		TResult Visit(UnifiedContinue element, TArg arg);
		TResult Visit(UnifiedReturn element, TArg arg);
		TResult Visit(UnifiedGoto element, TArg arg);
		TResult Visit(UnifiedYieldReturn element, TArg arg);
		TResult Visit(UnifiedDelete element, TArg arg);
		TResult Visit(UnifiedThrow element, TArg arg);
		TResult Visit(UnifiedAssert element, TArg arg);
		TResult Visit(UnifiedExec element, TArg arg);
		TResult Visit(UnifiedStringConversion element, TArg arg);
		TResult Visit(UnifiedPass element, TArg arg);
		TResult Visit(UnifiedPrint element, TArg arg);
		TResult Visit(UnifiedPrintChevron element, TArg arg);
		TResult Visit(UnifiedWith element, TArg arg);
		TResult Visit(UnifiedFix element, TArg arg);
		TResult Visit(UnifiedSynchronized element, TArg arg);
		TResult Visit(UnifiedConstType element, TArg arg);
		TResult Visit(UnifiedPointerType element, TArg arg);
		TResult Visit(UnifiedUnionType element, TArg arg);
		TResult Visit(UnifiedVolatileType element, TArg arg);
		TResult Visit(UnifiedStructType element, TArg arg);
		TResult Visit(UnifiedReferenceType element, TArg arg);
		TResult Visit(UnifiedConstructor element, TArg arg);
		TResult Visit(UnifiedStaticInitializer element, TArg arg);
		TResult Visit(UnifiedInstanceInitializer element, TArg arg);
		TResult Visit(UnifiedLambda element, TArg arg);
		TResult Visit(UnifiedDefaultConstrain element, TArg arg);
		TResult Visit(UnifiedExtendConstrain element, TArg arg);
		TResult Visit(UnifiedImplementsConstrain element, TArg arg);
		TResult Visit(UnifiedReferenceConstrain element, TArg arg);
		TResult Visit(UnifiedSuperConstrain element, TArg arg);
		TResult Visit(UnifiedValueConstrain element, TArg arg);
		TResult Visit(UnifiedSuperIdentifier element, TArg arg);
		TResult Visit(UnifiedThisIdentifier element, TArg arg);
		TResult Visit(UnifiedLabelIdentifier element, TArg arg);
		TResult Visit(UnifiedSizeof element, TArg arg);
		TResult Visit(UnifiedTypeof element, TArg arg);
		TResult Visit(UnifiedVariableIdentifier element, TArg arg);
	}
}