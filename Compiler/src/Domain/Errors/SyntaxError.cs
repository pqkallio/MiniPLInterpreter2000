using System;

namespace MiniPLInterpreter
{
	public class SyntaxError : Error
	{
		private TokenType expectedType;
		private string typeString;

		public SyntaxError (Token token, string typeString)
			: this(token, TokenType.UNDEFINED)
		{
			this.typeString = typeString;
		}

		public SyntaxError (Token token)
			: this(token, TokenType.UNDEFINED)
		{
		}

		public SyntaxError (Token token, TokenType expectedType) 
			: base(Constants.SYNTAX_ERROR_TITLE, Constants.SYNTAX_ERROR_MESSAGE, token)
		{
			this.expectedType = expectedType;
		}

		public override string ToString ()
		{
			if (expectedType == TokenType.UNDEFINED) {
				if (typeString != null) {
					return string.Format ("{0}: {1} expected near {2}/{3} at row {4}, column {5}",
						Title, typeString, Token.Value, Token.Type, Token.Row, Token.Column);
				} else {
					return string.Format ("{0}: Undefined token {1}/{2} at row {3}, column {4}", 
						Title, Token.Value, Token.Type, Token.Row, Token.Column);
				}
			} else {	
				return string.Format ("{0}: {1} expected near {2}/{3} at row {4}, column {5}", 
					Title, expectedType, Token.Value, Token.Type, Token.Row, Token.Column);
			}
		}
	}
}

