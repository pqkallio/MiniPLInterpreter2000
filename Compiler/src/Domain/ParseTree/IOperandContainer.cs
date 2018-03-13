using System;

namespace MiniPLInterpreter
{
	public interface IOperandContainer : ISyntaxTreeNode
	{
		void AddOperand(ISyntaxTreeNode operandNode);
	}
}

