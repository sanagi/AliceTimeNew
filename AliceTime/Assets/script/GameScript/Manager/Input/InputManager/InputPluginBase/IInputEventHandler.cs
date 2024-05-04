using UnityEngine;
using System.Collections;

namespace InputSupport
{
	public interface IInputEventHandler
	{
		int Order { get; }
		bool Process { get; }
	}
}
