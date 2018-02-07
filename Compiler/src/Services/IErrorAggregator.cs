using System;
using System.Collections.Generic;

namespace MiniPLInterpreter
{
	public interface IErrorAggregator
	{
		void notifyError (Error error);
		List<Error> getErrors ();
	}
}

