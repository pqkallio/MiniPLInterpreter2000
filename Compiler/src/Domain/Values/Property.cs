using System;

namespace MiniPLInterpreter
{
	public class Property<T>
	{
		private T value;

		public Property (T value)
		{
			this.value = value;
		}

		public T Value {
			get { return this.value; }
		}
	}
}

