using System;
using System.IO;
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
			using (StreamReader sr = new StreamReader (@args [0])) {
				Parser p = new Parser ();
				Scanner s = new Scanner (sr);
				p.Scanner = s;
				p.Parse ();

				Console.WriteLine ("*************");

				foreach (Error e in s.getErrors()) {
					Console.WriteLine (e);
				}

				Console.WriteLine ("*************");

				foreach (Error e in p.getErrors()) {
					Console.WriteLine (e);
				}
			}
			/*
			string input = System.IO.File.ReadAllText(@args[0]);

			Scanner scanner = new Scanner ();
			List<Token> tokens = scanner.tokenize (input);

			foreach (Object o in tokens) {
				Console.WriteLine (o);
			}

			if (scanner.getErrors ().Count > 0) {
				Console.WriteLine ("\n\nErrors were encountered while scanning:\n");
			
				foreach (Error e in scanner.getErrors()) {
					Console.WriteLine (e);
				}
			}

			Parser parser = new Parser (tokens);
			parser.ParseTokens ();

			if (parser.getErrors ().Count > 0) {
				Console.WriteLine ("\n\nErrors were encountered while parsing:\n");

				foreach (Error e in parser.getErrors()) {
					Console.WriteLine (e);
				}
			}
			*/
		}
	}
}
