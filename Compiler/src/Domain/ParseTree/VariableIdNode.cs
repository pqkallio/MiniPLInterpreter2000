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

		public VariableIdNode(Dictionary<string, IProperty> ids)
			: this(null, ids)
		{}

		public VariableIdNode (string id, Dictionary<string, IProperty> ids)
		{
			this.id = id;
			this.ids = ids;
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

		public TokenType GetOperation ()
		{
			return TokenType.BINARY_OP_NO_OP;
		}

		public TokenType GetValueType ()
		{
			return ids[ID].GetTokenType ();
		}

		public void Accept(NodeVisitor visitor) {
			visitor.VisitVariableIdNode (this);
		}
	}
}

