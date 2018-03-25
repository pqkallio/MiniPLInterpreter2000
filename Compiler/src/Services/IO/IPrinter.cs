using System;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	public interface IPrinter
	{
		void printErrors (List<Error> errors);
		void printError (Error error);
		void printAssertionFailure (AssertNode assertNode);
		void print (string str);
		void printLine (string str);
	}
}

