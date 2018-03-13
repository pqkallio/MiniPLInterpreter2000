using System;
using System.Collections;

namespace MiniPLInterpreter
{
	public class IntValueNode : IExpressionNode
	{
		private int value;

		public IntValueNode (int value)
		{
			this.value = value;
		}

		public TokenType Type ()
		{
			return TokenType.INT_VAL;
		}

		public object execute ()
		{
			return this.value;
		}

		public int Value {
			get { return this.value; }
			set { this.value = value; }
		}

		public void AddNodesToQueue (Queue q)
		{
			q.Enqueue (this);
		}

		public TokenType GetEvaluationType (TokenType parentType)
		{
			if (parentType == TokenType.UNDEFINED) {
				return Type();
			}

			if (parentType != Type()) {
				return TokenType.ERROR;
			}

			return Type();
		}

		public IExpressionNode[] GetExpressions()
		{
			return null;
		}

		public TokenType GetOperation ()
		{
			return TokenType.BINARY_OP_NO_OP;
		}

		public TokenType GetValueType ()
		{
			return Type ();
		}

		public void Accept(NodeVisitor visitor) {
			visitor.VisitIntValueNode (this);
		}
	}
}

