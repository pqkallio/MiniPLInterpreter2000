using System;

namespace MiniPLInterpreter
{
	public class Token
	{
		private readonly int line;
		private readonly int column;
		private readonly string value;
		private TokenType tokenType;

		public Token (int line, int column, string value)
			: this (line, column, value, TokenType.UNDEFINED)
		{ }

		public Token (int line, int column, string value, TokenType tokenType)
		{
			this.line = line;
			this.column = column;
			this.value = value;
			this.tokenType = tokenType;
		}

		public TokenType Type
		{
			get { return tokenType; }
			set { tokenType = value; }
		}

		public string Value
		{
			get { return value; }
		}

		public int Line
		{
			get { return line; }
		}

		public int Column
		{
			get { return column; }
		}

		public override string ToString ()
		{
			return string.Format ("[ Token: Type = {0}, Value = {1}, Line = {2}, Column = {3} ]", 
				Type, Value, Line, Column);
		}
	}
}