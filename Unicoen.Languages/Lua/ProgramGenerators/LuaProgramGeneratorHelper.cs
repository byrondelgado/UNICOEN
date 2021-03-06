﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Xml.Linq;
using Paraiba.Xml.Linq;
using Unicoen.Model;
using Unicoen.ProgramGenerators;

#region License

// Copyright (C) 2011-2012 The Unicoen Project
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

// ReSharper disable InvocationIsSkipped

namespace Unicoen.Languages.Lua.ProgramGenerators {
    public static class LuaProgramGeneratorHelper {
        public static Dictionary<string, UnifiedBinaryOperator>
                Sign2BinaryOperator;

        public static Dictionary<string, UnifiedUnaryOperator>
                Sign2PrefixUnaryOperator;

        static LuaProgramGeneratorHelper() {
            Sign2BinaryOperator =
                    UnifiedProgramGeneratorHelper.CreateBinaryOperatorDictionary
                            ();
            Sign2PrefixUnaryOperator =
                    UnifiedProgramGeneratorHelper.
                            CreatePrefixUnaryOperatorDictionaryForJava();
        }

        public static UnifiedProgram CreateChunk(XElement node) {
            Contract.Requires(node != null);
            Contract.Requires(node.Name() == "chunk");
            /*
             * chunk : (stat (';')?)* (laststat (';')?)?;
             */
            throw new NotImplementedException(); //TODO: implement
        }

        public static UnifiedElement CreateBlock(XElement node) {
            Contract.Requires(node != null);
            Contract.Requires(node.Name() == "block");
            /*
             * block : chunk;
             */
            throw new NotImplementedException(); //TODO: implement
        }

        public static UnifiedElement CreateStat(XElement node) {
            Contract.Requires(node != null);
            Contract.Requires(node.Name() == "stat");
            /*
             * stat :  varlist1 '=' explist1 |
             */
            throw new NotImplementedException(); //TODO: implement
        }

        public static UnifiedElement CreateLaststat(XElement node) {
            Contract.Requires(node != null);
            Contract.Requires(node.Name() == "laststat");
            /*
             * laststat : 'return' (explist1)? | 'break';
             */
            throw new NotImplementedException(); //TODO: implement
        }

        public static UnifiedElement CreateFuncname(XElement node) {
            Contract.Requires(node != null);
            Contract.Requires(node.Name() == "funcname");
            /*
             * funcname : NAME ('.' NAME)* (':' NAME)? ;
             */
            throw new NotImplementedException(); //TODO: implement
        }

        public static UnifiedElement CreateVarlist1(XElement node) {
            Contract.Requires(node != null);
            Contract.Requires(node.Name() == "varlist1");
            /*
             * varlist1 : var (',' var)*;
             */
            throw new NotImplementedException(); //TODO: implement
        }

        public static UnifiedElement CreateNamelist(XElement node) {
            Contract.Requires(node != null);
            Contract.Requires(node.Name() == "namelist");
            /*
             * namelist : NAME (',' NAME)*;
             */
            throw new NotImplementedException(); //TODO: implement
        }

        public static UnifiedElement CreateExplist1(XElement node) {
            Contract.Requires(node != null);
            Contract.Requires(node.Name() == "explist1");
            /*
             * explist1 : (exp ',')* exp;
             */
            throw new NotImplementedException(); //TODO: implement
        }

        public static UnifiedElement CreateExp(XElement node) {
            Contract.Requires(node != null);
            Contract.Requires(node.Name() == "exp");
            /*
             * exp :  ('nil' | 'false' | 'true' | lua_number | lua_string | '...' | function | prefixexp | tableconstructor | unop exp) (binop exp)* ;
             */
            throw new NotImplementedException(); //TODO: implement
        }

        public static UnifiedElement CreateVar(XElement node) {
            Contract.Requires(node != null);
            Contract.Requires(node.Name() == "var");
            /*
             * var: (NAME | '(' exp ')' varSuffix) varSuffix*;
             */
            throw new NotImplementedException(); //TODO: implement
        }

        public static UnifiedElement CreatePrefixexp(XElement node) {
            Contract.Requires(node != null);
            Contract.Requires(node.Name() == "prefixexp");
            /*
             * prefixexp: varOrExp nameAndArgs*;
             */
            throw new NotImplementedException(); //TODO: implement
        }

        public static UnifiedElement CreateFunctioncall(XElement node) {
            Contract.Requires(node != null);
            Contract.Requires(node.Name() == "functioncall");
            /*
             * functioncall: varOrExp nameAndArgs+;
             */
            throw new NotImplementedException(); //TODO: implement
        }

        public static UnifiedElement CreateVarOrExp(XElement node) {
            Contract.Requires(node != null);
            Contract.Requires(node.Name() == "varOrExp");
            /*
             * varOrExp: var | '(' exp ')';
             */
            throw new NotImplementedException(); //TODO: implement
        }

        public static UnifiedElement CreateNameAndArgs(XElement node) {
            Contract.Requires(node != null);
            Contract.Requires(node.Name() == "nameAndArgs");
            /*
             * nameAndArgs: (':' NAME)? args;
             */
            throw new NotImplementedException(); //TODO: implement
        }

        public static UnifiedElement CreateVarSuffix(XElement node) {
            Contract.Requires(node != null);
            Contract.Requires(node.Name() == "varSuffix");
            /*
             * varSuffix: nameAndArgs* ('[' exp ']' | '.' NAME);
             */
            throw new NotImplementedException(); //TODO: implement
        }

        public static UnifiedElement CreateArgs(XElement node) {
            Contract.Requires(node != null);
            Contract.Requires(node.Name() == "args");
            /*
             * args :  '(' (explist1)? ')' | tableconstructor | lua_string ;
             */
            throw new NotImplementedException(); //TODO: implement
        }

        public static UnifiedElement CreateFunction(XElement node) {
            Contract.Requires(node != null);
            Contract.Requires(node.Name() == "function");
            /*
             * function : 'function' funcbody;
             */
            throw new NotImplementedException(); //TODO: implement
        }

        public static UnifiedElement CreateFuncbody(XElement node) {
            Contract.Requires(node != null);
            Contract.Requires(node.Name() == "funcbody");
            /*
             * funcbody : '(' (parlist1)? ')' block 'end';
             */
            throw new NotImplementedException(); //TODO: implement
        }

        public static UnifiedElement CreateParlist1(XElement node) {
            Contract.Requires(node != null);
            Contract.Requires(node.Name() == "parlist1");
            /*
             * parlist1 : namelist (',' '...')? | '...';
             */
            throw new NotImplementedException(); //TODO: implement
        }

        public static UnifiedElement CreateTableconstructor(XElement node) {
            Contract.Requires(node != null);
            Contract.Requires(node.Name() == "tableconstructor");
            /*
             * tableconstructor : '{' (fieldlist)? '}';
             */
            throw new NotImplementedException(); //TODO: implement
        }

        public static UnifiedElement CreateFieldlist(XElement node) {
            Contract.Requires(node != null);
            Contract.Requires(node.Name() == "fieldlist");
            /*
             * fieldlist : field (fieldsep field)* (fieldsep)?;
             */
            throw new NotImplementedException(); //TODO: implement
        }

        public static UnifiedElement CreateField(XElement node) {
            Contract.Requires(node != null);
            Contract.Requires(node.Name() == "field");
            /*
             * field : '[' exp ']' '=' exp | NAME '=' exp | exp;
             */
            throw new NotImplementedException(); //TODO: implement
        }

        public static UnifiedElement CreateFieldsep(XElement node) {
            Contract.Requires(node != null);
            Contract.Requires(node.Name() == "fieldsep");
            /*
             * fieldsep : ',' | ';';
             */
            throw new NotImplementedException(); //TODO: implement
        }

        public static UnifiedElement CreateBinop(XElement node) {
            Contract.Requires(node != null);
            Contract.Requires(node.Name() == "binop");
            /*
             * binop : '+' | '-' | '*' | '/' | '^' | '%' | '..' |
             */
            throw new NotImplementedException(); //TODO: implement
        }

        public static UnifiedElement CreateUnop(XElement node) {
            Contract.Requires(node != null);
            Contract.Requires(node.Name() == "unop");
            /*
             * unop : '-' | 'not' | '#';
             */
            throw new NotImplementedException(); //TODO: implement
        }

        public static UnifiedElement CreateLua_number(XElement node) {
            Contract.Requires(node != null);
            Contract.Requires(node.Name() == "lua_number");
            /*
             * lua_number : INT | FLOAT | EXP | HEX;
             */
            throw new NotImplementedException(); //TODO: implement
        }

        public static UnifiedElement CreateLua_string(XElement node) {
            Contract.Requires(node != null);
            Contract.Requires(node.Name() == "lua_string");
            /*
             * lua_string  : NORMALSTRING | CHARSTRING | LONGSTRING;
             */
            throw new NotImplementedException(); //TODO: implement
        }
    }
}