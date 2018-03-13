using System;
using System.Collections;

namespace MiniPLInterpreter
{
	public class StatementsNode : IStatementsContainer
	{
		public ISyntaxTreeNode statement;
		public StatementsNode sequitor;

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

		public StatementsNode Sequitor {
			get { return sequitor; }
			set { sequitor = value; }
		}

		public void AddNodesToQueue (Queue q)
		{
			if (statement != null) {
				q.Enqueue (statement);
			}

			if (sequitor != null) {
				sequitor.AddNodesToQueue (q);
			}
		}

		public void Accept(NodeVisitor visitor) {
			visitor.VisitStatementsNode (this);
		}
	}
}

