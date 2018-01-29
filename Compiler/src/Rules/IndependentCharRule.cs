using System;

namespace MiniPLInterpreter
{
	public class IndependentCharRule : ScannerRule
	{
		public IndependentCharRule ()
		{
		}

		public bool scanToken(string input, ref string token, ref int index)
		{
			token += input[index];
			return true;
		}
	}
}

