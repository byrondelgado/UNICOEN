﻿#region License

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

namespace Unicoen.Model {
	/// <summary>
	/// 継承関係やデフォルトコンストラクタの存在などの制約を表します。
	/// なお、継承関係を表す場合、対象の型の個数は１つです。
	/// e.g. Javaにおける継承関係の制約( <c>class C extends P { ... }</c> の <c>extends P</c> 部分) 
	/// e.g. C#におけるデフォルトコンストラクタの制約( <c>where A : new()</c> の <c>: new()</c> 部分)
	/// </summary>
	public abstract class UnifiedTypeConstrain : UnifiedElement {}
}