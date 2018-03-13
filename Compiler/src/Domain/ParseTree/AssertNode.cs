using System;
using System.Collections;

namespace MiniPLInterpreter
{
	public class AssertNode : IExpressionContainer
	{
		private IExpressionNode expressionNode;

		public AssertNode ()
		{}

		public TokenType Type ()
		{
			return TokenType.ASSERT;
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

		public void Accept(NodeVisitor visitor) {
			visitor.VisitAssertNode (this);
		}
	}
}

