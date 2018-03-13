using System;
using System.Collections;

namespace MiniPLInterpreter
{
	public interface ISyntaxTreeNode
	{
		object execute();
		TokenType Type();
		void AddNodesToQueue(Queue q);
		void Accept(NodeVisitor visitor);
	}
}

