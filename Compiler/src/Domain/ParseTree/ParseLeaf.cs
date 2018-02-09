using System;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	public class ParseLeaf
	{
		private Token token;
		private ParseLeaf parent;
		private List<ParseLeaf> children;

		public ParseLeaf (ParseLeaf parent, Token token)
		{
			this.token = token;
			this.parent = parent;
			this.children = new List<ParseLeaf> ();
		}

		public Token Token {
			get { return token; }
		}

		public void AddChild(ParseLeaf child)
		{
			this.children.Add (child);
		}
	}
}

