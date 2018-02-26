using System;

namespace MiniPLInterpreter
{
	public class RootNode : ISyntaxTreeNode
	{
		private ISyntaxTreeNode sequitor;

		public RootNode ()
		{
			this.sequitor = null;
		}

		public ISyntaxTreeNode Sequitor {
			get { return sequitor; }
			set { sequitor = value; }
		}

		public TokenType Type () {
			return TokenType.PROGRAM;
		}

		public object execute () {
			return sequitor.execute ();
		}
	}
}

