using System;
using System.Collections;

namespace MiniPLInterpreter
{
	public class SyntaxTree
	{
		private IStatementsContainer root;

		public SyntaxTree (IStatementsContainer root)
		{
			this.root = root;
		}

		public SyntaxTree ()
			: this (null)
		{
		}

		public void execute ()
		{
			if (root != null) {
				root.execute ();
			}
		}

		public IStatementsContainer Root {
			get { return root; }
			set { root = value; }
		}

		public Queue NodeOrder ()
		{
			Queue q = new Queue ();
			root.AddNodesToQueue (q);
			return q;
		}

		public override string ToString ()
		{
			return root.ToString ();
		}
	}
}
