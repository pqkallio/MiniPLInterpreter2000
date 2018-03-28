using System;


namespace MiniPLInterpreterTests
{
	public class ScannerTestInputs
	{
		public static readonly string[] validInput1 = {
			"// ///// One line comment",
			"",
			"var nTimes3:int:=0;",
			"print \"How many times?\"; ",
			"read nTimes; ",
			"",
			"/*** // Some comments here",
			"\tspread on multiple",
			"\tlines ******/",
			"",
			"var x : int;",
			"for x in 0..nTimes-1 do ",
			"   print x;",
			"   print \" : Hello, World!\\n\";",
			"end for;",
			"assert (x = nTimes);"};

		public static readonly string[] validInput2 =
		{"var X : int := 4 + (6 * 2);",
			"print X;"};

		public static readonly string[] validInput3 =
			{"print \"Give a number\";",
			"var n : int;",
			"read n;",
			"var v : int := 1;",
			"var i : int;",
			"for i in 1..n do ",
			"    v := v * i;",
			"end for;",
			"print \"The result is: \";",
			"print v; "};

		public static readonly string[] validInput4 =
		{""};

		public static readonly string[] invalidInput1 =
		{"// ///// One line comment",
			"",
			"var nTimes3:int:=0;",
			"print \"How many times?\"; ",
			"read nTimes; ",
			"",
			"/*** // Some comments here",
			"\tspread on multiple",
			"\tlines ******/",
			"",
			"var x : int;",
			"for x in 0..nTimes-1 do ",
			"   print x;",
			"   print \" : Hello, World!\\n;",
			"end for;",
			"assert (x = nTimes);"};

		public static readonly string[] invalidInput2 =
		{"// ///// One line comment",
			"",
			"?var nTimes3:int:=0;",
			"print \"How many times?\"; ",
			"read nTimes; ",
			"var x : int;",
			"for x in 0..nTimes-1 do ",
			"   print x;",
			"   print \" : Hello, World!\\n\";",
			"end for;",
			"assert (x = nTimes);"};

		public static readonly string[] invalidInput3 =
		{
			"var nTimes3:int:=0;",
			"print \"How many times?\";",
			"read nTimes; ",
			"var var : int;",
			"for x in 0..nTimes-1 do ",
			"   print x;",
			"   print \" : Hello, World!\\n\";",
			"end for;",
			"assert (x = nTimes);"};

		public static readonly string[] invalidInput4 =
		{"var for : string := \"4 + (6 * 2)?",
			"98\";",
			"print X;"};
	}
}