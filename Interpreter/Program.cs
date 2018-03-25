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
			System.Text.Encoding encoding = System.Text.Encoding.UTF8;
			Console.OutputEncoding = encoding;

			Dictionary<string, IProperty> ids = new Dictionary<string, IProperty> ();
			Parser p = new Parser (ids);
			string[] sourceLines = File.ReadLines (@args [0], encoding).ToArray ();
			ConsolePrinter printer = new ConsolePrinter (sourceLines);
			Scanner s = new Scanner (sourceLines);
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
