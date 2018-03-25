using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace MiniPLInterpreter
{
	public class AssignNode : IExpressionContainer, IIdentifierContainer
	{
		private VariableIdNode idNode;
		private IExpressionNode exprNode;
		private Token token;

		public AssignNode (VariableIdNode idNode, Dictionary<string, IProperty> ids)
			: this (idNode, ids, null)
		{}

		public AssignNode (VariableIdNode idNode, Dictionary<string, IProperty> ids, Token t)
		{
			this.idNode = idNode;
			this.exprNode = null;
			this.token = t;
		}

		public VariableIdNode IDNode {
			get { return idNode; }
			set { idNode = value; }
		}

		public IExpressionNode ExprNode {
			get { return exprNode; }
			set { exprNode = value; }
		}

		public void AddExpression(IExpressionNode expressionNode)
		{
			this.exprNode = expressionNode;
		}

		public override string ToString ()
		{
			return "DECLARE AND/OR ASSIGN";
		}

		public ISemanticCheckValue Accept(INodeVisitor visitor) {
			return visitor.VisitAssignNode (this);
		}

		public Token Token
		{
			get { return this.token; }
			set { }
		}
	}
}

