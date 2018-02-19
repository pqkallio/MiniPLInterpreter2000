using System;
using System.Text;

namespace MiniPLInterpreter
{
	public class Token
	{
		private readonly int row;
		private readonly int column;
		private string value;
		private TokenType tokenType;

		public Token (int row, int column)
			: this (row, column, null, TokenType.UNDEFINED)
		{ }

		public Token (int row, int column, string value)
			: this (row, column, value, TokenType.UNDEFINED)
		{ }

		public Token (int row, int column, string value, TokenType tokenType)
		{
			this.row = row;
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
			set { this.value = value; }
			get { return this.value; }
		}

		public int Row
		{
			get { return row; }
		}

		public int Column
		{
			get { return column; }
		}

		public override string ToString ()
		{
			return string.Format ("[ Token: Type = {0}, Value = {1}, Line = {2}, Column = {3} ]", 
				Type, Value, Row, Column);
		}
	}
}