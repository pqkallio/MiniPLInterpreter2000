﻿using System;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	public class Interpreter
	{
		private SyntaxTree syntaxTree;
		private Dictionary<string, IProperty> ids;
		private ExecutionVisitor executor;

		public Interpreter (SyntaxTree syntaxTree)
		{
			this.syntaxTree = syntaxTree;
			this.ids = new Dictionary<string, IProperty> ();
			this.executor = new ExecutionVisitor (ids);
		}

		public void Interpret () {
			try {
				this.syntaxTree.Root.Accept (this.executor);
			} catch (Exception ex) {
				Console.WriteLine ("An error occured during execution:\n" + ex.Message);
			}
		}

		public Dictionary<string, IProperty> IDs
		{
			get { return ids; }
		}
	}
}
