using System;
using System.Collections;

namespace MiniPLInterpreter
{
	public class IntValueNode : IExpressionNode, ISemanticCheckValue
	{
		private int value;
		private Token token;

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

		public TokenType Operation 
		{
			get { return TokenType.BINARY_OP_NO_OP; }
			set { }
		}

		public TokenType GetValueType ()
		{
			return Type ();
		}

		public ISemanticCheckValue Accept(INodeVisitor visitor) {
			return visitor.VisitIntValueNode (this);
		}

		public IProperty asProperty ()
		{
			return new IntegerProperty(Value);
		}

		public Token Token
		{
			get { return this.token; }
			set { }
		}
	}
}

