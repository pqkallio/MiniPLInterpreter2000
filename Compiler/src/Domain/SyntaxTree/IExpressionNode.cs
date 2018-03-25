using System;

namespace MiniPLInterpreter
{
	public interface IExpressionNode : ISyntaxTreeNode
	{
		IExpressionNode[] GetExpressions ();
		TokenType Operation { 
			get;
			set;
		}
		TokenType EvaluationType {
			get;
			set;
		}
	}
}

