using System;
using MiniPLInterpreter;
using NUnit.Framework;

namespace MiniPLInterpreterTests
{
	[TestFixture()]
	public class NumericUtilsTest
	{
		[Test()]
		public void TestIntBetweenValMinMaxEqual()
		{
			Assert.True (NumericUtils.IntBetween (0, 0, 0));
		}

		[Test()]
		public void TestIntBetweenValEqualsMin()
		{
			Assert.True (NumericUtils.IntBetween (0, 0, 1));
		}

		[Test()]
		public void TestIntBetweenValLessThanMin()
		{
			Assert.False (NumericUtils.IntBetween (0, 1, 2));
		}

		[Test()]
		public void TestIntBetweenValGreaterThanMax()
		{
			Assert.False (NumericUtils.IntBetween (1, -1, 0));
		}

		[Test()]
		public void TestIntBetweenValEqualsMax()
		{
			Assert.True (NumericUtils.IntBetween (1, -1, 1));
		}

		[Test()]
		public void TestIntValEqualsMaxLessThanMin()
		{
			Assert.False (NumericUtils.IntBetween (0, 1, -1));
		}
	}
}

