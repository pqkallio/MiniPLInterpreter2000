using System;

namespace MiniPLInterpreter
{
	public class SyntaxTree
	{
		private ISyntaxTreeNode root;

		public SyntaxTree ()
		{
			this.root = null;
		}

		public void execute ()
		{
			if (root != null) {
				root.execute ();
			}
		}

		public ISyntaxTreeNode Root {
			get { return root; }
			set { root = value; }
		}
	}
}
