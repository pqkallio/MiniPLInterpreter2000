using System;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	public class Printer
	{
		private string[] sourceLines;

		public Printer (string[] sourceLines)
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

		public void print (string str)
		{
			Console.Write (str);
		}

		public void printLine (string str)
		{
			Console.WriteLine (str);
		}
	}
}

