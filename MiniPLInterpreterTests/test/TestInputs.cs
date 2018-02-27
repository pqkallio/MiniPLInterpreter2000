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
	}
}