using System;

namespace MiniPLInterpreter
{
	public interface IExpressionNode : ISyntaxTreeNode
	{
		TokenType GetEvaluationType (TokenType parentType);
		IExpressionNode[] GetExpressions ();
		TokenType Operation { 
			get;
			set;
		}
		TokenType GetValueType();
	}
}

