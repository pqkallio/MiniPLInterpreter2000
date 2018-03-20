using System;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	public class Printer
	{
		private List<string> sourceLines;

		public Printer (List<string> sourceLines)
		{
			this.sourceLines = sourceLines;
		}

		public void printErrors (List<Error> errors)
		{
			foreach (Error error in errors) {
				printError (error);
			}
		}

		public void printError (Error error)
		{
			printLine(StringFormatter.formatError (error, sourceLines));
		}

		public void printAssertionFailure (AssertNode assertNode)
		{
			printLine (StringFormatter.formatFailedAssertion (assertNode, sourceLines));
		}

		public static void print (string str)
		{
			Console.Write (str);
		}

		public static void printLine (string str)
		{
			Console.WriteLine (str);
		}
	}
}

