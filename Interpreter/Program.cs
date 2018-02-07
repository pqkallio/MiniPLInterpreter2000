using System;
using System.Collections.Generic;
using MiniPLInterpreter;

namespace Interpreter
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			if (args.Length < 1) {
				return;
			}

			string input = System.IO.File.ReadAllText(@args[0]);

			Scanner scanner = new Scanner ();
			List<Token> tokens = scanner.tokenize (input);

			foreach (Object o in tokens) {
				Console.WriteLine (o);
			}

			if (scanner.getErrors ().Count > 0) {
				Console.WriteLine ("\n\nErrors encoutered during scanning:\n");
			
				foreach (Error e in scanner.getErrors()) {
					Console.WriteLine (e);
				}
			}
		}
	}
}
