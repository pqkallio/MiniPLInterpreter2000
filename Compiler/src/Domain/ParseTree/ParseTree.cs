using System;

namespace MiniPLInterpreter
{
	public class ParseTree
	{
		private ParseLeaf root;

		public ParseTree()
			: this(null)
		{}

		public ParseTree (ParseLeaf root)
		{
			this.root = root;
		}

		public ParseLeaf Root
		{
			get { return this.root; }
			set { this.root = value; }
		}
	}
}

