using System;

namespace MiniPLInterpreter
{
	public interface IExpressionContainer
	{
		void AddExpression(ISyntaxTreeNode expressionNode);
	}
}

