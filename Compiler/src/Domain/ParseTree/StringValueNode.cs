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

