using System;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args.Length < 1) {
				return;
			}

			string input = System.IO.File.ReadAllText(@args[0]);

			Scanner scanner = new Scanner ();
			List<Token> tokens = scanner.tokenize (input);

			/*
			for (int i = 0; i < input.Length; i++) {
				string token = "";
				string str = input [i];
				for (int j = 0; j < str.Length; j++) {
					char c = input [(int)i] [(int)j];
					if (!Char.IsWhiteSpace (c)) {
						token += c;
					} else if (!String.IsNullOrEmpty(token)) {
						Token t = new Token (i, j, token);
						tokens.Add (t);
						token = "";
					}
				}
				if (!String.IsNullOrEmpty (token)) {
					Token t = new Token (i, (int)str.Length, token);
					tokens.Add (t);
					token = "";
				}
			}
			*/

			foreach (Object o in tokens) {
				Console.WriteLine (o);
			}
		}
	}
}

