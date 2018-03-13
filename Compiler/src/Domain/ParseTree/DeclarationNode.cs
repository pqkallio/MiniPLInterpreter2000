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

		public DeclarationNode (VariableIdNode idNode, Dictionary<string, IProperty> ids)
		{
			this.idNode = idNode;
			this.ids = ids;
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

		public void Accept(NodeVisitor visitor) {
			visitor.VisitDeclarationNode (this);
		}
	}
}

