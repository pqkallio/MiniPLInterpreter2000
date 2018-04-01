using System;
using System.Collections.Generic;
using MiniPLInterpreter;

namespace MiniPLInterpreterTests
{
	public class MockPrinter : IPrinter
	{
		public int RuntimeErrors;
		public int NonRuntimeErrors;
		public int Prints;

		public MockPrinter ()
		{
			this.RuntimeErrors = 0;
			this.NonRuntimeErrors = 0;
			this.Prints = 0;
		}

		public void printErrors (List<Error> errors) {
			foreach (Error e in errors) {
				printError (e);
			}
		}

		public void printError (Error error) {
			this.NonRuntimeErrors++;
		}

		public void print (string str) {
			this.Prints++;
		}

		public void printLine (string str) {
			this.Prints++;
		}

		public void printRuntimeException (RuntimeException exception) {
			this.RuntimeErrors++;
		}
	}
}

