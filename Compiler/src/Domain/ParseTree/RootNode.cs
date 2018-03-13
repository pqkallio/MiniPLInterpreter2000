using System;
using System.Collections;

namespace MiniPLInterpreter
{
	public class RootNode : IStatementsContainer
	{
		private StatementsNode sequitor;

		public RootNode ()
		{
			this.sequitor = null;
		}

		public StatementsNode Sequitor {
			get { return sequitor; }
			set { sequitor = value; }
		}

		public TokenType Type () {
			return TokenType.PROGRAM;
		}

		public object execute () {
			return sequitor.execute ();
		}

		public override string ToString ()
		{
			return "ROOT";
		}

		public void AddNodesToQueue (Queue q)
		{
			q.Enqueue (this);
			if (sequitor != null) {
				sequitor.AddNodesToQueue (q);
			}
		}

		public void Accept(NodeVisitor visitor) {
			visitor.VisitRootNode (this);
		}
	}
}

