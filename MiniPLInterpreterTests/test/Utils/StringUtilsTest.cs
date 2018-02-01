using System;
using MiniPLInterpreter;
using NUnit.Framework;

namespace MiniPLInterpreterTests
{
	[TestFixture()]
	[Category("TestIsInteger")]
	public class StringUtilsTestIsInteger
	{
		private readonly string negInt = "-123";
		private readonly string posIntWOSign = "123";
		private readonly string posIntWSign = "+123";
		private readonly string notInt = "123a";
		private readonly string notIntEither = "1-23";

		private bool test(string str)
		{
			return StringUtils.isInteger (str);
		}

		[Test()]
		public void NullPointer()
		{
			Assert.False (test(null));
		}

		[Test()]
		public void EmptyString()
		{
			Assert.False (test(""));
		}

		[Test()]
		public void NotInteger1()
		{
			Assert.False (test(this.notInt));
		}

		[Test()]
		public void NotInteger2()
		{
			Assert.False (test(this.notIntEither));
		}

		[Test()]
		public void PosIntWOSignString()
		{
			Assert.True (test(this.posIntWOSign));
		}

		[Test()]
		public void PosIntWSignString()
		{
			Assert.True (test(this.posIntWSign));
		}

		[Test()]
		public void NegIntString()
		{
			Assert.True (test(this.negInt));
		}
	}

	[TestFixture()]
	[Category("TestParseToInt")]
	public class StringUtilsTestParseToInt
	{
		private readonly string negIntStr = "-987654";
		private readonly string posIntWOSignStr = "123456";
		private readonly string posIntWSignStr = "+123456";
		private readonly string invalidString = "123a";
		private readonly int negInt = -987654;
		private readonly int posIntWOSign = 123456;

		private int test(string str)
		{
			return StringUtils.parseToInt (str);
		}

		[Test()]
		public void ThrowsForNullPointer() {
			Assert.Throws (typeof(ArgumentException), delegate {
				test(null);
			});
		}

		[Test()]
		public void ThrowsForEmptyString() {
			Assert.Throws (typeof(ArgumentException), delegate {
				test("");
			});
		}

		[Test()]
		public void ThrowsForUnparseableString() {
			Assert.Throws (typeof(ArgumentException), delegate {
				test(this.invalidString);
			});
		}

		[Test()]
		public void ParsesNegInt()
		{
			int parsed = test (this.negIntStr);
			Assert.AreEqual (parsed, this.negInt);
		}

		[Test()]
		public void ParsesPosIntWOSign()
		{
			int parsed = test (this.posIntWOSignStr);
			Assert.AreEqual (parsed, this.posIntWOSign);
		}

		[Test()]
		public void ParsesPosIntWSign()
		{
			int parsed = test (this.posIntWSignStr);
			Assert.AreEqual (parsed, this.posIntWOSign);
		}
	}

	[TestFixture()]
	[Category("TestSequenceMatch")]
	public class StringUtilsTestSequenceMatch
	{
		private readonly string input = "abcdefghijkl";
		private readonly string validSequence1 = "abc";
		private readonly string validSequence2 = "ghi";
		private readonly string invalidSequence1 = "abcdefghijklm";
		private readonly string invalidSequence2 = "abcdefghijkm";

		private bool test(string input, int index, string seq)
		{
			return StringUtils.sequenceMatch (input, index, seq);
		}

		[Test()]
		public void NullInputThrows()
		{
			Assert.Throws (typeof(ArgumentNullException), delegate {
				test(null, 0, this.validSequence1);
			});
		}

		[Test()]
		public void NullSequenceThrows()
		{
			Assert.Throws (typeof(ArgumentNullException), delegate {
				test(this.input, 0, null);
			});
		}

		[Test()]
		public void NegIndexThrows()
		{
			Assert.Throws (typeof(ArgumentOutOfRangeException), delegate {
				test(this.input, -1, this.validSequence1);
			});
		}

		[Test()]
		public void TooGreatIndexThrows()
		{
			Assert.Throws (typeof(ArgumentOutOfRangeException), delegate {
				test(this.input, this.input.Length, this.validSequence1);
			});
		}

		[Test()]
		public void EmptyStringsMatch()
		{
			Assert.True (test("", 0, ""));
		}

		[Test()]
		public void EmptySeqMatches()
		{
			Assert.True (test(this.input, 0, ""));
		}

		[Test()]
		public void TrueIfIndexIsRight1()
		{
			Assert.True (test(this.input, 0, this.validSequence1));
		}

		[Test()]
		public void FalseIfIndexIsWrong1()
		{
			Assert.False (test(this.input, 1, this.validSequence1));
		}

		[Test()]
		public void TrueIfIndexIsRight2()
		{
			Assert.True (test(this.input, 6, this.validSequence2));
		}

		[Test()]
		public void FalseIfIndexIsWrong2()
		{
			Assert.False (test(this.input, 7, this.validSequence2));
		}

		[Test()]
		public void FalseIfNotPerfectMatch()
		{
			Assert.False (test(this.input, 0, this.invalidSequence2));
		}

		[Test()]
		public void FalseIfSequenceIsTooLong()
		{
			Assert.False (test(this.input, 0, this.invalidSequence1));
		}
	}

	[TestFixture()]
	[Category("TestDelimited")]
	public class StringUtilsTestDelimited
	{
		private readonly char delimiter = '"';
		private readonly string delimited = "\"delimited, yes?\"";
		private readonly string notDelimited1 = "not delimited, yes?\"";
		private readonly string notDelimited2 = "\"not delimited, yes?";
		private readonly string notDelimited3 = "not delimited, yes?";

		public bool test(string str, char delimiter)
		{
			return StringUtils.delimited (str, delimiter);
		}

		[Test()]
		public void NullPointerThrows()
		{
			Assert.Throws (typeof(ArgumentNullException), delegate {
				test (null, delimiter);
			});
		}

		[Test()]
		public void FalseIfEmptyString()
		{
			Assert.False(test("", delimiter));
		}

		[Test()]
		public void FalseIfNotDelimited()
		{
			Assert.False(test(notDelimited3, delimiter));
		}

		[Test()]
		public void FalseIfNotLeftDelimited()
		{
			Assert.False(test(notDelimited1, delimiter));
		}

		[Test()]
		public void FalseIfNotRightDelimited()
		{
			Assert.False(test(notDelimited2, delimiter));
		}

		[Test()]
		public void TrueIfDelimited()
		{
			Assert.True(test(delimited, delimiter));
		}
	}
}