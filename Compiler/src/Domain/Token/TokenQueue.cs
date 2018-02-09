using System;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	public class TokenQueue
	{
		private Token[] queue;
		private int index;

		public TokenQueue (List<Token> tokens)
		{
			this.queue = new Token[tokens.Count];
			tokens.CopyTo (this.queue);
			this.index = 0;
		}

		public bool Empty()
		{
			return index >= this.queue.Length;
		}

		public Token Dequeue()
		{
			if (Empty()) {
				return null;
			}

			Token t = queue [index];
			index++;
			return t;
		}
	}
}

