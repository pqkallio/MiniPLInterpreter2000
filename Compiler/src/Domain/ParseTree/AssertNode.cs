using System;
using System.Collections;

namespace MiniPLInterpreter
{
	public class AssertNode : IExpressionContainer
	{
		private IExpressionNode expressionNode;
		private Token token;
		private int assertStatementRow;
		private int assertStatementStartCol;
		private int assertStatementEndCol;

		public AssertNode (Token t)
			: this(t, 0, 0)
		{}

		public AssertNode (Token t, int assertStatementRow, int assertStatementStartCol)
		{
			this.token = token;
			this.assertStatementRow = assertStatementRow;
			this.assertStatementStartCol = assertStatementStartCol;
		}

		public TokenType Type ()
		{
			return TokenType.ASSERT;
		}

		public int AssertStatementRow
		{
			get { return this.assertStatementRow; }
			set { this.assertStatementRow = value; }
		}

		public int AssertStatementStartCol
		{
			get { return this.assertStatementStartCol; }
			set { this.assertStatementStartCol = value; }
		}

		public int AssertStatementEndCol
		{
			get { return this.assertStatementEndCol; }
			set { this.assertStatementEndCol = value; }
		}

		public IExpressionNode Expression
		{
			get { return expressionNode; }
		}

		public object execute ()
		{
			bool eval = (bool)((ISyntaxTreeNode)expressionNode).execute ();

			if (!eval) {
				Console.WriteLine (String.Format("Assertion failed: {0}", expressionNode.ToString()));
			}

			return null;
		}

		public void AddExpression (IExpressionNode expressionNode)
		{
			this.expressionNode = expressionNode;
		}

		public override string ToString ()
		{
			return "ASSERT";
		}

		public void AddNodesToQueue(Queue q)
		{
			q.Enqueue (this);
			// expressionNode.AddNodesToQueue (q);
		}

		public ISemanticCheckValue Accept(INodeVisitor visitor) {
			return visitor.VisitAssertNode (this);
		}

		public Token Token
		{
			get { return this.token; }
			set { }
		}
	}
}

