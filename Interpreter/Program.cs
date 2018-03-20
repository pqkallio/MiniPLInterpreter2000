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
			if (args.Length < 1) {
				return;
			}
			using (StreamReader sr = new StreamReader (@args [0])) {
				Dictionary<string, IProperty> ids = new Dictionary<string, IProperty> ();
				Parser p = new Parser (ids);
				Printer printer = new Printer (File.ReadLines(@args [0]).ToList ());
				Scanner s = new Scanner (sr);
				p.Scanner = s;
				p.Parse ();

				printer.printErrors (s.getErrors ());

				printer.printErrors (p.getErrors ());

				if (p.SyntaxTreeBuilt) {
					SemanticAnalyzer se = new SemanticAnalyzer (p.SyntaxTree, ids);
					se.Analyze ();

					printer.printErrors (se.getErrors ());

					if (se.getErrors ().Count == 0) {
						MiniPLInterpreter.Interpreter ip = new MiniPLInterpreter.Interpreter (p.SyntaxTree, printer);
						ip.Interpret ();
					}
				}
			}
		}

		private static string GetLine (string filename, int line)
		{
			using (StreamReader sr = new StreamReader (filename)) {
				for (int i = 1; i < line; i++) {
					sr.ReadLine ();
				}
				return sr.ReadLine ();
			}
		}
	}
}
