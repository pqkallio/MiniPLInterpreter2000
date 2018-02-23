using System;

namespace MiniPLInterpreter
{
	public class StringValueNode : ISyntaxTreeNode
	{
		private string value;

		public StringValueNode (string value)
		{
			this.value = value;
		}

		public TokenType Type ()
		{
			return TokenType.STR_VAL;
		}

		public object execute ()
		{
			return this.value;
		}

		public string Value {
			get { return this.value; }
			set { this.value = value; }
		}
	}
}

