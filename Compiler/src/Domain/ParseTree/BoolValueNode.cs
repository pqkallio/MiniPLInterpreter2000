using System;
using System.Collections;

namespace MiniPLInterpreter
{
	public class BoolValueNode : IExpressionNode, ISemanticCheckValue
	{
		private bool value;
		private Token token;

		public BoolValueNode (bool value)
			: this(value, new Token (0, 0, "", TokenType.BOOL_VAL))
		{}

		public BoolValueNode (bool value, Token t)
		{
			this.value = value;
			this.token = t;
		}

		public TokenType EvaluationType
		{
			get { return TokenType.BOOL_VAL; }
			set { }
		}

		public TokenType Type ()
		{
			return TokenType.BOOL_VAL;
		}

		public object execute ()
		{
			return this.value;
		}

		public bool Value {
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
			return visitor.VisitBoolValueNode (this);
		}

		public IProperty asProperty ()
		{
			return new BooleanProperty(Value);
		}

		public Token Token
		{
			get { return this.token; }
			set { }
		}
	}
}

