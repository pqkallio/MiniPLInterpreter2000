using System;
using System.Collections;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	public class DeclarationNode : ISyntaxTreeNode
	{
		private VariableIdNode idNode;
		private AssignNode assignNode;
		private Dictionary<string, IProperty> ids;
		private Token token;

		public DeclarationNode (VariableIdNode idNode, Dictionary<string, IProperty> ids, Token t)
		{
			this.idNode = idNode;
			this.ids = ids;
			this.token = t;
		}

		public AssignNode AssignNode
		{
			get { return assignNode; }
			set { assignNode = value; }
		}

		public object execute ()
		{
			return null;
		}

		public VariableIdNode IDNode
		{
			get { return idNode; }
			set { this.idNode = value; }
		}

		public TokenType Type()
		{
			return TokenType.DECLARATION;
		}

		public void AddNodesToQueue(Queue q)
		{
			q.Enqueue (this);
		}

		public ISemanticCheckValue Accept(INodeVisitor visitor) {
			return visitor.VisitDeclarationNode (this);
		}

		public Token Token
		{
			get { return this.token; }
			set { }
		}
	}
}

