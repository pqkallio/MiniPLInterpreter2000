using System;

namespace MiniPLInterpreter
{
	public class UnexpectedTokenException : Exception
	{
		private Token token;

		public UnexpectedTokenException ()
		{
		}

		public UnexpectedTokenException (string message)
			: base(message)
		{
		}

		public UnexpectedTokenException (Token token)
			: this("Unexpected token")
		{
			this.token = token;
		}

		public Token Token
		{
			get { return token; }
		}
	}
}

