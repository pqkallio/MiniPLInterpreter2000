using System;

namespace MiniPLInterpreterTests
{
	public class SemanticAnalyzerTestInputs
	{
		public SemanticAnalyzerTestInputs ()
		{}

		public static readonly string[] sourceEmpty = 
		{};

		public static readonly string[] correctBoolAssignments = 
		{
			"var i1 : bool := true;",
			"var i2 : bool := false;",
			"var i3 : bool := 1 = 1;",
			"var i4 : bool := !(1 = 1);",
			"var i5 : bool := 1 < 1;",
			"var i6 : bool := !(1 < 1);",
			"var i7 : bool := true = false;",
			"var i8 : bool := !(true = false);",
			"var i9 : bool := true < false;",
			"var i10 : bool := !(true < false);",
			"var i11 : bool := \"ek\" = \"ak\";",
			"var i12 : bool := !(\"ek\" = \"ak\");",
			"var i13 : bool := \"ek\" < \"ak\";",
			"var i14 : bool := !(\"ek\" < \"ak\");",
			"i7 := true & false;",
			"i7 := !(true & false);",
			"i8 := i5 & i6;",
		};

		public static readonly string[] incorrectBoolAssignment1 = 
		{
			"var i : bool := \"microscope\";"
		};

		public static readonly string[] incorrectBoolAssignment2 = 
		{
			"var i : bool := !(\"microscope\");"
		};

		public static readonly string[] incorrectBoolAssignment3 = 
		{
			"var i : bool := 95;"
		};

		public static readonly string[] incorrectBoolAssignment4 = 
		{
			"var i : bool := !(95);"
		};

		public static readonly string[] incorrectBoolAssignment5 = 
		{
			"var i : bool := true + false;"
		};

		public static readonly string[] incorrectBoolAssignment6 = 
		{
			"var i : bool := true - false;"
		};

		public static readonly string[] incorrectBoolAssignment7 = 
		{
			"var i : bool := true * false;"
		};

		public static readonly string[] incorrectBoolAssignment8 = 
		{
			"var i : bool := true / false;"
		};

		public static readonly string[] correctStringAssignments = 
		{
			"var i1 : string := \"ek\";",
			"var i2 : string := \"ak\";",
			"var i3 : string := \"ek\" + \"ak\";",
			"var i4 : string := \"ek\" + i2;",
			"var i5 : string := i1 + \"ak\";",
			"var i6 : string := i1 + i2;"
		};

		public static readonly string[] incorrectStringAssignment1 = 
		{
			"var i : string := true;"
		};

		public static readonly string[] incorrectStringAssignment2 = 
		{
			"var i : string := !(true);"
		};

		public static readonly string[] incorrectStringAssignment3 = 
		{
			"var i : string := 95;"
		};

		public static readonly string[] incorrectStringAssignment4 = 
		{
			"var i : string := \"mok\" - \"pok\";"
		};

		public static readonly string[] incorrectStringAssignment5 = 
		{
			"var i : string := \"mok\" * \"pok\";"
		};

		public static readonly string[] incorrectStringAssignment6 = 
		{
			"var i : string := \"mok\" / \"pok\";"
		};

		public static readonly string[] incorrectStringAssignment7 = 
		{
			"var i : string := \"mok\" & \"pok\";"
		};

		public static readonly string[] incorrectStringAssignment8 = 
		{
			"var i : string := \"mok\" = \"pok\";"
		};

		public static readonly string[] incorrectStringAssignment9 = 
		{
			"var i : string := \"mok\" + 9;"
		};

		public static readonly string[] incorrectStringAssignment10 = 
		{
			"var i : string := \"mok\" + true;"
		};

		public static readonly string[] correctIntAssignments = 
		{
			"var i1 : int := 1;",
			"var i2 : int := 1 + 1;",
			"var i3 : int := 1 - 1;",
			"var i4 : int := 1 * 1;",
			"var i5 : int := 1 / 1;",
			"var i6 : int := i1 + i2;"
		};

		public static readonly string[] incorrectIntAssignment1 = 
		{
			"var i : int := true;"
		};

		public static readonly string[] incorrectIntAssignment2 = 
		{
			"var i : int := \"banana\";"
		};

		public static readonly string[] incorrectIntAssignment3 = 
		{
			"var i : int := 95 & 95;"
		};

		public static readonly string[] incorrectIntAssignment4 = 
		{
			"var i : int := 5 + \"pok\";"
		};

		public static readonly string[] incorrectIntAssignment5 = 
		{
			"var i : int := false + 9;"
		};

		public static readonly string[] notDeclaredForRead = {
			"read a;"
		};

		public static readonly string[] tryToReadToIntVar = {
			"var a : int;",
			"read a;"
		};

		public static readonly string[] tryToReadToStringVar = {
			"var a : string;",
			"read a;"
		};

		public static readonly string[] tryToReadToBoolVar = {
			"var a : bool;",
			"read a;"
		};

		public static readonly string[] notDeclaredForPrint = {
			"print a;"
		};

		public static readonly string[] doubleDeclaration = {
			"var a : string := \"gogogo\";\n",
			"var a : int := 666;"
		};

		public static readonly string[] assertionBoolean = {
			"assert (\"\" < \"bamboozle\");"
		};

		public static readonly string[] assertionNotBoolean = {
			"assert (\"\" + \"bamboozle\");"
		};

		public static readonly string[] forLoopOk = {
			"var i : int;\n",
			"for i in 1..10 do\n",
			"print i;\n",
			"end for;\n"
		};

		public static readonly string[] forLoopControlVarNotDeclared = {
			"for i in 1..10 do\n",
			"print i;\n",
			"end for;\n"
		};

		public static readonly string[] forLoopRangeFromNotInt = {
			"var i : int;\n",
			"var x : bool;\n",
			"for i in x..10 do\n",
			"print i;\n",
			"end for;\n"
		};

		public static readonly string[] forLoopRangeUptoNotInt = {
			"var i : int;\n",
			"var x : bool;\n",
			"for i in 1..x do\n",
			"print i;\n",
			"end for;\n"
		};

		public static readonly string[] forControlVariableReassignInsideLoop = {
			"var i : int;\n",
			"for i in 1..10 do\n",
			"i := 1;\n",
			"end for;\n"
		};

		public static readonly string[] forControlVariableReassignOutsideLoop = {
			"var i : int;\n",
			"for i in 1..10 do\n",
			"end for;\n",
			"i := 1;\n"
		};

		public static readonly string[] assignWhenNotDeclared = {
			"a := false;"
		};

		public static readonly string[] validMassiveInputForSemanticTesting =
		{
			"/**\n",
			"* BOOLEANS\n",
			"/* AND */\n",
			"// COMMENTS\n",
			"**/\n",
			"\n",
			"// Comment, one-liner\n",
			"\n",
			"var a : bool;	// Comment after code\n",
			"\n",
			"/* test that a is false by default */\n",
			"assert(!(a));\n",
			"assert(a = false);\n",
			"\n",
			"/* test that\n",
			"boolean assignment\n",
			"works */\n",
			"a := true;\n",
			"\n",
			"assert(!(!(a)));\n",
			"assert(a);\n",
			"assert(a = true);\n",
			"\n",
			"var b : bool := true; /* now we test\n",
			"with	\n",
			"		two\n",
			"			booleans */			assert((!(!(a))) & (!(!(b))));\n",
			"assert(a & b);\n",
			"\n",
			"// the next two lines should not be evaluated\n",
			"// assert(!(a & b));\n",
			"/* assert(!(a & b)); */\n",
			"\n",
			"b := false;\n",
			"assert(!(a & b));\n",
			"assert(!(a = b));\n",
			"assert(b < a);\n",
			"assert(a = true);\n",
			"assert(!(b < b));\n",
			"assert(!(a < b));\n",
			"\n",
			"\n",
			"\n",
			"\n",
			"\n",
			"\n",
			"/**\n",
			"* INTEGERS\n",
			"**/\n",
			"\n",
			"\n",
			"\n",
			"// and now with integers\n",
			"var c:int;\n",
			"var d :int:=-2147483648;\n",
			"var e: int:= 2147483647;\n",
			"var f :int :=+1	;\n",
			"\n",
			"assert(c=0);\n",
			"assert((c+f) = (c--1));\n",
			"assert((c+f) = (c-(-1)));\n",
			"assert((c+f) = (c+1));\n",
			"\n",
			"c := d + f;\n",
			"assert(c = -2147483647);\n",
			"assert(d = -2147483648);\n",
			"assert(!(c = d));\n",
			"assert(d < c);\n",
			"\n",
			"c := e - f;\n",
			"assert(c = 2147483646);\n",
			"assert(e = 2147483647);\n",
			"assert(!(c = e));\n",
			"assert(c < e);\n",
			"assert(c = c);\n",
			"\n",
			"c := 2;\n",
			"d := 3;\n",
			"e := c * d;\n",
			"f := c * (-1 * c);\n",
			"\n",
			"assert(e = 6);\n",
			"assert(f = -4);\n",
			"assert((12 / -3) = f);\n",
			"assert((14 / -3) = f);\n",
			"var g :int:= (((1+1) + 1) + 1) - (1 - (1 - (1 - 1)));\n",
			"assert(g = 4);\n",
			"var h : int := -1 * ((((1 + 1))) + ((1 + 1) + ((1 + 1))));\n",
			"assert(h = -6);\n",
			"\n",
			"var i : int;\n",
			"var j : int := 2;\n",
			"var k : int := 1;\n",
			"\n",
			"for i in 1..10 do\n",
			"	k := j * k;\n",
			"	print k;\n",
			"	print \"\t\";\n",
			"end for;\n",
			"\n",
			"print \"\n\";\n",
			"\n",
			"assert(k = 1024);\n",
			"\n",
			"for i in 1..10 do\n",
			"	k := k / j;\n",
			"	print k;\n",
			"	print \"\t\";\n",
			"end for;\n",
			"\n",
			"print \"\n\n\";\n",
			"assert(k = 1);\n",
			"\n",
			"\n",
			"\n",
			"\n",
			"\n",
			"/**		***\n",
			"** STRINGS ***\n",
			"**		*	/	*	*/\n",
			"\n",
			"\n",
			"var l : string;\n",
			"var m : string := \"marvellous\";\n",
			"var n : string := \"things\";\n",
			"\n",
			"assert(m < n);\n",
			"assert(!(m = n));\n",
			"assert(l = \"\");\n",
			"\n",
			"l := (m + \" \") + n;\n",
			"\n",
			"assert(l = \"marvellous things\");\n",
			"assert(l < n);\n",
			"assert(m < l);\n",
			"\n",
			"m := l;\n",
			"assert(m = l);\n",
			"\n",
			"print \"This assertion should fail:\n\";\n",
			"assert((!(a)) & b);\n",
			"\n",
			"print (\"\nAll \" + \"is\") + (\" well!\" + (\"\n\" + \"\\\"Yeah!\\\"\"));\n"
		};
	}
}

