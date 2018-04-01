using System;

namespace MiniPLInterpreterTests
{
	public class InterpreterTestInput
	{
		public InterpreterTestInput ()
		{}

		public static readonly string[] declarationsWorkAsIntended =
		{
			"var x : bool;\n",
			"var y : string;\n",
			"var z : int;\n"
		};

		public static readonly string[] assignmentsWorkAsIntended =
		{
			"var x : bool := true;\n",
			"var y : string := \"foobar\";\n",
			"var z : int := 1984;\n"
		};

		public static readonly string[] intRead =
		{
			"var a : int;\n",
			"read a;\n"
		};

		public static readonly string[] tooBigAddition =
		{
			"var a : int := 2147483647 + 1;\n"
		};

		public static readonly string[] tooSmallSubtraction =
		{
			"var a : int := -2147483648 - 1;\n"
		};

		public static readonly string[] tooBigMultiplication =
		{
			"var a : int := 2147483647 * 2;\n"
		};

		public static readonly string[] divisionByZero =
		{
			"var a : int := 2147483647 / 0;\n"
		};

		public static readonly string[] failedAssertion = {
			"assert(true = false);"
		};

		public static readonly string[] printStatementInForLoop = {
			"var i : int;",
			"for i in 1..10 do",
			"print \"Hello world!\n\";",
			"end for;"
		};
	}
}

