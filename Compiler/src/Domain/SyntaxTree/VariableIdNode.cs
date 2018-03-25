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

		public VariableIdNode(Dictionary<string, IProperty> ids)
			: this(null, ids, null)
		{}

		public VariableIdNode(Dictionary<string, IProperty> ids, Token token)
			: this(null, ids, token)
		{}

		public VariableIdNode (string id, Dictionary<string, IProperty> ids, Token token)
		{
			this.id = id;
			this.ids = ids;
			this.token = token;
		}

		public TokenType EvaluationType
		{
			get { return ids [id].GetTokenType (); }
			set { }
		}

		public string ID {
			get { return id; }
			set { id = value; }
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

		public IExpressionNode[] GetExpressions()
		{
			return null;
		}

		public TokenType Operation
		{
			get { return TokenType.BINARY_OP_NO_OP; }
			set { }
		}

		public ISemanticCheckValue Accept(INodeVisitor visitor) {
			return visitor.VisitVariableIdNode (this);
		}

		public Token Token
		{
			get { return this.token; }
			set { this.token = value; }
		}
	}
}