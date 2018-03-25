using System;

namespace MiniPLInterpreter
{
	public interface IIdentifierContainer : ISyntaxTreeNode
	{
		VariableIdNode IDNode {
			get;
			set;
		}
	}
}

