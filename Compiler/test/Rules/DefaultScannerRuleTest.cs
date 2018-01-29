using NUnit.Framework;
using System;
using System.Collections.Generic;
using MiniPLInterpreter;

namespace MiniPLInterpreter.Tests
{
	[TestFixture ()]
	public class DefaultScannerRuleTest
	{
		private DefaultScannerRule rule;
		private string input1 = " 34 test123  6A3_";
		private string input2 = "test:=testagain";

		[SetUp]
		public void SetUp()
		{
			this.rule = new DefaultScannerRule ();
		}

		[Test ()]
		public void TestInput1 ()
		{
			List<string> outputs = new List<string>();
			scanTokens (ref outputs, input1);
			Assert.AreEqual (outputs.Count, 3);
			Assert.AreEqual (outputs [0], "34");
			Assert.AreEqual (outputs [1], "test123");
			Assert.AreEqual (outputs [2], "6A3_");
		}

		[Test ()]
		public void TestInput2 ()
		{
			List<string> outputs = new List<string>();
			scanTokens (ref outputs, input2);
			Assert.AreEqual (outputs.Count, 2);
			Assert.AreEqual (outputs [0], "test");
			Assert.AreEqual (outputs [1], "testagain");
		}

		private void scanTokens(ref List<string> outputs, string input)
		{
			string temp = "";
			bool tokenScanned = false;
			for (int i = 0; i < input.Length; i++) {
				tokenScanned = this.rule.scanToken (input, ref temp, ref i);
				if (!String.IsNullOrEmpty (temp)) {
					outputs.Add (String.Copy(temp));
					temp = "";
				}
			}
		}
	}
}

