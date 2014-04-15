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

public static class Log
{
	public static void debug(string category, string format, params object[] args) 
	{
		string s = String.Format(format, args);
		s = String.Format("[{0}] [{1}] {2}", DateTime.Now.ToString(), category, s);
		Debug.Log(s);
	}
}