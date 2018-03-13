using System;
using System.IO;
using NUnit.Framework;
using System.Collections.Generic;
using MiniPLInterpreter;

namespace MiniPLInterpreterTests
{
	[TestFixture ()]
	public class ScannerTest
	{
		private Scanner s;
		private StreamReader sr;
		private List<Token> tokens;

		private static Stream StringStream(string s)
		{
			MemoryStream stream = new MemoryStream ();
			StreamWriter sw = new StreamWriter (stream);
			sw.Write (s);
			sw.Flush ();
			stream.Position = 0;
			return stream;
		}

		private void InitScanner(string s)
		{
			Stream stream = StringStream (s);
			this.sr = new StreamReader (stream);
			this.s = new Scanner (this.sr);
		}

		[SetUp]
		public void SetUp()
		{
			this.tokens = new List<Token> ();
		}

		[TearDown]
		public void TearDown ()
		{
			if (this.sr != null) {
				this.sr.Close ();
			}
		}

		[Test]
		public void TestValidInput1Tokens ()
		{
			InitScanner (TestInputs.validInput1);
			Token t = null;

			while (t == null || t.Type != TokenType.END_OF_FILE) {
				t = s.getNextToken (t);
				tokens.Add(t);
			}

			Assert.AreEqual (44, tokens.Count);
			Assert.AreEqual (0, s.getErrors ().Count);
		}


		[Test]
		public void TestValidInput2Tokens ()
		{
			InitScanner (TestInputs.validInput2);
			Token t = null;

			while (t == null || t.Type != TokenType.END_OF_FILE) {
				t = s.getNextToken (t);
				tokens.Add(t);
			}

			Assert.AreEqual (17, tokens.Count);
			Assert.AreEqual (0, s.getErrors ().Count);
		}

		[Test]
		public void TestValidInput3Tokens ()
		{
			InitScanner (TestInputs.validInput3);
			Token t = null;

			while (t == null || t.Type != TokenType.END_OF_FILE) {
				t = s.getNextToken (t);
				tokens.Add(t);
			}

			Assert.AreEqual (46, tokens.Count);
			Assert.AreEqual (0, s.getErrors ().Count);
		}

		[Test]
		public void TestValidInput4Tokens ()
		{
			InitScanner (TestInputs.validInput4);
			Token t = null;

			while (t == null || t.Type != TokenType.END_OF_FILE) {
				t = s.getNextToken (t);
				tokens.Add(t);
			}

			Assert.AreEqual (1, tokens.Count);
			Assert.AreEqual (tokens [0].Type, TokenType.END_OF_FILE);
			Assert.AreEqual (0, s.getErrors ().Count);
		}

		[Test]
		public void TestInvalidInput1Tokens ()
		{
			InitScanner (TestInputs.invalidInput1);
			Token t = null;

			while (t == null || t.Type != TokenType.END_OF_FILE) {
				t = s.getNextToken (t);
				tokens.Add(t);
			}

			Assert.AreEqual (3, s.getErrors ().Count);

			foreach (Error e in s.getErrors()) {
				Assert.AreEqual (e.GetType ().Name, nameof(StringLiteralError));
			}
		}

		[Test]
		public void TestInvalidInput2Tokens ()
		{
			InitScanner (TestInputs.invalidInput2);
			Token t = null;

			while (t == null || t.Type != TokenType.END_OF_FILE) {
				t = s.getNextToken (t);
				tokens.Add(t);
			}

			Assert.AreEqual (1, s.getErrors ().Count);
			foreach (Error e in s.getErrors()) {
				Assert.AreEqual (e.GetType ().Name, nameof(TokenError));
			}
		}

		[Test]
		public void TestInvalidInput3Tokens ()
		{
			InitScanner (TestInputs.invalidInput3);
			Token t = null;

			while (t == null || t.Type != TokenType.END_OF_FILE) {
				t = s.getNextToken (t);
				tokens.Add(t);
				Console.WriteLine (t.Type);
			}

			Assert.AreEqual (1, s.getErrors ().Count);
			foreach (Error e in s.getErrors()) {
				Assert.AreEqual (e.GetType ().Name, nameof(TokenError));
			}
		}

		[Test]
		public void TestTwoErrorsInOneStatement ()
		{
			InitScanner (TestInputs.invalidInput4);
			Token t = null;

			while (t == null || t.Type != TokenType.END_OF_FILE) {
				t = s.getNextToken (t);
				tokens.Add(t);
				Console.WriteLine (t.Type);
			}

			Assert.AreEqual (2, s.getErrors ().Count);
		}
	}
}

