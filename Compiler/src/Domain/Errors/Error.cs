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

			return string.Format ("{0}: {1}: {2} at row {3} column {4}", Title, ErrorMessage, Token.Value, Token.Row, Token.Column);
		}
	}
}

