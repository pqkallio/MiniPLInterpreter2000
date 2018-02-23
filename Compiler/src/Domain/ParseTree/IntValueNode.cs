using System;

namespace MiniPLInterpreter
{
	public class IntValueNode : ISyntaxTreeNode
	{
		private int value;

		public IntValueNode (int value)
		{
			this.value = value;
		}

		public TokenType Type ()
		{
			return TokenType.INT_VAL;
		}

		public object execute ()
		{
			return this.value;
		}

		public int Value {
			get { return this.value; }
			set { this.value = value; }
		}
	}
}

