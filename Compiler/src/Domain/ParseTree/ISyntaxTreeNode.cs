using System;

namespace MiniPLInterpreter
{
	public interface ISyntaxTreeNode
	{
		object execute();
		TokenType Type();
	}
}

