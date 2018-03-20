using System;
using System.Collections;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	public class ForLoopNode : IExpressionContainer
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
			accum.AddOperand (idNode);
			accum.AddOperand (new IntValueNode (1, t));
			this.indexAccumulator.ExprNode = accum;
		}

		public TokenType Type ()
		{
			return TokenType.FOR_LOOP;
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

		public object execute() {
			rangeFrom.execute ();
			int maxVal = 5;//(int)((IntValueNode)max).execute ();
			while (((IntegerProperty)idNode.execute ()).Value < maxVal) {
				statements.execute ();
				indexAccumulator.execute ();
			}

			return null;
		}

		public void AddExpression(IExpressionNode expressionNode)
		{
			this.max = expressionNode;
		}

		public void AddNodesToQueue (Queue q)
		{
			q.Enqueue (this);
			idNode.AddNodesToQueue (q);
			rangeFrom.AddNodesToQueue (q);
			max.AddNodesToQueue (q);
			statements.AddNodesToQueue (q);
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

