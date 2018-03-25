using System;

namespace MiniPLInterpreter
{
	public class ConsoleReader : IReader
	{
		public ConsoleReader ()
		{}

		public string readLine ()
		{
			return Console.ReadLine ();
		}
	}
}

