using UnityEngine;
using System.Collections;
using System;

public class Helper
{
	public static T CreateObject<T>(String name) where T : UnityEngine.Component
	{
		GameObject go = new GameObject(name, typeof(T));
		return go.GetComponent<T>();
	}
	
	public static T CreateObject<T>(String name, GameObject parent) where T : UnityEngine.Component
	{
		GameObject go = new GameObject(name, typeof(T));
		go.transform.parent = parent.transform;
		return go.GetComponent<T>();
	}
	
	public static T InstansiateAndAdd<T>(GameObject prefab) where T : UnityEngine.Component
	{
		GameObject go = GameObject.Instantiate(prefab) as GameObject;
		go.AddComponent<T>();
		return go.GetComponent<T>();
	}
	public static T InstansiateAndAdd<T>(GameObject prefab, GameObject parent) where T : UnityEngine.Component
	{
		GameObject go = GameObject.Instantiate(prefab) as GameObject;
		go.AddComponent<T>();
		go.transform.parent = parent.transform;
		return go.GetComponent<T>();
	}
	
	public static T Instansiate<T>(GameObject prefab) where T : UnityEngine.Component
	{
		GameObject go = GameObject.Instantiate(prefab) as GameObject;
		return go.GetComponent<T>();
	}
	public static T Instansiate<T>(GameObject prefab, GameObject parent) where T : UnityEngine.Component
	{
		GameObject go = GameObject.Instantiate(prefab) as GameObject;
		go.transform.parent = parent.transform;
		return go.GetComponent<T>();
	}

	public static GameObject Instansiate(GameObject prefab)
	{
		GameObject go = GameObject.Instantiate(prefab) as GameObject;
		return go;
	}
	public static GameObject Instansiate(GameObject prefab, GameObject parent)
	{
		GameObject go = GameObject.Instantiate(prefab) as GameObject;
		go.transform.parent = parent.transform;
		return go;
	}

	public static T Find<T>(String name) where T : UnityEngine.Component
	{
		GameObject go = GameObject.Find(name) as GameObject;
		return go.GetComponent<T>();
	}
	
	public static T FindAndAdd<T>(String name) where T : UnityEngine.Component
	{
		GameObject go = GameObject.Find(name) as GameObject;
		return go.AddComponent<T>();
	}
}
