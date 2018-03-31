using System;

namespace MiniPLInterpreterTests
{
	public class ParserTestInputs
	{
		public static readonly string[] emptyInput = {};

		public static readonly string[] statementNotEnded =
		{"var x : int := 4 + (6 * 2)"};

		public static readonly string[] rightParenthesisMissing =
		{"var x : int := 4 + (6 * 2;"};

		public static readonly string[] declarationColonMissing =
		{"var x  int := 4 + (6 * 2);"};

		public static readonly string[] declarationAssignMissing =
		{"var x : int = 4 + (6 * 2);"};

		public static readonly string[] declarationAssignValueMissing =
		{"var x : int := ;"};

		public static readonly string[] illegalStartOfStatement =
		{"(6 * 2) var x : int := 4 + (6 * 2);"};

		public static readonly string[] forLoopMissingVar =
		{
			"for  in 0..nTimes-1 do ",
			"   print x;",
			"   print \" : Hello, World!\\n\";",
			"end for;"
		};

		public static readonly string[] forLoopMissingRangeFrom =
		{
			"for x  0..nTimes-1 do ",
			"   print x;",
			"   print \" : Hello, World!\\n\";",
			"end for;"
		};

		public static readonly string[] forLoopMissingRangeFromExpression =
		{
			"for x in ..nTimes-1 do ",
			"   print x;",
			"   print \" : Hello, World!\\n\";",
			"end for;"
		};

		public static readonly string[] forLoopMissingRangeUptoExpression =
		{
			"for x in 0.. do ",
			"   print x;",
			"   print \" : Hello, World!\\n\";",
			"end for;"
		};

		public static readonly string[] forLoopMissingRangeUpto =
		{
			"for x in 0 30 do ",
			"   print x;",
			"   print \" : Hello, World!\\n\";",
			"end for;"
		};

		public static readonly string[] forLoopMissingStartBlock =
		{
			"for x in 0..nTimes-1 ",
			"   print x;",
			"   print \" : Hello, World!\\n\";",
			"end for;"
		};

		public static readonly string[] forLoopNoStatements =
		{
			"for x in 0..nTimes-1 do",
			"end for;"
		};

		public static readonly string[] forLoopMissingEnd =
		{
			"for x in 0..nTimes-1 do",
			"   print x;",
			"   print \" : Hello, World!\\n\";",
			"for;"
		};

		public static readonly string[] forLoopMissingForAfterEnd =
		{
			"for x in 0..nTimes-1 ",
			"   print x;",
			"   print \" : Hello, World!\\n\";",
			"end;"
		};

		public static readonly string[] forLoopMissingEndStatementAfterEndFor =
		{
			"for x in 0..nTimes-1 ",
			"   print x;",
			"   print \" : Hello, World!\\n\";",
			"end for"
		};

		public static readonly string[] integerOverflowOnPositiveInt = {
			"var too_big : int := 2147483648;"
		};

		public static readonly string[] integerOverflowOnNegativeInt = {
			"var too_small : int := -2147483649;"
		};

		public static readonly string[] noIntegerOverflowOnPositiveInt = {
			"var big : int := 2147483647;"
		};

		public static readonly string[] noIntegerOverflowOnNegativeInt = {
			"var small : int := -2147483648;"
		};

		public static readonly string[] validDeclaration = {
			"var banana : string;"
		};

		public static readonly string[] invalidTypeOnDeclaration = {
			"var banana : strng;"
		};

		public static readonly string[] invalidIdInDeclaration = {
			"var ~banana : string;"
		};

		public static readonly string[] invalidDeclarationStart = {
			"?var banana : string;"
		};

		public static readonly string[] threeErrorsInDeclaration = {
			"?var ~banana : strng;"
		};

		public static readonly string[] readStatementOk = {
			"read a;"
		};

		public static readonly string[] readStatementMissingVarId = {
			"read ;"
		};

		public static readonly string[] readStatementInvalidId = {
			"read 3;"
		};

		public static readonly string[] printStatementOk = {
			"print a;"
		};

		public static readonly string[] printStatementMissingExpression = {
			"print ;"
		};

		public static readonly string[] printStatementInvalidId = {
			"print var;"
		};

		public static readonly string[] assertOk = {
			"assert (a = true);"
		};

		public static readonly string[] assertNoParentheses = {
			"assert a = true;"
		};

		public static readonly string[] validMassiveInputForParserTesting =
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

