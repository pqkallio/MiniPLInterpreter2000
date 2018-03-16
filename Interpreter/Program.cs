using System;
using System.IO;
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
				Scanner s = new Scanner (sr);
				p.Scanner = s;
				p.Parse ();

				Console.WriteLine ("*************");

				foreach (Error e in s.getErrors()) {
					int line = e.Token.Row;
					int column = e.Token.Column;

					string l = GetLine (@args [0], line);
					Console.WriteLine (e);
					Console.WriteLine (l);
					string x = "";
					for (int i = 1; i < column; i++) {
						x += ' ';
					}
					x += '^';
					Console.WriteLine (x);
				}

				Console.WriteLine ("*************");

				foreach (Error e in p.getErrors()) {
					int line = e.Token.Row;
					int column = e.Token.Column;

					string l = GetLine (@args [0], line);
					Console.WriteLine (e);
					Console.WriteLine (l);
					string x = "";
					for (int i = 1; i < column; i++) {
						x += ' ';
					}
					x += '^';
					Console.WriteLine (x);
				}

				/*
				SyntaxTree tree = p.SyntaxTree;

				Queue q = tree.NodeOrder ();

				while (q.Count > 0) {
					object o = q.Dequeue ();
					Console.WriteLine (o);
				}
				*/

				Console.WriteLine ("*************");
				if (p.SyntaxTreeBuilt) {
					SemanticAnalyzer se = new SemanticAnalyzer (p.SyntaxTree, ids);
					se.Analyze ();
					Console.WriteLine ("*************");

					foreach (Error e in se.getErrors ()) {
						Console.WriteLine (e);
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
