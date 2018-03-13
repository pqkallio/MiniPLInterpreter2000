using System;


namespace MiniPLInterpreterTests
{
	public class TestInputs
	{
		public static readonly string validInput1 =
			"// ///// One line comment\n" +
			"\n" +
			"var nTimes3:int:=0;\n" +
			"print \"How many times?\"; \n" +
			"read nTimes; \n" +
			"\n" +
			"/*** // Some comments here\n" +
			"\tspread on multiple\n" +
			"\tlines ******/\n" +
			"\n" +
			"var x : int;\n" +
			"for x in 0..nTimes-1 do \n" +
			"   print x;\n" +
			"   print \" : Hello, World!\\n\";\n" +
			"end for;\n" +
			"assert (x = nTimes);";

		public static readonly string validInput2 =
			"var X : int := 4 + (6 * 2);\n" +
			"print X;";

		public static readonly string validInput3 =
			"print \"Give a number\"; \n" +
			"var n : int;\n" +
			"read n;\n" +
			"var v : int := 1;\n" +
			"var i : int;\n" +
			"for i in 1..n do \n" +
			"    v := v * i;\n" +
			"end for;\n" +
			"print \"The result is: \";\n" +
			"print v; ";

		public static readonly string validInput4 =
			"";

		public static readonly string invalidInput1 =
			"// ///// One line comment\n" +
			"\n" +
			"var nTimes3:int:=0;\n" +
			"print \"How many times?\"; \n" +
			"read nTimes; \n" +
			"\n" +
			"/*** // Some comments here\n" +
			"\tspread on multiple\n" +
			"\tlines ******/\n" +
			"\n" +
			"var x : int;\n" +
			"for x in 0..nTimes-1 do \n" +
			"   print x;\n" +
			"   print \" : Hello, World!\\n;\n" +
			"end for;\n" +
			"assert (x = nTimes);";

		public static readonly string invalidInput2 =
			"// ///// One line comment\n" +
			"\n" +
			"?var nTimes3:int:=0;\n" +
			"print \"How many times?\"; \n" +
			"read nTimes; \n" +
			"\n" +
			"/*** // Some comments here\n" +
			"\tspread on multiple\n" +
			"\tlines ******/\n" +
			"\n" +
			"var x : int;\n" +
			"for x in 0..nTimes-1 do \n" +
			"   print x;\n" +
			"   print \" : Hello, World!\\n\";\n" +
			"end for;\n" +
			"assert (x = nTimes);";

		public static readonly string invalidInput3 =
			"// ///// One line comment\n" +
			"\n" +
			"var nTimes3:int:=0;\n" +
			"print \"How many times?\"; \n" +
			"read nTimes; \n" +
			"\n" +
			"/*** // Some comments here\n" +
			"\tspread on multiple\n" +
			"\tlines ******/\n" +
			"\n" +
			"var var : int;\n" +
			"for x in 0..nTimes-1 do \n" +
			"   print x;\n" +
			"   print \" : Hello, World!\\n\";\n" +
			"end for;\n" +
			"assert (x = nTimes);";

		public static readonly string invalidInput4 =
			"var for : int := 4 + (6 * 2)?\n" +
			"print X;";

		public static readonly string statementNotEnded =
			"var x : int := 4 + (6 * 2)\n";
		
		public static readonly string rightParenthesisMissing =
			"var x : int := 4 + (6 * 2;\n";

		public static readonly string declarationColonMissing =
			"var x  int := 4 + (6 * 2);\n";

		public static readonly string declarationAssignMissing =
			"var x : int = 4 + (6 * 2);\n";
		
		public static readonly string declarationAssignValueMissing =
			"var x : int := ;\n";

		public static readonly string illegalStartOfStatement =
			"(6 * 2) var x : int := 4 + (6 * 2);\n";

		public static readonly string forLoopMissingVar =
			"for  in 0..nTimes-1 do \n" +
			"   print x;\n" +
			"   print \" : Hello, World!\\n\";\n" +
			"end for;\n";
		
		public static readonly string forLoopMissingRangeFrom =
			"for x  0..nTimes-1 do \n" +
			"   print x;\n" +
			"   print \" : Hello, World!\\n\";\n" +
			"end for;\n";

		public static readonly string forLoopMissingRangeFromExpression =
			"for x in ..nTimes-1 do \n" +
			"   print x;\n" +
			"   print \" : Hello, World!\\n\";\n" +
			"end for;\n";

		public static readonly string forLoopMissingRangeUptoExpression =
			"for x in 0.. do \n" +
			"   print x;\n" +
			"   print \" : Hello, World!\\n\";\n" +
			"end for;\n";

		public static readonly string forLoopMissingRangeUpto =
			"for x in 0 30 do \n" +
			"   print x;\n" +
			"   print \" : Hello, World!\\n\";\n" +
			"end for;\n";

		public static readonly string forLoopMissingStartBlock =
			"for x in 0..nTimes-1 \n" +
			"   print x;\n" +
			"   print \" : Hello, World!\\n\";\n" +
			"end for;\n";
	}
}