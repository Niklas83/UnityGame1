using UnityEngine;
using System.Collections;

public class Player 
{
	private string _name;
	private int _score;

	public string Name { get { return _name; } set { _name = value; } }
	public int Score { get { return _score; } set { _score = value; } }
}
