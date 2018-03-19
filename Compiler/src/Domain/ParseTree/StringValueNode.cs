using System;
using System.Collections;

namespace MiniPLInterpreter
{
	public class StringValueNode : IExpressionNode, ISemanticCheckValue
	{
		private string value;
		private Token token;

		public StringValueNode (string value)
		{
			this.value = value;
			this.token = new Token (0, 0, "", TokenType.STR_VAL);
		}

		public StringValueNode (string value, Token t)
		{
			this.value = value;
			this.token = t;
		}

		public TokenType EvaluationType
		{
			get { return TokenType.STR_VAL; }
			set { }
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
			return visitor.VisitStringValueNode (this);
		}

		public IProperty asProperty ()
		{
			return new StringProperty(Value);
		}

		public Token Token
		{
			get { return this.token; }
			set { }
		}
	}
}

