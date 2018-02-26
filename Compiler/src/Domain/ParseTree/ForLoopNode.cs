using System;

namespace MiniPLInterpreter
{
	public class ForLoopNode : ISyntaxTreeNode, IExpressionContainer
	{
		private VariableIdNode idNode;
		private BinOpNode indexAccumulator;
		private ISyntaxTreeNode max;
		private StatementsNode statements;
		private AssignNode rangeFrom;

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

		public ISyntaxTreeNode MaxValue
		{
			get { return max; }
			set { max = value; }
		}

		public object execute() {
			rangeFrom.execute ();
			int maxVal = (int)max.execute ();
			while ((int)idNode.execute () < maxVal) {
				statements.execute ();
				indexAccumulator.execute ();
			}

			return null;
		}

		public void AddExpression(ISyntaxTreeNode expressionNode)
		{
			this.max = expressionNode;
		}
	}
}

