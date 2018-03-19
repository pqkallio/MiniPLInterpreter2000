using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace MiniPLInterpreter
{
	public class AssignNode : IExpressionContainer
	{
		private VariableIdNode idNode;
		private IExpressionNode exprNode;
		private Dictionary<string, IProperty> ids;
		private Token token;

		public AssignNode (VariableIdNode idNode, Dictionary<string, IProperty> ids)
			: this (idNode, ids, null)
		{
		}

		public AssignNode (VariableIdNode idNode, Dictionary<string, IProperty> ids, Token t)
		{
			this.idNode = idNode;
			this.exprNode = null;
			this.ids = ids;
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

		public TokenType Type ()
		{
			return TokenType.ASSIGN;
		}

		public object execute()
		{
			var evaluation = exprNode.execute ();
			Type evalType = evaluation.GetType ();
			if (evalType == ids [idNode.ID].GetPropertyType ()) {
				if (evalType == typeof(int)) {
					IntegerProperty prop = (IntegerProperty) ids [idNode.ID];
					prop.Value = (int)evaluation;
				} else if (evalType == typeof(string)) {
					StringProperty prop = (StringProperty) ids [idNode.ID];
					prop.Value = (string)evaluation;
				} else if (evalType == typeof(bool)) {
					BooleanProperty prop = (BooleanProperty) ids [idNode.ID];
					prop.Value = (bool)evaluation;
				}
			} else {
				// käsittele runtime error täällä
			}

			return null;
		}

		public void AddExpression(IExpressionNode expressionNode)
		{
			this.exprNode = expressionNode;
		}

		public override string ToString ()
		{
			return "DECLARE AND/OR ASSIGN";
		}

		public void AddNodesToQueue (Queue q)
		{
			q.Enqueue (this);
			idNode.AddNodesToQueue (q);
			if (exprNode != null) {
				exprNode.AddNodesToQueue (q);
			}
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

