using System;
using System.Collections.Generic;
using System.Reflection;

namespace MiniPLInterpreter
{
	public class AssignNode : ISyntaxTreeNode, IExpressionContainer
	{
		private VariableIdNode idNode;
		private ISyntaxTreeNode exprNode;
		private Dictionary<string, IProperty> ids;

		public AssignNode (VariableIdNode idNode, Dictionary<string, IProperty> ids)
		{
			this.idNode = idNode;
			this.exprNode = null;
			this.ids = ids;
		}

		public VariableIdNode IDNode {
			get { return idNode; }
			set { idNode = value; }
		}

		public ISyntaxTreeNode ExprNode {
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

		public void AddExpression(ISyntaxTreeNode expressionNode)
		{
			this.exprNode = expressionNode;
		}
	}
}

