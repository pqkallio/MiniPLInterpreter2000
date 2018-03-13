using System;

namespace MiniPLInterpreter
{
	public interface IStatementsContainer : ISyntaxTreeNode
	{
		StatementsNode Sequitor {
			get;
			set;
		}
	}
}

