using System;

namespace MiniPLInterpreter
{
	public class StatementsNode : ISyntaxTreeNode
	{
		public ISyntaxTreeNode statement;
		public ISyntaxTreeNode sequitor;

		public StatementsNode ()
		{
			this.statement = null;
			this.sequitor = null;
		}

		public object execute ()
		{
			if (this.statement != null) {
				this.statement.execute ();
			}

			if (this.sequitor != null) {
				this.sequitor.execute ();
			}

			return null;
		}

		public TokenType Type ()
		{
			return TokenType.STATEMENTS;
		}

		public ISyntaxTreeNode Statement {
			get { return statement; }
			set { statement = value; }
		}

		public ISyntaxTreeNode Sequitor {
			get { return sequitor; }
			set { sequitor = value; }
		}
	}
}

