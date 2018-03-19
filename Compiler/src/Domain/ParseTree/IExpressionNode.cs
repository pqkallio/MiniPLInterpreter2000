using System;

namespace MiniPLInterpreter
{
	public interface IExpressionNode : ISyntaxTreeNode
	{
		TokenType GetEvaluationType (TokenType parentType); // to be abolished
		IExpressionNode[] GetExpressions ();
		TokenType Operation { 
			get;
			set;
		}
		TokenType EvaluationType {
			get;
			set;
		}
		TokenType GetValueType(); // to be replaced with EvaluationType
	}
}

