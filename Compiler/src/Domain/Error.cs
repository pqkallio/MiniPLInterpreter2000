using System;

namespace MiniPLInterpreter
{
	public class Error
	{
		private string title;
		private string errorMessage;
		private Token token;

		public Error (string title, string errorMessage, Token token)
		{
			this.title = title;
			this.errorMessage = errorMessage;
			this.token = token;
		}

		public string Title
		{
			get { return this.title; }
		}

		public string ErrorMessage
		{
			get { return this.errorMessage; }
		}

		public Token Token
		{
			get { return this.token; }
		}

		public override string ToString ()
		{
			return string.Format ("{0}: {1} at row {2} column {3}", Title, ErrorMessage, Token.Line, Token.Column);
		}
	}
}

