using UnityEngine;
using System.Collections;
using System;

public static class DebugAux
{
	public static void Assert(bool condition, string errorMessage)
	{
		if (!condition) throw new Exception(errorMessage);
	}
}

