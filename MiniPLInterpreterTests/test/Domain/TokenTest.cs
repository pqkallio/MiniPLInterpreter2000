using NUnit.Framework;
using System;
using MiniPLInterpreter;

namespace MiniPLInterpreterTests
{
	[TestFixture ()]
	public class TokenTest
	{
		private Token token;
		private string tokenValue;
		private int tokenLine;
		private int tokenColumn;
		private TokenType tokenType;

		[SetUp]
		public void SetUp()
		{
			this.tokenValue = "test";
			this.tokenLine = 1;
			this.tokenColumn = 3;
			this.tokenType = TokenType.ASSERT;
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

		[Test ()]
		public void TestGetTypeUNDEFINEDInConstruction ()
		{
			Assert.AreEqual (this.token.Type, TokenType.UNDEFINED);
		}

		[Test ()]
		public void TestGetTypeDefinedInContruction ()
		{
			Token t = new Token (this.tokenLine, this.tokenColumn, this.tokenValue, this.tokenType);
			Assert.AreEqual (t.Type, this.tokenType);
		}

		[Test ()]
		public void TestSetTokenType ()
		{
			Assert.AreEqual (this.token.Type, TokenType.UNDEFINED);
			this.token.Type = this.tokenType;
			Assert.AreEqual (this.token.Type, this.tokenType);
		}
	}
}