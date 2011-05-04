﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;
using Code2Xml.Languages.C.CodeToXmls;
using Mocomoco.Xml.Linq;
using Paraiba.Linq;
using Unicoen.Core.Model;

namespace Unicoen.Languages.C.ModelFactories
{
	public static partial class CModelFactoryHelper
	{

		public static UnifiedProgram CreateTranslationUnit(XElement node)
		{
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "translation_unit");
			/*
			 * translation_unit
			 */
			throw new NotImplementedException(); //TODO: implement
		}

		public static IUnifiedElement CreateExternalDeclaration(XElement node)
		{
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "external_declaration");
			/*
			 * external_declaration
			 */
			throw new NotImplementedException(); //TODO: implement
		}

		public static IUnifiedElement CreateFunctionDefinition(XElement node)
		{
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "function_definition");
			/*
			function_definition
			:	declaration_specifiers? declarator
				(	declaration+ compound_statement	// K&R style
				|	compound_statement				// ANSI style
				)
			*/


			var typeSpecifier = CreateTypeSpecifier(node.Element("type_specifier"));






			// var functionDefinition;))
			throw new NotImplementedException(); //TODO: implement
		}

		public static IUnifiedElement CreateDeclaration(XElement node)
		{
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "declaration");
			/*
			declaration
			:	'typedef' declaration_specifiers? {$declaration::isTypedef=true;}
				init_declarator_list ';' // special case, looking for typedef	
			| declaration_specifiers init_declarator_list? ';'
			 */

			throw new NotImplementedException(); //TODO: implement
		}

		public static IUnifiedElement CreateDeclarationSpecifiers(XElement node)
		{
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "declaration_specifiers");
			/*
			declaration_specifiers
			:   (   storage_class_specifier
				|   type_specifier
				|   type_qualifier
				)+
			*/
			throw new NotImplementedException(); //TODO: implement
		}

		public static IUnifiedElement CreateInitDeclaratorList(XElement node)
		{
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "init_declarator_list");
			/*
			init_declarator_list
			: init_declarator (',' init_declarator)*
			*/

			throw new NotImplementedException(); //TODO: implement
		}

		public static IUnifiedElement CreateInitDeclarator(XElement node)
		{
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "init_declarator");
			/*
			init_declarator
			: declarator ('=' initializer)?
			*/

			throw new NotImplementedException(); //TODO: implement
		}

		public static IUnifiedElement CreateStorageClassSpecifier(XElement node)
		{
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "storage_class_specifier");
			/*
			storage_class_specifier
			: 'extern'
			| 'static'
			| 'auto'
			| 'register'
			*/

			throw new NotImplementedException(); //TODO: implement
		}

		public static IUnifiedElement CreateTypeSpecifier(XElement node)
		{
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "type_specifier");
			/*
			type_specifier
			: 'void'
			| 'char'
			| 'short'
			| 'int'
			| 'long'
			| 'float'
			| 'double'
			| 'signed'
			| 'unsigned'
			| struct_or_union_specifier
			| enum_specifier
			| type_id
			 */

			throw new NotImplementedException(); //TODO: implement
		}

		public static IUnifiedElement CreateTypeId(XElement node)
		{
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "type_id");
			/*
			type_id
			: {isTypeName(input.LT(1).getText())}? IDENTIFIER
			*/

			throw new NotImplementedException(); //TODO: implement
		}

		public static IUnifiedElement CreateStructOrUnionSpecifier(XElement node)
		{
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "struct_or_union_specifier");
			/*
			struct_or_union_specifier
			: struct_or_union IDENTIFIER? '{' struct_declaration_list '}'
			| struct_or_union IDENTIFIER
			*/

			throw new NotImplementedException(); //TODO: implement
		}

		public static IUnifiedElement CreateStructOrUnion(XElement node)
		{
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "struct_or_union");
			/*
			struct_or_union
			: 'struct'
			| 'union'
			 */

			throw new NotImplementedException(); //TODO: implement
		}

		public static IUnifiedElement CreateStructDeclarationList(XElement node)
		{
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "struct_declaration_list");
			/*
			struct_declaration_list
			: struct_declaration+
			*/

			throw new NotImplementedException(); //TODO: implement
		}

		public static IUnifiedElement CreateStructDeclaration(XElement node)
		{
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "struct_declaration");
			/*
			struct_declaration
			: specifier_qualifier_list struct_declarator_list ';'
			 */

			throw new NotImplementedException(); //TODO: implement
		}

		public static IUnifiedElement CreateSpecifierQualifierList(XElement node)
		{
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "specifier_qualifier_list");
			/*
			specifier_qualifier_list
			: ( type_qualifier | type_specifier )+
			*/
			
			throw new NotImplementedException(); //TODO: implement
		}

		public static IUnifiedElement CreateStructDeclaratorList(XElement node)
		{
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "struct_declarator_list");
			/*
			struct_declarator_list
			: struct_declarator (',' struct_declarator)*
			*/
			
			throw new NotImplementedException(); //TODO: implement
		}

		public static IUnifiedElement CreateStructDeclarator(XElement node)
		{
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "struct_declarator");
			/*
			struct_declarator
			: declarator (':' constant_expression)?
			| ':' constant_expression
			*/
			
			throw new NotImplementedException(); //TODO: implement
		}

		public static IUnifiedElement CreateEnumSpecifier(XElement node)
		{
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "enum_specifier");
			/*
			enum_specifier
			: 'enum' '{' enumerator_list '}'
			| 'enum' IDENTIFIER '{' enumerator_list '}'
			| 'enum' IDENTIFIER
			*/
			throw new NotImplementedException(); //TODO: implement
		}

		public static IUnifiedElement CreateEnumeratorList(XElement node)
		{
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "enumerator_list");
			/*
			enumerator_list
			: enumerator (',' enumerator)*
			*/
			
			throw new NotImplementedException(); //TODO: implement
		}

		public static IUnifiedElement CreateEnumerator(XElement node)
		{
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "enumerator");
			/*
			enumerator
			: IDENTIFIER ('=' constant_expression)?
			*/
			
			throw new NotImplementedException(); //TODO: implement
		}

		public static IUnifiedElement CreateTypeQualifier(XElement node)
		{
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "type_qualifier");
			/*
			type_qualifier
			: 'const'
			| 'volatile'
			*/
			throw new NotImplementedException(); //TODO: implement
		}

		public static IUnifiedElement CreateDeclarator(XElement node)
		{
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "declarator");
			/*
			declarator
			: pointer? direct_declarator
			| pointer
			*/
			
			throw new NotImplementedException(); //TODO: implement
		}

		public static IUnifiedElement CreateDirectDeclarator(XElement node)
		{
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "direct_declarator");
			/*
			direct_declarator
			:   (	IDENTIFIER
				|	'(' declarator ')'
				)
				declarator_suffix*
			*/
			
			throw new NotImplementedException(); //TODO: implement
		}

		public static IUnifiedElement CreateDeclaratorSuffix(XElement node)
		{
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "declarator_suffix");
			/*
			declarator_suffix
			:   '[' constant_expression ']'
			|   '[' ']'
			|   '(' parameter_type_list ')'
			|   '(' identifier_list ')'
			|   '(' ')'
			;
			*/
			
			throw new NotImplementedException(); //TODO: implement
		}

		public static IUnifiedElement CreatePointer(XElement node)
		{
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "pointer");
			/*
			pointer
			: '*' type_qualifier+ pointer?
			| '*' pointer
			| '*'
			 */
			
			throw new NotImplementedException(); //TODO: implement
		}

		public static IUnifiedElement CreateParameterTypeList(XElement node)
		{
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "parameter_type_list");
			/*
			parameter_type_list
			: parameter_list (',' '...')?
			*/

			throw new NotImplementedException(); //TODO: implement
		}

		public static IUnifiedElement CreateParameterList(XElement node)
		{
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "parameter_list");
			/*
			parameter_list
			: parameter_declaration (',' parameter_declaration)*
			*/

			throw new NotImplementedException(); //TODO: implement
		}

		public static IUnifiedElement CreateParameterDeclaration(XElement node)
		{
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "parameter_declaration");
			/*
			parameter_declaration
			: declaration_specifiers (declarator|abstract_declarator)*
			 */

			throw new NotImplementedException(); //TODO: implement
		}

		public static IUnifiedElement CreateIdentifierList(XElement node)
		{
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "identifier_list");
			/*
			identifier_list
			: IDENTIFIER (',' IDENTIFIER)*
			*/
			
			throw new NotImplementedException(); //TODO: implement
		}

		public static IUnifiedElement CreateTypeName(XElement node)
		{
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "type_name");
			/*
			type_name
			: specifier_qualifier_list abstract_declarator?
			*/
			
			throw new NotImplementedException(); //TODO: implement
		}

		public static IUnifiedElement CreateAbstractDeclarator(XElement node)
		{
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "abstract_declarator");
			/*
			abstract_declarator
			: pointer direct_abstract_declarator?
			| direct_abstract_declarator
			 */
			
			throw new NotImplementedException(); //TODO: implement
		}

		public static IUnifiedElement CreateDirectAbstractDeclarator(XElement node)
		{
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "direct_abstract_declarator");
			/*
			direct_abstract_declarator
			:	( '(' abstract_declarator ')' | abstract_declarator_suffix ) abstract_declarator_suffix*
			 */

			throw new NotImplementedException(); //TODO: implement
		}

		public static IUnifiedElement CreateAbstractDeclaratorSuffix(XElement node)
		{
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "abstract_declarator_suffix");
			/*
			abstract_declarator_suffix
			:	'[' ']'
			|	'[' constant_expression ']'
			|	'(' ')'
			|	'(' parameter_type_list ')'
			*/
			
			throw new NotImplementedException(); //TODO: implement
		}

		public static IUnifiedElement CreateInitializer(XElement node)
		{
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "initializer");
			/*
			initializer
			: assignment_expression
			| '{' initializer_list ','? '}'
			 */

			throw new NotImplementedException(); //TODO: implement
		}

		public static IUnifiedElement CreateInitializerList(XElement node)
		{
			Contract.Requires(node != null);
			Contract.Requires(node.Name() == "initializer_list");
			/*
			initializer_list
			: initializer (',' initializer)*
			 */
	
			throw new NotImplementedException(); //TODO: implement
		}
		


		

	}
}