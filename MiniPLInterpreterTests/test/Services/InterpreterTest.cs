using System;
using System.IO;
using System.Collections.Generic;
using NUnit.Framework;
using MiniPLInterpreter;

namespace MiniPLInterpreterTests
{
	[TestFixture ()]
	public class InterpreterTest
	{
		private Scanner s;
		private Parser p;
		private SemanticAnalyzer sa;
		private Interpreter interpreter;

		public InterpreterTest ()
		{}

		private bool InitInterpreter (string[] s, IReader reader)
		{
			Dictionary<string, IProperty> ids = new Dictionary<string, IProperty> ();
			this.s = new Scanner (s);
			this.p = new Parser (ids);
			this.p.Scanner = this.s;
			SyntaxTree tree = this.p.Parse ();
			if (!this.p.SyntaxTreeBuilt)
				return false;
			this.sa = new SemanticAnalyzer (tree, ids);
			sa.Analyze ();
			if (this.sa.getErrors ().Count > 0)
				return false;
			this.interpreter = new Interpreter (tree, new ConsolePrinter (), reader);
			return true;
		}

		private bool Interpret (string[] s, IReader reader)
		{
			bool initOk = InitInterpreter (s, reader);
			if (!initOk)
				return false;
			interpreter.Interpret ();
			return true;
		}


	}
}

