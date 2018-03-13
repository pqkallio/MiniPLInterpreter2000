using System;
using System.Collections;

namespace MiniPLInterpreter
{
	public class StringValueNode : IExpressionNode
	{
		private string value;

		public StringValueNode (string value)
		{
			this.value = value;
		}

		public TokenType Type ()
		{
			return TokenType.STR_VAL;
		}

		public object execute ()
		{
			return this.value;
		}

		public string Value {
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
			visitor.VisitStringValueNode (this);
		}
	}
}

