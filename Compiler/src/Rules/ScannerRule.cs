using System;
using System.Collections;

namespace MiniPLInterpreter
{
	public interface ScannerRule
	{
		bool scanToken(string input, ref string token, ref int index);
	}
}

