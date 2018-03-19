using System;
using System.Collections;

namespace MiniPLInterpreter
{
	public class RootNode : IStatementsContainer
	{
		private StatementsNode sequitor;
		private Token token;

		public RootNode ()
		{
			this.sequitor = null;
			this.token = new Token (1, 0, null, TokenType.PROGRAM);
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

		public void AddNodesToQueue (Queue q)
		{
			q.Enqueue (this);
			if (sequitor != null) {
				sequitor.AddNodesToQueue (q);
			}
		}

		public ISemanticCheckValue Accept(INodeVisitor visitor) {
			return visitor.VisitRootNode (this);
		}

		public Token Token
		{
			get { return this.token; }
			set { }
		}

		public override string ToString ()
		{
			return "ROOT";
		}
	}
}

