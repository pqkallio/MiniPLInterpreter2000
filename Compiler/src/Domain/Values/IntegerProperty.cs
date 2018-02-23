using System;

namespace MiniPLInterpreter
{
	public class IntegerProperty : IProperty
	{
		private int value;

		public IntegerProperty (int value)
		{
			this.value = value;
		}

		public Type GetPropertyType ()
		{
			return typeof(int);
		}

		public int Value {
			get { return this.value; }
			set { this.value = value; }
		}
	}
}

