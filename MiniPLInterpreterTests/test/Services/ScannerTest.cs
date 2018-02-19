using System;
using NUnit.Framework;
using System.Collections.Generic;
using MiniPLInterpreter;

namespace MiniPLInterpreterTests
{
	[TestFixture ()]
	public class ScannerTest
	{
		private Scanner s;

		/*
		[SetUp]
		public void SetUp()
		{
			this.s = new Scanner ();
		}

		[Test]
		public void TestValidInput1Tokens ()
		{
			List<Token> tokens = s.tokenize (TestInputs.validInput1);
			Assert.AreEqual (44, tokens.Count);
			Assert.AreEqual (0, s.getErrors ().Count);
		}

		[Test]
		public void TestValidInput2Tokens ()
		{
			List<Token> tokens = s.tokenize (TestInputs.validInput2);
			Assert.AreEqual (17, tokens.Count);
			Assert.AreEqual (0, s.getErrors ().Count);
		}

		[Test]
		public void TestValidInput3Tokens ()
		{
			List<Token> tokens = s.tokenize (TestInputs.validInput3);
			Assert.AreEqual (46, tokens.Count);
			Assert.AreEqual (0, s.getErrors ().Count);
		}

		[Test]
		public void TestValidInput4Tokens ()
		{
			List<Token> tokens = s.tokenize (TestInputs.validInput4);
			Assert.AreEqual (1, tokens.Count);
			Assert.AreEqual (tokens [0].TokenType, TokenType.END_OF_FILE);
			Assert.AreEqual (0, s.getErrors ().Count);
		}

		[Test]
		public void TestInvalidInput1Tokens ()
		{
			s.tokenize (TestInputs.invalidInput1);
			Assert.AreEqual (3, s.getErrors ().Count);
			foreach (Error e in s.getErrors()) {
				Assert.AreEqual (e.GetType ().Name, nameof(StringLiteralError));
			}
		}

		[Test]
		public void TestInvalidInput2Tokens ()
		{
			s.tokenize (TestInputs.invalidInput2);
			Assert.AreEqual (1, s.getErrors ().Count);
			foreach (Error e in s.getErrors()) {
				Assert.AreEqual (e.GetType ().Name, nameof(TokenError));
			}
		}
		*/
	}
}

