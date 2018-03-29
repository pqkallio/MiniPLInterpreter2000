using System;
using System.Collections;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	public class ForLoopNode : IExpressionContainer, IIdentifierContainer
	{
		private VariableIdNode idNode;
		private AssignNode indexAccumulator;
		private IExpressionNode max;
		private StatementsNode statements;
		private AssignNode rangeFrom;
		private Token token;

		public ForLoopNode (VariableIdNode idNode, Dictionary<string, IProperty> ids, Token t)
		{
			this.idNode = idNode;
			this.token = t;
			this.indexAccumulator = new AssignNode (idNode, ids, t);
			BinOpNode accum = new BinOpNode (t);
			accum.Operation = TokenType.BINARY_OP_ADD;
			accum.AddExpression (idNode);
			accum.AddExpression (new IntValueNode (1, t));
			this.indexAccumulator.ExprNode = accum;
		}

		public AssignNode Accumulator
		{
			get { return indexAccumulator; }
		}

		public StatementsNode Statements
		{
			get { return statements; }
			set { statements = value; }
		}

		public AssignNode RangeFrom
		{
			get { return rangeFrom; }
			set { rangeFrom = value;
				  rangeFrom.IDNode = idNode; }
		}

		public IExpressionNode MaxValue
		{
			get { return max; }
			set { max = value; }
		}

		public VariableIdNode IDNode
		{
			get { return idNode; }
			set { idNode = value; }
		}

		public void AddExpression(IExpressionNode expressionNode)
		{
			this.max = expressionNode;
		}

		public ISemanticCheckValue Accept(INodeVisitor visitor) {
			return visitor.VisitForLoopNode (this);
		}

		public Token Token
		{
			get { return this.token; }
			set { }
		}
	}
}

