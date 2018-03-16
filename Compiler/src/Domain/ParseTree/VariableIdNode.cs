using System;
using System.Collections;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	public class VariableIdNode : IExpressionNode
	{
		private string id;
		private Dictionary<string, IProperty> ids;
		private TokenType variableType;
		private Token token;

		public VariableIdNode(Dictionary<string, IProperty> ids, Token token)
			: this(null, ids, token)
		{}

		public VariableIdNode (string id, Dictionary<string, IProperty> ids, Token token)
		{
			this.id = id;
			this.ids = ids;
			this.token = token;
		}

		public string ID {
			get { return id; }
			set { id = value; }
		}

		public object execute ()
		{
			return this.ids[ID];
		}

		public TokenType Type ()
		{
			return TokenType.ID;
		}

		public TokenType VariableType
		{
			get { return variableType; }
			set { variableType = value; }
		}

		public override string ToString ()
		{
			return "id: " + ID;
		}

		public void AddNodesToQueue (Queue q)
		{
			q.Enqueue (this);
		}

		public TokenType GetEvaluationType (TokenType parentType)
		{
			TokenType thisType = ids [ID].GetTokenType ();
			if (parentType == TokenType.UNDEFINED) {
				return thisType;
			}

			if (parentType != thisType) {
				return TokenType.ERROR;
			}

			return thisType;
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
			return ids[ID].GetTokenType ();
		}

		public ISemanticCheckValue Accept(INodeVisitor visitor) {
			return visitor.VisitVariableIdNode (this);
		}

		public Token Token
		{
			get { return this.token; }
			set { }
		}
	}
}

