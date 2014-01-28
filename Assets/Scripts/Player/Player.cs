using UnityEngine;
using System.Collections;

public class Player {
	private string mName;
	private int mScore;

	public string Name { get { return mName; } set { mName = value; } }
	public int Score { get { return mScore; } set { mScore = value; } }
}
