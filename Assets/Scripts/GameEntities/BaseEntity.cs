using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class BaseEntity : MonoBehaviour, IActivatable 
{
	protected GridManager GridManager { get { return mGridManager; } }
	private GridManager mGridManager;
	public bool Active = true;

	public virtual void Init(GridManager iGridManager) {
		mGridManager = iGridManager;
	}
	
	public void SetActive(bool iActive) {
		Active = iActive;
		GetComponent<MeshRenderer>().enabled = iActive;
		if (iActive)
			OnActivated();
		else
			OnDeactivated();
	}

	public bool IsActive() {
		return Active;
	}

	protected virtual void OnActivated() {}
	protected virtual void OnDeactivated() {}
}
