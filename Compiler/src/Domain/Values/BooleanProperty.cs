using System;

namespace MiniPLInterpreter
{
	public class BooleanProperty : IProperty
	{
		private bool value;

		public BooleanProperty (bool value)
		{
			this.value = value;
		}

		public Type GetPropertyType ()
		{
			return typeof(bool);
		}

		public bool Value {
			get { return this.value; }
			set { this.value = value; }
		}
	}
}

