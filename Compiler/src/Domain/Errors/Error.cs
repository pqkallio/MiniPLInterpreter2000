using System;

namespace MiniPLInterpreter
{
	public class Error
	{
		private string title;
		private string errorMessage;
		private Token token;
		private ISyntaxTreeNode node;

		public Error (string title, string errorMessage, ISyntaxTreeNode node)
		{
			this.title = title;
			this.errorMessage = errorMessage;
			this.node = node;
		}

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

		public virtual Token Token
		{
			get {
					if (this.node != null) {
						return this.node.Token;
					}

					return this.token;
				}
		}

		public ISyntaxTreeNode Node {
			get { return this.Node; }
		}

		public override string ToString ()
		{
			if (this.token == null) {
				return string.Format ("{0}: {1}", Title, ErrorMessage);
			}
			string value = Token.Value != null && Token.Value != "" ? Token.Value : Constants.tokenTypeStrings [Token.Type];
			return string.Format ("{0}: {1}: {2}", Title, ErrorMessage, value);
		}
	}
}

