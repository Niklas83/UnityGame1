using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class BaseState 
{
	Animator _animator;
	int _enterAnimationHash;
	
	public BaseState(GameObject gameObject, string enterAnimation) 
	{
		_animator = gameObject.GetComponentInChildren<Animator>();
		_enterAnimationHash = Animator.StringToHash(enterAnimation);
	}
	public virtual void OnEnter() 
	{
		_animator.SetTrigger(_enterAnimationHash);
	}
	public virtual void Update() {}
	public virtual void OnExit() {}
}

public class StateMachine {

	private BaseState _currentState;
	private Dictionary<int, BaseState> _allStates;
	
	public StateMachine(int startState, Dictionary<int, BaseState> allStates) 
	{
		_currentState = allStates[startState];
		_currentState.OnEnter();
		_allStates = allStates;
	}

	public void ChangeState(int newState)
	{
		BaseState previous = _currentState;
		previous.OnExit();
		
		BaseState next = _allStates[newState];
		next.OnEnter();
		
		_currentState = next;
	}
	
	public void Update()
	{
		_currentState.Update();
	}
}
