using System;

namespace MiniPLInterpreter
{
	public interface IExpressionContainer : ISyntaxTreeNode
	{
		void AddExpression(IExpressionNode expressionNode);
	}
}

