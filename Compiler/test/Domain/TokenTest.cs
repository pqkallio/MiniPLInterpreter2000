using NUnit.Framework;
using System;
using MiniPLInterpreter;

namespace MiniPLInterpreter.Tests
{
	[TestFixture ()]
	public class TokenTest
	{
		private Token token;
		private string tokenValue;
		private int tokenLine;
		private int tokenColumn;

		[SetUp]
		public void SetUp()
		{
			this.tokenValue = "test";
			this.tokenLine = 1;
			this.tokenColumn = 3;
			this.token = new Token (tokenLine, tokenColumn, tokenValue);
		}

		[Test ()]
		public void TestGetTokenValue ()
		{
			Assert.AreEqual (this.token.Value, this.tokenValue);
		}

		[Test ()]
		public void TestGetTokenLine ()
		{
			Assert.AreEqual (this.token.Line, this.tokenLine);
		}

		[Test ()]
		public void TestGetTokenColumn ()
		{
			Assert.AreEqual (this.token.Column, this.tokenColumn);
		}
	}
}