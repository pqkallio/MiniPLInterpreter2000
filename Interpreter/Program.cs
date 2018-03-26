using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using MiniPLInterpreter;

namespace Interpreter
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			ConsolePrinter cp = new ConsolePrinter ();
			ConsoleReader cr = new ConsoleReader ();

			if (args.Length < 1) {
				cp.printLine ("Please give a file path to open as argument, e.g.\nInterpreter.exe C:\\path\\file.txt");
				return;
			}

			string filePath = @args [0];

			CompilerFrontend cf = new CompilerFrontend ();
			SyntaxTree syntaxTree = null;

			try {
				syntaxTree = cf.Compile (filePath);
			} catch (Exception ex) {
				Console.WriteLine ("Unexpected exception:\n" + ex.GetType ().Name + " " + ex.Message);
				Console.WriteLine ("\nExecution halted");
			}

			cp.SourceLines = cf.SourceLines;
			List<Error> errors = cf.getErrors ();

			if (cf.getErrors ().Count > 0) {
				cp.printErrors (errors);
				return;
			}

			try {
				MiniPLInterpreter.Interpreter interpreter = new MiniPLInterpreter.Interpreter (syntaxTree, cp, cr);
				interpreter.Interpret ();
			} catch {
				Console.WriteLine ("\nExecution halted");
			}
		}
	}
}
