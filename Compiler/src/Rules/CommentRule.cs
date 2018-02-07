using System;

namespace MiniPLInterpreter
{
	public class CommentRule : IScannerRule
	{
		public CommentRule ()
		{
		}

		public bool scanToken(string input, ref string token, ref int index)
		{
			foreach (string commentStartDelimiter in Constants.COMMENT_DELIMITERS.Keys) {
				if (StringUtils.sequenceMatch (input, index, commentStartDelimiter)) {
					index = findCommentEnd (input, index, Constants.COMMENT_DELIMITERS [commentStartDelimiter]);

					return true;
				}
			}

			return false;
		}

		private int findCommentEnd(string input, int index, string delimiter)
		{
			int i;

			for (i = index; i < input.Length; i++) {
				if (input [i] == delimiter [0] && StringUtils.sequenceMatch (input, i, delimiter)) {
					i += delimiter.Length - 1;
					break;
				}
			}

			return i;
		}
	}
}

