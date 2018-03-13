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
					Console.WriteLine (e);
				}

				Console.WriteLine ("*************");

				foreach (Error e in p.getErrors()) {
					Console.WriteLine (e);
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
					SemanticAnalyzer se = new SemanticAnalyzer (p.SyntaxTree.NodeOrder (), ids);
					se.Analyze ();
					Console.WriteLine ("*************");

					foreach (Error e in se.getErrors ()) {
						Console.WriteLine (e);
					}
				}
			}
		}
	}
}
