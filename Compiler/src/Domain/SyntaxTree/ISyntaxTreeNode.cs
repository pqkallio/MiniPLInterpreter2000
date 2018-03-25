using System;
using System.Collections;

namespace MiniPLInterpreter
{
	public interface ISyntaxTreeNode
	{
		ISemanticCheckValue Accept(INodeVisitor visitor);
		Token Token { 
			get; 
			set; 
		}
	}
}

