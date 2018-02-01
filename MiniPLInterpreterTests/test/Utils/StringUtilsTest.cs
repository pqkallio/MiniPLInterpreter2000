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

		[Test()]
		public void TestIsIntegerNullPointer()
		{
			Assert.False (StringUtils.isInteger(null));
		}

		[Test()]
		public void TestIsIntegerEmptyString()
		{
			Assert.False (StringUtils.isInteger(""));
		}

		[Test()]
		public void TestIsIntegerNotInteger1()
		{
			Assert.False (StringUtils.isInteger(this.notInt));
		}

		[Test()]
		public void TestIsIntegerNotInteger2()
		{
			Assert.False (StringUtils.isInteger(this.notIntEither));
		}

		[Test()]
		public void TestIsIntegerPosIntWOSignString()
		{
			Assert.True (StringUtils.isInteger(this.posIntWOSign));
		}

		[Test()]
		public void TestIsIntegerPosIntWSignString()
		{
			Assert.True (StringUtils.isInteger(this.posIntWSign));
		}

		[Test()]
		public void TestIsIntegerNegIntString()
		{
			Assert.True (StringUtils.isInteger(this.negInt));
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

		[Test()]
		public void TestParseToIntThrowsForNullPointer() {
			Assert.Throws (typeof(ArgumentException), delegate {
				StringUtils.parseToInt(null);
			});
		}

		[Test()]
		public void TestParseToIntThrowsForEmptyString() {
			Assert.Throws (typeof(ArgumentException), delegate {
				StringUtils.parseToInt("");
			});
		}

		[Test()]
		public void TestParseToIntThrowsForUnparseableString() {
			Assert.Throws (typeof(ArgumentException), delegate {
				StringUtils.parseToInt(this.invalidString);
			});
		}

		[Test()]
		public void TestParseToIntParsesNegInt()
		{
			int parsed = StringUtils.parseToInt (this.negIntStr);
			Assert.AreEqual (parsed, this.negInt);
		}

		[Test()]
		public void TestParseToIntParsesPosIntWOSign()
		{
			int parsed = StringUtils.parseToInt (this.posIntWOSignStr);
			Assert.AreEqual (parsed, this.posIntWOSign);
		}

		[Test()]
		public void TestParseToIntParsesPosIntWSign()
		{
			int parsed = StringUtils.parseToInt (this.posIntWSignStr);
			Assert.AreEqual (parsed, this.posIntWOSign);
		}
	}

	[TestFixture()]
	[Category("TestSequenceMatch")]
	public class StringUtilsTestSequenceMatch
	{
		
	}
}

