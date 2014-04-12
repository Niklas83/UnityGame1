using UnityEngine;
using System.Collections;

public interface IActivatable
{
	void SetActive(bool active);
	bool IsActive();
}

