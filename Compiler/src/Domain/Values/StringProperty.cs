using System;

namespace MiniPLInterpreter
{
	public class StringProperty : IProperty
	{
		private string value;

		public StringProperty (string value)
		{
			this.value = value;
		}

		public Type GetPropertyType ()
		{
			return typeof(string);
		}

		public string Value {
			get { return this.value; }
			set { this.value = value; }
		}
	}
}

