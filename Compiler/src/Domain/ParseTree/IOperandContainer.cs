using System;

namespace MiniPLInterpreter
{
	public interface IOperandContainer
	{
		void AddOperand(ISyntaxTreeNode operandNode);
	}
}

