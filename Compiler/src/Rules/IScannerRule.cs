using System;
using System.Collections;

namespace MiniPLInterpreter
{
	public interface IScannerRule
	{
		bool scanToken(string input, ref string token, ref int index);
	}
}

