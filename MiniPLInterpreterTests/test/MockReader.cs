using System;
using MiniPLInterpreter;

namespace MiniPLInterpreterTests
{
	public class MockReader : IReader
	{
		private string input;

		public MockReader (string input)
		{
			this.input = input;
		}

		public string readLine ()
		{
			return this.input;
		}
	}
}

