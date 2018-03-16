using System;
using System.Collections;

namespace MiniPLInterpreter
{
	public class ForLoopNode : IExpressionContainer
	{
		private VariableIdNode idNode;
		private BinOpNode indexAccumulator;
		private IExpressionNode max;
		private StatementsNode statements;
		private AssignNode rangeFrom;
		private Token token;

		public ForLoopNode (VariableIdNode idNode)
		{
			this.idNode = idNode;
			indexAccumulator = new BinOpNode ();
			indexAccumulator.Operation = TokenType.BINARY_OP_ADD;
			indexAccumulator.AddOperand (idNode);
			indexAccumulator.AddOperand (new IntValueNode (1));
		}

		public TokenType Type ()
		{
			return TokenType.FOR_LOOP;
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

